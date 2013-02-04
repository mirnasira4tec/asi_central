using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using asi.asicentral.web.Models.sgr;
using asi.asicentral.interfaces;
using asi.asicentral.model.sgr;

namespace asi.asicentral.web.Controllers.sgr
{
    public class ProductController : Controller
    {
        public ProductController()
        {
        }

        public IObjectService ObjectService { get; set; }

        [HttpGet]
        public virtual ActionResult List(ViewCompany viewCompany)
        {
            Company company = ObjectService.GetAll<Company>().Where(c => c.Id == viewCompany.Id).SingleOrDefault();
            
            if (company == null) 
                throw new Exception("Invalid identifier for a company: " + viewCompany.Id);

            company.CopyTo(viewCompany);
            
            Category category = company.Categories.Where(c => c.Id == viewCompany.CategoryID).SingleOrDefault();

            viewCompany.Products = category.Products;

            return View("../sgr/Product/List", viewCompany);
        }

        [HttpGet]
        public virtual ActionResult Add(int companyId, int categoryId)
        {
            ViewBag.Title = Resource.TitleAddProduct;

            ViewProduct viewProduct = new ViewProduct();
            viewProduct.Company = new Company();

            viewProduct.Company.Id = companyId;
            viewProduct.CategoryID = categoryId;

            return View("../sgr/Product/Edit", viewProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult Add(ViewProduct viewProduct)
        {
            ModelState.Remove("Company.Summary");
            ModelState.Remove("Company.Name");

            if (ModelState.IsValid)
            {
                Company company = ObjectService.GetAll<Company>().Where(c => c.Id == viewProduct.Company.Id).SingleOrDefault();
                if (company == null)
                    throw new Exception("Invalid identifier for a product: " + viewProduct.Company.Id);

                Product product = viewProduct.GetProduct();

                Category category = ObjectService.GetAll<Category>().Where(c => c.Id == viewProduct.CategoryID).SingleOrDefault();
                if (category == null)
                    throw new Exception("Invalid identifier for a category: " + viewProduct.CategoryID);

                product.Categories.Add(category);
                company.Products.Add(product);
                ObjectService.Update<Company>(company);
                ObjectService.SaveChanges();

                return RedirectToAction("List", new ViewCompany { Id = viewProduct.Company.Id, CategoryID = viewProduct.CategoryID });
            }
            else
            {
                ViewBag.Title = Resource.TitleAddProduct;
                return View("../sgr/Product/Edit", viewProduct);
            }
        }

        [HttpGet]
        public virtual ActionResult Edit(int productId, int categoryId)
        {
            ViewBag.Title = Resource.TitleEditProduct;

            Product product = ObjectService.GetAll<Product>(false).Where(p => p.Id == productId).SingleOrDefault();
            if (product == null) 
                throw new Exception("Invalid identifier for a product: " + productId);

            ViewProduct viewProduct = new ViewProduct();
            viewProduct.CategoryID = categoryId;
            product.CopyTo(viewProduct);

            return View("../sgr/Product/Edit", viewProduct);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult Edit(ViewProduct viewProduct)
        {
            ModelState.Remove("Company.Summary");
            ModelState.Remove("Company.Name");

            if (ModelState.IsValid)
            {
                ObjectService.Update<Product>(viewProduct.GetProduct());
                ObjectService.SaveChanges();
                return RedirectToAction("List", new ViewCompany { Id = viewProduct.Company.Id, CategoryID = viewProduct.CategoryID });
            }
            else
            {
                ViewBag.Title = Resource.TitleEditProduct;

                Product product = ObjectService.GetAll<Product>(false).Where(p => p.Id == viewProduct.Id).SingleOrDefault();
                if (product == null) 
                    throw new Exception("Invalid identifier for a product: " + viewProduct.Id);
                
                product.CopyTo(viewProduct);
                return View("../sgr/Product/Edit", viewProduct);
            }
        }

        [HttpPost]
        public virtual ActionResult Delete(int id, int categoryid)
        {
            Product product = ObjectService.GetAll<Product>().Where(p => p.Id == id).SingleOrDefault();
            if (product == null)
                throw new Exception("Invalid identifier for a product: " + id);

            int companyId = product.Company.Id;
            ObjectService.Delete<Product>(product);
            ObjectService.SaveChanges();

            return RedirectToAction("List", new ViewCompany { Id = companyId, CategoryID = categoryid });
        }
    }
}
