using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public enum OrderStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        PersonifyError = 3
    }

    public class StoreOrder
    {
        public StoreOrder()
        {
            if (this.GetType() == typeof(StoreOrder))
            {
                OrderDetails = new List<StoreOrderDetail>();
            }
        }

        public int Id { get; set; }
        public int? LegacyId { get; set; }
        public string UserId { get; set; }
        public bool IsCompleted { get; set; }
        public OrderStatus ProcessStatus { get; set; }
        public int CompletedStep { get; set; }
        public int? ContextId { get; set; }
        public bool IsStoreRequest { get; set; }
        public string OrderRequestType { get; set; }
        public string Campaign { get; set; }
        public string ExternalReference { get; set; }
        public string UserReference { get; set; }
        public string LoggedUserEmail { get; set; }
        public string IPAdd { get; set; }
        public decimal Total { get; set; }
        public decimal AnnualizedTotal { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedBy { get; set; }
        public virtual StoreCompany Company { get; set; }
        public virtual StoreCreditCard CreditCard { get; set; }
        public virtual StoreIndividual BillingIndividual { get; set; }
        public virtual IList<StoreOrderDetail> OrderDetails { get; set; }
        public virtual Context Context { get; set; }
        public string ProductName
        {
            get
            {
                if (OrderDetails != null && OrderDetails.FirstOrDefault().Order != null && OrderDetails.FirstOrDefault().Order.ContextId != null && OrderDetails.FirstOrDefault().Order.Context != null)
                {

                    if (OrderDetails.FirstOrDefault().Order.Context.Type != "Product")
                    {
                        return OrderDetails.FirstOrDefault().Order.Context.Name;

                    }
                    else
                    {
                        if (Context.Type.StartsWith("SGR")) return "SGR Membership";
                        else return Context.Type + " Membership";
                    }
                }
                else if (OrderDetails != null && OrderDetails.Where(detail => detail.Product != null).FirstOrDefault() != null) return OrderDetails.Where(detail => detail.Product != null).FirstOrDefault().Product.Name;
                else return "(Unknown)";
            }
        }
        public string CouponCode
        {
            get
            {
                if (OrderDetails != null && OrderDetails.Where(detail => detail.Coupon != null).FirstOrDefault() != null) return OrderDetails.Where(detail => detail.Coupon != null).FirstOrDefault().Coupon.CouponCode;
                else return "(Unknown)";
            }
        }
        public string ConfirmationNumber
        {
            get { return string.Format("{0:#00000}", Id); }
        }
        public override string ToString()
        {
            return "Order (" + Id + ")";
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            StoreOrder order = obj as StoreOrder;
            if (order != null) equals = (order.Id == Id && ((LegacyId.HasValue && LegacyId == order.LegacyId) || !LegacyId.HasValue));
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Gets the more realistic contact for the order
        /// </summary>
        /// <returns></returns>
        public StoreIndividual GetContact()
        {
            StoreIndividual contact = null;
            if (Company != null) contact = Company.Individuals.Where(ctct => ctct.IsPrimary).FirstOrDefault();
            if (contact == null && BillingIndividual != null) contact = BillingIndividual;
            return contact;
        }
    }
}
