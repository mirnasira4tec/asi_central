using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public class SupplierMembershipApplication
    {
        public SupplierMembershipApplication()
        {
            //this.CENT_SuppJoinAppContact_SAPP = new List<CENT_SuppJoinAppContact_SAPP>();
        }

        public System.Guid ApplicationId { get; set; }
        public System.Guid UserId { get; set; }
        public Nullable<int> ApplicationStatusId { get; set; }
        public string Company { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingZip { get; set; }
        public string BillingPhone { get; set; }
        public string BillingTollFree { get; set; }
        public string BillingFax { get; set; }
        public string BillingWebUrl { get; set; }
        public string BillingEmail { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingZip { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string LineNames { get; set; }
        public Nullable<bool> LineMinorityOwned { get; set; }
        public string SalesVolume { get; set; }
        public Nullable<int> YearEstablished { get; set; }
        public Nullable<int> YearEnteredAdvertising { get; set; }
        public string OfficeHourStart { get; set; }
        public string OfficeHourEnd { get; set; }
        public string NumberOfEmployee { get; set; }
        public string NumberOfSalesEmployee { get; set; }
        public Nullable<bool> IsImprinterVsDecorator { get; set; }
        public Nullable<bool> IsImporter { get; set; }
        public Nullable<bool> IsManufacturer { get; set; }
        public Nullable<bool> IsRetailer { get; set; }
        public Nullable<bool> IsWholesaler { get; set; }
        public Nullable<int> FedTaxId { get; set; }
        public Nullable<bool> SellToEndUsers { get; set; }
        public Nullable<bool> SellThruDistributors { get; set; }
        public Nullable<bool> SellThruInternet { get; set; }
        public Nullable<bool> SellThruDirectMarketing { get; set; }
        public Nullable<bool> SellThruRetail { get; set; }
        public Nullable<bool> SellThruAffiliate { get; set; }
        public string AffiliateCompanyName { get; set; }
        public Nullable<int> AffiliateASINumber { get; set; }
        public Nullable<bool> IsUnionMade { get; set; }
        public string ProductionTime { get; set; }
        public Nullable<bool> IsRushServiceAvailable { get; set; }
        public string OtherDec { get; set; }
        public Nullable<bool> IsUPSAvailable { get; set; }
        public string UPSAddress { get; set; }
        public string UPSCity { get; set; }
        public string UPSState { get; set; }
        public string UPSZip { get; set; }
        public string UPSShippingNumber { get; set; }
        public Nullable<bool> AuthorizeUPSNewAccount { get; set; }
        public Nullable<bool> AgreeUPSTermsAndConditions { get; set; }
        public Nullable<bool> AgreeASITermsAndConditions { get; set; }
        //public virtual ICollection<CENT_SuppJoinAppContact_SAPP> CENT_SuppJoinAppContact_SAPP { get; set; }
    }
}
