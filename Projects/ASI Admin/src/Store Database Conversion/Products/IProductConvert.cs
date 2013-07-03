using asi.asicentral.database;
using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store_Database_Conversion
{
    interface IProductConvert
    {
        void Convert(StoreOrderDetail newOrderDetail, LegacyOrderDetail detail, StoreContext storeContext, ASIInternetContext asiInternetContext);
    }
}
