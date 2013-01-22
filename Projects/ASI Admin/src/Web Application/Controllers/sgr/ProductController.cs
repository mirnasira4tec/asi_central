using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.services.interfaces;

namespace asi.asicentral.web.Controllers.sgr
{
    public class ProductController : Controller
    {
        private IObjectService _objectService;

        public ProductController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public ActionResult List()
        {
            return View("../sgr/Product/Index");
        }

    }
}
