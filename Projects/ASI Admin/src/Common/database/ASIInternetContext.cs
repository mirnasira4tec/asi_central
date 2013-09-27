﻿using asi.asicentral.database.mappings.asiinternet;
using asi.asicentral.model.sgr;
using System.Data.Entity;
using asi.asicentral.model.store;

namespace asi.asicentral.database
{
    public class ASIInternetContext : BaseContext
    {
        public ASIInternetContext()
            : this("ASIInternetContext")
        {
        }

        public ASIInternetContext(string connectionName)
            : base("name=" + connectionName)
        {
            Database.SetInitializer<ASIInternetContext>(null);
            EnableTracing(typeof(ASIInternetContext));
        }

        public DbSet<ASPNetMembership> ASPNetMemberships { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<LegacyCENTUserProfilesPROF> UserProfileDetails { get; set; }
        public DbSet<LegacyDistributorAccountType> DistributorAccountTypes { get; set; }
        public DbSet<LegacyDistributorBusinessRevenue> DistributorBusinessRevenues { get; set; }
        public DbSet<LegacyDistributorMembershipApplication> DistributorMembershipApplications { get; set; }
        public DbSet<LegacyDistributorMembershipApplicationContact> DistributorMembershipApplicationContacts { get; set; }
        public DbSet<LegacyDistributorProductLine> DistributorProductLines { get; set; }
        public DbSet<LegacyOrder> Orders { get; set; }
        public DbSet<LegacyOrderCatalog> OrderCatalogs { get; set; }
        public DbSet<LegacyOrderCatalogOption> OrderCatalogOptions { get; set; }
        public DbSet<LegacyOrderContact> OrderContacts { get; set; }
        public DbSet<LegacyOrderCreditCard> OrderCreditCards { get; set; }
        public DbSet<LegacyOrderDetail> OrderDetails { get; set; }
        public DbSet<LegacyMagazineAddress> LegacyMagazineAddresses { get; set; }
        public DbSet<LegacyOrderMagazineAddress> LegacyOrderMagazineAddresses { get; set; }
        public DbSet<LegacyOrderProduct> OrderProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<LegacySupplierDecoratingType> SupplierDecoratingTypes { get; set; }
        public DbSet<LegacySupplierMembershipApplication> SupplierMembershipApplications { get; set; }
        public DbSet<LegacySupplierMembershipApplicationContact> SupplierMembershipApplicationContacts { get; set; }
        public DbSet<LegacyOrderDistributorAddress> OrderDistributorAddresses { get; set; }
        public DbSet<LegacyOrderAddress> OrderAddresses { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations
               .Add(new ASPNetMembershipMap())
               .Add(new CategoryMap())
               .Add(new CompanyMap())
               .Add(new DistributorAccountTypeMap())
               .Add(new DistributorBusinessRevenueMap())
               .Add(new DistributorMembershipApplicationMap())
               .Add(new DistributorMembershipApplicationContactMap())
               .Add(new DistributorProductLineMap())
               .Add(new LegacyCENTUserProfilesPROFMap())
               .Add(new LegacyOrderAddressMap())
               .Add(new LegacyOrderDistributorAddressMap())
               .Add(new LegacyOrderCatalogMap())
               .Add(new LegacyOrderCatalogOptionMap())
               .Add(new LegacyOrderContactMap())
               .Add(new OrderCreditCardMap())
               .Add(new OrderDetailMap())
               .Add(new OrderMap())
               .Add(new LegacyMagazineAddressMap())
               .Add(new LegacyOrderMagazineAddressMap())
               .Add(new ProductMap())
               .Add(new StoreProductConfiguration())
               .Add(new SupplierDecoratingTypeMap())
               .Add(new SupplierMembershipApplicationMap())
               .Add(new SupplierMembershipApplicationContactMap());
        }
    }
}
