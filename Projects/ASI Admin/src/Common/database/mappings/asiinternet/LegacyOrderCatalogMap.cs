using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class LegacyOrderCatalogMap : EntityTypeConfiguration<LegacyOrderCatalog>
    {
        public LegacyOrderCatalogMap()
        {
            // Primary Key
            this.HasKey(t => new { t.OrderID, t.ProdID });

            // Properties
            this.Property(t => t.OrderID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProdID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NewLine1)
                .HasMaxLength(500);

            this.Property(t => t.NewLine2)
                .HasMaxLength(500);

            this.Property(t => t.NewLine3)
                .HasMaxLength(500);

            this.Property(t => t.NewLine4)
                .HasMaxLength(500);

            this.Property(t => t.NewLine5)
                .HasMaxLength(500);

            this.Property(t => t.NewLine6)
                .HasMaxLength(500);

            this.Property(t => t.Email)
                .HasMaxLength(250);

            this.Property(t => t.Web)
                .HasMaxLength(250);

            this.Property(t => t.BackLine1)
                .HasMaxLength(500);

            this.Property(t => t.BackLine2)
                .HasMaxLength(500);

            this.Property(t => t.BackLine3)
                .HasMaxLength(500);

            this.Property(t => t.BackLine4)
                .HasMaxLength(500);

            this.Property(t => t.Artwork)
                .HasMaxLength(250);

            this.Property(t => t.Logo)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("STOR_CatalogOrderDetails_CODT");
            this.Property(t => t.OrderID).HasColumnName("CODT_OrderID");
            this.Property(t => t.ProdID).HasColumnName("CODT_ProdID");
            this.Property(t => t.NewLine1).HasColumnName("CODT_NewLine1");
            this.Property(t => t.NewLine2).HasColumnName("CODT_NewLine2");
            this.Property(t => t.NewLine3).HasColumnName("CODT_NewLine3");
            this.Property(t => t.NewLine4).HasColumnName("CODT_NewLine4");
            this.Property(t => t.NewLine5).HasColumnName("CODT_NewLine5");
            this.Property(t => t.NewLine6).HasColumnName("CODT_NewLine6");
            this.Property(t => t.Email).HasColumnName("CODT_Email");
            this.Property(t => t.Web).HasColumnName("CODT_Web");
            this.Property(t => t.BackLine1).HasColumnName("CODT_BackLine1");
            this.Property(t => t.BackLine2).HasColumnName("CODT_BackLine2");
            this.Property(t => t.BackLine3).HasColumnName("CODT_BackLine3");
            this.Property(t => t.BackLine4).HasColumnName("CODT_BackLine4");
            this.Property(t => t.ArtworkProof).HasColumnName("CODT_ArtworkProof");
            this.Property(t => t.Artwork).HasColumnName("CODT_Artwork");
            this.Property(t => t.Logo).HasColumnName("CODT_Logo");
            this.Property(t => t.ShipType).HasColumnName("CODT_ShipType");
        }
    }
}
