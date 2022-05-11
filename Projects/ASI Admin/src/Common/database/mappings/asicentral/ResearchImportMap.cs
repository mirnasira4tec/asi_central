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
    public class ResearchImportMap : EntityTypeConfiguration<ResearchImport>
    {
        public ResearchImportMap()
        {
            ToTable("ASI_ResearchImport");
            HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("ResearchImportId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
