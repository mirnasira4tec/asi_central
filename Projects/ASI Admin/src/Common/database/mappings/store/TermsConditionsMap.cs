using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.TermCondition
{
    public class TermsConditionsMap : EntityTypeConfiguration<TermsConditionsType>
    {
        public TermsConditionsMap()
        {
            this.ToTable("TERM_Type");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("TypeId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Name)
                .HasColumnName("Name")
                .IsRequired();

            this.Property(t => t.Header)
                .HasColumnName("Header");

            this.Property(t => t.Body)
                .HasColumnName("Body");

            this.Property(t => t.IsActive)
                .HasColumnName("IsActive");

            this.Property(t => t.StartDate)
                .HasColumnName("StartDate");

             this.Property(t => t.EndDate)
                .HasColumnName("EndDate");

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            this.Property(t => t.UpdateSource)
                .HasColumnName("UpdateSource");
        }    
    }
}
