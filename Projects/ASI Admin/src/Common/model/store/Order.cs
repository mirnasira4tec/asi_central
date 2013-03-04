using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public enum OrderStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }

    public class Order
    {
        public Order()
        {
            this.OrderDetails = new List<OrderDetail>();
        }

        public int Id { get; set; }
        public Nullable<System.Guid> UserId { get; set; }
        public Nullable<System.Guid> TransId { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string BillFirstName { get; set; }
        public string BillLastName { get; set; }
        public string BillStreet1 { get; set; }
        public string BillStreet2 { get; set; }
        public string BillCity { get; set; }
        public string BillState { get; set; }
        public string BillZip { get; set; }
        public string BillCountry { get; set; }
        public string BillPhone { get; set; }
        public Nullable<bool> Status { get; set; }
        public OrderStatus ProcessStatus { get; set; }
        public string IPAdd { get; set; }
        public Nullable<int> OrderTypeId { get; set; }
        public string ExternalReference { get; set; }
        public int CompletedStep { get; set; }
        public string Campaign { get; set; }

        public virtual OrderCreditCard CreditCard { get; set; }
        public virtual ASPNetMembership Membership { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public override string ToString()
        {
            return string.Format("Order: {0}", Id);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            Order order = obj as Order;
            if (order != null) equals = order.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
