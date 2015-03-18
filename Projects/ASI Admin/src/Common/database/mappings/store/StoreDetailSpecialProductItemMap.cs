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
    public class StoreDetailSpecialProductItemMap : EntityTypeConfiguration<StoreDetailSpecialProductItem>
    {
        public StoreDetailSpecialProductItemMap()
        {
            this.ToTable("STOR_SpecialProductItem");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("SpecialProductId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //Relationships
            HasOptional(item => item.OrderDetail)
                 .WithMany()
                 .HasForeignKey(item => item.OrderDetailId);
        }
    }
}
