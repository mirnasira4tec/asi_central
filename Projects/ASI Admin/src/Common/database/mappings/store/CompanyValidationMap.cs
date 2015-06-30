using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.model.store;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace asi.asicentral.database.mappings.store
{

    class CompanyValidationMap : EntityTypeConfiguration<CompanyValidation>
    {

        public CompanyValidationMap()
        {
            this.ToTable("COMP_Validation");
            this.HasKey(t => t.Id);

            Property(t => t.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                            .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");    
         
        }
    }
}
