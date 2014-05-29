using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using asi.asicentral.Resources;

namespace asi.asicentral.util.store
{
    public class CompanyStoreHelper
    {
        public static readonly decimal[] CompanyStore_Cost = {49,99} ;

        public static IList<SelectListItem> GetQuantityOptions()
        {
            IList<SelectListItem> quantityOptions = new List<SelectListItem>();
            quantityOptions.Add(new SelectListItem() { Text = "1 License", Value = "1" });
            quantityOptions.Add(new SelectListItem() { Text = "2 Licenses", Value = "2" });
            quantityOptions.Add(new SelectListItem() { Text = "3 Licenses", Value = "3" });
            quantityOptions.Add(new SelectListItem() { Text = "4 Licenses", Value = "4" });
            quantityOptions.Add(new SelectListItem() { Text = "5 Licenses", Value = "5" });
            return quantityOptions;
        }

        public static decimal GetCost(int optionId)
        {
            return CompanyStore_Cost[optionId];
        }
    }
}
