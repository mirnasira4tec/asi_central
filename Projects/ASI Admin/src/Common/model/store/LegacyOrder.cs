using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public class LegacyOrder
    {
        public LegacyOrder()
        {
            if (this.GetType() == typeof(LegacyOrder))
            {
                this.OrderDetails = new List<LegacyOrderDetail>();
                this.DistributorAddresses = new List<LegacyOrderDistributorAddress>();
                this.Addresses = new List<LegacyOrderAddress>();
            }
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
        public int? ContextId { get; set; }

        public virtual LegacyOrderCreditCard CreditCard { get; set; }
        public virtual ASPNetMembership Membership { get; set; }
        public virtual ICollection<LegacyOrderDetail> OrderDetails { get; set; }
        public virtual IList<LegacyOrderDistributorAddress> DistributorAddresses { get; set; }
        public virtual IList<LegacyOrderAddress> Addresses { get; set; }

        public override string ToString()
        {
            return string.Format("Order: {0}", Id);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            LegacyOrder order = obj as LegacyOrder;
            if (order != null) equals = order.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public OrderStep GetOrderCompletedStep()
        {
            switch (CompletedStep)
            {
                case 1:
                    return OrderStep.ProductInfo;
                case 2:
                    return OrderStep.Initial;
                case 3:
                    return OrderStep.BillingInfo;
                case 4:
                    return OrderStep.PlaceOrder;
                case 5:
                    return OrderStep.PlaceOrder;
                default:
                    return OrderStep.Initial;
            }
        }
    }
}
