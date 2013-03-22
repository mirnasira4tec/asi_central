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
        public const string COMMAND_SAVE = "Save";
        public const string COMMAND_REJECT = "Reject";
        public const string COMMAND_ACCEPT = "Accept";

        public IStoreService StoreService { get; set; }
        public IFulfilmentService FulfilmentService { get; set; }

        [HttpGet]
        public virtual ActionResult Edit(Guid id, int orderId)
        {
            OrderDetailApplication application = GetApplication(id);
            Order order = StoreService.GetAll<Order>(true).Where(ordr => ordr.Id == orderId).SingleOrDefault();
            if (application != null && order != null)
            {
                if (application is SupplierMembershipApplication) return View("../Store/Application/Supplier", new SupplierApplicationModel((SupplierMembershipApplication)application, order));
                else if (application is DistributorMembershipApplication) return View("../Store/Application/Distributor", new DistributorApplicationModel((DistributorMembershipApplication)application, order));
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
        public virtual ActionResult EditDistributor(DistributorApplicationModel application)
        {
            Order order = StoreService.GetAll<Order>().Where(ordr => ordr.Id == application.OrderId).SingleOrDefault();
            DistributorMembershipApplication distributorApplication = StoreService.GetAll<DistributorMembershipApplication>().Where(app => app.Id == application.Id).SingleOrDefault();
            if (order == null) throw new Exception("Invalid reference to an order");
            if (distributorApplication == null) throw new Exception("Invalid reference to an application");
            order.ExternalReference = application.ExternalReference;
            application.CopyTo(distributorApplication);
            return ProcessCommand(StoreService, FulfilmentService, order, distributorApplication, application.ActionName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult EditSupplier(SupplierApplicationModel application)
        {
            Order order = StoreService.GetAll<Order>().Where(ordr => ordr.Id == application.OrderId).SingleOrDefault();
            SupplierMembershipApplication supplierApplication = StoreService.GetAll<SupplierMembershipApplication>().Where(app => app.Id == application.Id).SingleOrDefault();
            if (order == null) throw new Exception("Invalid reference to an order");
            if (supplierApplication == null) throw new Exception("Invalid reference to an application");
            order.ExternalReference = application.ExternalReference;
            application.CopyTo(supplierApplication);
            return ProcessCommand(StoreService, FulfilmentService, order, supplierApplication, application.ActionName);
        }

        /// <summary>
        /// Common code between Edit supplier and distributor
        /// </summary>
        /// <param name="storeService"></param>
        /// <param name="order"></param>
        /// <param name="applicationId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private ActionResult ProcessCommand(IStoreService storeService, IFulfilmentService fulfilmentService, Order order, OrderDetailApplication application, string command)
        {
            if (command == ApplicationController.COMMAND_ACCEPT)
            {
                //make sure we have external reference
                if (string.IsNullOrEmpty(order.ExternalReference)) throw new Exception("You need to specify a Timms id to approve an order");
                fulfilmentService.Process(order, application);
                order.ProcessStatus = OrderStatus.Approved;
            }
            else if (command == ApplicationController.COMMAND_REJECT)
            {
                order.ProcessStatus = OrderStatus.Rejected;
            }
            StoreService.SaveChanges();
            if (command == ApplicationController.COMMAND_REJECT)
                return RedirectToAction("List", "../Orders");
            else
                return RedirectToAction("Edit", "Application", new { id = application.Id, orderId = order.Id });
        }

        private OrderDetailApplication GetApplication(Guid id)
        {
            OrderDetailApplication application = null;
            //we have more distributor applications than supplier ones
            application = StoreService.GetAll<DistributorMembershipApplication>(true).Where(theApp => theApp.Id == id).SingleOrDefault();
            if (application == null)
            {
                application = StoreService.GetAll<SupplierMembershipApplication>(true).Where(theApp => theApp.Id == id).SingleOrDefault();
            }
            return application;
        }
    }
}
