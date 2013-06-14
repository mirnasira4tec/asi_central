using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    public class StoreService : ObjectService, IStoreService
    {
        public StoreService(IContainer container)
            : base(container)
        {
            //nothing to do right now
        }

        public override IQueryable<T> GetAll<T>(bool readOnly = false)
        {
            return base.GetAll<T>(readOnly);
        }

        public virtual LegacyDistributorMembershipApplication GetDistributorApplication(model.store.LegacyOrderDetail orderDetail)
        {
            LegacyDistributorMembershipApplication application = null;
            //103 hardcoded value coming from the legacy application representing a distributor membership application
            if (orderDetail.Order != null && orderDetail.Order.UserId != null && orderDetail.ProductId == LegacyOrderProduct.DISTRIBUTOR_APPLICATION)
            {
                LegacyOrder order = orderDetail.Order;
                IRepository<LegacyDistributorMembershipApplication> distributorRepository = GetRepository<LegacyDistributorMembershipApplication>();
                application = distributorRepository.GetAll().Where(app => app.UserId == order.UserId).SingleOrDefault();
            }
            return application;
        }

        public virtual model.store.LegacySupplierMembershipApplication GetSupplierApplication(model.store.LegacyOrderDetail orderDetail)
        {
            LegacySupplierMembershipApplication application = null;
            //102 hardcoded value coming from the legacy application representing a supplier membership application
            if (orderDetail.Order != null && orderDetail.Order.UserId != null && orderDetail.ProductId == LegacyOrderProduct.SUPPLIER_APPLICATION)
            {
                LegacyOrder order = orderDetail.Order;
                IRepository<LegacySupplierMembershipApplication> supplierRepository = GetRepository<LegacySupplierMembershipApplication>();
                application = supplierRepository.GetAll().Where(app => app.UserId == order.UserId).SingleOrDefault();
            }
            return application;
        }

        public LegacyOrderDetailApplication GetApplication(LegacyOrderDetail orderDetail)
        {
            LegacyOrderDetailApplication application = null;
            if (orderDetail.Order != null && orderDetail.Order.UserId != null)
            {
                switch (orderDetail.ProductId)
                {
                    case LegacyOrderProduct.DISTRIBUTOR_APPLICATION:
                        return GetDistributorApplication(orderDetail);
                    case LegacyOrderProduct.SUPPLIER_APPLICATION:
                        return GetSupplierApplication(orderDetail);
                }
            }
            return application;
        }
    }
}
