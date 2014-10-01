using asi.asicentral.model.findsupplier;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.memberdemogr
{
    public class SupplierPolicyMap : EntityTypeConfiguration<SupplierPolicy>
    {
        public SupplierPolicyMap()
        {
            this.ToTable("MBDM_SPLR_Policy_SPOL");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("SPOL_Id");

            this.Property(t => t.SupplierId)
                .HasColumnName("SPOL_SPLR_SUPPID");

            this.Property(t => t.PloicyId)
                .HasColumnName("SPOL_POLICYID");

            this.Property(t => t.Ploicy)
                .HasColumnName("SPOL_POLICY");

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDate");

            this.Property(t => t.CreateSource)
                .HasColumnName("CreateSource");
        }
    }
}
