using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ezWeb.Models;

namespace ezWeb.Controllers
{
    public class homeController : Controller
    {
        public IActionResult index()
        {
            return View();
        }

        public IActionResult about()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult error()
        {
            return View(new errorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
