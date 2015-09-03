using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using asi.asicentral.model.show;

namespace asi.asicentral.database.mappings.show
{
    public class ShowCompanyAddressMap : EntityTypeConfiguration<ShowCompanyAddress>
    {
        public ShowCompanyAddressMap()
        {
            this.ToTable("ATT_CompanyAddress");
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
