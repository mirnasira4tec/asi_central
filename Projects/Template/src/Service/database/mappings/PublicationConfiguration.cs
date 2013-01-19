using asi.asicentral.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace asi.asicentral.database.mappings
{
    public class PublicationConfiguration : EntityTypeConfiguration<Publication>
    {
        public PublicationConfiguration()
        {
            ToTable("CENT_PUBLICATION");
            HasKey(publication => publication.PublicationId);
            HasMany(publication => publication.Issues)
                .WithMany(issue => issue.Publications)
                .Map( pubIssue => {
                    pubIssue.MapLeftKey("PubId");
                    pubIssue.MapRightKey("PubScheduleId");
                    pubIssue.ToTable("CENT_PUB_SCHED_UNI");
                });

            Property(publication => publication.PublicationId)
                .HasColumnName("PubId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(publication => publication.Name)
                .HasColumnName("PubName");
            Property(publication => publication.IsPublic)
                .HasColumnName("PublicFlag");
        }
    }
}
