using asi.asicentral.model.show;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.show
{
    class CompanyProfileMap : EntityTypeConfiguration<CompanyProfile>
    {
        public CompanyProfileMap()
        {
            ToTable("SHW_CompanyProfile");
            HasKey(k => k.Id);

            Property(k => k.Id)
                .HasColumnName("CompanyProfileId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasMany(m => m.CompanyProfileData)
                .WithRequired(m => m.CompanyProfile)
                .HasForeignKey(m => m.CompanyProfileId);

            HasOptional(m => m.ShowCompany)
                .WithMany()
                .HasForeignKey(m => m.CompanyId);

            HasOptional(m => m.Show)
                .WithMany()
                .HasForeignKey(m => m.ShowId);
        }
    }
}
