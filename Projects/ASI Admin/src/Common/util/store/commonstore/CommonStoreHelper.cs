using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using asi.asicentral.Resources;

namespace asi.asicentral.util.store
{
    public class CommonStoreHelper
    {
        public static readonly decimal[] CommonStore_Cost = {29,99} ;

        public static IList<SelectListItem> GetQuantityOptions()
        {
            IList<SelectListItem> quantityOptions = new List<SelectListItem>();
            quantityOptions.Add(new SelectListItem() { Text = "1", Value = "1" });
            quantityOptions.Add(new SelectListItem() { Text = "10", Value = "2" });
            return quantityOptions;
        }

        public static decimal GetCost(int optionId)
        {
            return CommonStore_Cost[optionId-1];
        }
    }
}
