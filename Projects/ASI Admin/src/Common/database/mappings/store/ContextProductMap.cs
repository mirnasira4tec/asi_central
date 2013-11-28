using asi.asicentral.model.news;
using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.product
{
    public class ContextProductMap : EntityTypeConfiguration<ContextProduct>
    {
        public ContextProductMap()
        {
            this.ToTable("PROD_Product");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("ProductId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.HasTax)
                .HasColumnName("HasTaxFlag");

            this.Property(t => t.HasShipping)
                .HasColumnName("HasShippingFlag");

            this.Property(t => t.IsSubscription)
                .HasColumnName("IsSubscriptionFlag");

            this.Property(t => t.IsAvailable)
                .HasColumnName("IsAvailableFlag");

            this.Property(t => t.NotificationEmails)
                .HasColumnName("NotificationEmails");

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            // Relationships
            HasOptional(product => product.Coupon)
                .WithMany()
                .HasForeignKey(product => product.CouponId);
        }
    }
}
