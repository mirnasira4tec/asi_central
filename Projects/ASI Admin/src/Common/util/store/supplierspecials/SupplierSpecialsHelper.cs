using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using asi.asicentral.Resources;

namespace asi.asicentral.util.store
{
    public class SupplierSpecialsHelper
    {
        public static readonly decimal[] SupplierSpecials_Cost = { 450, 650 };

        public static IList<SelectListItem> GetPackagesOptions()
        {
            IList<SelectListItem> quantityOptions = new List<SelectListItem>();
            quantityOptions.Add(new SelectListItem() { Text = Resource.AllProducts, Value = "0" });
            quantityOptions.Add(new SelectListItem() { Text = Resource.UptoTen, Value = "1" });
            return quantityOptions;
        }

        public static decimal GetCost(int optionId)
        {
            return SupplierSpecials_Cost[optionId];
        }
    }
}
