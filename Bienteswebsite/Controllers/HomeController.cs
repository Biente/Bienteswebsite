using Bienteswebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using MySql.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Bienteswebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {         
            return View();
        }

        [Route("prijzen")]
        public IActionResult Prijzen()
        {
            return View();
        }

        [Route("data")]
        public IActionResult Data()
        {
            return View();
        }

        [Route("regels")]
        public IActionResult Regels()
        {
            return View();
        }

        [Route("locaties")]
        public IActionResult Locaties()
        {
            return View();
        }


        public IActionResult Privacy()
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
