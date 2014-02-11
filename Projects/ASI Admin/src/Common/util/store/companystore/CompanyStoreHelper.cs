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
        public static readonly decimal[] CompanyStore_Cost = {29,99} ;

        public static IList<SelectListItem> GetQuantityOptions()
        {
            IList<SelectListItem> quantityOptions = new List<SelectListItem>();
            quantityOptions.Add(new SelectListItem() { Text = "1 License", Value = "0" });
            quantityOptions.Add(new SelectListItem() { Text = "10 Licenses", Value = "1" });
            return quantityOptions;
        }

        public static decimal GetCost(int optionId)
        {
            return CompanyStore_Cost[optionId];
        }
    }
}
