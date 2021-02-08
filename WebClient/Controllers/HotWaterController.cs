using Microsoft.AspNetCore.Mvc;
using ReestrBKS.BusinessLogic.Import;
using ReestrBKS.BusinessLogic.Report;
using ReestrBKS.DataAccess;
using ReestrBKS.DataModel;
using System.IO;

namespace ReestrBKS.WebClient.Controllers
{
    public class HotWaterController : AbstractLineController<HotWaterLine>
    {
        public HotWaterController(ReestrContext context) : base(context)
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

        internal override AbstractImporter<HotWaterLine> GetImporter(string fileName, Stream fileStream, ReestrContext context)
        {
            return new HotWaterImporter(fileName, fileStream, context);
        }

        internal override AbstractReporter<HotWaterLine> GetReporter()
        {
            return new HotWaterReporter(User.Identity.Name);
        }
    }
}
