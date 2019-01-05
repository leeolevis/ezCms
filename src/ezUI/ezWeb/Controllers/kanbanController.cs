using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ezWeb.Controllers
{
    public class kanbanController : Controller
    {
        public IActionResult index()
        {
            return View();
        }
    }
}