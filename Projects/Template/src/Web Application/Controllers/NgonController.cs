using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers
{
    public class NgonController : Controller
    {
        public ActionResult Ngon()
        {
            ViewBag.Title = "Ngon";
            ViewBag.Message = "First commit";
            return View();
        }

    }
}
