using asi.asicentral.model.show.form;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.show
{
    public class SHW_FormPropertyValueMap : EntityTypeConfiguration<SHW_FormPropertyValue>
    {
        public SHW_FormPropertyValueMap()
        {
            this.ToTable("SHW_FormPropertyValue");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("FormPropertyValueId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.FormInstanceId)
                .HasColumnName("FormInstanceId");

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            HasRequired(p => p.FormInstance)
                .WithMany(i => i.PropertyValues)
                .HasForeignKey(p => p.FormInstanceId);
        }
    }
}
