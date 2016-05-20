using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class Coupon
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? ContextId { get; set; }
        public string CouponCode { get; set; }
        public string Description { get; set; }
        public bool IsSubscription { get; set; }
        public bool IsFixedAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public int DiscountPercentage { get; set; }
        public decimal MonthlyCost { get; set; }
        public decimal AppFeeDiscount { get; set; }
        public decimal ProductDiscount { get; set; }
        [DataType(DataType.Date)]
        public DateTime ValidFrom { get; set; }
        [DataType(DataType.Date)]
        public DateTime ValidUpto { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public virtual Context Context { get; set; }
        public virtual ContextProduct Product { get; set; }
    }
}
