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
            this.ToTable("Attendee");
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
           
            HasRequired(x => x.Address)
             .WithMany()
             .HasForeignKey(x => x.AddressId);
        }
    }
}
