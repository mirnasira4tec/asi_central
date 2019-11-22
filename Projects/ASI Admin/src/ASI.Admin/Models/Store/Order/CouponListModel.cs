using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.model.store
{
    public class CouponListModel
    {
        public string CouponCode { get; set; }
        public bool ShowValidOnly { get; set; }
        public IList<Coupon> Coupons { set; get; }
        public string MemberType { set; get; }
        public static IList<SelectListItem> GetMemberTypes()
        {
            IList<SelectListItem> selItems = new List<SelectListItem>();
            selItems.Add(new SelectListItem() { Selected = true, Text = "Select ", Value = "" });
            selItems.Add(new SelectListItem() { Selected = false, Text = "Distributor", Value = "Distributor Membership" });
            selItems.Add(new SelectListItem() { Selected = false, Text = "Supplier", Value = "Supplier Membership" });
            selItems.Add(new SelectListItem() { Selected = false, Text = "Decorator", Value = "Decorator Membership" });
            selItems.Add(new SelectListItem() { Selected = false, Text = "Others", Value = "Others" });
            return selItems;
        }
    }
}