using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.model.store;
using System.Text;
using asi.asicentral.web.model.store.order;
using System.Data.Objects.SqlClient;

namespace asi.asicentral.web.Controllers.Store
{

    [Authorize]
    public class ProductsController : Controller
    {
        public IStoreService StoreService { get; set; }

        public ActionResult Index()
        {
            return View("List");
        }

        [HttpGet]
        public ActionResult List()
        {
            IList<ContextProduct> productList = StoreService.GetAll<ContextProduct>(true).ToList();
            return View("../Store/Products/List", productList);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult List(IList<ContextProduct> products)
        {
            IList<ContextProduct> productList = StoreService.GetAll<ContextProduct>().ToList();
            foreach (ContextProduct product in products)
            {
                ContextProduct productToUpdate = productList.Where(item => item.Id == product.Id).FirstOrDefault();
                if (productToUpdate != null)
                {
                    productToUpdate.NextAvailableDate = product.NextAvailableDate;
                    productToUpdate.IsAvailable = product.IsAvailable;
                    productToUpdate.NotificationEmails = product.NotificationEmails;
                }
            }
            StoreService.SaveChanges();
            return new RedirectResult("/Store/Products/List");
        }

    }
}
