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
    public class ShowAttendeeMap : EntityTypeConfiguration<ShowAttendee>
    {
        public ShowAttendeeMap()
        {
            this.ToTable("ATT_Attendee");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                 .HasColumnName("AttendeeId")
                 .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //Relationships
            HasRequired(x => x.Company)
             .WithMany()
             .HasForeignKey(x => x.CompanyId);

            HasRequired(x => x.Show)
             .WithMany()
             .HasForeignKey(x => x.ShowId);

            HasMany(t => t.EmployeeAttendees)
                  .WithOptional()
                  .HasForeignKey(t => t.AttendeeId)
                  .WillCascadeOnDelete();

            HasMany(t => t.DistShowLogos)
                  .WithOptional()
                  .HasForeignKey(t => t.AttendeeId)
                  .WillCascadeOnDelete();

            HasMany(t =>t.ProfileRequests)
                .WithOptional()
                  .HasForeignKey(t => t.AttendeeId)
                  .WillCascadeOnDelete();

            HasMany(x => x.TravelForms)
             .WithOptional()
                  .HasForeignKey(t => t.AttendeeId)
                  .WillCascadeOnDelete();

            
        }
    }
}
