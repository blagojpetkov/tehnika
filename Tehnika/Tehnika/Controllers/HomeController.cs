using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tehnika.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Информации за нас.";

            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Контактирајте не.";

            return View();
        }
        public ActionResult Showrooms()
        {
            ViewBag.Message = "Your showroom page.";

            return View();
        }
    }
}