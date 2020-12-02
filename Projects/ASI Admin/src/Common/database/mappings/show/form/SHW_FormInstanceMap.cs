using asi.asicentral.model.show.form;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.show
{
    public class SHW_FormInstanceMap : EntityTypeConfiguration<SHW_FormInstance>
    {
        public SHW_FormInstanceMap()
        {
            this.ToTable("SHW_FormInstance");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("FormInstanceId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            HasMany(instance => instance.PropertyValues)
                .WithRequired()
                .HasForeignKey(formValue => formValue.FormInstanceId)
                .WillCascadeOnDelete();
        }
    }
}
