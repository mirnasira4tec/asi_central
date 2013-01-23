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
        public ActionResult ListCategory(int id)
        {
            Company company = _objectService.GetAll<Company>().Where(c => c.Id == id).SingleOrDefault();

            return View("../sgr/Product/ListCategory", company);
        }

        [HttpGet]
        public ActionResult ListProduct(int companyId, int categoryId)
        {
            Company company = _objectService.GetAll<Company>().Where(c => c.Id == companyId).SingleOrDefault();
            Category category = company.Categories.Where(c => c.Id == categoryId).SingleOrDefault();
            List<Product> products = category.Products.Where(p => p.Company.Id == companyId).ToList();
            
            category.Products.Clear();
            category.Products = products;

            company.Categories.Clear();
            company.Categories.Add(category);
            
            return View("../sgr/Product/ListProduct", company);
        }

        [HttpGet]
        public ActionResult Add(int companyId, int categoryId)
        {
            Product product = new Product();
            product.Company = _objectService.GetAll<Company>().Where(c => c.Id == companyId).SingleOrDefault();
            
            product.Categories.Clear();
            product.Categories.Add(product.Company.Categories.Where(c => c.Id == categoryId).SingleOrDefault());

            return View("../sgr/Product/Edit", product);
        }

        //TODO figure out how to validate data while allowing html data
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Add(Product _product, int companyId, int categoryId)
        {
            
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int productId, int categoryId)
        {
            Product product = _objectService.GetAll<Product>().Where(p => p.Id == productId).SingleOrDefault();
            Category category = product.Categories.Where(c => c.Id == categoryId).SingleOrDefault();

            product.Categories.Clear();
            product.Categories.Add(category);

            return View("../sgr/Product/Edit", product);
        }

        //TODO figure out how to validate data while allowing html data
        //TODO figure out why validation is coming up false
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Product product, int categoryId)
        {
                _objectService.Update<Product>(product);
                _objectService.SaveChanges();
                return RedirectToAction("ListProduct", new { companyId = product.Company.Id, categoryId = categoryId });
        }
    }
}
