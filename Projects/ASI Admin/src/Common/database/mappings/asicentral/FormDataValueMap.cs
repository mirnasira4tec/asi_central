using asi.asicentral.model.asicentral;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.asicentral
{
    class FormDataValueMap : EntityTypeConfiguration<FormDataValue>
    {
        public FormDataValueMap()
        {
            ToTable("USR_FormData");
            HasKey(t => t.Id);
            
            Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(p => p.FormInstance)
                .WithMany(i => i.DataValues)
                .HasForeignKey(p => p.InstanceId);
        }
    }
}
