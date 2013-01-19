using asi.asicentral.model.sgr;
using asi.asicentral.services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers.sgr
{
    public class CompanyController : Controller
    {
        private IObjectService _objectService;

        public CompanyController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public ActionResult List()
        {
            IList<Company> companies = _objectService.GetAll<Company>().OrderBy(company => company.Name).ToList();
            ViewBag.Title = "List of Companies";
            return View("../sgr/Company/List", companies);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Company company = _objectService.GetAll<Company>().Where(comp => comp.Id == id).FirstOrDefault();
            if (company == null) throw new Exception("Invalid identifier for a company");
            ViewBag.Title = "Edit a Company";
            return View("../sgr/Company/Edit", company);
        }
    }
}
