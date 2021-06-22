using asi.asicentral.model.show;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.show
{
    class ShowProfileRequestMap : EntityTypeConfiguration<ShowProfileRequests>
    {
        public ShowProfileRequestMap()
        {
            this.ToTable("ATT_ProfileRequests");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("ProfileRequestId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            this.Property(t => t.UpdateSource)
                .HasColumnName("UpdateSource");

            //Relationships
            HasOptional(x => x.Attendee)
             .WithMany(x => x.ProfileRequests)
             .HasForeignKey(x => x.AttendeeId);

            HasMany(t => t.ProfileRequestOptionalDetails)
                 .WithRequired(t=>t.ProfileRequests)
                 .HasForeignKey(t => t.ProfileRequestId)
                 .WillCascadeOnDelete();
        }
    }
}
