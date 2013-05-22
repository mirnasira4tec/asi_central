using asi.asicentral.database.mappings.timss;
using asi.asicentral.model.timss;
using System.Data.Entity;

namespace asi.asicentral.database
{
    public class TIMSSContext : BaseContext
    {
        public TIMSSContext()
            : base("name=TIMSSContext")
        {
            Database.SetInitializer<TIMSSContext>(null);
        }

        public DbSet<TIMSSAccountType> AccountTypes { get; set; }
        public DbSet<TIMSSAdditionalInfo> AdditionalInfos { get; set; }
        public DbSet<TIMSSCompany> Companies { get; set; }
        public DbSet<TIMSSContact> Contacts { get; set; }
        public DbSet<TIMSSCreditInfo> CreditInfos { get; set; }
        public DbSet<TIMSSProductType> ProductTypes { get; set; }

        /// <summary>
        /// Use to enhance the default mapping for the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations
                .Add(new LegacyTIMSSAccountTypeMap())
                .Add(new LegacyTIMSSAdditionalInfoMap())
                .Add(new LegacyTIMSSCompanyMap())
                .Add(new LegacyTIMSSContactMap())
                .Add(new LegacyTIMSSCreditInfoMap())
                .Add(new LegacyTIMSSProductTypeMap());
        }
    }
}
