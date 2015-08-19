using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using asi.asicentral.model.show;

namespace asi.asicentral.database.mappings.show
{
   public class ShowCompanyMap :EntityTypeConfiguration<ShowCompany>
    {
       public ShowCompanyMap()
        {
            this.ToTable("Company");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("CompanyId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            HasMany(t => t.Employees)
                .WithOptional()
                .HasForeignKey(t => t.CompanyId)
                .WillCascadeOnDelete();

            HasMany(t => t.Attendees)
                 .WithOptional()
                 .HasForeignKey(t => t.CompanyId)
                 .WillCascadeOnDelete();

            HasMany(t => t.CompanyAddresses)
                  .WithOptional()
                  .HasForeignKey(t => t.CompanyId)
                  .WillCascadeOnDelete();

        }
    }
}
