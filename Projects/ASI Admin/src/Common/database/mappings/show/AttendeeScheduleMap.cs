using asi.asicentral.model.show;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.show
{
    public class AttendeeScheduleMap : EntityTypeConfiguration<AttendeeSchedule>
    {
        public AttendeeScheduleMap()
        {
            this.ToTable("ATT_AttendeeSchedule");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                 .HasColumnName("AttendeeScheduleId")
                 .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //Relationships
            HasRequired(x => x.SupplierAttendee)
             .WithMany(x=> x.AttendeeSchedulesSuppliers)
             .HasForeignKey(x => x.SupplierAttendeeId);

            HasRequired(x => x.DistributorAttendee)
             .WithMany(x=> x.AttendeeSchedulesDistributors)
             .HasForeignKey(x => x.DistributorAttendeeId);

            HasRequired(x => x.ShowScheduleDetail)
            .WithMany(x=> x.AttendeeSchedules)
            .HasForeignKey(x => x.ShowScheduleDetailId);

            
        }
    }
}
