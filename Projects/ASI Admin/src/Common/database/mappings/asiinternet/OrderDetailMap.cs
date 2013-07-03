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
    internal class OrderDetailMap : EntityTypeConfiguration<LegacyOrderDetail>
    {
        public OrderDetailMap()
        {
            this.ToTable("STOR_OrderDetail_ODET");
            this.HasKey(detail => new { detail.OrderId, detail.ProductId });
            
            this.Property(detail => detail.OrderId)
                .HasColumnName("ORDR_OrderID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(detail => detail.ProductId)
                .HasColumnName("PROD_ProdID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(detail => detail.Quantity)
                .HasColumnName("ODET_Quan");

            this.Property(detail => detail.Added)
                .HasColumnName("ODET_Added");

            this.Property(detail => detail.Application)
                .HasColumnName("ODET_Application");

            this.Property(detail => detail.TaxSubtotal)
                .HasColumnName("ODET_TaxSubtotal");

            this.Property(detail => detail.Shipping)
                .HasColumnName("ODET_Shipping");

            this.Property(detail => detail.PreTaxSubtotal)
                .HasColumnName("ODET_PreTaxSubtotal");

            this.Property(detail => detail.Subtotal)
                .HasColumnName("ODET_Subtotal");

            this.Property(detail => detail.ApplicationFee)
                .HasColumnName("ODET_ApplicationFee");

            this.Property(detail => detail.HallmarkResult)
                .HasColumnName("ODET_HallmarkResult");

            this.Property(detail => detail.ExternalReference)
                .HasColumnName("ODET_ExternalReference");

            // Relationships
            this.HasRequired(detail => detail.Order)
                .WithMany(order => order.OrderDetails)
                .HasForeignKey(detail => detail.OrderId);

            this.HasRequired(detail => detail.Product)
                .WithMany(product => product.OrderDetails)
                .HasForeignKey(detail => detail.ProductId);
        }
    }
}
