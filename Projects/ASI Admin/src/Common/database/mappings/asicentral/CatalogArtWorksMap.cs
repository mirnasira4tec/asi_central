using System.Data.Entity.ModelConfiguration;
using asi.asicentral.model.asicentral;

namespace asi.asicentral.database.mappings.asicentral
{
   public class CatalogArtWorksMap: EntityTypeConfiguration<CatalogArtWorks>
    {
        public CatalogArtWorksMap()
        {
            ToTable("USR_CatalogArtWorks");
            HasKey(key => key.ArtworkId);
        }
    }
}
