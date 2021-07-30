using System.Data.Entity;
using asi.asicentral.database.mappings.asicentral;
using asi.asicentral.model.asicentral;

namespace asi.asicentral.database
{
    public class AsicentralContext : BaseContext
    {
        public AsicentralContext()
            : this("umbracoDbDSN")
        {
        }
        public AsicentralContext(string connectionName)
            : base("name=" + connectionName)
        {
            System.Data.Entity.Database.SetInitializer<AsicentralContext>(null);
            Configuration.LazyLoadingEnabled = true;
        }
        public DbSet<RateSupplierFormDetail> ASPNetMemberships { get; set; }
        public DbSet<RateSupplierForm> ASPNetUsers { get; set; }
        public DbSet<RateSupplierImport> Categories { get; set; }

        public DbSet<CatalogContactImport> CatalogContactImports { get; set; }
        public DbSet<CatalogContactSale> CatalogContactSales { get; set; }
        public DbSet<CatalogContactSaleDetail> CatalogContactSaleDetails { get; set; }
        public DbSet<CatalogContact> CatalogContacts { get; set; }
        public DbSet<CatalogArtWorks> CatalogArtWorks { get; set; }

        public DbSet<AsicentralFormType> FormTypes { get; set; }
        public DbSet<AsicentralFormInstance> FormInstances { get; set; }
        public DbSet<AsicentralFormValue> FormValues { get; set; }
        public DbSet<AsicentralFormQuestion> FormQuestions { get; set; }
        public DbSet<AsicentralFormQuestionOption> FormQuestionOptions { get; set; }
        public DbSet<FormDataValue> FormDataValue { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations
                .Add(new RateSupplierFormDetailMap())
                .Add(new RateSupplierFormMap())
                .Add(new RateSupplierImportMap())
                .Add(new CatalogContactImportMap())
                .Add(new CatalogContactMap())
                .Add(new CatalogContactSaleMap())
                .Add(new CatalogContactSaleDetailMap())
                .Add(new CatalogArtWorksMap())
                .Add(new FormTypeMap())
                .Add(new FormInstanceMap())
                .Add(new FormValueMap())
                .Add(new AsicentralFormQuestionMap())
                .Add(new AsicentralFormQuestionOptionMap())
                .Add(new FormDataValueMap());
        }
    }
}
