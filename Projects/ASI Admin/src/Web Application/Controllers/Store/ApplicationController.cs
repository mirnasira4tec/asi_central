using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.model.store;

namespace asi.asicentral.web.Controllers.Store
{
    public class ApplicationController : Controller
    {
        public IStoreService StoreObjectService { get; set; }

        [HttpGet]
        public virtual ActionResult Edit(Guid id, int orderId)
        {
            OrderDetailApplication application = GetApplication(id);
            Order order = StoreObjectService.GetAll<Order>(true).Where(ordr => ordr.Id == orderId).SingleOrDefault();
            if (application != null && order != null)
            {
                if (application is SupplierMembershipApplication) return View("../Store/Application/Supplier", new SupplierApplicationModel((SupplierMembershipApplication)application, order));
                else if (application is DistributorMembershipApplication) return View("../Store/Application/Supplier", (DistributorMembershipApplication)application);
                else throw new Exception("Retieved an unknown type of application");
            }
            else
            {
                throw new Exception("Could not find the application or the order");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult Approve(OrderDetailApplication orderDetailApplication)
        {
            return Save(orderDetailApplication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult Save(OrderDetailApplication orderDetailApplication)
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
