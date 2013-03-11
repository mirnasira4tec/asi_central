using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Application.Controllers
{
    public class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            ViewBag.Message = "Playing with the technology stack and good practices";

            return View("Index");
        }
    }
}
