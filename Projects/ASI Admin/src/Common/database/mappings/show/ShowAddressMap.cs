using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using asi.asicentral.model.show;

namespace asi.asicentral.database.mappings.show
{
    public class ShowAddressMap : EntityTypeConfiguration<ShowAddress>
    {
        public ShowAddressMap()
        {
            this.ToTable("ATT_Address");
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
