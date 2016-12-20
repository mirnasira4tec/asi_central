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
        /// Creates an application based on the order detail information
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        StoreDetailApplication CreateApplication(StoreOrderDetail orderDetail, IBackendService backendService);

        /// <summary>
        /// Retrieves the application associated with the order detail
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns>The application if applicable, null otherwise</returns>
        StoreDetailApplication GetApplication(StoreOrderDetail orderDetail);

        /// <summary>
        /// UpdateTaxAndShipping
        /// </summary>
        /// <param name="order"></param>
        void UpdateTaxAndShipping(StoreOrder order);

        /// <summary>
        /// Calculates the taxes in case shipping to the USA
        /// </summary>
        /// <param name="info"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        decimal CalculateTaxes(StoreAddress address, decimal? amount);

        /// <summary>
        /// Retrieves the distributor membership application associated with the order detail
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns>The application if applicable, null otherwise</returns>
        [Obsolete]
        StoreDetailDistributorMembership GetDistributorApplication(StoreOrderDetail orderDetail);

        /// <summary>
        /// Retrieves the supplier membership application associated with the order detail
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns>The application if applicable, null otherwise</returns>
        [Obsolete]
        StoreDetailSupplierMembership GetSupplierApplication(StoreOrderDetail orderDetail);
    }
}
