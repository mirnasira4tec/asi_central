using asi.asicentral.model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings
{
    public class PublicationIssueConfiguration : EntityTypeConfiguration<PublicationIssue>
    {
        public PublicationIssueConfiguration()
        {
            ToTable("CENT_PUB_SCHEDULE");
            HasKey(issue => issue.PublicationIssueId);
            Property(issue => issue.PublicationIssueId)
                .HasColumnName("ScheduleId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(issue => issue.Name)
                .HasColumnName("ScheduleName");
        }
    }
}
