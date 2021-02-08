using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using OpenXmlUtils;
using ReestrBKS.DataAccess;
using ReestrBKS.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace ReestrBKS.BusinessLogic.Import
{
    public abstract class AbstractImporter<T> : IDisposable where T : class, IAbstractLine
    {
        private string fileName;
        private SpreadsheetDocument xcell;
        private WorksheetPart workSheet;
        private WorkbookPart workBook;
        private List<string> sharedStrings;

        private ReestrContext context;
        private Dictionary<string, AmountType> amountTypes;
        private Dictionary<string, DataModel.Person> persons;
        private Dictionary<string, Street> streets;
        private Dictionary<string, SubjectType> subjectTypes;
        private Dictionary<string, Subject> subjects;
        private List<T> lines;

        protected abstract int startRowIndex { get; }
        protected abstract int cellsCount { get; }

        protected abstract T GetAbstractLine(int year, int month, List<string> cellsText);

        public AbstractImporter(string fileName, Stream fileStream, ReestrContext context)
        {
            xcell = SpreadsheetDocument.Open(fileStream, false);
            workBook = xcell.WorkbookPart;
            workSheet = workBook.WorksheetParts.First();

            this.context = context;
            this.fileName = fileName;
        }

        public void Dispose()
        {
            xcell.Close();
        }

        public void Import(int year, int month)
        {
            #region logger
            Logger.WriteStr("\r\n");
            Logger.WriteStr("Начало импорта");
            Logger.WriteStr(string.Format("Файл: {0}, год: {1}, месяц: {2}", fileName, year, CommonService.GetMonthName(month)));
            #endregion

            try
            {
                if (ImportStatus.Status != ImportStatuses.Ready)
                    throw new Exception("Импорт уже запущен, дождитесь окончания импорта.");

                ImportStatus.Status = ImportStatuses.Import;
                DateTime startDate = DateTime.Now;

                lines = new List<T>();
                amountTypes = context.AmountTypes.ToDictionary(p => p.Name);
                persons = context.People.ToDictionary(p => p.Name);
                streets = context.Streets.ToDictionary(p => p.Name);
                subjectTypes = context.SubjectTypes.ToDictionary(p => p.Name);
                subjects = context.Subjects.ToDictionary(p => p.GetAddress());
                sharedStrings = workBook.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().Select(p => p.Text.Text).ToList();

                int i = 1;
                OpenXmlReader reader = OpenXmlReader.Create(workSheet);
                while (reader.Read())
                {
                    if (reader.ElementType == typeof(Row))
                    {
                        Console.WriteLine(i);
                        Row row = (Row)reader.LoadCurrentElement();
                        List<string> cellsText = GetCellsText(row);

                        if (i >= startRowIndex)
                        {
                            T abstractLine = GetAbstractLine(year, month, cellsText);
                            lines.Add(abstractLine);
                        }

                        i++;
                    }
                }

                Logger.WriteStr(string.Format("Прочитано строк из файла: {0}", lines.Count()));

                try
                {
                    context.ChangeTracker.AutoDetectChangesEnabled = false;

                    IList<T> linesToRemove = context.Set<T>()
                        .Where(p => p.Year == year && p.Month == month)
                        .ToList<T>();
                    context.Set<T>().RemoveRange(linesToRemove);
                    context.SaveChanges();

                    List<AmountType> amountTypesNew = amountTypes
                                .Where(p => p.Value.Id == 0)
                                .Select(p => p.Value)
                                .ToList();
                    context.AddRange(amountTypesNew);

                    List<DataModel.Person> personsNew = persons
                        .Where(p => p.Value.Id == 0)
                        .Select(p => p.Value)
                        .ToList();
                    context.AddRange(personsNew);

                    List<Street> streetsNew = streets
                        .Where(p => p.Value.Id == 0)
                        .Select(p => p.Value)
                        .ToList();
                    context.AddRange(streetsNew);

                    List<SubjectType> subjectTypesNew = subjectTypes
                        .Where(p => p.Value.Id == 0)
                        .Select(p => p.Value)
                        .ToList();
                    context.AddRange(subjectTypesNew);

                    List<Subject> subjectsNew = subjects
                        .Where(p => p.Value.Id == 0)
                        .Select(p => p.Value)
                        .ToList();
                    context.AddRange(subjectsNew);

                    int j = 0;
                    int saveCount = 10000;
                    int saveAmont = 0;

                    List<T> linesToSave = new List<T>();
                    do
                    {
                        Console.WriteLine(string.Format("lines: {0} - {1}", j * saveCount, (j + 1) * saveCount));
                        linesToSave = lines.Skip(j * saveCount).Take(saveCount).ToList();
                        context.AddRange(linesToSave);
                        context.SaveChanges();
                        j += 1;
                        saveAmont += linesToSave.Count();
                        Logger.WriteStr(string.Format("Записано строк в базу: {0} из {1}", saveAmont, lines.Count()));
                    }
                    while (linesToSave.Count == saveCount);

                    Console.WriteLine((DateTime.Now - startDate).TotalMinutes + " мин.");
                    Logger.WriteStr("Конец импорта");
                }
                catch (Exception exc)
                {
                    Logger.WriteStr("Ошибка иморта: " + exc.Message);
                    throw new Exception("Не удалось сохранить данные импорта в БД, ошибка: " + exc.Message);
                }
                finally
                {
                    ImportStatus.Status = ImportStatuses.Ready;
                }
            }
            catch(Exception exc)
            {
                ImportStatus.Status = ImportStatuses.Ready;
                Logger.WriteStr("Ошибка: " + exc.Message);
                throw new Exception(exc.Message);
            }
        }

        /// <summary>
        /// Возвращает список значений в ячейках строки таблицы Excel.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private List<string> GetCellsText(Row row)
        {
            List<string> cellsText = new List<string>();
            OpenXmlReader reader = OpenXmlReader.Create(row);
            List<Cell> cells = new List<Cell>();

            while (reader.Read())
            {
                if (reader.ElementType == typeof(Cell))
                {
                    Cell cell = (Cell)reader.LoadCurrentElement();
                    cells.Add(cell);
                }
            }

            cells = ExcellUtil.FixCells(cells).ToList();
            if (cells.Count < cellsCount)
            {
                int n = cellsCount - cells.Count();
                for (int i=0; i < n; i++)
                {
                    cells.Add(null);
                }
            }
            else if (cells.Count() != cellsCount)
                throw new Exception("Не верное число колонок (" + cells.Count() + "), строка " + row.RowIndex);

            foreach(Cell cell in cells)
            {
                if (cell == null)
                    cellsText.Add(null);
                else if (cell.DataType != null && cell.DataType == CellValues.SharedString)
                    cellsText.Add(sharedStrings[int.Parse(cell.CellValue.InnerText)]);
                else
                    cellsText.Add(cell.CellValue?.InnerText);
            }

            return cellsText;
        }

        /// <summary>
        /// Возвращает вид суммы из БД. Если вид суммы не найден вернет null.
        /// </summary>
        /// <param name="amountTypeName"></param>
        /// <returns></returns>
        internal AmountType GetAmountType(string amountTypeName)
        {
            if (string.IsNullOrEmpty(amountTypeName))
                throw new Exception("Вид суммы не может быть пустым");

            AmountType amountType = null;
            if (amountTypes.ContainsKey(amountTypeName))
                amountType = amountTypes[amountTypeName];
            else
                throw new Exception("Неизвестный вид суммы: " + amountTypeName);

            return amountType;
        }

        /// <summary>
        /// Возвращает нанимателя из БД. Если наниматель не найден вернет null.
        /// </summary>
        /// <param name="personName">ФИО нанимателя</param>
        /// <returns></returns>
        internal DataModel.Person GetPerson(string personName)
        {
            if (string.IsNullOrEmpty(personName))
                throw new Exception("Не указан наниматель");

            DataModel.Person person = null;
            if (persons.ContainsKey(personName))
                person = persons[personName];
            else
            {
                person = new DataModel.Person() { Name = personName };
                persons.Add(personName, person);
            }

            return person;
        }

        /// <summary>
        /// Возвращает улицу из БД. Если улица не найдена вернет null.
        /// </summary>
        /// <param name="address">Адрес объекта</param>
        /// <returns></returns>
        private Street GetStreet(string address)
        {
            string streetName = address.Split(',').First().Replace(" ул.", "");
            if (string.IsNullOrEmpty(streetName))
                throw new Exception("Не указана улица");

            Street street = null;
            if (streets.ContainsKey(streetName))
                street = streets[streetName];
            else
            {
                street = new Street() { Name = streetName };
                streets.Add(streetName, street);
            }

            return street;
        }

        /// <summary>
        /// Возвращает номер дома из строки адреса.
        /// </summary>
        /// <param name="address">Адрес</param>
        /// <returns></returns>
        private string GetHouseNumber(string address)
        {
            string house = address.Split(',')
                .FirstOrDefault(p => p.Trim().StartsWith("д."));

            return house == null ? null : house.Trim().Replace("д.", "");
        }
        
        /// <summary>
        /// Возвращает номер квартиры из строки адреса.
        /// </summary>
        /// <param name="address">Адрес</param>
        /// <returns></returns>
        private string GetApartment(string address)
        {
            string apartment = address.Split(',')
                .FirstOrDefault(p => p.Trim().StartsWith("кв."));

            return apartment == null ? null : apartment.Trim().Replace("кв.", "");
        }

        /// <summary>
        /// Возвращает номер комнаты из строки адреса.
        /// </summary>
        /// <param name="address">Адрес</param>
        /// <returns></returns>
        private string GetRoom(string address)
        {
            string room = address.Split(',')
                .FirstOrDefault(p => p.Trim().StartsWith("ком."));

            return room == null ? null : room.Trim().Replace("ком.", "");
        }

        /// <summary>
        /// Возвращает тип объекта. Если тип объекта не найден вернет null.
        /// </summary>
        /// <param name="subjectTypeName"></param>
        /// <returns></returns>
        internal SubjectType GetSubjectType(string subjectTypeName)
        {
            if (string.IsNullOrEmpty(subjectTypeName))
                return null;

            SubjectType subjectType = null;
            if (subjectTypes.ContainsKey(subjectTypeName))
                subjectType = subjectTypes[subjectTypeName];
            else
            {
                subjectType = new SubjectType() { Name = subjectTypeName };
                subjectTypes.Add(subjectTypeName, subjectType);
            }

            return subjectType;
        }

        /// <summary>
        /// Возвращает объект аренды из БД. Если объект не найден вернет null.
        /// </summary>
        /// <param name="address">Адрес</param>
        /// <param name="subjectType">Тип</param>
        /// <param name="totalArea">Общая площадь</param>
        /// <param name="livingArea">Жилая площадь</param>
        /// <param name="accountNumber">Лицевой счет</param>
        /// <returns></returns>
        internal Subject GetSubject(string address, string subjectType, float totalArea, float livingArea)
        {
            Street street = GetStreet(address);
            string house = GetHouseNumber(address);
            string apartment = GetApartment(address);
            string room = GetRoom(address);
            SubjectType sType = GetSubjectType(subjectType);

            Subject subject = new Subject()
            {
                Street = street,
                House = house,
                Apartment = apartment,
                Room = room,
                SubjectType = sType,
                TotalArea = totalArea,
                LivingArea = livingArea
            };

            if (subjects.ContainsKey(subject.GetAddress()))
                subject = subjects[subject.GetAddress()];
            else
            {
                subjects.Add(subject.GetAddress(), subject);
            }

            return subject;
        }
    }
}
