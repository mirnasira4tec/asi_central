﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreOrderDetail
    {
        public StoreOrderDetail()
        {
            if (this.GetType() == typeof(StoreOrderDetail))
            {
                MagazineSubscriptions = new List<StoreMagazineSubscription>();
            }
        }

        private decimal _cost;
        public int Id { get; set; }
        public int? LegacyProductId { get; set; }
        public int? CouponId { get; set; }
        public int Quantity { get; set; }
        public decimal ApplicationCost { get; set; }
        public decimal? DiscountedCost { get; set; }
        public decimal ShippingCost { get; set; }
        public string ShippingMethod { get; set; }
        public decimal TaxCost { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsSubscription { get; set; }
        public string AcceptedByName { get; set; }
        public int? OptionId { get; set; }
        public DateTime? DateOption { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public string Comments { get; set; }
        public decimal Cost
        {
            get { return _cost; }
            set
            {
                if( Coupon != null && Coupon.MonthlyCost.HasValue )
                {
                    _cost = Coupon.MonthlyCost.Value;
                }
                else
                {
                    _cost = value;
                }
            }
        }

        public virtual StoreOrder Order { get; set; }
        public virtual ContextProduct Product { get; set; }
        public virtual Coupon Coupon { get; set; }
        public virtual IList<StoreMagazineSubscription> MagazineSubscriptions { get; set; }

        public override string ToString()
        {
            return "OrderDetail (" + Id + ")";
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            StoreOrderDetail orderDetail = obj as StoreOrderDetail;
            if (orderDetail != null) equals = orderDetail.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
