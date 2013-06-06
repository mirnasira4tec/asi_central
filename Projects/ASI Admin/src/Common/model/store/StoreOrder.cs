﻿using System;
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
        Rejected = 2
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
        public bool IsCompleted { get; set; }
        public OrderStatus ProcessStatus { get; set; }
        public int CompletedStep { get; set; }
        public int? ContextId { get; set; }
        public string OrderRequestType { get; set; }
        public string Campaign { get; set; }
        public string ExternalReference { get; set; }
        public string IPAdd { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ApprovedBy { get; set; }
        public virtual StoreCompany Company { get; set; }
        public virtual StoreCreditCard CreditCard { get; set; }
        public virtual StoreIndividual BillingIndividual { get; set; }
        public virtual IList<StoreOrderDetail> OrderDetails { get; set; }
    }
}
