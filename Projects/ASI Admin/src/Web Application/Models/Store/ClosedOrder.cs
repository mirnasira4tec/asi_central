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
        public DateTime DateCreate { get; set; }
        public String BillingAddress { get; set; }
        public String BillPhone { set; get; }
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

            foreach (OrderDetail orderDetailItem in order.OrderDetails)
            {
                TotalAmount += orderDetailItem.Subtotal;
            }
            if (TotalAmount == null) TotalAmount = 0.00M;
        }
    }
}