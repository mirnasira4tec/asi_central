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
        public static IList<SelectListItem> GetCurrentMembershipOptions(string value = null)
        {
            IList<SelectListItem> membershipOptions = new List<SelectListItem>();
            membershipOptions.Add(new SelectListItem() { Text = Resource.Basic, Value = "35", Selected = ("35" == value) });
            membershipOptions.Add(new SelectListItem() { Text = Resource.Standard, Value = "30", Selected = ("30" == value) });
            membershipOptions.Add(new SelectListItem() { Text = Resource.Executive, Value = "25", Selected = ("25" == value) });
            membershipOptions.Add(new SelectListItem() { Text = Resource.Professional, Value = "35", Selected = ("35" == value) });
            membershipOptions.Add(new SelectListItem() { Text = Resource.NA, Value = "35", Selected = ("35" == value) });
            return membershipOptions;
        }
    }
}
