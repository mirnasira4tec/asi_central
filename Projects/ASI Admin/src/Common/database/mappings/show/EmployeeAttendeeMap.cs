using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.model.show;

namespace asi.asicentral.database.mappings.show
{
    public class EmployeeAttendeeMap : EntityTypeConfiguration<EmployeeAttendee>
    {
        public EmployeeAttendeeMap()
        {
            this.ToTable("EmployeeAttendee");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("EmployeeAttendeeId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

          //Relationships
            //HasRequired(t => t.Employee).WithRequiredDependent(u => u.EmployeeAttendee);
            //HasRequired(t => t.Attendee).WithRequiredDependent(u => u.EmployeeAttendee);

            HasRequired(x => x.Attendee)
             .WithMany()
             .HasForeignKey(x => x.AttendeesId);

            HasRequired(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId);

        }
    }
}
