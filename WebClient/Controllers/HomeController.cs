using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using ReestrBKS.BusinessLogic;
using ReestrBKS.BusinessLogic.Import;
using ReestrBKS.DataAccess;
using ReestrBKS.DataAccess.Interfaces;
using ReestrBKS.DataModel;
using ReestrBKS.WebClient.Models;
using WebClient.Models;

namespace ReestrBKS.WebClient.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
