using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.Models.Store;
using asi.asicentral.web.Models.Store.Application;

namespace asi.asicentral.web.Controllers.Store
{
    public class ApplicationController : Controller
    {
        public IStoreService StoreObjectService { get; set; }

        [HttpGet]
        public virtual ActionResult Edit(Guid id)
        {
            OrderDetailApplication application = StoreObjectService.GetAll<SupplierMembershipApplication>(true).Where(theApp => theApp.Id == id).SingleOrDefault() as SupplierMembershipApplication;
            if (application != null)
            {
                ApplicationPageModel pageView = new ApplicationPageModel(StoreObjectService, application);
                return View("../Store/Application/Supplier", pageView);
            }

            application = StoreObjectService.GetAll<DistributorMembershipApplication>(true).Where(theApp => theApp.Id == id).SingleOrDefault() as DistributorMembershipApplication;
            {
                ApplicationPageModel pageView = new ApplicationPageModel(StoreObjectService, application);
                if (application != null) return View("../Store/Application/Distributor", pageView);
            }

            // can't find any application - return nothing
            return new EmptyResult();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult Edit(OrderDetailApplication orderDetailApplication, String command)
        {

            return new EmptyResult();
        }
    }
}
