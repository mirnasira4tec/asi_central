using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;

namespace asi.asicentral.web.Controllers.Store
{
    public class OrdersController : Controller
    {
        public IStoreService StoreObjectService { get; set; }

        public OrdersController() { }

        [HttpGet]
        public virtual ActionResult List()
        {
            //TODO get list of orders by date range and return the data to the view
            return View("../Store/Admin/Orders");
        }

        [HttpGet]
        public virtual ActionResult GetSupplierApplication(Guid applicationId)
        {
            if (applicationId == null) throw new Exception("Parameter userId cannot be null");
            else
            {
                SupplierMembershipApplication supplierApplication =
                    StoreObjectService.GetAll<SupplierMembershipApplication>(true).Where(application => application.Id == applicationId).SingleOrDefault();
                             
                if (supplierApplication == null)
                    throw new Exception("Invalid application with application id " + applicationId);
                else
                    return View("../Store/Application/Supplier", supplierApplication);
            }
        }

        [HttpGet]
        public virtual ActionResult GetDistributorApplication(Guid applicationId)
        {
            if (applicationId == null) throw new Exception("Parameter userId cannot by null");
            else
            {
                DistributorMembershipApplication distributorApplication =
                    StoreObjectService.GetAll<DistributorMembershipApplication>(true).Where(application => application.Id == applicationId).SingleOrDefault();

                if (distributorApplication == null)
                    throw new Exception("Invalid application with application id " + applicationId);
                else
                    return View("../Store/Application/Distributor", distributorApplication);
            }
        }
    }
}
