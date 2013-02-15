using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using asi.asicentral.model.store;

namespace asi.asicentral.web.Models.Store
{
    public class Detail
    {
        public int OrderId { set; get; }
        public int ProductId { set; get; }
        public String ProductName { set; get; }
        public Nullable<int> Quantity { set; get; }
        public Nullable<Decimal> Price { set; get; }
        public OrderDetailApplication Application { set; get; }
        public bool HasApplication { set; get; }

        public void GetDataFrom(OrderDetail orderDetail)
        {
            this.ProductId = orderDetail.ProductId;
            this.OrderId = orderDetail.OrderId;
            this.ProductName = orderDetail.Product.Description;
            this.Quantity = orderDetail.Quantity;
            this.Price = orderDetail.Subtotal;
        }
    }
}