using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DFUNK.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Database for Developers. KEA 2015 Autumn.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Team Supermen Contact Page.";

            return View();
        }
    }
}