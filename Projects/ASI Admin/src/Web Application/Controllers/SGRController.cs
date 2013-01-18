using asi.asicentral.model.sgr;
using asi.asicentral.services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers
{
    public class SGRController : Controller
    {
        private IObjectService _objectService;

        public SGRController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public ActionResult Index()
        {
            return View("CompanyList");
        }

        public ActionResult CompanyList()
        {
            IList<Company> companies = _objectService.GetAll<Company>().OrderBy(company => company.Name).ToList();
            ViewBag.Title = "List of Companies";
            return View("CompanyList", companies);
        }

        [HttpGet]
        public ActionResult CompanyEdit(int id)
        {
            Company company = _objectService.GetAll<Company>().Where(comp => comp.Id == id).FirstOrDefault();
            if (company == null) throw new Exception("Invalid identifier for a company");
            ViewBag.Title = "Edit a Company";
            return View("CompanyEdit", company);
        }
    }
}
