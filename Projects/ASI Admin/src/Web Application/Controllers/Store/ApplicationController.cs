using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.Models.Store;

namespace asi.asicentral.web.Controllers.Store
{
    public class ApplicationController : Controller
    {
        public IStoreService StoreObjectService { get; set; }

        [HttpGet]
        public virtual ActionResult Edit(Guid id)
        {
            OrderDetailApplication application = GetApplication(id);
            if (application != null)
            {
                if (application is SupplierMembershipApplication) return View("../Store/Application/Supplier", (SupplierMembershipApplication)application);
                else if (application is DistributorMembershipApplication) return View("../Store/Application/Supplier", (DistributorMembershipApplication)application);
                else throw new Exception("Retieved an unknown type of application");
            }
            else
            {
                throw new Exception("Could not find the application");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult Edit(OrderDetailApplication orderDetailApplication, String command)
        {
            return new EmptyResult();
        }

        private OrderDetailApplication GetApplication(Guid id)
        {
            OrderDetailApplication application = null;
            //we have more distributor applications than supplier ones
            application = StoreObjectService.GetAll<DistributorMembershipApplication>(true).Where(theApp => theApp.Id == id).SingleOrDefault();
            if (application == null)
            {
                application = StoreObjectService.GetAll<SupplierMembershipApplication>(true).Where(theApp => theApp.Id == id).SingleOrDefault();
            }
            return application;
        }
    }
}
