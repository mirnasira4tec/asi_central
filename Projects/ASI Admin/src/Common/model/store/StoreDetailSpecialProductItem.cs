﻿using asi.asicentral.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace asi.asicentral.model.store
{
    public class StoreDetailSpecialProductItem
    {
        public int Id { get; set; }
        public virtual StoreOrderDetail OrderDetail { get; set; }
        public int? OrderDetailId { get; set; }
        public int Sequence { get; set; }
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string ServiceType { get; set; }
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string ServiceTypeOther { get; set; }
		public string ASIContactName { get; set; }
        public string ASIContactEmail { get; set; }
        public string CustomerEmail { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal OfferPrice { get; set; }
        public bool IsSetupCharges { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
