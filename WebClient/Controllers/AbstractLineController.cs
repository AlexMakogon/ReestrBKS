using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReestrBKS.BusinessLogic;
using ReestrBKS.BusinessLogic.Import;
using ReestrBKS.BusinessLogic.Report;
using ReestrBKS.DataAccess;
using ReestrBKS.DataModel;
using ReestrBKS.WebClient.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReestrBKS.WebClient.Controllers
{
    public abstract class AbstractLineController<T> : Controller where T : class, IAbstractLine
    {
        internal ReestrContext context;
        internal abstract AbstractReporter<T> GetReporter();
        internal abstract AbstractImporter<T> GetImporter(string fileName, Stream fileStream, ReestrContext context);

        internal AbstractLineController(ReestrContext context)
        {
            this.context = context;
        }

        public void ImportReestr(int year, int month, IFormFile file)
        {
            MemoryStream fileStream = new MemoryStream();
            file.CopyTo(fileStream);
            AbstractImporter<T> importer = GetImporter(file.FileName, fileStream, context);
            importer.Import(year, month);

            try
            {
                context.CleanDataBase();
            }
            catch (Exception exc)
            {
                Logger.WriteStr("Ошибка: Не удалось реорганизовать БД. Выполните скрипт вручную. " + exc.Message);
            }
        }

        public void ImportReestr2(IFormFile file)
        {
            string monthStr = file.FileName.Substring(0, 2);
            string yearStr = "20" + file.FileName.Substring(2, 2);

            if (monthStr[0] == '0')
                monthStr = monthStr[1].ToString();

            int month = int.Parse(monthStr);
            int year = int.Parse(yearStr);

            ImportReestr(year, month, file);
        }

        public JsonResult GetAbstractLines(AbstractLineFilter filter, int pageNumber, int itemsOnPage)
        {
            IQueryable<T> linesQuery = context.Set<T>()
                .Include(p => p.Person)
                .Include(p => p.Subject)
                .ThenInclude(p => p.Street);

            if (filter != null)
                linesQuery = FilterAbstractLine(linesQuery, filter);

            IQueryable<object> tempQuery = DistinctLines(linesQuery);
            int pagesCount = tempQuery.Count() / itemsOnPage;
            List<string> pages = GetPages(pageNumber, 10, pagesCount);

            object[] lines = tempQuery
                .Skip((pageNumber - 1) * itemsOnPage)
                .Take(itemsOnPage)
                .ToArray();

            return Json(new { lines, pages, pageNumber, itemsOnPage });
        }

        internal virtual IQueryable<object> DistinctLines(IQueryable<T> linesQuery)
        {
            IQueryable<object> tempQuery = linesQuery
                .Select(p => new
                {
                    p.Person,
                    Subject = new Subject()
                    {
                        Id = p.Subject.Id,
                        Street = new Street() { Id = p.Subject.Street.Id, Name = p.Subject.Street.Name },
                        House = p.Subject.House,
                        Apartment = p.Subject.Apartment,
                        Room = p.Subject.Room
                    },
                    p.AccountNumber
                })
                .Distinct()
                .OrderBy(p => p.Person.Name);

            return tempQuery;
        }

        public JsonResult GetInfo(int subjectId, int personId, string accountNumber, string startDate = null, string finishDate = null)
        {
            Subject subject = context.Subjects
                .Include(p => p.Street)
                .FirstOrDefault(p => p.Id == subjectId);
            Person person = context.People.FirstOrDefault(p => p.Id == personId);
            List<T> lines = GetAbstractLines(subject.Id, personId, accountNumber, startDate, finishDate);

            object vSubject = new
            {
                subject.Id,
                Street = new { subject.Street.Id, subject.Street.Name },
                subject.House,
                subject.Room,
                subject.Apartment
            };

            object info = new
            {
                Lines = lines.ToArray(),
                Subject = vSubject,
                Person = person
            };

            return Json(info);
        }

        public IActionResult GetReport(int subjectId, int personId, string reportName, string accountNumber, string startDate = null, string finishDate = null)
        {
            string userName = HttpContext.User.Identity.Name;
            Subject subject = context.Subjects
                .Include(p => p.Street)
                .FirstOrDefault(p => p.Id == subjectId);
            Person person = context.People.FirstOrDefault(p => p.Id == personId);
            List<T> lines = GetAbstractLines(subject.Id, personId, accountNumber, startDate, finishDate);

            if (lines.Count() == 0)
                return BadRequest("Не найдены записи за указанный период");

            string fileName;
            AbstractReporter<T> reporter = GetReporter(); //Reporter(HttpContext.User.Identity.Name);
            if (reportName == "General")
                fileName = reporter.GetGeneralReport(lines, subject, person, GetStartDate(lines), GetFinishDate(lines));
            else
                fileName = reporter.GetDetailsReport(lines, subject, person, GetStartDate(lines), GetFinishDate(lines));
            Stream fs = new FileStream(fileName, FileMode.Open);
            return File(fs, "application/xlsx", string.Format("Расчет {0} {1}.xlsx", lines.First().AccountNumber, person?.Name));
        }

        private DateTime GetStartDate(List<T> lines)
        {
            lines.OrderBy(p => p.Year)
                .ThenBy(p => p.Month);

            int month = lines.First().Month;
            int year = lines.First().Year;
            return new DateTime(year, month, 1);
        }

        private DateTime GetFinishDate(List<T> lines)
        {
            lines.OrderBy(p => p.Year)
                .ThenBy(p => p.Month);

            int month = lines.Last().Month;
            int year = lines.Last().Year;
            return new DateTime(year, month, DateTime.DaysInMonth(year, month));
        }

        internal List<T> GetAbstractLines(int? subjectId, int? personId, string accountNumber, string startDate, string finishDate)
        {
            IQueryable<T> lines = context.Set<T>()
                .Include(p => p.AmountType)
                .Include(p => p.Person)
                .OrderBy(p => p.Year)
                .ThenBy(p => p.Month)
                .ThenBy(p => p.AmountTypeId);

            if (subjectId != null && subjectId != 0)
            {
                lines = lines.Where(p => p.SubjectId == subjectId);
            }

            if (personId != null && personId != 0)
            {
                lines = lines.Where(p => p.PersonId == personId);
            }

            if (!string.IsNullOrEmpty(accountNumber))
            {
                lines = lines.Where(p => p.AccountNumber == accountNumber);
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                int month = int.Parse(startDate.Split("-")[0]);
                int year = int.Parse(startDate.Split("-")[1]);
                lines = lines.Where(p => (p.Year >= year && p.Month >= month) || p.Year > year);
            }

            if (!string.IsNullOrEmpty(finishDate))
            {
                int month = int.Parse(finishDate.Split("-")[0]);
                int year = int.Parse(finishDate.Split("-")[1]);
                lines = lines.Where(p => (p.Year <= year && p.Month <= month) || p.Year < year);
            }

            List<T> result = lines.ToList<T>();
            return result;
        }

        private IQueryable<T> FilterAbstractLine(IQueryable<T> lines, AbstractLineFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Street))
            {
                // Так сделано специально для SQLite, lower в нём некоректно работает с кирилицей
                List<Street> streets = context.Streets.ToList();
                streets = streets.Where(p => p.Name.ToLower().Contains(filter.Street.ToLower())).ToList();

                int[] ids = streets.Select(p => p.Id).ToArray();
                lines = lines.Where(p => ids.Contains(p.Subject.StreetId));
            }

            if (!string.IsNullOrEmpty(filter.House))
                lines = lines.Where(p => p.Subject.House == filter.House);

            if (!string.IsNullOrEmpty(filter.Apartment))
                lines = lines.Where(p => p.Subject.Apartment == filter.Apartment);

            if (!string.IsNullOrEmpty(filter.Room))
                lines = lines.Where(p => p.Subject.Room == filter.Room);

            if (!string.IsNullOrEmpty(filter.AccountNumber))
                lines = lines.Where(p => p.AccountNumber == filter.AccountNumber);

            if (!string.IsNullOrEmpty(filter.Person))
            {
                // Так сделано специально для SQLite, lower в нём некоректно работает с кирилицей
                List<Person> persons = context.People.ToList();
                persons = persons.Where(p => p.Name.ToLower().Contains(filter.Person.ToLower())).ToList();

                int[] ids = persons.Select(p => p.Id).ToArray();
                lines = lines.Where(p => p.PersonId != null && ids.Contains(p.PersonId.Value));
            }

            return lines;
        }

        private List<string> GetPages(int pageNumber, int maxVisiblePage, int totalPages)
        {
            List<string> pages = new List<string>();
            int minp = pageNumber - (int)Math.Floor((maxVisiblePage - 1) / (double)2);
            minp = minp < 1 ? 1 : minp;
            int maxp = minp + maxVisiblePage - 1;

            if (maxp > totalPages)
            {
                maxp = totalPages;
                int difp = maxp - minp + 1;
                minp -= Math.Min(maxVisiblePage, totalPages) - difp;
            }

            for (int i = minp; i <= maxp; i++)
            {
                if (minp != 1 && i == minp)
                {
                    pages.Add("1");
                }
                else if ((minp != 1 && i == minp + 1) || (maxp != totalPages && i == maxp - 1))
                {
                    pages.Add("...");
                }
                else if (maxp != totalPages && i == maxp)
                {
                    pages.Add(totalPages.ToString());
                }
                else
                {
                    pages.Add(i.ToString());
                }
            }

            return pages;
        }
    }
}
