using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;

namespace asi.asicentral.database.mappings.store
{
    class StoreDetailCatalogAdvertisingItemMap : EntityTypeConfiguration<StoreDetailCatalogAdvertisingItem>
    {
        public StoreDetailCatalogAdvertisingItemMap()
        {
            ToTable("STOR_CatalogAdvertisingItem");
            HasKey(t => t.Id);
            Property(t => t.Id)
                .HasColumnName("AdItemId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.OrderDetailId).IsRequired();
            Property(t => t.AdSize).IsRequired();
            Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");
            Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");
        }
    }
}
