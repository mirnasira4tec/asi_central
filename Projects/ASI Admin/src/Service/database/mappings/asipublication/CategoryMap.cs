using asi.asicentral.model.counselor;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asipublication
{
    public class CategoryMap : EntityTypeConfiguration<CounselorCategory>
    {
        public CategoryMap()
        {
            this.ToTable("COUN_Categories_CSCT");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("CatId_CSCT")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Description)
                .HasColumnName("CatDesc_CSCT")
                .HasMaxLength(50);

            // Relationships
            this.HasMany(t => t.Contents)
                .WithMany(t => t.Categories)
                .Map(m =>
                    {
                        m.ToTable("COUN_CatContent_CNCC");
                        m.MapLeftKey("CatID_CSCT");
                        m.MapRightKey("ID_CSCO");
                    });
        }
    }
}
