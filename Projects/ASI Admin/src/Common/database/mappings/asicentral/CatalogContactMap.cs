using System.Data.Entity.ModelConfiguration;
using asi.asicentral.model.asicentral;

namespace asi.asicentral.database.mappings.asicentral
{
   public class CatalogContactMap: EntityTypeConfiguration<CatalogContact>
    {
        public CatalogContactMap()
        {
            ToTable("USR_CatalogContact");
            HasKey(key => key.CatalogContactId);

            // Relationships
            this.HasRequired(t => t.CatalogContactImport)
                .WithMany(t => t.CatalogContacts)
                .HasForeignKey(t => t.CatalogContactImportId);
        }
    }
}
