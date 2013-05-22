using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.asiinternet
{
    internal class LegacyStoreProductConfiguration : EntityTypeConfiguration<OrderProduct>
    {
        public LegacyStoreProductConfiguration()
        {
            this.ToTable("STOR_Products_PROD");
            this.HasKey(product => product.Id);

            this.Property(product => product.Id)
                .HasColumnName("PROD_ProdID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(product => product.Description)
                .HasColumnName("PROD_ProdDesc");

            this.Property(product => product.Summary)
                .HasColumnName("PROD_Summary");

            this.Property(product => product.LgImg)
                .HasColumnName("PROD_LgImg")
                .HasMaxLength(250);

            this.Property(product => product.LgImgHover)
                .HasColumnName("PROD_LgImgHover")
                .HasMaxLength(250);

            this.Property(product => product.MedImg)
                .HasColumnName("PROD_MedImg")
                .HasMaxLength(250);

            this.Property(product => product.MedImgHover)
                .HasColumnName("PROD_MedImgHover")
                .HasMaxLength(250);

            this.Property(product => product.Price)
                .HasColumnName("PROD_Price");

            this.Property(product => product.PriceUnitId)
                .HasColumnName("PUNT_PriceUnitID");

            this.Property(product => product.Display)
                .HasColumnName("PROD_Display");

            this.Property(product => product.Taxable)
                .HasColumnName("PROD_Taxable");

            //this.Property(product => product.CustomerServiceNo)
            //    .HasColumnName("PROD_CustomerServiceNo")
            //    .HasMaxLength(100);

            this.Property(product => product.Deleted)
                .HasColumnName("PROD_Deleted");

            this.Property(product => product.LocaleId)
                .HasColumnName("LOCL_LocaleID");

            this.Property(product => product.TermId)
                .HasColumnName("TERM_TermID");
        }
    }
}
