using asi.asicentral.model.news;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.internet
{
    public class LegacyNewsRotatorMap : EntityTypeConfiguration<NewsRotator>
    {
        public LegacyNewsRotatorMap()
        {
            this.ToTable("CENT_NewsRotator_NROT");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("NROT_ItemID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CategoryId)
                .HasColumnName("NROT_CatID");

            this.Property(t => t.CategoryPriority)
                .HasColumnName("NROT_CatPriority");

            this.Property(t => t.FollowLink)
                .HasColumnName("NROT_FollowLink")
                .HasMaxLength(500);

            this.Property(t => t.VideoLink)
                .HasColumnName("NROT_VideoLink")
                .HasMaxLength(500);

            this.Property(t => t.AudioLink)
                .HasColumnName("NROT_AudioLink")
                .HasMaxLength(500);

            this.Property(t => t.MainImage)
                .HasColumnName("NROT_MainImg")
                .HasMaxLength(250);

            this.Property(t => t.SubImage)
                .HasColumnName("NROT_SubImg")
                .HasMaxLength(250);

            this.Property(t => t.ThumbnailImage)
                .HasColumnName("NROT_ThumbImg")
                .HasMaxLength(250);

            this.Property(t => t.IsFeature)
                .HasColumnName("NROT_Feature");

            this.Property(t => t.IsSubFeature)
                .HasColumnName("NROT_SubFeature");

            // Relationships
            this.HasRequired(t => t.News)
                .WithOptional(t => t.NewsRotator);
        }
    }
}
