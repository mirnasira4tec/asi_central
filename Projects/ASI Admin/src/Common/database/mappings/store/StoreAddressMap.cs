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
    public class StoreAddressMap : EntityTypeConfiguration<StoreAddress>
    {
        public StoreAddressMap()
        {
            this.ToTable("STOR_Address");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("AddressId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");
        }
    }
}
