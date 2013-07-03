using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreDetailSupplierMembership
    {
        public StoreDetailSupplierMembership()
        {
            if (this.GetType() == typeof(StoreDetailSupplierMembership))
            {
                DecoratingTypes = new List<LookSupplierDecoratingType>();
            }
        }

        public int OrderDetailId { get; set; }
        public string LegacyApplicationId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ApplicationStatus")]
        public Nullable<int> AppStatusId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "LineNames")]
        public string LineNames { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "MinorityOwned")]
        public Nullable<bool> IsMinorityOwned { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SalesVolume")]
        public string SalesVolume { get; set; }

        [Range(1700, 2050, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldYearRange")]
        [Display(ResourceType = typeof(Resource), Name = "YearEstablished")]
        public Nullable<int> YearEstablished { get; set; }

        [Range(1700, 2050, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldYearRange")]
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

        [Display(ResourceType = typeof(Resource), Name = "HasAmericanProducts")]
        public Nullable<bool> HasAmericanProducts { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "BusinessHours")]
        public string BusinessHours { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public virtual IList<LookSupplierDecoratingType> DecoratingTypes { get; set; }

        public override string ToString()
        {
            return "Supplier Membership " + OrderDetailId;
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            StoreDetailSupplierMembership supplier = obj as StoreDetailSupplierMembership;
            if (supplier != null) equals = supplier.OrderDetailId == OrderDetailId;
            return equals;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + "StoreDetailSupplierMembership".GetHashCode();
            hash = hash * 31 + OrderDetailId.GetHashCode();
            return hash;
        }
    }
}
