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

    class LookAdSizeMap : EntityTypeConfiguration<LookAdSize>
    {

        public LookAdSizeMap()
        {
            this.ToTable("LOOK_MagazineAdvertisingAdSize");
            this.HasKey(t => t.Id);
            Property(t => t.Id)
                .HasColumnName("MagazineAdSizeId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
