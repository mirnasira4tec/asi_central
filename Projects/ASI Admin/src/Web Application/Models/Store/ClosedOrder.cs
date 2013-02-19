using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using asi.asicentral.model.store;

namespace asi.asicentral.web.Models.Store
{
    public class ClosedOrder
    {
        public ClosedOrder() 
        {
            Details = new List<Detail>();
            TotalAmount = 0; 
        }

        public int OrderId { get; set; }
        public String Name { get; set; }
        public String BillingAddress { get; set; }
        public String BillPhone { set; get; }
        public Nullable<DateTime> DateCreated { get; set; }
        public Nullable<Decimal> TotalAmount { get; set; }
        public List<Detail> Details { set; get; }

        public void GetDataFrom(Order order)
        {
            this.OrderId = order.Id;
            this.Name = order.BillFirstName + " " + order.BillLastName;
            String billingAddress;
            billingAddress = order.BillFirstName + " " + order.BillLastName + ", ";
            billingAddress += order.BillStreet1 + ", " + order.BillCity + ", " + order.BillState + " " + order.BillZip + " ";
            billingAddress += order.BillCountry;
            this.BillingAddress = billingAddress;
            this.BillPhone = order.BillPhone;
            
            if (order.DateCreated.HasValue) this.DateCreated = order.DateCreated;
            
            foreach (OrderDetail orderDetailItem in order.OrderDetails)
            {
                if (orderDetailItem.Subtotal.HasValue) 
                {
                    TotalAmount += orderDetailItem.Subtotal;
                    orderDetailItem.Subtotal = Math.Round(orderDetailItem.Subtotal.Value, 2);
                }
            }
            if (TotalAmount == null) TotalAmount = 0.00M;
            else TotalAmount = Math.Round(TotalAmount.Value, 2);
        }
    }
}