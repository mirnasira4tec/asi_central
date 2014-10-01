using asi.asicentral.database.mappings.memberdemogr;
using asi.asicentral.model.findsupplier;
using System.Data.Entity;

namespace asi.asicentral.database
{
    public class MemberDemogrContext : BaseContext
    {
        public MemberDemogrContext()
            : base("name=MemberDemogrContext")
        {
            Database.SetInitializer<MemberDemogrContext>(null);
        }

        public DbSet<SupplierPolicy> SupplierPolicies { get; set; }
        public DbSet<SupplierPhone> SupplierPhones { get; set; }
        public DbSet<SupplierRating> SupplierRatings { get; set; }
        public DbSet<SupplierSeadElectronicAddress> SupplierSeadElectronicAddresses { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations
                .Add(new SupplierPolicyMap())
                .Add(new SupplierPhoneMap())
                .Add(new SupplierRatingMap())
                .Add(new SupplierSeadElectronicAddressMap());
        }
    }
}
