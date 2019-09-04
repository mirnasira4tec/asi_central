using System.Data.Entity.ModelConfiguration;
using asi.asicentral.model.asicentral;

namespace asi.asicentral.database.mappings.asicentral
{
    public class CatalogContactSaleMap : EntityTypeConfiguration<CatalogContactSale>
    {
        public CatalogContactSaleMap()
        {
            ToTable("USR_CatalogContactSale");
            HasKey(key => key.CatalogContactSaleId);
            HasMany<CatalogArtWorks>(u => u.CatalogArtWorks)
                .WithMany()
                .Map(cs =>
                {
                    cs.MapLeftKey("CatalogContactSaleId");
                    cs.MapRightKey("ArtworkId");
                    cs.ToTable("USR_CatalogSaleArtworkMapping");
                });
        }
    }
}
