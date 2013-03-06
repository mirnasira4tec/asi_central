using asi.asicentral.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class OrderDetailApplication
    {
        public System.Guid Id { get; set; }
        public System.Guid UserId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanyName")]
        public string Company { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "Street1")]
        public string Address1 { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "Street2")]
        public string Address2 { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "City")]
        public string City { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "Zipcode")]
        public string Zip { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "State")]
        public string State { get; set; }
        public string Country { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "Phone")]
        public string Phone { get; set; }
        public string InternationalPhone { get; set; }
        public bool HasShipAddress { get; set; }
        public bool HasBillAddress { get; set; }

        #region Billing information

        [Display(ResourceType = typeof(Resource), Name = "Street1")]
        public string BillingAddress1 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Street2")]
        public string BillingAddress2 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "City")]
        public string BillingCity { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "State")]
        public string BillingState { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Zipcode")]
        public string BillingZip { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Phone")]
        public string BillingPhone { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Fax")]
        public string BillingFax { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Email")]
        public string BillingEmail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "WebUrl")]
        public string BillingWebUrl { get; set; }

        #endregion Billing information


        #region shipping information

        [Display(ResourceType = typeof(Resource), Name = "ShippingAddress")]
        public string ShippingStreet1 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ShippingAddress2")]
        public string ShippingStreet2 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ShippingCity")]
        public string ShippingCity { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ShippingState")]
        public string ShippingState { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ShippingZip")]
        public string ShippingZip { get; set; }

        #endregion shipping information

        public override string ToString()
        {
            return string.Format( this.GetType().Name + ": {0}", Id);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            OrderDetailApplication orderApplication = obj as OrderDetailApplication;
            if (orderApplication != null) equals = orderApplication.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
