using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.store
{
    public class FormInstanceMap : EntityTypeConfiguration<FormInstance>
    {
        public FormInstanceMap()
        {
            this.ToTable("FRM_Instance");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("InstanceId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.FormTypeId)
                .HasColumnName("TypeId");

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

	        this.HasRequired(instance => instance.FormType)
		        .WithMany(type => type.FormInstances)
		        .HasForeignKey(instance => instance.FormTypeId);

            HasMany(instance => instance.Values)
                .WithRequired()
                .HasForeignKey(formValue => formValue.FormInstanceId)
                .WillCascadeOnDelete();
        }
    }
}
