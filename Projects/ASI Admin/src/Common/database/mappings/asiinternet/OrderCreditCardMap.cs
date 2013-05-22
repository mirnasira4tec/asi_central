using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class OrderCreditCardMap : EntityTypeConfiguration<LegacyOrderCreditCard>
    {
        public OrderCreditCardMap()
        {
            this.ToTable("STOR_OrderCredit_ORCC");
            this.HasKey(t => t.OrderId);

            // Properties
            this.Property(t => t.OrderId)
                .HasColumnName("ORDR_OrderID");

            this.Property(t => t.Name)
                .HasColumnName("ORCC_CCName")
                .HasMaxLength(150);

            this.Property(t => t.Type)
                .HasColumnName("ORCC_CCType")
                .HasMaxLength(15);

            this.Property(t => t.Number)
                .HasColumnName("ORCC_CCNo")
                .HasMaxLength(75);

            this.Property(t => t.ExpMonth)
                .HasColumnName("ORCC_ExpMonth")
                .HasMaxLength(50);

            this.Property(t => t.ExpYear)
                .HasColumnName("ORCC_ExpYear")
                .HasMaxLength(15);

            this.Property(t => t.ExternalReference)
                .HasColumnName("ORCC_ExternalReference")
                .HasMaxLength(150);

            this.Property(t => t.TotalAmount)
                .HasColumnName("ORCC_TotalAmt");

            // Relationships
            this.HasRequired(credit => credit.Order)
                .WithRequiredDependent(order => order.CreditCard);
        }
    }
}
