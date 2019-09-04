using System.Data.Entity.ModelConfiguration;
using asi.asicentral.model.asicentral;

namespace asi.asicentral.database.mappings.asicentral
{
   public class CatalogContactImportMap: EntityTypeConfiguration<CatalogContactImport>
    {
        public CatalogContactImportMap()
        {
            ToTable("USR_CatalogContactImport");
            HasKey(key => key.CatalogContactImportId);
        }
    }
}
