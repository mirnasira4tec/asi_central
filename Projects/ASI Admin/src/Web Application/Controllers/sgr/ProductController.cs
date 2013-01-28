using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using asi.asicentral.web.Models.sgr;
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
        public ActionResult List(ViewCompany viewCompany)
        {
            Company company = _objectService.GetAll<Company>().Where(c => c.Id == viewCompany.Id).SingleOrDefault();
            
            if (company == null) 
                throw new Exception("Invalid identifier for a company: " + viewCompany.Id);

            company.CopyTo(viewCompany);
            viewCompany.Products = company.Categories.Where(c => c.Id == viewCompany.CategoryID).SingleOrDefault().Products.ToList();

            return View("../sgr/Product/List", viewCompany);
        }

        [HttpGet]
        public ActionResult Add(int id, int? categoryId)
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
            _objectService.SaveChanges();

            return RedirectToAction("List", new { id = product.Company.Id, categoryId = categoryId });
        }

        [HttpGet]
        public ActionResult Edit(int productId, int categoryId)
        {
            Product product = _objectService.GetAll<Product>(false).Where(p => p.Id == productId).SingleOrDefault();
            
            if (product == null) throw new Exception("Invalid identifier for a product: " + productId);

            ViewProduct viewProduct = new ViewProduct();
            viewProduct.CategoryID = categoryId;
            product.CopyTo(viewProduct);

            return View("../sgr/Product/Edit", viewProduct);
        }

        //TODO figure out why model validation is still false
        //TODO figure out how to validate data while allowing html data
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ViewProduct viewProduct)
        {
            if (ModelState.IsValid)
            {
                _objectService.Update<Product>(viewProduct);
                _objectService.SaveChanges();
                RedirectToAction("List", new ViewCompany { Id = viewProduct.Company.Id, CategoryID = viewProduct.CategoryID });
            }

            Product product = _objectService.GetAll<Product>(false).Where(p => p.Id == viewProduct.Id).SingleOrDefault();

            if (product == null) throw new Exception("Invalid identifier for a product: " + viewProduct.Id);

            product.CopyTo(viewProduct);

            return View("../sgr/Product/Edit", viewProduct);
        }
    }
}
