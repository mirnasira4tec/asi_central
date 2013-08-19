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
        StoreDetailApplication CreateApplication(StoreOrderDetail orderDetail);

        /// <summary>
        /// Retrieves the application associated with the order detail
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns>The application if applicable, null otherwise</returns>
        StoreDetailApplication GetApplication(StoreOrderDetail orderDetail);

        /// <summary>
        /// Provide the Product Shipping cost
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        decimal GetShippingCost(ContextProduct product, string country, string shippingMethod = null, int quantity = 1);

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
