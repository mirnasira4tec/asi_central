using asi.asicentral.model.counselor;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asipublication
{
    public class LegacyFeatureContentMap : EntityTypeConfiguration<CounselorFeature>
    {
        public LegacyFeatureContentMap()
        {
            this.ToTable("COUN_FeatureContent_COFC");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("CatId_CSCT")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ContentId)
                .HasColumnName("ID_CSCO");

            // Relationships
            this.HasOptional(t => t.Content)
                .WithMany(t => t.Features)
                .HasForeignKey(d => d.ContentId);
        }
    }
}
