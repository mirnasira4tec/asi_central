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

        public void CopyTo(DistributorMembershipApplication application)
        {
            application.AccountTypes = AccountTypes;
            application.AgreeReceivePromotionalProducts = AgreeReceivePromotionalProducts;
            application.AgreeTermsAndConditions = AgreeTermsAndConditions;
            application.AnnualSalesVolume = AnnualSalesVolume;
            application.AnnualSalesVolumeASP = AnnualSalesVolumeASP;
            application.ApplicantEmail = ApplicantEmail;
            application.ApplicantName = ApplicantName;
            application.ApplicationStatusId = ApplicationStatusId;
            application.ASIContact = ASIContact;
            application.City = City;
            application.Company = Company;
            application.Contacts = Contacts;
            application.CorporateOfficer = CorporateOfficer;
            application.Custom1 = Custom1;
            application.Custom2 = Custom2;
            application.Custom3 = Custom3;
            application.Custom4 = Custom4;
            application.Custom5 = Custom5;
            application.Email = Email;
            application.EstablishedDate = EstablishedDate;
            application.Fax = Fax;
            application.FirstName = FirstName;
            application.Id = Id;
            application.InformASIOfChange = InformASIOfChange;
            application.IPAddress = IPAddress;
            application.IsForProfit = IsForProfit;
            application.IsMajorForResale = IsMajorForResale;
            application.IsMajorityDistributeForResale = IsMajorityDistributeForResale;
            application.IsSolelyWork = IsSolelyWork;
            application.LastName = LastName;
            application.NumberOfEmployee = NumberOfEmployee;
            application.NumberOfSalesEmployee = NumberOfSalesEmployee;
            application.OtherBusinessRevenue = OtherBusinessRevenue;
            application.Phone = Phone;
            application.PrimaryBusinessRevenueId = PrimaryBusinessRevenueId;
            application.ProductLines = ProductLines;
            application.ProvideInvoiceOnDemand = ProvideInvoiceOnDemand;
            application.ShippingCity = ShippingCity;
            application.ShippingState = ShippingState;
            application.ShippingStreet1 = ShippingStreet1;
            application.ShippingStreet2 = ShippingStreet2;
            application.ShippingZip = ShippingZip;
            application.SignatureType = SignatureType;
            application.SolelyWorkName = SolelyWorkName;
            application.State = State;
            application.Street1 = Street1;
            application.Street2 = Street2;
            application.TrueAnswers = TrueAnswers;
            application.UserId = UserId;
            application.WebUrl = WebUrl;
            application.Zip = Zip;
        }
    }
}