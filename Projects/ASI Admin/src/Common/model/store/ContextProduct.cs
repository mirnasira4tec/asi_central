using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace asi.asicentral.model.store
{
    public class ContextProduct
    {
        public ContextProduct()
        {
            if (this.GetType() == typeof(ContextProduct))
            {
                Features = new List<ContextFeatureProduct>();
            }
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public decimal ApplicationCost { get; set; }
        public decimal ShippingCostUS { get; set; }
        public decimal ShippingCostOther { get; set; }
        public decimal? DiscountedCost { get; set; }
        public bool HasTax { get; set; }
        public bool HasShipping { get; set; }
        public bool IsSubscription { get; set; }
        public string NotificationEmails { get; set; }
		public bool HasBackendNotification { get; set; }
		public string SubscriptionFrequency { get; set; }
        public bool IsASINumberFlag { get; set; }
		public bool IsASINumberOptionalFlag { get; set; }
		public bool HasBackEndIntegration { get; set; }
        public bool IsAvailable { get; set; }
        public bool HasBankInformation { get; set; }
        public string ChatSettings { get; set; }
        public string ConversionSettings { get; set; }
        [DataType(DataType.Date)]
        public DateTime? NextAvailableDate { get; set; }
        public decimal? Weight { get; set; }
        public string Origin { get; set; }
		public string ASICompany { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
     
        public virtual ICollection<ContextFeatureProduct> Features { get; set; }

        public bool IsMembership()
        {
            return !string.IsNullOrEmpty(Name) && Name.ToLower().Contains("membership") || 
                   !string.IsNullOrEmpty(Type) && Type.ToLower().Contains("membership");
        }

        public override string ToString()
        {
            return string.Format("Product: {0} - {1}", Id, Name);
        }

        public override bool Equals(object obj)
        {
            var equals = false;

            var product = obj as ContextProduct;
            if (product != null) equals = product.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
