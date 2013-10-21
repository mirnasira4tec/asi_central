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
    public class StoreDetailDecoratorMembershipMap : EntityTypeConfiguration<StoreDetailDecoratorMembership>
    {
        public StoreDetailDecoratorMembershipMap()
        {
            this.ToTable("STOR_DecoratorMembership");
            this.HasKey(t => t.OrderDetailId);

            //Properties
            this.Property(t => t.OrderDetailId)
                .HasColumnName("OrderDetailId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //Relationships
            HasMany(t => t.ImprintTypes)
                .WithMany()
                .Map(category =>
                {
                    category.MapLeftKey("OrderDetailId");
                    category.MapRightKey("ImprintingId");
                    category.ToTable("STOR_DecoratorMembershipImprinting");
                });
        }
    }
}
