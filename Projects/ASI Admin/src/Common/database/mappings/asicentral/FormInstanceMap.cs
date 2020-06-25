using asi.asicentral.model.asicentral;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace asi.asicentral.database.mappings.asicentral
{
    public class FormInstanceMap : EntityTypeConfiguration<AsicentralFormInstance>
    {
        public FormInstanceMap()
        {
            this.ToTable("USR_FormInstance");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("InstanceId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

	        this.HasRequired(instance => instance.FormType)
                .WithMany()
                .HasForeignKey(instance => instance.TypeId);

            HasMany(instance => instance.Values)
                .WithRequired()
                .HasForeignKey(formValue => formValue.InstanceId)
                .WillCascadeOnDelete();
        }
    }
}
