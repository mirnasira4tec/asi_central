using asi.asicentral.model.news;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.internet
{
    public class NewsSourceMap : EntityTypeConfiguration<NewsSource>
    {
        public NewsSourceMap()
        {
            this.ToTable("tblNewsSource");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("SourceID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .HasColumnName("SourceName")
                .HasMaxLength(30);
        }
    }
}
