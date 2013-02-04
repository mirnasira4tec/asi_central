using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using asi.asicentral.model.sgr;
using asi.asicentral.interfaces;
using asi.asicentral.web.Models.sgr;

namespace asi.asicentral.web.Controllers.sgr
{
    public class CategoryController : Controller
    {
        public CategoryController()
        {
        }

        public IObjectService ObjectService { get; set; }

        [HttpGet]
        public virtual ActionResult Add(int companyId)
        {
            ViewBag.Title = Resource.TitleAddCategory;

            ViewCategory viewCategory = new ViewCategory();
            viewCategory.CompanyID = companyId;

            return View("../sgr/Category/Edit", viewCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public virtual ActionResult Add(ViewCategory viewCategory)
        {
            if (ModelState.IsValid)
            {
                Company company = ObjectService.GetAll<Company>().Where(c => c.Id == viewCategory.CompanyID).SingleOrDefault();
                if (company == null) 
                    throw new Exception("Invalid identifier for a company: " + viewCategory.CompanyID);                

                // check to see if the category exist
                Category existingCategory = ObjectService.GetAll<Category>().Where(c => c.Name == viewCategory.Name).SingleOrDefault();
                if (existingCategory != null)
                {
                    company.Categories.Add(existingCategory);
                }
                else
                {
                    Category category = new Category();
                    viewCategory.CopyTo(category);
                    company.Categories.Add(category);
                }
                ObjectService.Update<Company>(company);
                ObjectService.SaveChanges();
                return RedirectToAction("List", "Product", new ViewCompany { Id = viewCategory.CompanyID, CategoryID = Category.CATEGORY_ALL });
            }
            else
            {
                ViewBag.Title = Resource.TitleAddCategory;
                return View("../sgr/Category/Edit", viewCategory);
            }
        }

        [HttpGet]
        public virtual ActionResult Edit(ViewCategory viewCategory)
        {
            ViewBag.Title = Resource.TitleEditCategory;

            Category category = ObjectService.GetAll<Category>().Where(c => c.Id == viewCategory.Id).SingleOrDefault();
            
            if (category == null)
                throw new Exception("Invalid identifier for a category: " + viewCategory.Id);

            category.CopyTo(viewCategory);

            return View("../sgr/Category/Edit", viewCategory); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public virtual ActionResult Edit(Category category, int companyId)
        {
            if (ModelState.IsValid)
            {
                ObjectService.Update<Category>(category);
                ObjectService.SaveChanges();
                return RedirectToAction("List", "Product", new ViewCompany { Id = companyId, CategoryID = category.Id });
            }
            else
            {
                ViewBag.Title = Resource.TitleEditCategory;
                return View("../sgr/Category/Edit", new ViewCategory { Id = category.Id, CompanyID = companyId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int id, int companyId)
        {
            Category category = ObjectService.GetAll<Category>().Where(c => c.Id == id).SingleOrDefault();
            if (category == null)
                throw new Exception("Invalid identifier for a category: " + id);
            else if (category.Id == Category.CATEGORY_ALL)
                throw new Exception("Cannot delete a category with identifier: " + id);

            Company company = category.Companies.Where(c => c.Id == companyId).SingleOrDefault();
            if (company == null)
                throw new Exception("Invalid identifier for a company: " + companyId);

            category.Companies.Remove(company);

            ObjectService.Update<Category>(category);
            ObjectService.SaveChanges();

            return RedirectToAction("List", "Product", new ViewCompany { Id = companyId, CategoryID = Category.CATEGORY_ALL });
        }
    }
}
