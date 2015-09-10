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
            this.ToTable("ATT_Employee");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("EmployeeId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            this.Property(t => t.EPhone)
               .HasColumnName("Phone");

            this.Property(t => t.EPhoneAreaCode)
              .HasColumnName("PhoneAreaCode");

             //Relationships
         
            HasOptional(t => t.Company)
               .WithMany()
               .HasForeignKey(t => t.CompanyId);

            HasOptional(x => x.Address)
              .WithMany()
              .HasForeignKey(x => x.AddressId);

        }
    }
}
