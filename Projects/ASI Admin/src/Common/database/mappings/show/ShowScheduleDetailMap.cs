using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using asi.asicentral.model.show;

namespace asi.asicentral.database.mappings.show
{
    public class ShowScheduleDetailMap : EntityTypeConfiguration<ShowScheduleDetail>
    {
        public ShowScheduleDetailMap()
        {
            this.ToTable("ATT_ShowScheduleDetail");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                 .HasColumnName("ShowScheduleDetailId")
                 .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //Relationships
            HasRequired(x => x.ShowSchedule)
             .WithMany(x=> x.ShowScheduleDetails)
             .HasForeignKey(x => x.ShowScheduleId);

        }
    }
}
