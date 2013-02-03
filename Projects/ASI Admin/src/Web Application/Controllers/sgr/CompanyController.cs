using asi.asicentral.model.sgr;
using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security.AntiXss;

namespace asi.asicentral.web.Controllers.sgr
{
    public class CompanyController : Controller
    {
        public CompanyController()
        {
        }

        public IObjectService ObjectService { get; set; }

        public virtual ActionResult List()
        {
            IList<Company> companies = ObjectService.GetAll<Company>().ToList();
            ViewBag.Title = Resource.TitleListCompanies;
            return View("../sgr/Company/List", companies);
        }

        [HttpGet]
        public virtual ActionResult Edit(int id)
        {
            Company company = ObjectService.GetAll<Company>().Where(comp => comp.Id == id).FirstOrDefault();
            if (company == null) throw new Exception("Invalid identifier for a company");
            ViewBag.Title = Resource.TitleEditCompany;
            return View("../sgr/Company/Edit", company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(Company company)
        {
            ViewBag.Title = Resource.TitleEditCompany;
            if (ModelState.IsValid)
            {
                ObjectService.Update<Company>(company);
                ObjectService.SaveChanges();
                return RedirectToAction("List");
            }
            else
                return View("../sgr/Company/Edit", company);
        }

        [HttpGet]
        public virtual ActionResult Add()
        {
            ViewBag.Title = Resource.TitleAddCompany;
            Company company = new Company();
            return View("../sgr/Company/Edit", company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Add(Company company)
        {
            ViewBag.Title = Resource.TitleAddCompany;
            if (ModelState.IsValid)
            {
                Category category = ObjectService.GetAll<Category>().Where(c => c.Id == Category.CATEGORY_ALL).SingleOrDefault();

                if (category == null) 
                    throw new Exception("Invalid identifier for a category: " + Category.CATEGORY_ALL);
                
                company.Categories.Add(category);
                ObjectService.Add<Company>(company);
                ObjectService.SaveChanges();

                return RedirectToAction("List");
            }
            else
                return View("../sgr/Company/Edit", company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int id)
        {
            Company company = ObjectService.GetAll<Company>().Where(c => c.Id == id).SingleOrDefault();
            if (company != null)
            {
                //first delete the products associated with the company
                int productCount = company.Products.Count;
                for (int i = productCount; i > 0; i--)
                {
                    ObjectService.Delete<Product>(company.Products.ElementAt(i - 1));
                }
                ObjectService.Delete<Company>(company);
                ObjectService.SaveChanges();

                return Redirect("List");
            }
            else
                throw new Exception("IObjectService returned a null value.");
        }
    }
}
