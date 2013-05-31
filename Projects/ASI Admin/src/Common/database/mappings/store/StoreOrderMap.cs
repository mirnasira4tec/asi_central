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
    public class StoreOrderMap : EntityTypeConfiguration<StoreOrder>
    {
        public StoreOrderMap()
        {
            this.ToTable("STOR_Order");
            this.HasKey(t => t.StoreOrderId);

            //Properties
            this.Property(t => t.StoreOrderId)
                .HasColumnName("StoreOrderId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");
        }
    }
}
