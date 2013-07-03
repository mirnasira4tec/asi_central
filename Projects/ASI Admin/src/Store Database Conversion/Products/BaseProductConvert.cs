using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store_Database_Conversion.Products
{
    abstract class BaseProductConvert : IProductConvert
    {
        public bool IgnoreOrderIssues(LegacyOrderDetail detail)
        {
            bool ignore = (detail.Order.Status.HasValue && !detail.Order.Status.Value);
            ignore = ignore || (detail.Order.Status.HasValue && detail.Order.Status.Value && (detail.Order.BillCity == null || detail.Order.BillLastName == null));
            return ignore;
        }

        public abstract void Convert(StoreOrderDetail newOrderDetail, LegacyOrderDetail detail, asi.asicentral.database.StoreContext storeContext, asi.asicentral.database.ASIInternetContext asiInternetContext);
    }
}
