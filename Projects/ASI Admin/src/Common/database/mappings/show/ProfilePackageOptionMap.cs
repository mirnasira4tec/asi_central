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
    class ProfilePackageOptionMap : EntityTypeConfiguration<ProfilePackageOption>
    {
        public ProfilePackageOptionMap()
        {
            ToTable("SHW_ProfilePackageOption");
            HasKey(k => k.Id);

            Property(k => k.Id)
                .HasColumnName("ProfilePackageOptionId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(bc => bc.ProfilePackage)
            .WithMany(b => b.ProfilePackageOptions)
            .HasForeignKey(bc => bc.ProfilePackageId);

            HasRequired(bc => bc.ProfileOption);
        }
    }
}
