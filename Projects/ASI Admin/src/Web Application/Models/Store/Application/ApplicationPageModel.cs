using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.model.store;
using asi.asicentral.interfaces;

namespace asi.asicentral.web.Models.Store.Application
{
    public class ApplicationPageModel
    {
        public const String ACCEPT = "accept";
        public const String REJECT = "reject";
        public Order order { set; get; }
        public SupplierMembershipApplication supplierApplication { set; get; }
        public DistributorMembershipApplication distributorApplication { set; get; }
        
        public ApplicationPageModel(IStoreService StoreService, OrderDetailApplication application)
        {
            this.order = StoreService.GetAll<Order>(false).Where
                (theOrder => theOrder.Application.UserId == application.UserId).SingleOrDefault();

            if (this.order == null) throw new Exception("");

            if (application != null && application is SupplierMembershipApplication)
                supplierApplication = application as SupplierMembershipApplication;

            if (application != null && application is DistributorMembershipApplication)
                distributorApplication = application as DistributorMembershipApplication;
        }
    }
}