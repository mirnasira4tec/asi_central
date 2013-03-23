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
            return ProcessCommand(StoreService, FulfilmentService, order, application.Id, application.ActionName);
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
            SaveDecoratingTypesTo(application, supplierApplication);
            return ProcessCommand(StoreService, FulfilmentService, order, application.Id, application.ActionName);
        }


        /// <summary>
        /// Common code between Edit supplier and distributor
        /// </summary>
        /// <param name="storeService"></param>
        /// <param name="order"></param>
        /// <param name="applicationId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private ActionResult ProcessCommand(IStoreService storeService, IFulfilmentService fulfilmentService, Order order, Guid applicationId, string command)
        {
            if (command == ApplicationController.COMMAND_ACCEPT)
            {
                //make sure we have external reference
                if (string.IsNullOrEmpty(order.ExternalReference)) throw new Exception("You need to specify a Timms id to approve an order");
                fulfilmentService.Process(order, GetApplication(applicationId));
                order.ProcessStatus = OrderStatus.Approved;
            }
            else if (command == ApplicationController.COMMAND_REJECT)
            {
                order.ProcessStatus = OrderStatus.Rejected;
            }
            StoreService.SaveChanges();
            if (command == ApplicationController.COMMAND_REJECT)
                return RedirectToAction("List", "Orders");
            else
                return RedirectToAction("Edit", "Application", new { id = applicationId, orderId = order.Id });
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

        public void SaveDecoratingTypesTo(SupplierApplicationModel applicationModel, SupplierMembershipApplication application)
        {
            // TODO save decorating types to the application
            List<SupplierDecoratingType> decoratingTypes = StoreService.GetAll<SupplierDecoratingType>(false).ToList();
            AddTypeToApplication(applicationModel.Etching, SupplierDecoratingType.DECORATION_ETCHING, application, decoratingTypes);
            AddTypeToApplication(applicationModel.HotStamping, SupplierDecoratingType.DECORATION_HOTSTAMPING, application, decoratingTypes);
            AddTypeToApplication(applicationModel.SilkScreen, SupplierDecoratingType.DECORATION_SILKSCREEN, application, decoratingTypes);
            AddTypeToApplication(applicationModel.PadPrint, SupplierDecoratingType.DECORATION_PADPRINT, application, decoratingTypes);
            AddTypeToApplication(applicationModel.DirectEmbroidery, SupplierDecoratingType.DECORATION_DIRECTEMBROIDERY, application, decoratingTypes);
            AddTypeToApplication(applicationModel.FoilStamping, SupplierDecoratingType.DECORATION_FOILSTAMPING, application, decoratingTypes);
            AddTypeToApplication(applicationModel.Lithography, SupplierDecoratingType.DECORATION_LITHOGRAPHY, application, decoratingTypes);
            AddTypeToApplication(applicationModel.Sublimination, SupplierDecoratingType.DECORATION_SUBLIMINATION, application, decoratingTypes);
            AddTypeToApplication(applicationModel.FourColourProcess, SupplierDecoratingType.DECORATION_FOURCOLOR, application, decoratingTypes);
            AddTypeToApplication(applicationModel.Engraving, SupplierDecoratingType.DECORATION_ENGRAVING, application, decoratingTypes);
            AddTypeToApplication(applicationModel.Laser, SupplierDecoratingType.DECORATION_LASER, application, decoratingTypes);
            AddTypeToApplication(applicationModel.Offset, SupplierDecoratingType.DECORATION_OFFSET, application, decoratingTypes);
            AddTypeToApplication(applicationModel.Transfer, SupplierDecoratingType.DECORATION_TRANSFER, application, decoratingTypes);
            AddTypeToApplication(applicationModel.FullColourProcess, SupplierDecoratingType.DECORATION_FULLCOLOR, application, decoratingTypes);
            AddTypeToApplication(applicationModel.DieStamp, SupplierDecoratingType.DECORATION_DIESTAMP, application, decoratingTypes);
        }

        private void AddTypeToApplication(bool selected, String typeName, SupplierMembershipApplication application, List<SupplierDecoratingType> decoratingTypes)
        {
            SupplierDecoratingType existing = application.DecoratingTypes.Where(type => type.Name == typeName).SingleOrDefault();
            if (selected && existing == null) application.DecoratingTypes.Add(decoratingTypes.Where(type => type.Name == typeName).SingleOrDefault());
            else if (!selected && existing != null) application.DecoratingTypes.Remove(existing);
        }

    }
}
