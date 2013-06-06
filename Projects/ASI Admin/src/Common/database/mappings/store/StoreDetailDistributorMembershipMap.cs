using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.store
{
    public class StoreDetailDistributorMembershipMap : EntityTypeConfiguration<StoreDetailDistributorMembership>
    {
        public StoreDetailDistributorMembershipMap()
        {
            this.ToTable("STOR_DistributorMembership");
            this.HasKey(t => t.OrderDetailId);

            //Properties
            this.Property(t => t.OrderDetailId)
                .HasColumnName("OrderDetailId");

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");
        }
    }
}
