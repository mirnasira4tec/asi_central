using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class LegacyOrderCatalogOptionMap : EntityTypeConfiguration<LegacyOrderCatalogOption>
    {
        public LegacyOrderCatalogOptionMap()
        {
            // Primary Key
            this.HasKey(t => new { t.COPS_OrderID, t.COPS_ProdID, t.COPS_OptionID });

            // Properties
            this.Property(t => t.COPS_OrderID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.COPS_ProdID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.COPS_OptionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("STOR_CatalogOptSelected_COPS");
            this.Property(t => t.COPS_OrderID).HasColumnName("COPS_OrderID");
            this.Property(t => t.COPS_ProdID).HasColumnName("COPS_ProdID");
            this.Property(t => t.COPS_OptionID).HasColumnName("COPS_OptionID");
        }
    }
}
