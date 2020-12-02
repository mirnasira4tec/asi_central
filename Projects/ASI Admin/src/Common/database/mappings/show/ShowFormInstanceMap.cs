using asi.asicentral.model.show;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.show
{
    public class ShowFormInstanceMap : EntityTypeConfiguration<ShowFormInstance>
    {
        public ShowFormInstanceMap()
        {
            this.ToTable("FRM_Instance");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("InstanceId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            HasMany(instance => instance.PropertyValues)
                .WithRequired()
                .HasForeignKey(formValue => formValue.FormInstanceId)
                .WillCascadeOnDelete();

            HasOptional(x => x.Attendee)
             .WithMany(x => x.TravelForms)
             .HasForeignKey(x => x.AttendeeId);

            HasRequired(x => x.FormType)
                .WithMany()
                .HasForeignKey(x => x.TypeId);
        }
    }
}
