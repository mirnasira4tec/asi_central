using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using asi.asicentral.services.interfaces;
using asi.asicentral.model.sgr;

namespace asi.asicentral.web.Controllers.sgr
{
    public class ProductController : Controller
    {
        private IObjectService _objectService;

        public ProductController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        [HttpGet]
        public ActionResult List(int id, int? categoryId)
        {
            if (categoryId == null)
                ViewBag.CategoryID = 32;
            else
                ViewBag.CategoryID = categoryId;

            Company company = _objectService.GetAll<Company>().Where(c => c.Id == id).SingleOrDefault();

            return View("../sgr/Product/List", company);
        }

        [HttpGet]
        public ActionResult Add(int id, int categoryId)
        {
            Product product = new Product();
            product.Company = _objectService.GetAll<Company>().Where(c => c.Id == id).SingleOrDefault();
            
            product.Categories.Clear();
            product.Categories.Add(product.Company.Categories.Where(c => c.Id == categoryId).SingleOrDefault());

            ViewBag.CategoryID = categoryId;

            return View("../sgr/Product/Edit", product);
        }

        //TODO figure out how to validate data while allowing html data
        //TODO figure out why validation for Model is showing false
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Add(int id, int categoryId, Product product)
        {
            //if (ModelState.IsValid)
            //{
            //    Company company = _objectService.GetAll<Company>(false).Where(c => c.Id == id).SingleOrDefault();
            //    Category category = company.Categories.Where(c => c.Id == categoryId).SingleOrDefault();

            //    product.Categories.Add(category);
            //    company.Products.Add(product);

            //    _objectService.Update<Company>(company);
            //    _objectService.SaveChanges();

            //    return RedirectToAction("List", new { id = product.Company.Id, categoryId = categoryId });
            //}
            //else
            //{
            //    ViewBag.CategoryID = categoryId;
            //    return RedirectToAction("List", new { id = product.Company.Id, categoryId = categoryId });
            //}

            Company company = _objectService.GetAll<Company>(false).Where(c => c.Id == id).SingleOrDefault();
            Category category = company.Categories.Where(c => c.Id == categoryId).SingleOrDefault();

            product.Categories.Add(category);
            company.Products.Add(product);

            _objectService.Update<Company>(company);
            _objectService.SaveChanges();

            return RedirectToAction("List", new { id = product.Company.Id, categoryId = categoryId });
        }

        [HttpGet]
        public ActionResult Edit(int productId, int? categoryId)
        {
            if (categoryId == null)
                ViewBag.CategoryID = 32;
            else
                ViewBag.CategoryID = categoryId;

            Product product = _objectService.GetAll<Product>().Where(p => p.Id == productId).SingleOrDefault();
            Category category = product.Categories.Where(c => c.Id == categoryId).SingleOrDefault();

            product.Categories.Clear();
            product.Categories.Add(category);

            return View("../sgr/Product/Edit", product);
        }

        //TODO figure out how to validate data while allowing html data
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Product product, int categoryId)
        {
            // TODO figure out why validation is coming up false
            //if (ModelState.IsValid)
            //{
            //    _objectService.Update<Product>(product);
            //    _objectService.SaveChanges();
            //    return RedirectToAction("ListCategory", new { id = product.Company.Id, categoryId = categoryId });
            //}
            //else
            //{
            //    ViewBag.CategoryID = categoryId;
            //    return View("../sgr/Product/Edit", product);
            //}

            _objectService.Update<Product>(product);
            _objectService.SaveChanges();
            return RedirectToAction("List", new { id = product.Company.Id, categoryId = categoryId });
        }
    }
}
