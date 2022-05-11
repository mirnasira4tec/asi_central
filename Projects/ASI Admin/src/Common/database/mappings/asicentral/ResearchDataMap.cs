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
    public class ResearchDataMap : EntityTypeConfiguration<ResearchData>
    {
        public ResearchDataMap()
        {
            ToTable("ASI_ResearchData");
            HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("ResearchDataId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.HasRequired(t => t.ResearchImport)
                .WithMany(t => t.ResearchDataList)
                .WillCascadeOnDelete();

        }
    }
}
