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
        public virtual ActionResult Edit(OrderDetailApplication orderDetailApplication)
        {
            OrderDetailApplication application;

            application = StoreObjectService.GetAll<SupplierMembershipApplication>(true).Where(theApp => theApp.Id == orderDetailApplication.Id).SingleOrDefault() as SupplierMembershipApplication;
            if (application != null) return View("../Store/Application/Supplier", application);

            application = StoreObjectService.GetAll<DistributorMembershipApplication>(true).Where(theApp => theApp.Id == orderDetailApplication.Id).SingleOrDefault() as DistributorMembershipApplication;
            if (application != null) return View("../Store/Application/Distributor", application);

            // can't find any application - return nothing
            return new EmptyResult();
        }
    }
}
