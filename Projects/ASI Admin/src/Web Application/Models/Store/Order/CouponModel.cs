using asi.asicentral.interfaces;
using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace asi.asicentral.model.store
{
    public class CouponModel
    {
        public int Id { get; set; }
        public bool IsProduct { get; set; }
        public int? ProductId { get; set; }
        public int? ContextId { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CouponCode { get; set; }
        [StringLength(1000, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string Description { get; set; }

        [RegularExpression(@"^[0-9]\d*(\.\d+)?$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        public decimal MonthlyCost { get; set; }
        [RegularExpression(@"^[0-9]\d*(\.\d+)?$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        public decimal AppFeeDiscount { get; set; }
        [RegularExpression(@"^[0-9]\d*(\.\d+)?$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        public decimal ProductDiscount { get; set; }
        public bool IsSubscription { get; set; }
        public bool IsFixedAmount { get; set; }
        [RegularExpression(@"^[0-9]\d*(\.\d+)?$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        public string DiscountAmount { get; set; }
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        public string DiscountPercentage { get; set; }
        [DataType(DataType.Date)]
        public DateTime ValidFrom { get; set; }
        [DataType(DataType.Date)]
        public DateTime ValidUpto { get; set; }

        public IList<SelectListItem> Products { get; set; }
        public IList<SelectListItem> Contexts { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public string ActionName { get; set; }
        public bool chkValidCoupon { get; set; }
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