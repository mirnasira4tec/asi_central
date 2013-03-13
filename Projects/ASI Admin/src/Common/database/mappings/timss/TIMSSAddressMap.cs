using asi.asicentral.model.timss;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.timss
{
    public class TIMSSAddressMap : EntityTypeConfiguration<TIMSSAddress>
    {
        public TIMSSAddressMap()
        {
            this.ToTable("TIMSS_APPLICATION_INPUT_ADDRESS");
            this.HasKey(t => t.DAPP_UserId);

            // Properties
            this.Property(t => t.DAPP_UserId)
                .HasColumnName("DAPP_UserID");

            this.Property(t => t.Address1)
                .HasColumnName("ADDRESS_1")
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.Address2)
                .HasColumnName("ADDRESS_2")
                .HasMaxLength(80);

            this.Property(t => t.City)
                .HasColumnName("CITY")
                .IsRequired()
                .HasMaxLength(40);

            this.Property(t => t.State)
                .HasColumnName("STATE")
                .IsRequired()
                .HasMaxLength(40);

            this.Property(t => t.PostalCode)
                .HasColumnName("POSTAL_CODE")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.CountryCode)
                .HasColumnName("COUNTRY_CODE")
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.BillingPerson)
                .HasColumnName("BILLING_PERSON")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ShipToFlag)
                .HasColumnName("SHIP_TO_FLAG")
                .HasMaxLength(1);

            this.Property(t => t.BillToFlag)
                .HasColumnName("BILL_TO_FLAG")
                .HasMaxLength(1);

            this.Property(t => t.ProcessedFlag)
                .HasColumnName("PROCESSED_FLAG")
                .HasMaxLength(50);

            this.Property(t => t.ProcessedDate)
                .HasColumnName("PROCESSED_DATE");

            this.Property(t => t.PrimaryFlag)
                .HasColumnName("PRIMARY_FLAG")
                .HasMaxLength(1);
        }
    }
}
