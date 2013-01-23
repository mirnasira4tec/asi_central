using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using asi.asicentral.model.sgr;
using asi.asicentral.services.interfaces;

namespace asi.asicentral.web.Controllers.sgr
{
    public class CategoryController : Controller
    {
        private IObjectService _objectService;

        public CategoryController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        // TODO figure out how to get list of categories
        // TODO figour out how to add new category
        public ActionResult Edit(int id)
        {
            return View("../sgr/Category/Edit");
        }
    }
}
