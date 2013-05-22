using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class LegacyDistributorBusinessRevenueMap : EntityTypeConfiguration<DistributorBusinessRevenue>
    {
        public LegacyDistributorBusinessRevenueMap() 
        {
            this.ToTable("CENT_DistBusRev");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("DAPP_BusinessRevID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Name)
                .HasColumnName("DAPP_BusinessRevName");

        }
    }
}
