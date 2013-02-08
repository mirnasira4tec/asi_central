using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Publications.Models.Mapping
{
    public class CounselorFeatureContentMap : EntityTypeConfiguration<COUN_FeatureContent_COFC>
    {
        public CounselorFeatureContentMap()
        {
            // Primary Key
            this.HasKey(t => t.CatId_CSCT);

            // Properties
            this.Property(t => t.CatId_CSCT)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("COUN_FeatureContent_COFC");
            this.Property(t => t.CatId_CSCT).HasColumnName("CatId_CSCT");
            this.Property(t => t.ID_CSCO).HasColumnName("ID_CSCO");

            // Relationships
            this.HasRequired(t => t.COUN_Categories_CSCT)
                .WithOptional(t => t.COUN_FeatureContent_COFC);
            this.HasOptional(t => t.COUN_Content_CSCO)
                .WithMany(t => t.COUN_FeatureContent_COFC)
                .HasForeignKey(d => d.ID_CSCO);

        }
    }
}
