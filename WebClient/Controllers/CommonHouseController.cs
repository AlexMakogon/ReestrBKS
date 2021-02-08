using Microsoft.AspNetCore.Mvc;
using ReestrBKS.BusinessLogic.Import;
using ReestrBKS.BusinessLogic.Report;
using ReestrBKS.DataAccess;
using ReestrBKS.DataModel;
using System.IO;
using System.Linq;

namespace ReestrBKS.WebClient.Controllers
{
    public class CommonHouseController : AbstractLineController<CommonHouseLine>
    {
        public CommonHouseController(ReestrContext context) : base(context)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Info()
        {
            return View();
        }

        internal override AbstractImporter<CommonHouseLine> GetImporter(string fileName, Stream fileStream, ReestrContext context)
        {
            return new CommonHouseImporter(fileName, fileStream, context);
        }

        internal override AbstractReporter<CommonHouseLine> GetReporter()
        {
            return new CommonHouseReporter(User.Identity.Name);
        }

        internal override IQueryable<object> DistinctLines(IQueryable<CommonHouseLine> linesQuery)
        {
            IQueryable<object> tempQuery = linesQuery
                .Select(p => new
                {
                    Subject = new Subject()
                    {
                        Id = p.Subject.Id,
                        Street = new Street() { Id = p.Subject.Street.Id, Name = p.Subject.Street.Name },
                        House = p.Subject.House,
                        Apartment = p.Subject.Apartment,
                        Room = p.Subject.Room
                    }
                })
                .Distinct()
                .OrderBy(p => p.Subject.Street.Name)
                .ThenBy(p => p.Subject.House)
                .ThenBy(p => p.Subject.Apartment);

            return tempQuery;
        }
    }
}
