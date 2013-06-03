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

        public DbSet<Context> Contexts { get; set; }
        public DbSet<ContextProduct> Products { get; set; }
        public DbSet<ContextFeature> Features { get; set; }
        public DbSet<ContextFeatureProduct> FeatureProducts { get; set; } 
        public DbSet<ContextProductSequence> ProductSequences { get; set; }
        public DbSet<StoreAddress> StoreAddresses { get; set; }
        public DbSet<StoreCompany> StoreCompanies { get; set; }
        public DbSet<StoreCreditCard> StoreCreditCards { get; set; }
        public DbSet<StoreOrder> StoreOrders { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations
                .Add(new ContextMap())
                .Add(new ContextProductMap())
                .Add(new ContextFeatureMap())
                .Add(new ContextFeatureProductMap())
                .Add(new ContextProductSequenceMap())
                .Add(new StoreAddressMap())
                .Add(new StoreCompanyMap())
                .Add(new StoreCreditCardMap())
                .Add(new StoreOrderMap())
                .Add(new TaxRateMap());
        }
    }
}
