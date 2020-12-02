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
    class ProfilePackageMap : EntityTypeConfiguration<ProfilePackage>
    {
        public ProfilePackageMap()
        {
            this.ToTable("SHW_ProfilePackage");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("ProfilePackageId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            HasMany(m => m.ProfilePackageOptions)
                .WithRequired(m => m.ProfilePackage)
                .HasForeignKey(m => m.ProfilePackageId);
        }
    }
}
