using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.product
{
    public class ContextFeatureProductMap : EntityTypeConfiguration<ContextFeatureProduct>
    {
        public ContextFeatureProductMap()
        {
            this.ToTable("PROD_ContextFeatureProduct");
            HasKey(t => t.ContextFeatureProductId);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

        }
    }
}
