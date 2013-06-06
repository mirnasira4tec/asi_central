using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreOrderDetail
    {
        public int Id { get; set; }
        public int? LegacyProductId { get; set; }
        public int Quantity { get; set; }
        public decimal ApplicationCost { get; set; }
        public decimal Cost { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxCost { get; set; }
        public bool IsSubscription { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public virtual StoreOrder Order { get; set; }
        public virtual ContextProduct Product { get; set; }
    }
}
