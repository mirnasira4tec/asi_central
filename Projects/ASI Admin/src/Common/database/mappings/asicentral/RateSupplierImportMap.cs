using System.Data.Entity.ModelConfiguration;
using asi.asicentral.model.asicentral;

namespace asi.asicentral.database.mappings.asicentral
{
    public class RateSupplierImportMap : EntityTypeConfiguration<RateSupplierImport>
    {
        public RateSupplierImportMap()
        {
            ToTable("USR_RateSupplierImport");
            HasKey(key => key.RateSupplierImportId);
        }
    }
}
