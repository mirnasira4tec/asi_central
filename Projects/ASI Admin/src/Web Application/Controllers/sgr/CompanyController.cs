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
            IList<Company> companies = _objectService.GetAll<Company>().ToList();
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

        //TODO figure out how to validate data while allowing html data
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Company company)
        {
            ViewBag.Title = "Edit a Company";
            if (ModelState.IsValid)
            {
                _objectService.Update<Company>(company);
                _objectService.SaveChanges();
                return RedirectToAction("List");
            }
            else
                return View("../sgr/Company/Edit", company);
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Title = "Add a Company";
            Company company = new Company();
            return View("../sgr/Company/Edit", company);
        }

        //TODO fiure out how to validate data while allowing html data
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Add(Company company)
        {
            ViewBag.Title = "Add a Company";
            if (ModelState.IsValid)
            {
                // TODO Fix hardcoded value
                //Category category = _objectService.GetAll<Category>().Where(c => c.Id == 32).SingleOrDefault();
                //company.Categories = new List<Category>();
                //_objectService.Add<Company>(company);
                //_objectService.SaveChanges();
                //company.Categories.Add(category);
                //_objectService.Update(company);
                //_objectService.SaveChanges();

                Category category = _objectService.GetAll<Category>().Where(c => c.Id == 32).SingleOrDefault();
                company.Categories.Add(category);
                _objectService.Add<Company>(company);
                _objectService.SaveChanges();

                return RedirectToAction("List");
            }
            else
                return View("../sgr/Company/Edit", company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Company company = _objectService.GetAll<Company>().Where(c => c.Id == id).SingleOrDefault();
            if (company != null)
            {
                //first delete the products associated with the company
                int productCount = company.Products.Count;
                for (int i = productCount; i > 0; i--)
                {
                    _objectService.Delete<Product>(company.Products[i - 1]);
                }
                _objectService.Delete<Company>(company);
                _objectService.SaveChanges();

                return Redirect("List");
            }
            else
                throw new Exception("IObjectService returned a null value.");
        }
    }
}
