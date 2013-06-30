using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class LegacyOrderMagazineAddressMap : EntityTypeConfiguration<LegacyOrderMagazineAddress>
    {
        public LegacyOrderMagazineAddressMap()
        {
            // Primary Key
            this.HasKey(t => new { t.OrderID, t.SubscribeID, t.ProdID });

            // Properties
            this.Property(t => t.OrderID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SubscribeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProdID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("STOR_MagSbcrLongOrders_MGOR");
            this.Property(t => t.OrderID).HasColumnName("ORDR_OrderID");
            this.Property(t => t.SubscribeID).HasColumnName("MAGA_SubscribeID");
            this.Property(t => t.ProdID).HasColumnName("PROD_ProdID");

            // Relationships
            this.HasRequired(t => t.Address)
                .WithMany()
                .HasForeignKey(d => d.SubscribeID);
        }
    }
}
