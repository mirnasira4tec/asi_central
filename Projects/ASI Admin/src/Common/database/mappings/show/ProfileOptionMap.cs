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
    class ProfileOptionMap : EntityTypeConfiguration<ProfileOption>
    {
        public ProfileOptionMap()
        {
            ToTable("SHW_ProfileOption");
            HasKey(k => k.Id);

            Property(k => k.Id)
                .HasColumnName("ProfileOptionId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasMany(m => m.ProfileOptionValues)
                .WithRequired(m => m.ProfileOption)
                .HasForeignKey(m => m.ProfileOptionId);
        }
    }
}
