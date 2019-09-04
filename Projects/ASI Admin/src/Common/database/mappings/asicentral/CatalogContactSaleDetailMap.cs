using System.Data.Entity.ModelConfiguration;
using asi.asicentral.model.asicentral;

namespace asi.asicentral.database.mappings.asicentral
{
    public class CatalogContactSaleDetailMap : EntityTypeConfiguration<CatalogContactSaleDetail>
    {
        public CatalogContactSaleDetailMap()
        {
            ToTable("USR_CatalogContactSaleDetail");
            HasKey(key => key.CatalogContactSaleDetailId);

            this.HasRequired(t => t.CatalogContacts)
                .WithMany(t=>t.CatalogContactSaleDetails)
                .HasForeignKey(t => t.CatalogContactId);

            this.HasRequired(t => t.CatalogContactSale)
               .WithMany(t=>t.CatalogContactSaleDetails)
               .HasForeignKey(t => t.CatalogContactSaleId);
        }
    }
}
