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
            
            Category category = company.Categories.Where(c => c.Id == viewCompany.CategoryID).SingleOrDefault();

            viewCompany.Products = category.Products;

            return View("../sgr/Product/List", viewCompany);
        }

        [HttpGet]
        public ActionResult Add(int companyId, int categoryId)
        {
            ViewProduct viewProduct = new ViewProduct();
            viewProduct.Company = new Company();

            viewProduct.Company.Id = companyId;
            viewProduct.CategoryID = categoryId;

            return View("../sgr/Product/Edit", viewProduct);
        }

        //TODO figure out how to validate data while allowing html data
        //TODO figure out why model validation is still showing up false
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Add(ViewProduct viewProduct)
        {
            Company company = _objectService.GetAll<Company>().Where(c => c.Id == viewProduct.Company.Id).SingleOrDefault();
            if (company == null)
                throw new Exception("Invalid identifier for a product: " + viewProduct.Company.Id);

            Product product = viewProduct.GetProduct();

            Category category = _objectService.GetAll<Category>().Where(c => c.Id == viewProduct.CategoryID).SingleOrDefault();
            if (category == null)
                throw new Exception("Invalid identifier for a category: " + viewProduct.CategoryID);

            product.Categories.Add(category);
            company.Products.Add(product);
            _objectService.Update<Company>(company);
            _objectService.SaveChanges();
            
            return RedirectToAction("List", new ViewCompany { Id = viewProduct.Company.Id, CategoryID = viewProduct.CategoryID });
        }

        [HttpGet]
        public ActionResult Edit(int productId, int categoryId)
        {
            Product product = _objectService.GetAll<Product>(false).Where(p => p.Id == productId).SingleOrDefault();
            if (product == null) 
                throw new Exception("Invalid identifier for a product: " + productId);

            ViewProduct viewProduct = new ViewProduct();
            viewProduct.CategoryID = categoryId;
            product.CopyTo(viewProduct);

            return View("../sgr/Product/Edit", viewProduct);
        }

        
        //TODO figure out how to validate data while allowing html data
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ViewProduct viewProduct)
        {
            Product product = viewProduct.GetProduct();
            _objectService.Update(product);
            _objectService.SaveChanges();
            return RedirectToAction("List", new ViewCompany { Id = viewProduct.Company.Id, CategoryID = viewProduct.CategoryID });

            //TODO figure out why model validation is still showing up false
            //viewProduct.Company = _objectService.GetAll<Company>().Where(c => c.Id == viewProduct.Company.Id).SingleOrDefault();
            //if (ModelState.IsValid)
            //{
            //    _objectService.Update<Product>(viewProduct);
            //    _objectService.SaveChanges();
            //    RedirectToAction("List", new ViewCompany { Id = viewProduct.Company.Id, CategoryID = viewProduct.CategoryID });
            //}

            //Product product = _objectService.GetAll<Product>(false).Where(p => p.Id == viewProduct.Id).SingleOrDefault();
            //if (product == null) throw new Exception("Invalid identifier for a product: " + viewProduct.Id);
            //product.CopyTo(viewProduct);
            //return View("../sgr/Product/Edit", viewProduct);
        }

        [HttpPost]
        public ActionResult Delete(int id, int categoryid)
        {
            Product product = _objectService.GetAll<Product>().Where(p => p.Id == id).SingleOrDefault();
            if (product == null)
                throw new Exception("Invalid identifier for a product: " + id);

            int companyId = product.Company.Id;
            _objectService.Delete<Product>(product);
            _objectService.SaveChanges();

            return RedirectToAction("List", new ViewCompany { Id = companyId, CategoryID = categoryid });
        }
    }
}
