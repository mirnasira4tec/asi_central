using asi.asicentral.model.timss;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.timss
{
    public class LegacyTIMSSCompanyMap : EntityTypeConfiguration<TIMSSCompany>
    {
        public LegacyTIMSSCompanyMap()
        {
            this.ToTable("TIMSS_APPLICATION_INPUT_COMPANY");
            this.HasKey(t => t.DAPP_UserId);

            // Properties
            this.Property(t => t.DAPP_UserId)
                .HasColumnName("DAPP_UserID");

            this.Property(t => t.SequenceNumber)
                .HasColumnName("SEQ_NO")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CompanyRecordId)
                .HasColumnName("COMPANY_RECORD_ID");

            this.Property(t => t.MasterCustomerId)
                .HasColumnName("MASTER_CUSTOMER_ID")
                .HasMaxLength(12);

            this.Property(t => t.SUB_CUSTOMER_ID)
                .HasColumnName("SUB_CUSTOMER_ID");

            this.Property(t => t.ASI_Central_ID)
                .HasColumnName("ASI_Central_ID");

            this.Property(t => t.Name)
                .HasColumnName("COMPANY_NAME")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CustomerClass)
                .HasColumnName("COMPANY_CUSTOMER_CLASS")
                .IsRequired()
                .HasMaxLength(24);

            this.Property(t => t.BillAddress1)
                .HasColumnName("Bill_ADDRESS_1")
                .HasMaxLength(150);

            this.Property(t => t.BillAddress2)
                .HasColumnName("Bill_ADDRESS_2")
                .HasMaxLength(150);

            this.Property(t => t.BillAddress3)
                .HasColumnName("Bill_ADDRESS_3")
                .HasMaxLength(80);

            this.Property(t => t.BillAddress4)
                .HasColumnName("Bill_ADDRESS_4")
                .HasMaxLength(80);

            this.Property(t => t.BillCity)
                .HasColumnName("Bill_CITY")
                .HasMaxLength(75);

            this.Property(t => t.BillState)
                .HasColumnName("Bill_STATE")
                .HasMaxLength(40);

            this.Property(t => t.BillPostalCode)
                .HasColumnName("Bill_POSTALCODE")
                .HasMaxLength(15);

            this.Property(t => t.BillCountryCode)
                .HasColumnName("Bill_COUNTRY_CODE")
                .HasMaxLength(5);

            this.Property(t => t.ShipAddress1)
                .HasColumnName("Ship_Address_1")
                .HasMaxLength(150);

            this.Property(t => t.ShipAddress2)
                .HasColumnName("Ship_Address_2")
                .HasMaxLength(150);

            this.Property(t => t.ShipAddress3)
                .HasColumnName("Ship_Address_3")
                .HasMaxLength(80);

            this.Property(t => t.ShipAddress4)
                .HasColumnName("Ship_Address_4")
                .HasMaxLength(80);

            this.Property(t => t.ShipCity)
                .HasColumnName("Ship_City")
                .HasMaxLength(75);

            this.Property(t => t.ShipState)
                .HasColumnName("Ship_State")
                .HasMaxLength(40);

            this.Property(t => t.ShipPostalCode)
                .HasColumnName("Ship_PostalCode")
                .HasMaxLength(15);

            this.Property(t => t.ShipCountryCode)
                .HasColumnName("Ship_Country_Code")
                .HasMaxLength(5);

            this.Property(t => t.PhoneCountryCode)
                .HasColumnName("COMPANY_PHONE_COUNTRY_CODE")
                .HasMaxLength(5);

            this.Property(t => t.PhoneAreaCode)
                .HasColumnName("COMPANY_PHONE_AREA_CODE")
                .HasMaxLength(5);

            this.Property(t => t.PhoneNumber)
                .HasColumnName("COMPANY_PHONE_NUMBER")
                .HasMaxLength(20);

            this.Property(t => t.PhoneExtension)
                .HasColumnName("COMPANY_PHONE_EXTENSION")
                .HasMaxLength(20);

            this.Property(t => t.FaxCountryCode)
                .HasColumnName("COMPANY_FAX_COUNTRY_CODE")
                .HasMaxLength(5);

            this.Property(t => t.FaxAreaCode)
                .HasColumnName("COMPANY_FAX_AREA_CODE")
                .HasMaxLength(5);

            this.Property(t => t.FaxNumber)
                .HasColumnName("COMPANY_FAX_NUMBER")
                .HasMaxLength(20);

            this.Property(t => t.FaxExtension)
                .HasColumnName("COMPANY_FAX_EXTENSION")
                .HasMaxLength(20);

            this.Property(t => t.Email)
                .HasColumnName("COMPANY_EMAIL")
                .HasMaxLength(100);

            this.Property(t => t.Url)
                .HasColumnName("COMPANY_URL")
                .HasMaxLength(100);

            this.Property(t => t.PROCESSED_FLAG)
                .HasColumnName("PROCESSED_FLAG")
                .HasMaxLength(1);

            this.Property(t => t.ERROR_FLAG)
                .HasColumnName("ERROR_FLAG")
                .HasMaxLength(1);

            this.Property(t => t.REJECT_REASON)
                .HasColumnName("REJECT_REASON")
                .HasMaxLength(1);

            this.Property(t => t.CONCURRENCY_ID)
                .HasColumnName("CONCURRENCY_ID")
                .HasMaxLength(50);

            this.Property(t => t.Load_Status)
                .HasColumnName("Load_Status")
                .HasMaxLength(50);

            this.Property(t => t.Load_date)
                .HasColumnName("Load_date");
        }
    }
}
