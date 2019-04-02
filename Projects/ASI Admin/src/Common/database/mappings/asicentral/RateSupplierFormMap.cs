
using System.Data.Entity.ModelConfiguration;
using asi.asicentral.model.asicentral;

namespace asi.asicentral.database.mappings.asicentral
{
    public class RateSupplierFormMap : EntityTypeConfiguration<RateSupplierForm>
    {

        public RateSupplierFormMap()
        {
            ToTable("USR_RateSupplierForm");
            HasKey(key => key.RateSupplierFormId);

            // Relationships
            this.HasRequired(t => t.RateSupplierImports)
                .WithMany(t=>t.RateSupplierForms)
                .HasForeignKey(t => t.RateSupplierImportId);


        }
    }
}
