using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.store
{
    public class TermsConditionsInstanceMap: EntityTypeConfiguration<TermsConditionsInstance>
    {
        public TermsConditionsInstanceMap()
        {
            this.ToTable("TERM_Instance");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("InstanceId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //relationships
            HasRequired(t => t.TermsAndConditions)
                .WithMany()
                .HasForeignKey(t => t.TypeId);
        }    
    }
}
