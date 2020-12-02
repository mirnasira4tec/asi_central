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
    class CompanyProfileDataMap : EntityTypeConfiguration<CompanyProfileData>
    {
        public CompanyProfileDataMap()
        {
            ToTable("SHW_CompanyProfileData");
            HasKey(k => k.Id);

            Property(k => k.Id)
                .HasColumnName("CompanyProfileDataId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(m => m.CompanyProfile)
                .WithMany(m => m.CompanyProfileData)
                .HasForeignKey(m => m.CompanyProfileId);

            HasRequired(m => m.ProfileOption);
        }
    }
}
