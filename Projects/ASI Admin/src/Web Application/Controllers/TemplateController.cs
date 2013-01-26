using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers
{
    public class TemplateController : Controller
    {
        public ActionResult Form()
        {
            return View("Form");
        }
        public ActionResult Dialog()
        {
            return View("Dialog");
        }
    }
}
