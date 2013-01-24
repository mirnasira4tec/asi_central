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

        [HttpGet]
        public ActionResult Add(int id)
        {
            ViewBag.Title = "Add Category";
            ViewBag.CompanyID = id;
            ViewBag.CategoryID = 32;

            Category category = new Category();
            return View("../sgr/Category/Edit", category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Add(Category category, int companyId)
        {
            if (ModelState.IsValid)
            {
                Company company = _objectService.GetAll<Company>().Where(c => c.Id == companyId).SingleOrDefault();
                company.Categories.Add(category);
                _objectService.Update<Company>(company);
                _objectService.SaveChanges();

                return RedirectToAction("List", "Product", new { id = companyId });
            }
            else
            {
                ViewBag.Title = "Add Category";
                ViewBag.CompanyID = companyId;
                ViewBag.CategoryID = 32;
                return View("../sgr/Category/Edit", category);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id, int? categoryId)
        {
            ViewBag.Title = "Edit Category";
            ViewBag.CompanyID = id;
            ViewBag.CategoryID = categoryId;

            Category category = _objectService.GetAll<Category>().Where(c => c.Id == categoryId).SingleOrDefault();

            return View("../sgr/Category/Edit", category); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Category category, int companyId)
        {
            if (ModelState.IsValid)
            {
                _objectService.Update<Category>(category);
                _objectService.SaveChanges();
                return RedirectToAction("List", "Product", new { id = companyId });
            }
            else
            {
                ViewBag.Title = "Edit Category";
                ViewBag.CompanyID = companyId;
                ViewBag.CategoryID = category.Id;
                return View("../sgr/Category/Edit", category);
            }
        }
    }
}
