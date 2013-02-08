using asi.asicentral.model.counselor;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asipublication
{
    public class FeatureContentRotatorMap : EntityTypeConfiguration<CounselorFeatureRotator>
    {
        public FeatureContentRotatorMap()
        {
            this.ToTable("COUN_FeatureContentRotator_CFCR");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("ID_ROTATOR");

            this.Property(t => t.ContentId)
                    .HasColumnName("ID_CSCO");

            this.Property(t => t.Active)
                .HasColumnName("Active");

            this.Property(t => t.SmallImage)
                .HasColumnName("img_sm_csco")
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.LargeImage)
                .HasColumnName("img_lg_csco")
                .IsRequired()
                .HasMaxLength(250);

            // Relationships
            this.HasRequired(t => t.Content)
                .WithMany(t => t.FeatureRotators)
                .HasForeignKey(d => d.ContentId);
        }
    }
}
