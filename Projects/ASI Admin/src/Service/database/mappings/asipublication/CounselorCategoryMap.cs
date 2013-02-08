using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Publications.Models.Mapping
{
    public class CounselorCategoryMap : EntityTypeConfiguration<COUN_Categories_CSCT>
    {
        public CounselorCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.CatId_CSCT);

            // Properties
            this.Property(t => t.CatDesc_CSCT)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("COUN_Categories_CSCT");
            this.Property(t => t.CatId_CSCT).HasColumnName("CatId_CSCT");
            this.Property(t => t.CatDesc_CSCT).HasColumnName("CatDesc_CSCT");
            this.Property(t => t.FeatureID_CSCT).HasColumnName("FeatureID_CSCT");

            // Relationships
            this.HasMany(t => t.COUN_Content_CSCO1)
                .WithMany(t => t.COUN_Categories_CSCT1)
                .Map(m =>
                    {
                        m.ToTable("COUN_CatContent_CNCC");
                        m.MapLeftKey("CatID_CSCT");
                        m.MapRightKey("ID_CSCO");
                    });

            this.HasOptional(t => t.COUN_Content_CSCO)
                .WithMany(t => t.COUN_Categories_CSCT)
                .HasForeignKey(d => d.FeatureID_CSCT);

        }
    }
}
