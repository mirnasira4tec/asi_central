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
    class ProfileOptionValueMap : EntityTypeConfiguration<ProfileOptionValue>
    {
        public ProfileOptionValueMap()
        {
            ToTable("SHW_ProfileOptionValue");
            HasKey(k => k.Id);

            this.Property(k => k.Id)
                .HasColumnName("ProfileOptionValueId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(m => m.ProfileOption)
                .WithMany(t => t.ProfileOptionValues)
                .HasForeignKey(t => t.ProfileOptionId);
        }
    }
}
