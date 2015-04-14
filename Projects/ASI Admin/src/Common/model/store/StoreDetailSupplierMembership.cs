using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreDetailSupplierMembership : StoreDetailApplication
    {
        //In the below list of product id's 1 to 4 are supplier products and 9 to 24 are SGR products
        //Among them 9 to 16 are Chinese SGR products
        //17 to 24 are Chinese English products
        //83 is ESP Advantage, it is a supplier membership product
        public static int[] Identifiers = new int[] { 1, 2, 3, 4, 9, 10, 11, 12 ,13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 83};

        public StoreDetailSupplierMembership()
        {
            if (this.GetType() == typeof(StoreDetailSupplierMembership))
            {
                DecoratingTypes = new List<LookSupplierDecoratingType>();
            }
        }

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

        public virtual void CopyTo(StoreDetailSupplierMembership supplier)
        {
            base.CopyTo(supplier);
            supplier.AffiliateASINumber = AffiliateASINumber;
            supplier.AffiliateCompanyName = AffiliateCompanyName;
            supplier.AgreeASITermsAndConditions = AgreeASITermsAndConditions;
            supplier.AgreeUPSTermsAndConditions = AgreeUPSTermsAndConditions;
            supplier.AuthorizeUPSNewAccount = AuthorizeUPSNewAccount;
            supplier.BusinessHours = BusinessHours;
            supplier.DecoratingTypes = DecoratingTypes;
            supplier.FedTaxId = FedTaxId;
            supplier.HasAmericanProducts = HasAmericanProducts;
            supplier.IsImporter = IsImporter;
            supplier.IsImprinterVsDecorator = IsImprinterVsDecorator;
            supplier.IsManufacturer = IsManufacturer;
            supplier.IsMinorityOwned = IsMinorityOwned;
            supplier.IsRetailer = IsRetailer;
            supplier.IsRushServiceAvailable = IsRushServiceAvailable;
            supplier.IsUnionMade = IsUnionMade;
            supplier.IsUPSAvailable = IsUPSAvailable;
            supplier.IsWholesaler = IsWholesaler;
            supplier.LineNames = LineNames;
            supplier.NumberOfEmployee = NumberOfEmployee;
            supplier.NumberOfSalesEmployee = NumberOfSalesEmployee;
            supplier.OfficeHourEnd = OfficeHourEnd;
            supplier.OfficeHourStart = OfficeHourStart;
            supplier.OtherDec = OtherDec;
            supplier.ProductionTime = ProductionTime;
            supplier.SalesVolume = SalesVolume;
            supplier.SellThruAffiliate = SellThruAffiliate;
            supplier.SellThruDirectMarketing = SellThruDirectMarketing;
            supplier.SellThruDistributors = SellThruDistributors;
            supplier.SellThruInternet = SellThruInternet;
            supplier.SellThruRetail = SellThruRetail;
            supplier.SellToEndUsers = SellToEndUsers;
            supplier.UPSAddress = UPSAddress;
            supplier.UPSCity = UPSCity;
            supplier.UPSShippingNumber = UPSShippingNumber;
            supplier.UPSState = UPSState;
            supplier.UPSZip = UPSZip;
            supplier.WomanOwned = WomanOwned;
            supplier.YearEnteredAdvertising = YearEnteredAdvertising;
            supplier.YearEstablished = YearEstablished;
        }
    }
}
