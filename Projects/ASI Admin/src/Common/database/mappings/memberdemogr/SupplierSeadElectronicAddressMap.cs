using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.model.findsupplier;

namespace asi.asicentral.database.mappings.memberdemogr
{
    public class SupplierSeadElectronicAddressMap : EntityTypeConfiguration<SupplierSeadElectronicAddress>
    {
        public SupplierSeadElectronicAddressMap()
        {
            this.ToTable("MBDM_SPLR_ElectronicAddr_SEAD");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("SEAD_Id");

            this.Property(t => t.SupplierId)
                .HasColumnName("SEAD_SPLR_SUPPID");

            this.Property(t => t.EATPId)
                .HasColumnName("SEAD_EATP_ID");

            this.Property(t => t.ElectronicAddress)
                .HasColumnName("SEAD_ElectronicAddress");

            this.Property(t => t.IsPrimary)
                .HasColumnName("SEAD_IsPrimary");

            this.Property(t => t.UserName)
               .HasColumnName("SEAD_UserName");

            this.Property(t => t.Password)
               .HasColumnName("SEAD_Password");

            this.Property(t => t.MaxSize)
               .HasColumnName("SEAD_MaxSize");

            this.Property(t => t.Setting)
               .HasColumnName("SEAD_Setting");

            this.Property(t => t.Protocal)
               .HasColumnName("SEAD_Protocol");

            this.Property(t => t.Software)
               .HasColumnName("SEAD_Software");

        }
    }
}
