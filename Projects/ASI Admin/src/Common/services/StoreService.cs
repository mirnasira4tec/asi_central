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

        public virtual DistributorMembershipApplication GetDistributorApplication(model.store.OrderDetail orderDetail)
        {
            DistributorMembershipApplication application = null;
            //103 hardcoded value coming from the legacy application representing a distributor membership application
            if (orderDetail.Order != null && orderDetail.Order.UserId != null && orderDetail.ProductId == OrderProduct.DISTRIBUTOR_APPLICATION)
            {
                Order order = orderDetail.Order;
                IRepository<DistributorMembershipApplication> distributorRepository = GetRepository<DistributorMembershipApplication>();
                application = distributorRepository.GetAll().Where(app => app.UserId == order.UserId).SingleOrDefault();
            }
            return application;
        }

        public virtual model.store.SupplierMembershipApplication GetSupplierApplication(model.store.OrderDetail orderDetail)
        {
            SupplierMembershipApplication application = null;
            //102 hardcoded value coming from the legacy application representing a supplier membership application
            if (orderDetail.Order != null && orderDetail.Order.UserId != null && orderDetail.ProductId == OrderProduct.SUPPLIER_APPLICATION)
            {
                Order order = orderDetail.Order;
                IRepository<SupplierMembershipApplication> supplierRepository = GetRepository<SupplierMembershipApplication>();
                application = supplierRepository.GetAll().Where(app => app.UserId == order.UserId).SingleOrDefault();
            }
            return application;
        }

        public OrderDetailApplication GetApplication(OrderDetail orderDetail)
        {
            OrderDetailApplication application = null;
            if (orderDetail.Order != null && orderDetail.Order.UserId != null)
            {
                switch (orderDetail.ProductId)
                {
                    case OrderProduct.DISTRIBUTOR_APPLICATION:
                        return GetDistributorApplication(orderDetail);
                    case OrderProduct.SUPPLIER_APPLICATION:
                        return GetSupplierApplication(orderDetail);
                }
            }
            return application;
        }
    }
}
