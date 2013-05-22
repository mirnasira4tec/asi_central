using asi.asicentral.model.timss;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.timss
{
    public class LegacyTIMSSCreditInfoMap : EntityTypeConfiguration<TIMSSCreditInfo>
    {
        public LegacyTIMSSCreditInfoMap()
        {
            this.ToTable("TIMSS_APPLICATION_INPUT_Credit_Info");
            this.HasKey(t => t.DAPP_UserId);

            // Properties
            this.Property(t => t.DAPP_UserId)
                .HasColumnName("DAPP_UserID");

            this.Property(t => t.Id)
                .HasColumnName("RecId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Name)
                .HasColumnName("CCName")
                .HasMaxLength(150);

            this.Property(t => t.Type)
                .HasColumnName("CCType")
                .HasMaxLength(15);

            this.Property(t => t.Number)
                .HasColumnName("CCNo")
                .HasMaxLength(75);

            this.Property(t => t.ExpirationMonth)
                .HasColumnName("ExpMonth")
                .HasMaxLength(50);

            this.Property(t => t.ExpirationYear)
                .HasColumnName("ExpYear")
                .HasMaxLength(15);

            this.Property(t => t.TotalAmt)
                .HasColumnName("TotalAmt");

            this.Property(t => t.DateCreated)
                .HasColumnName("DateCreated");

            this.Property(t => t.FirstName)
                .HasColumnName("BillFirstName")
                .HasMaxLength(150);

            this.Property(t => t.LastName)
                .HasColumnName("BillLastName")
                .HasMaxLength(150);

            this.Property(t => t.Street1)
                .HasColumnName("BillStreet1")
                .HasMaxLength(250);

            this.Property(t => t.Street2)
                .HasColumnName("BillStreet2")
                .HasMaxLength(250);

            this.Property(t => t.City)
                .HasColumnName("BillCity")
                .HasMaxLength(150);

            this.Property(t => t.State)
                .HasColumnName("BillState")
                .HasMaxLength(40);

            this.Property(t => t.Zip)
                .HasColumnName("BillZip")
                .HasMaxLength(25);

            this.Property(t => t.Country)
                .HasColumnName("BillCountry")
                .HasMaxLength(15);

            this.Property(t => t.Phone)
                .HasColumnName("BillPhone")
                .HasMaxLength(25);

            this.Property(t => t.ExternalReference)
                .HasColumnName("CCVoltID");

            this.Property(t => t.SecurityCode)
                .HasColumnName("SECURITY_CODE")
                .HasMaxLength(6);
        }
    }
}
