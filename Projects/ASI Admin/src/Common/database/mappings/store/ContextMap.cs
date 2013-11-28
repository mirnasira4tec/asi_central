using asi.asicentral.model.news;
using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.product
{
    public class ContextMap : EntityTypeConfiguration<Context>
    {
        public ContextMap()
        {
            this.ToTable("PROD_Context");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("ContextId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.NotificationEmails)
                .HasColumnName("NotificationEmails");

            this.Property(t => t.HeaderImage)
                .HasColumnName("HeaderImage");

            this.Property(t => t.ExpiryDate)
                .HasColumnName("ExpiryDateUTC");

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //Relationships
            HasMany(ctxt => ctxt.Features)
                .WithOptional()
                .Map(m => m.MapKey("ContextId"));

            HasMany(ctxt => ctxt.Products)
                .WithOptional()
                .Map(m => m.MapKey("ContextId"));

            HasOptional(ctxt => ctxt.Coupon)
                .WithMany()
                .HasForeignKey(ctxt => ctxt.CouponId);
        }
    }
}
