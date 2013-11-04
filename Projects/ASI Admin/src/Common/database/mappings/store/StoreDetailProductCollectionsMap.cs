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
    public class StoreDetailProductCollectionMap : EntityTypeConfiguration<StoreDetailProductCollection>
    {
        public StoreDetailProductCollectionMap()
        {
            this.ToTable("stor_productcollectionmonths");
            this.HasKey(t => t.ItemMonthId);

            //Properties
            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

           //Relationship
            HasMany(t => t.ProductCollectionItems)
                .WithOptional()
                .HasForeignKey(t => t.ItemMonthId)
                .WillCascadeOnDelete();
        }
    }
}
