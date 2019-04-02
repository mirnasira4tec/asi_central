using System.Data.Entity.ModelConfiguration;
using asi.asicentral.model.asicentral;

namespace asi.asicentral.database.mappings.asicentral
{
    public class RateSupplierFormDetailMap : EntityTypeConfiguration<RateSupplierFormDetail>
    {
        public RateSupplierFormDetailMap()
        {
            ToTable("USR_RateSupplierFormDetail");
            HasKey(key => key.RateSupplierFormDetailId);

            // Relationships
            this.HasRequired(t => t.RateSupplierForm)
                .WithMany(t=>t.RateSupplierFormDetails)
                .HasForeignKey(t => t.RateSupplierFormId);
        }
    }
}
