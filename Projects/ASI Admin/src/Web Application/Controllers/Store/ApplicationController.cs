using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.model.store;
using asi.asicentral.services;

namespace asi.asicentral.web.Controllers.Store
{
    public class ApplicationController : Controller
    {
        public const string COMMAND_SAVE = "Save";
        public const string COMMAND_REJECT = "Reject";
        public const string COMMAND_ACCEPT = "Accept";

        public IStoreService StoreService { get; set; }
        public IFulfilmentService FulfilmentService { get; set; }
        public ICreditCardService CreditCardService { get; set; }

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
            if (ModelState.IsValid)
            {
                Order order = StoreService.GetAll<Order>().Where(ordr => ordr.Id == application.OrderId).SingleOrDefault();
                DistributorMembershipApplication distributorApplication = StoreService.GetAll<DistributorMembershipApplication>().Where(app => app.Id == application.Id).SingleOrDefault();
                if (order == null) throw new Exception("Invalid reference to an order");
                if (distributorApplication == null) throw new Exception("Invalid reference to an application");
                order.ExternalReference = application.ExternalReference;
                //TODO temporary until the ui gets the contacts
                application.Contacts = distributorApplication.Contacts;
                //view does not contain some of the collections, copy from the ones in the database
                application.AccountTypes = distributorApplication.AccountTypes;
                application.ProductLines = distributorApplication.ProductLines;
                
                DistributorBusinessRevenue PrimaryBusinessRevenue = StoreService.GetAll<DistributorBusinessRevenue>(false).Where(revenue => revenue.Name == application.BuisnessRevenue).SingleOrDefault();
                if (PrimaryBusinessRevenue != null)
                {
                    application.PrimaryBusinessRevenue = PrimaryBusinessRevenue;
                    application.PrimaryBusinessRevenueId = PrimaryBusinessRevenue.Id;
                }
                else
                {
                    application.PrimaryBusinessRevenue = null;
                    application.PrimaryBusinessRevenueId = null;
                }
                application.CopyTo(distributorApplication);

                ProcessCommand(StoreService, FulfilmentService, order, distributorApplication, application.ActionName);
                StoreService.SaveChanges();
                if (application.ActionName == ApplicationController.COMMAND_REJECT)
                    return RedirectToAction("List", "Orders");
                else
                    return RedirectToAction("Edit", "Application", new { id = application.Id, orderId = order.Id });
            }
            else
            {
                return View("../Store/Application/Distributor", application);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult EditSupplier(SupplierApplicationModel application)
        {
            if (ModelState.IsValid)
            {
                Order order = StoreService.GetAll<Order>(false).Where(ordr => ordr.Id == application.OrderId).SingleOrDefault();
                SupplierMembershipApplication supplierApplication = StoreService.GetAll<SupplierMembershipApplication>(false).Where(app => app.Id == application.Id).SingleOrDefault();
                if (order == null) throw new Exception("Invalid reference to an order");
                if (supplierApplication == null) throw new Exception("Invalid reference to an application");
                order.ExternalReference = application.ExternalReference;
                //view model does not have the decorating types, get the ones from the database
                application.DecoratingTypes = supplierApplication.DecoratingTypes;
                application.UpdateDecoratingTypes(StoreService.GetAll<SupplierDecoratingType>().ToList());
                application.CopyTo(supplierApplication);
                ProcessCommand(StoreService, FulfilmentService, order, supplierApplication, application.ActionName);
                StoreService.SaveChanges();
                if (application.ActionName == ApplicationController.COMMAND_REJECT)
                    return RedirectToAction("List", "Orders");
                else
                    return RedirectToAction("Edit", "Application", new { id = application.Id, orderId = order.Id });
            }
            else
            {
                return View("../Store/Application/Supplier", application);
            }
        }

        /// <summary>
        /// Common code between Edit supplier and distributor
        /// </summary>
        /// <param name="storeService"></param>
        /// <param name="order"></param>
        /// <param name="applicationId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private void ProcessCommand(IStoreService storeService, IFulfilmentService fulfilmentService, Order order, OrderDetailApplication application, string command)
        {
            if (command == ApplicationController.COMMAND_ACCEPT)
            {
                //make sure we have external reference
                if (string.IsNullOrEmpty(order.ExternalReference)) throw new Exception("You need to specify a Timms id to approve an order");

                //make sure timms id contains numbers only
                int num;
                bool success = int.TryParse(order.ExternalReference, out num);
                if (!success) throw new Exception("Timms id must be numbers only.");
                
                fulfilmentService.Process(order, application);
                order.ProcessStatus = OrderStatus.Approved;
            }
            else if (command == ApplicationController.COMMAND_REJECT)
            {
                order.ProcessStatus = OrderStatus.Rejected;
                try
                {
                    if (order.CreditCard != null && !string.IsNullOrEmpty(order.CreditCard.ExternalReference))
                        CreditCardService.Delete(order.CreditCard.ExternalReference);
                }
                catch (Exception exception)
                {
                    ILogService log = LogService.GetLog(this.GetType());
                    log.Error("Could not remove a credit card record: " + exception.Message);
                }
                order.CreditCard.ExternalReference = null;
            }
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