using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public class DistributorMembershipApplication : OrderDetailApplication
    {
        public Nullable<int> ApplicationStatusId { get; set; }
        public string Company { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string WebUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ShippingStreet1 { get; set; }
        public string ShippingStreet2 { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingZip { get; set; }
        public Nullable<int> NumberOfEmployee { get; set; }
        public Nullable<int> NumberOfSalesEmployee { get; set; }
        public string AnnualSalesVolume { get; set; }
        public string ASIContact { get; set; }
        public string AnnualSalesVolumeASP { get; set; }
        public Nullable<bool> CorporateOfficer { get; set; }
        public Nullable<int> SignatureType { get; set; }
        public Nullable<bool> IsMajorForResale { get; set; }
        public Nullable<bool> IsForProfit { get; set; }
        public Nullable<bool> ProvideInvoiceOnDemand { get; set; }
        public Nullable<bool> IsSolelyWork { get; set; }
        public string SolelyWorkName { get; set; }
        public Nullable<bool> InformASIOfChange { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantEmail { get; set; }
        public Nullable<bool> TrueAnswers { get; set; }
        public Nullable<bool> AgreeReceivePromotionalProducts { get; set; }
        public Nullable<bool> AgreeTermsAndConditions { get; set; }
        public Nullable<bool> IsMajorityDistributeForResale { get; set; }
        public string IPAddress { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string Custom4 { get; set; }
        public string Custom5 { get; set; }
        public Nullable<int> PrimaryBusinessRevenueId { get; set; }
        public string OtherBusinessRevenue { get; set; }
    }
}
