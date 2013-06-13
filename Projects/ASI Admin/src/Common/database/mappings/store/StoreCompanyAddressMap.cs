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
    public class StoreCompanyAddressMap : EntityTypeConfiguration<StoreCompanyAddress>
    {
        public StoreCompanyAddressMap()
        {
            this.ToTable("STOR_CompanyAddress");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("CompanyAddressId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //relationships
            HasOptional(t => t.Address)
                .WithMany()
                .Map(t => t.MapKey("AddressId"));
        }
    }
}
