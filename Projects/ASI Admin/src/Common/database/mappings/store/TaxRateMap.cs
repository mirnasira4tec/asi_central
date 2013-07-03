using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.product
{
    public class TaxRateMap : EntityTypeConfiguration<TaxRate>
    {
        public TaxRateMap()
        {
            this.ToTable("TAX_StateZipTax");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("StateZipTaxId");

            this.Property(t => t.State)
                .HasColumnName("State")
                .IsRequired()
                .HasMaxLength(40);

            this.Property(t => t.Zip)
                .HasColumnName("Zip");

            this.Property(t => t.County)
                .HasColumnName("County")
                .HasMaxLength(100);

            this.Property(t => t.Rate)
                .HasColumnName("Rate");

            this.Property(t => t.CreateDateUTC)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDateUTC)
                .HasColumnName("UpdateDateUTC");

            this.Property(t => t.UpdateSource)
                .HasColumnName("UpdateSource")
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
