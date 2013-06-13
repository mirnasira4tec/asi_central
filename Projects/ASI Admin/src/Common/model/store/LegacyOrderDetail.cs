using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public class LegacyOrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<System.DateTime> Added { get; set; }
        public string Application { get; set; }
        public Nullable<decimal> TaxSubtotal { get; set; }
        public Nullable<decimal> PreTaxSubtotal { get; set; }
        public Nullable<decimal> Shipping { get; set; }
        public Nullable<decimal> Subtotal { get; set; }
        public Nullable<bool> HallmarkResult { get; set; }
        public string ExternalReference { get; set; }

        public virtual LegacyOrder Order { get; set; }

        public virtual LegacyOrderProduct Product { get; set; }

        public override string ToString()
        {
            return string.Format("Order Detail of {0} for {1}", OrderId, ProductId);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            LegacyOrderDetail orderDetail = obj as LegacyOrderDetail;
            if (orderDetail != null) equals = (orderDetail.OrderId == OrderId && orderDetail.ProductId == ProductId);
            return equals;
        }

        public override int GetHashCode()
        {
            return OrderId.GetHashCode() * 47 + ProductId.GetHashCode();
        }
    }
}
