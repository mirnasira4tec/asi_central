using asi.asicentral.database.mappings.asiinternet;
using asi.asicentral.model.sgr;
using System.Data.Entity;
using asi.asicentral.model.store;

namespace asi.asicentral.database
{
    public class ASIInternetContext : BaseContext
    {
        public ASIInternetContext()
            : base("name=ASIInternetContext")
        {
            Database.SetInitializer<ASIPublicationContext>(null);
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<OrderCreditCard> OrderCreditCards { get; set; }
        public DbSet<DistributorMembershipApplication> DistributorMembershipApplications { get; set; }
        public DbSet<SupplierMembershipApplication> SupplierMembershipApplications { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations
               .Add(new CompanyMap())
               .Add(new ProductMap())
               .Add(new CategoryMap())
               .Add(new OrderMap())
               .Add(new OrderDetailMap())
               .Add(new StoreProductConfiguration())
               .Add(new OrderCreditCardMap())
               .Add(new DistributorMembershipApplicationMap())
               .Add(new SupplierMembershipApplicationMap());
        }
    }
}
