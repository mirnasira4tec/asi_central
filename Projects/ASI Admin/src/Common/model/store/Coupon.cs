using System;
using System.Collections.Generic;
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
        public bool IsSubscription { get; set; }
        public bool IsFixedAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUpto { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public virtual Context Context { get; set; }
        public virtual ContextProduct Product { get; set; }
    }
}
