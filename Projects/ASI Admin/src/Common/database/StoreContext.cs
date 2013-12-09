using asi.asicentral.database.mappings.product;
using asi.asicentral.database.mappings.store;
using asi.asicentral.model.store;
using System.Data.Entity;

namespace asi.asicentral.database
{
    public class StoreContext : BaseContext
    {
        public StoreContext(string connectionName)
            : base("name=" + connectionName)
        {
            Database.SetInitializer<StoreContext>(null);
            EnableTracing(typeof(StoreContext));
        }

        public StoreContext()
            : this("ProductContext")
        {
        }

        public DbSet<ClosedCampaignDate> ClosedCampaignDates { get; set; }
        public DbSet<Context> Contexts { get; set; }
        public DbSet<ContextProduct> Products { get; set; }
        public DbSet<ContextFeature> Features { get; set; }
        public DbSet<ContextFeatureProduct> FeatureProducts { get; set; }
        public DbSet<ContextProductSequence> ProductSequences { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<StoreDetailHallmarkRequest> HallmarkFormRequests { get; set; }
        public DbSet<LookCatalogOption> LookCatalogOptions { get; set; }
        public DbSet<LookDecoratorImprintingType> LookDecoratorImprintingTypes { get; set; }
        public DbSet<LookDistributorAccountType> LookDistributorAccountTypes { get; set; }
        public DbSet<LookDistributorRevenueType> LookDistributorRevenueTypes { get; set; }
        public DbSet<LookProductLine> LookProductLines { get; set; }
        public DbSet<LookProductShippingRate> LookProductShippingRates { get; set; }
        public DbSet<LookSupplierDecoratingType> LookSupplierDecoratingTypes { get; set; }
        public DbSet<StoreAddress> StoreAddresses { get; set; }
        public DbSet<StoreCompany> StoreCompanies { get; set; }
        public DbSet<StoreCompanyAddress> StoreCompanyAddresses { get; set; }
        public DbSet<StoreCreditCard> StoreCreditCards { get; set; }
        public DbSet<StoreDetailCatalog> StoreDetailCatalogs { get; set; }
        public DbSet<StoreDetailDecoratorMembership> StoreDetailDecoratorMemberships { get; set; }
        public DbSet<StoreDetailDistributorMembership> StoreDetailDistributorMemberships { get; set; }
        public DbSet<StoreDetailESPAdvertising> StoreDetailESPAdvertisings { get; set; }
        public DbSet<StoreDetailEspTowerAd> StoreDetailEspTowerAds { get; set; }
        public DbSet<StoreDetailESPAdvertisingItem> StoreDetailESPAdvertisingItems { get; set; }
        public DbSet<StoreDetailPayForPlacement> StoreDetailPayForPlacements { get; set; }
        public DbSet<StoreDetailSupplierMembership> StoreDetailSupplierMemberships { get; set; }
        public DbSet<StoreIndividual> StoreIndividuals { get; set; }
        public DbSet<StoreMagazineSubscription> StoreMagazineSubscriptions { get; set; }
        public DbSet<StoreOrder> StoreOrders { get; set; }
        public DbSet<StoreOrderDetail> StoreOrderDetails { get; set; }
        public DbSet<StoreTieredProductPricing> StoreTieredProductPricings { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }
        public DbSet<StoreDetailEmailExpress> StoreDetailEmailExpresss { get; set; }
        public DbSet<StoreDetailEmailExpressItem> StoreDetailEmailExpressItems { get; set; }
        public DbSet<StoreDetailProductCollection> StoreDetailProductCollection { get; set; }
        public DbSet<StoreDetailProductCollectionItem> StoreDetailProductCollectionItems { get; set; }
        public DbSet<LookProductCollections> LookProductCollections { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Configuration.LazyLoadingEnabled = true;
            modelBuilder.Configurations
                .Add(new ClosedCampaignDateMap())
                .Add(new ContextMap())
                .Add(new ContextProductMap())
                .Add(new ContextFeatureMap())
                .Add(new ContextFeatureProductMap())
                .Add(new ContextProductSequenceMap())
                .Add(new CouponMap())
                .Add(new StoreDetailHallmarkRequestMap())
                .Add(new LookCatalogOptionMap())
                .Add(new LookDecoratorImprintingTypeMap())
                .Add(new LookDistributorAccountTypeMap())
                .Add(new LookDistributorRevenueTypeMap())
                .Add(new LookProductLineMap())
                .Add(new LookProductShippingRateMap())
                .Add(new LookSupplierDecoratingTypeMap())
                .Add(new StoreAddressMap())
                .Add(new StoreCompanyMap())
                .Add(new StoreCompanyAddressMap())
                .Add(new StoreCreditCardMap())
                .Add(new StoreDetailCatalogMap())
                .Add(new StoreDetailDecoratorMembershipMap())
                .Add(new StoreDetailDistributorMembershipMap())
                .Add(new StoreDetailESPAdvertisingMap())
                .Add(new StoreDetailEspTowerAdMap())
                .Add(new StoreDetailESPAdvertisingItemMap())
                .Add(new StoreDetailPayForPlacementMap())
                .Add(new StoreDetailSupplierMembershipMap())
                .Add(new StoreIndividualMap())
                .Add(new StoreMagazineSubscriptionMap())
                .Add(new StoreOrderMap())
                .Add(new StoreOrderDetailMap())
                .Add(new StoreTieredProductPricingMap())
                .Add(new TaxRateMap())
                .Add(new StoreDetailEmailExpressMap())
                .Add(new StoreDetailEmailExpressItemMap())
            .Add(new StoreDetailProductCollectionMap())
                .Add(new StoreDetailProductCollectionItemMap())
                .Add(new LookProductCollectionsMap());
        }
    }
}
