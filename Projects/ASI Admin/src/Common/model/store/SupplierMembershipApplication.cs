using asi.asicentral.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace asi.asicentral.model.store
{
    public class SupplierMembershipApplication : OrderDetailApplication
    {
        public SupplierMembershipApplication()
        {
            if (this.GetType() == typeof(SupplierMembershipApplication))
            {
                Contacts = new List<SupplierMembershipApplicationContact>();
            }
        }

        [Display(ResourceType = typeof(Resource), Name = "ApplicationStatus")]
        public Nullable<int> ApplicationStatusId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "BillingTollPhone")]
        public string BillingTollFree { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ContactName")]
        public string ContactName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ContactTitle")]
        public string ContactTitle { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ContactEmail")]
        public string ContactEmail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ContactPhone")]
        public string ContactPhone { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "LineNames")]
        public string LineNames { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "MinorityOwned")]
        public Nullable<bool> LineMinorityOwned { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SalesVolume")]
        public string SalesVolume { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "YearEstablished")]
        public Nullable<int> YearEstablished { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "YearEnteredAdvertising")]
        public Nullable<int> YearEnteredAdvertising { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "OfficeHour")]
        public string OfficeHourStart { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "OfficeHourEnd")]
        public string OfficeHourEnd { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NumberOfEmployee")]
        public string NumberOfEmployee { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NumberOfSalesEmployee")]
        public string NumberOfSalesEmployee { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsImprinterVsDecorator")]
        public Nullable<bool> IsImprinterVsDecorator { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsImporter")]
        public Nullable<bool> IsImporter { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsManufacturer")]
        public Nullable<bool> IsManufacturer { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsRetailer")]
        public Nullable<bool> IsRetailer { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsWholesaler")]
        public Nullable<bool> IsWholesaler { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "FedTaxId")]
        public Nullable<int> FedTaxId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SellToEndUsers")]
        public Nullable<bool> SellToEndUsers { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SellThruDistributors")]
        public Nullable<bool> SellThruDistributors { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SellThruInternet")]
        public Nullable<bool> SellThruInternet { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SellThruDirectMarketing")]
        public Nullable<bool> SellThruDirectMarketing { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SellThruRetail")]
        public Nullable<bool> SellThruRetail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SellThruAffiliate")]
        public Nullable<bool> SellThruAffiliate { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AffiliateCompanyName")]
        public string AffiliateCompanyName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AffiliateASINumber")]
        public Nullable<int> AffiliateASINumber { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsUnionMade")]
        public Nullable<bool> IsUnionMade { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ProductionTime")]
        public string ProductionTime { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsRushServiceAvailable")]
        public Nullable<bool> IsRushServiceAvailable { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "OtherDec")]
        public string OtherDec { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsUPSAvailable")]
        public Nullable<bool> IsUPSAvailable { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "UPSAddress")]
        public string UPSAddress { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "UPSCity")]
        public string UPSCity { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "UPSState")]
        public string UPSState { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "UPSZip")]
        public string UPSZip { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "UPSShippingNumber")]
        public string UPSShippingNumber { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AuthorizeUPSNewAccount")]
        public Nullable<bool> AuthorizeUPSNewAccount { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AgreeUPSTermsAndConditions")]
        public Nullable<bool> AgreeUPSTermsAndConditions { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AgreeASITermsAndConditions")]
        public Nullable<bool> AgreeASITermsAndConditions { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsWomanOwned")]
        public Nullable<bool> WomanOwned { get; set; }

        public virtual ICollection<SupplierMembershipApplicationContact> Contacts { get; set; }

        public void CopyTo(SupplierMembershipApplication application)
        {
            application.Contacts = Contacts;
            application.AffiliateASINumber = AffiliateASINumber;
            application.AffiliateCompanyName = AffiliateCompanyName;
            application.AgreeASITermsAndConditions = AgreeASITermsAndConditions;
            application.AgreeUPSTermsAndConditions = AgreeUPSTermsAndConditions;
            application.ApplicationStatusId = ApplicationStatusId;
            application.AuthorizeUPSNewAccount = AuthorizeUPSNewAccount;
            application.BillingAddress1 = BillingAddress1;
            application.BillingCity = BillingCity;
            application.BillingEmail = BillingEmail;
            application.BillingFax = BillingFax;
            application.BillingPhone = BillingPhone;
            application.BillingState = BillingState;
            application.BillingTollFree = BillingTollFree;
            application.BillingWebUrl = BillingWebUrl;
            application.BillingZip = BillingZip;
            application.Company = Company;
            application.ContactEmail = ContactEmail;
            application.ContactName = ContactName;
            application.ContactPhone = ContactPhone;
            application.ContactTitle = ContactTitle;
            application.FedTaxId = FedTaxId;
            application.Id = Id;
            application.IsImporter = IsImporter;
            application.IsImprinterVsDecorator = IsImprinterVsDecorator;
            application.IsManufacturer = IsManufacturer;
            application.IsRetailer = IsRetailer;
            application.IsRushServiceAvailable = IsRushServiceAvailable;
            application.IsUnionMade = IsUnionMade;
            application.IsUPSAvailable = IsUPSAvailable;
            application.IsWholesaler = IsWholesaler;
            application.LineMinorityOwned = LineMinorityOwned;
            application.LineNames = LineNames;
            application.NumberOfEmployee = NumberOfEmployee;
            application.NumberOfSalesEmployee = NumberOfSalesEmployee;
            application.OfficeHourEnd = OfficeHourEnd;
            application.OfficeHourStart = OfficeHourStart;
            application.OtherDec = OtherDec;
            application.ProductionTime = ProductionTime;
            application.SalesVolume = SalesVolume;
            application.SellThruAffiliate = SellThruAffiliate;
            application.SellThruDirectMarketing = SellThruDirectMarketing;
            application.SellThruDistributors = SellThruDistributors;
            application.SellThruInternet = SellThruInternet;
            application.SellThruRetail = SellThruRetail;
            application.SellToEndUsers = SellToEndUsers;
            application.ShippingStreet1 = ShippingStreet1;
            application.ShippingCity = ShippingCity;
            application.ShippingState = ShippingState;
            application.ShippingZip = ShippingZip;
            application.UPSAddress = UPSAddress;
            application.UPSCity = UPSCity;
            application.UPSShippingNumber = UPSShippingNumber;
            application.UPSState = UPSState;
            application.UPSZip = UPSZip;
            application.UserId = UserId;
            application.WomanOwned = WomanOwned;
            application.YearEnteredAdvertising = YearEnteredAdvertising;
            application.YearEstablished = YearEstablished;
        }
    }
}
