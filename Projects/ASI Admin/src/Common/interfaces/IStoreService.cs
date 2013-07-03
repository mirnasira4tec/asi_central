using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.interfaces
{
    public interface IStoreService : IObjectService
    {
        /// <summary>
        /// Retrieves the application associated with the order detail
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns>The application if applicable, null otherwise</returns>
        LegacyOrderDetailApplication GetApplication(LegacyOrderDetail orderDetail);

        /// <summary>
        /// Retrieves the distributor membership application associated with the order detail
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns>The application if applicable, null otherwise</returns>
        LegacyDistributorMembershipApplication GetDistributorApplication(LegacyOrderDetail orderDetail);

        /// <summary>
        /// Retrieves the supplier membership application associated with the order detail
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns>The application if applicable, null otherwise</returns>
        LegacySupplierMembershipApplication GetSupplierApplication(LegacyOrderDetail orderDetail);
    }
}
