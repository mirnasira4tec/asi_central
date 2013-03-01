using asi.asicentral.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace asi.asicentral.model.store
{
    public class DistributorMembershipApplication : OrderDetailApplication
    {
        [Display(ResourceType = typeof(Resource), Name = "ApplicationStatus")]
        public Nullable<int> ApplicationStatusId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Street1")]
        public string Street1 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Street2")]
        public string Street2 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "City")]
        public string City { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "State")]
        public string State { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Zipcode")]
        public string Zip { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Phone")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Fax")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "WebUrl")]
        public string WebUrl { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "LastName")]
        public string LastName { get; set; }

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

        [Display(ResourceType = typeof(Resource), Name = "NumberOfEmployee")]
        public Nullable<int> NumberOfEmployee { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NumberOfSalesEmployee")]
        public Nullable<int> NumberOfSalesEmployee { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AnnualSalesVolume")]
        public string AnnualSalesVolume { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ASIContact")]
        public string ASIContact { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AnnualSalesVolumeASP")]
        public string AnnualSalesVolumeASP { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CorporateOfficer")]
        public Nullable<bool> CorporateOfficer { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SignatureType")]
        public Nullable<int> SignatureType { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsMajorForResale")]
        public Nullable<bool> IsMajorForResale { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsForProfit")]
        public Nullable<bool> IsForProfit { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ProvideInvoiceOnDemand")]
        public Nullable<bool> ProvideInvoiceOnDemand { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsSolelyWork")]
        public Nullable<bool> IsSolelyWork { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SolelyWorkName")]
        public string SolelyWorkName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "InformASIOfChange")]
        public Nullable<bool> InformASIOfChange { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ApplicantName")]
        public string ApplicantName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ApplicantEmail")]
        public string ApplicantEmail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "TrueAnswers")]
        public Nullable<bool> TrueAnswers { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AgreeReceivePromotionalProducts")]
        public Nullable<bool> AgreeReceivePromotionalProducts { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AgreeTermsAndConditions")]
        public Nullable<bool> AgreeTermsAndConditions { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsMajorityDistributeForResale")]
        public Nullable<bool> IsMajorityDistributeForResale { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IPAddress")]
        public string IPAddress { get; set; }

        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string Custom4 { get; set; }
        public string Custom5 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IPAddress")]
        public Nullable<int> PrimaryBusinessRevenueId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "OtherBusinessRevenue")]
        public string OtherBusinessRevenue { get; set; }

        public string AccountTypes { get; set; }

        public string ProductLines { get; set; }
        public Nullable<DateTime> EstablishedDate { get; set; }

        public virtual ICollection<DistributorMembershipApplicationContact> Contacts { get; set; }
    }
}
