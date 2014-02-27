using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.product
{
    class CouponMap : EntityTypeConfiguration<Coupon>
    {
        public CouponMap()
        {
            this.ToTable("PROD_Coupon");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("CouponId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //Relationships
            HasOptional(coupon => coupon.Context)
                .WithMany()
                .HasForeignKey(coupon => coupon.ContextId);

            HasOptional(coupon => coupon.Product)
               .WithMany()
               .HasForeignKey(coupon => coupon.ProductId);
        }
    }
}
