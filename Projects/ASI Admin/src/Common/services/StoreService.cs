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

        public virtual StoreDetailDistributorMembership GetDistributorApplication(model.store.StoreOrderDetail orderDetail)
        {
            StoreDetailDistributorMembership application = null;
            if (orderDetail.Product != null && StoreDetailDistributorMembership.Identifiers.Contains(orderDetail.Product.Id))
            {
                IRepository<StoreDetailDistributorMembership> distributorRepository = GetRepository<StoreDetailDistributorMembership>();
                application = distributorRepository.GetAll().Where(app => app.OrderDetailId == orderDetail.Id).SingleOrDefault();
            }
            return application;
        }

        public virtual model.store.StoreDetailSupplierMembership GetSupplierApplication(model.store.StoreOrderDetail orderDetail)
        {
            StoreDetailSupplierMembership application = null;
            if (orderDetail.Product != null && StoreDetailSupplierMembership.Identifiers.Contains(orderDetail.Product.Id))
            {
                IRepository<StoreDetailSupplierMembership> supplierRepository = GetRepository<StoreDetailSupplierMembership>();
                application =  supplierRepository.GetAll().Where(app => app.OrderDetailId == orderDetail.Id).SingleOrDefault();
            }
            return application;
        }

        public StoreDetailApplication GetApplication(StoreOrderDetail orderDetail)
        {
            StoreDetailApplication application = null;
            if (orderDetail.Product != null)
            {
                if (StoreDetailSupplierMembership.Identifiers.Contains(orderDetail.Product.Id)) return GetSupplierApplication(orderDetail);
                else if (StoreDetailDistributorMembership.Identifiers.Contains(orderDetail.Product.Id)) return GetDistributorApplication(orderDetail);
            }
            return application;
        }
    }
}
