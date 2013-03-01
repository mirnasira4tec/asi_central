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
            SupplierMembershipApplication supplierApplication = StoreObjectService.GetAll<SupplierMembershipApplication>(true).Where(theApp => theApp.Id == id).SingleOrDefault();
            if (supplierApplication != null)
                return View("../Store/Application/Supplier", supplierApplication);
            else
            {
                DistributorMembershipApplication distributorApplication = StoreObjectService.GetAll<DistributorMembershipApplication>(true).Where(theApp => theApp.Id == id).SingleOrDefault();
                if (distributorApplication != null) return View("../Store/Application/Distributor", distributorApplication);
                else
                {
                    // can't find any application - return nothing
                    return new EmptyResult();
                }
            }
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
