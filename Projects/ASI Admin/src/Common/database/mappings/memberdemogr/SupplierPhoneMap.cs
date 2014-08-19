using asi.asicentral.model.findsupplier;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.memberdemogr
{
    public class SupplierPhoneMap : EntityTypeConfiguration<SupplierPhone>
    {
        public SupplierPhoneMap()
        {
            this.ToTable("MBDM_SPLR_Phone_SPHN");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("SPHN_Id");

            this.Property(t => t.SupplierId)
                .HasColumnName("SPHN_SPLR_SUPPID");

            this.Property(t => t.PhoneTypeCode)
                .HasColumnName("SPHN_PHTP_CD");

            this.Property(t => t.Phone)
                .HasColumnName("SPHN_Phone");

            this.Property(t => t.IsPrimary)
                .HasColumnName("SPHN_Primary");

        }
    }
}
