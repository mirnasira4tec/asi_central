using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class SupplierDecoratingTypeMap : EntityTypeConfiguration<LegacySupplierDecoratingType>
    {
        public SupplierDecoratingTypeMap()
        {
            this.ToTable("CENT_SuppJoinDecType_SAPP");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasColumnName("SAPP_DecType")
                .HasMaxLength(50);

            this.Property(t => t.Id)
                .HasColumnName("SAPP_DecID");
        }
    }
}
