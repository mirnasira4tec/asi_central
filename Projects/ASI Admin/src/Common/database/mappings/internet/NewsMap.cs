using asi.asicentral.model.news;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.internet
{
    public class LegacyNewsMap : EntityTypeConfiguration<News>
    {
        public LegacyNewsMap()
        {
            this.ToTable("tblNews");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("ID");

            this.Property(t => t.DateEntered)
                .HasColumnName("DateEntered");

            this.Property(t => t.Priority)
                .HasColumnName("Priority");

            this.Property(t => t.LiveDate)
                .HasColumnName("LiveDate");

            this.Property(t => t.Duration)
                .HasColumnName("Duration");

            this.Property(t => t.Post)
                .HasColumnName("Post");

            this.Property(t => t.Title)
                .HasColumnName("Title")
                .HasMaxLength(150);

            this.Property(t => t.Summary)
                .HasColumnName("Summary");

            this.Property(t => t.Description)
                .HasColumnName("Description");

            this.Property(t => t.SourceId)
                .HasColumnName("SourceID");

            // Relationships
            this.HasOptional(t => t.Source)
                .WithMany(t => t.News)
                .HasForeignKey(d => d.SourceId);
        }
    }
}
