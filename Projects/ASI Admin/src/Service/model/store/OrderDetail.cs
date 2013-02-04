using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProdId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<System.DateTime> Added { get; set; }
        public string Application { get; set; }
        public Nullable<decimal> Subtotal { get; set; }
        public Nullable<bool> HallmarkResult { get; set; }

        public virtual Order Order { get; set; }
        public virtual OrderProduct Product { get; set; }

        public override string ToString()
        {
            return string.Format("Order Detail of {0}", OrderId);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            OrderDetail orderDetail = obj as OrderDetail;
            if (orderDetail != null) equals = (orderDetail.OrderId == OrderId && orderDetail.ProdId == ProdId);
            return equals;
        }

        public override int GetHashCode()
        {
            return OrderId.GetHashCode() * 47 + ProdId.GetHashCode();
        }
    }
}
