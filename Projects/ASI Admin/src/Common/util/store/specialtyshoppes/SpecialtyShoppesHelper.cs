using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using asi.asicentral.Resources;

namespace asi.asicentral.util.store
{
    public class SpecialtyShoppesHelper
    {
        public const int SPECIALTY_SHOPPES = 112;
        public static readonly decimal[] SpecialtyShoppes_Cost = { 49, 99 };

        public static IList<SelectListItem> GetQuantityOptions()
        {
            IList<SelectListItem> quantityOptions = new List<SelectListItem>();
            quantityOptions.Add(new SelectListItem() { Text = "Healthcare Store", Value = "1" });
            quantityOptions.Add(new SelectListItem() { Text = "Education Store", Value = "2" });
            quantityOptions.Add(new SelectListItem() { Text = "Finance Store", Value = "3" });
            quantityOptions.Add(new SelectListItem() { Text = "Construction Store", Value = "4" });
            quantityOptions.Add(new SelectListItem() { Text = "Technology Store", Value = "5" });
            quantityOptions.Add(new SelectListItem() { Text = "5 Pack of Stores", Value = "6" });
            return quantityOptions;
        }

        public static decimal GetCost(int optionId)
        {
            return SpecialtyShoppes_Cost[optionId];
        }

        public static bool IsBonus(decimal total, int option)
        {
            if (option != 6 && total != SpecialtyShoppes_Cost[0]) return true;
            if (option == 6 && total != SpecialtyShoppes_Cost[1]) return true;
            return false;
        }
    }
}
