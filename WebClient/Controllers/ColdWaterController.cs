using Microsoft.AspNetCore.Mvc;
using ReestrBKS.BusinessLogic.Import;
using ReestrBKS.BusinessLogic.Report;
using ReestrBKS.DataAccess;
using ReestrBKS.DataModel;
using System.IO;

namespace ReestrBKS.WebClient.Controllers
{
    public class ColdWaterController : AbstractLineController<ColdWaterLine>
    {
        public ColdWaterController(ReestrContext context) : base(context)
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

        internal override AbstractImporter<ColdWaterLine> GetImporter(string fileName, Stream fileStream, ReestrContext context)
        {
            return new ColdWaterImporter(fileName, fileStream, context);
        }

        internal override AbstractReporter<ColdWaterLine> GetReporter()
        {
            return new ColdWaterReporter(User.Identity.Name);
        }
    }
}
