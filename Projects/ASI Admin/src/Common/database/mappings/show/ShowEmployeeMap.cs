using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.model.show;

namespace asi.asicentral.database.mappings.show
{
    public class ShowEmployeeMap : EntityTypeConfiguration<ShowEmployee>
    {
        public ShowEmployeeMap()
        {
            this.ToTable("Employee");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("EmployeeId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

             //Relationships
         
            HasOptional(t => t.Company)
               .WithMany()
               .HasForeignKey(t => t.CompanyId);

            HasRequired(x => x.Address)
              .WithMany()
              .HasForeignKey(x => x.AddressId);

        }
    }
}
