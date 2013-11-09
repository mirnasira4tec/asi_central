using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.store
{
    public class StoreDetailProductCollectionItemMap : EntityTypeConfiguration<StoreDetailProductCollectionItem>
    {
        public StoreDetailProductCollectionItemMap()
        {
            this.ToTable("STOR_ProductCollectionItems");
            this.HasKey(t => t.ItemId);

            //Properties

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");
        }
    }
}
