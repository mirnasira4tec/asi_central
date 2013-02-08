using asi.asicentral.model.counselor;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asipublication
{
    public class ContentMap : EntityTypeConfiguration<CounselorContent>
    {
        public ContentMap()
        {
            this.ToTable("COUN_Content_CSCO");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("ID_CSCO");

            this.Property(t => t.Title)
                .HasColumnName("Title_CSCO")
                .HasMaxLength(250);

            this.Property(t => t.Date)
                .HasColumnName("Date_CSCO");

            this.Property(t => t.Author)
                .HasColumnName("Author_CSCO")
                .HasMaxLength(1000);

            this.Property(t => t.TagLine)
                .HasColumnName("TagLine_CSCO")
                .HasMaxLength(4000);

            this.Property(t => t.Teaser)
                .HasColumnName("Teaser_CSCO");

            this.Property(t => t.Content)
                .HasColumnName("Content_CSCO");

            this.Property(t => t.Active)
                .HasColumnName("Active_CSCO");

            this.Property(t => t.SmallImage)
                .HasColumnName("Img_Sm_CSCO")
                .HasMaxLength(250);

            this.Property(t => t.LargeImage)
                .HasColumnName("Img_Lg_CSCO")
                .HasMaxLength(250);
        }
    }
}
