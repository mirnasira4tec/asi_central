using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asi.asicentral.interfaces;
using System.Web.Mvc;
using asi.asicentral.Resources;

namespace asi.asicentral.util.store
{
    public class TrafficBuilderHelper
    {
        public static string GetProductName(int price)
        {
            string productName = string.Empty;
            if (price >= 200 && price <= 499) {
                productName = "Standard";
            }
            else if (price > 499 && price <= 999) {
                productName = "Pro";
            }
            else if (price > 999)
            {
                productName = "Platinum";
            }
            return productName;
        }
    }
}
