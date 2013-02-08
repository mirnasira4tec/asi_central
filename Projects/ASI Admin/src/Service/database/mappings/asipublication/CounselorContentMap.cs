using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Publications.Models.Mapping
{
    public class CounselorContentMap : EntityTypeConfiguration<COUN_Content_CSCO>
    {
        public CounselorContentMap()
        {
            // Primary Key
            this.HasKey(t => t.ID_CSCO);

            // Properties
            this.Property(t => t.Title_CSCO)
                .HasMaxLength(250);

            this.Property(t => t.Author_CSCO)
                .HasMaxLength(1000);

            this.Property(t => t.TagLine_CSCO)
                .HasMaxLength(4000);

            this.Property(t => t.Img_Sm_CSCO)
                .HasMaxLength(250);

            this.Property(t => t.Img_Lg_CSCO)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("COUN_Content_CSCO");
            this.Property(t => t.ID_CSCO).HasColumnName("ID_CSCO");
            this.Property(t => t.Title_CSCO).HasColumnName("Title_CSCO");
            this.Property(t => t.Date_CSCO).HasColumnName("Date_CSCO");
            this.Property(t => t.Author_CSCO).HasColumnName("Author_CSCO");
            this.Property(t => t.TagLine_CSCO).HasColumnName("TagLine_CSCO");
            this.Property(t => t.Teaser_CSCO).HasColumnName("Teaser_CSCO");
            this.Property(t => t.Content_CSCO).HasColumnName("Content_CSCO");
            this.Property(t => t.Active_CSCO).HasColumnName("Active_CSCO");
            this.Property(t => t.Img_Sm_CSCO).HasColumnName("Img_Sm_CSCO");
            this.Property(t => t.Img_Lg_CSCO).HasColumnName("Img_Lg_CSCO");
        }
    }
}
