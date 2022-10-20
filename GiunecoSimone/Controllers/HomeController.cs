using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiunecoSimone.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult CreateNew()
        {
            ViewBag.Message = "Your contact page.";

            return RedirectToAction("Home");
        }


    }
}