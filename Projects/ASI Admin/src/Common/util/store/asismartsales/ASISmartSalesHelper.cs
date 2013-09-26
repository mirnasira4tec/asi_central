using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace asi.asicentral.util.store
{
    public class ASISmartSalesHelper
    {
        public static readonly decimal[] DISTRIBUTOR_MAGAZINE_PRODUCT_IDS = { 35, 35, 30, 25, 35 };

        public static IList<SelectListItem> GetCurrentMembershipOptions(string value = null)
        {
            IList<SelectListItem> membershipOptions = new List<SelectListItem>();
            membershipOptions.Add(new SelectListItem() { Text = Resource.Membership, Value = "0", Selected = ("0" == value) });
            membershipOptions.Add(new SelectListItem() { Text = Resource.Basic, Value = "1", Selected = ("1" == value) });
            membershipOptions.Add(new SelectListItem() { Text = Resource.Standard, Value = "2", Selected = ("2" == value) });
            membershipOptions.Add(new SelectListItem() { Text = Resource.Executive, Value = "3", Selected = ("3" == value) });
            membershipOptions.Add(new SelectListItem() { Text = Resource.NA, Value = "4", Selected = ("4" == value) });
            return membershipOptions;
        }

        public static decimal GetCost(int optionId)
        {
            return DISTRIBUTOR_MAGAZINE_PRODUCT_IDS[optionId];
         }
    }
}
