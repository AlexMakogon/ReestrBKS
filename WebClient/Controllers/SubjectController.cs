//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using DocumentFormat.OpenXml.Vml;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using ReestrBKS.BusinessLogic;
//using ReestrBKS.DataAccess;
//using ReestrBKS.DataAccess.Interfaces;
//using ReestrBKS.DataModel;
//using ReestrBKS.WebClient.Models;

//namespace ReestrBKS.WebClient.Controllers
//{
//    public class SubjectController : Controller
//    {
//        private ReestrContext context;
//        private IRepositoryStore repStore;
//        private int[] typesArr = new int[] { 1, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

//        public SubjectController(IRepositoryStore repStore, ReestrContext context)
//        {
//            this.repStore = repStore;
//            this.context = context;
//        }

//        public IActionResult Info()
//        {
//            return View();
//        }

//        public JsonResult GetSubjectInfo(int subjectId, int personId, string accountNumber, string startDate = null, string finishDate = null)
//        {
//            Subject subject = GetSubject(subjectId);
//            Person person = repStore.PersonRepository
//                .GetAll()
//                .FirstOrDefault(p => p.Id == personId);

//            List<IAbstractLine> lines = GetAbstractLines(subject.Id, personId, accountNumber, typesArr, startDate, finishDate);

//            object[] vLines = lines
//                .Select(p => new { 
//                    p.Year, 
//                    p.Month, 
//                    AmountType = new { p.AmountType.Id, p.AmountType.Name }, 
//                    AbstractValues = new { }
//                }).ToArray();

//            object vSubject = new { 
//                subject.Id, 
//                Street = new { subject.Street.Id, subject.Street.Name },
//                subject.House,
//                subject.Room,
//                subject.Apartment
//            };

//            object vPerson = new { person.Id, person.Name };

//            object info = new
//            {
//                ValueTypes = new { },
//                Lines = vLines,
//                Subject = vSubject,
//                Person = vPerson
//            };

//            return Json(info);
//        }

//        public IActionResult GetReport(int subjectId, int personId, string reportName, string accountNumber, string startDate = null, string finishDate = null)
//        {
//            string userName = HttpContext.User.Identity.Name;
//            Subject subject = GetSubject(subjectId);
//            List<IAbstractLine> lines = GetAbstractLines(subject.Id, personId, accountNumber, typesArr, startDate, finishDate);

//            if (lines.Count() == 0)
//                return BadRequest("Не найдены записи за указанный период");

//            Person person = repStore.PersonRepository
//                .GetAll()
//                .FirstOrDefault(p => p.Id == personId);

//            string fileName;
//            HotWaterReporter reporter = new HotWaterReporter(HttpContext.User.Identity.Name);
//            if (reportName == "General")
//                fileName = reporter.GetGeneralReport(lines, subject, person, GetStartDate(lines), GetFinishDate(lines));
//            else
//                fileName = reporter.GetDetailsReport(lines, subject, person, GetStartDate(lines), GetFinishDate(lines));
//            Stream fs = new FileStream(fileName, FileMode.Open);
//            return File(fs, "application/xlsx", string.Format("Расчет {0} {1}.xlsx", lines.First().AccountNumber, person.Name));
//        }

//        private DateTime GetStartDate(List<IAbstractLine> lines)
//        {
//            lines.OrderBy(p => p.Year)
//                .ThenBy(p => p.Month);

//            int month = lines.First().Month;
//            int year = lines.First().Year;
//            return new DateTime(year, month, 1);
//        }

//        private DateTime GetFinishDate(List<IAbstractLine> lines)
//        {
//            lines.OrderBy(p => p.Year)
//                .ThenBy(p => p.Month);

//            int month = lines.Last().Month;
//            int year = lines.Last().Year;
//            return new DateTime(year, month, DateTime.DaysInMonth(year, month));
//        }

//        private Subject GetSubject(int id)
//        {
//            Subject subject = repStore.SubjectRepository
//               .GetAll()
//               .Include(p => p.Street)
//               .FirstOrDefault(p => p.Id == id);

//            return subject;
//        }

//        private List<IAbstractLine> GetAbstractLines(int subjectId, int personId, string accountNumber, int[] typesArr, string startDate, string finishDate)
//        {
//            IQueryable<HotWaterLine> lines = context.HotWaterLines
//                .Include(p => p.AmountType)
//                .Include(p => p.Person)
//                .Where(p => p.SubjectId == subjectId && p.PersonId == personId && p.AccountNumber == accountNumber && p.AmountTypeId != 1)
//                .OrderBy(p => p.Year)
//                .ThenBy(p => p.Month)
//                .ThenBy(p => p.AmountTypeId);

//            if (!string.IsNullOrEmpty(startDate))
//            {
//                int month = int.Parse(startDate.Split("-")[0]);
//                int year = int.Parse(startDate.Split("-")[1]);
//                lines = lines.Where(p => (p.Year >= year && p.Month >= month) || p.Year > year);
//            }

//            if (!string.IsNullOrEmpty(finishDate))
//            {
//                int month = int.Parse(finishDate.Split("-")[0]);
//                int year = int.Parse(finishDate.Split("-")[1]);
//                lines = lines.Where(p => (p.Year <= year && p.Month <= month) || p.Year < year);
//            }

//            //   List<IAbstractLine> result = lines.ToList<IAbstractLine>();
//            return new List<IAbstractLine>(); //result;
//        }

//        private Person GetPerson(int personId)
//        {
//            Person person = repStore.PersonRepository
//                .Get(personId);

//            return person;
//        }
//    }
//}
