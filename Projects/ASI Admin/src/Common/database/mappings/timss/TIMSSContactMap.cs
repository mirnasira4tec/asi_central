using asi.asicentral.model.timss;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.timss
{
    public class TIMSSContactMap : EntityTypeConfiguration<TIMSSContact>
    {
        public TIMSSContactMap()
        {
            this.ToTable("TIMSS_APPLICATION_INPUT_CONTACT");
            this.HasKey(t => new { t.DAPP_UserId, t.RecordId });

            // Properties
            this.Property(t => t.RecordId)
                .HasColumnName("RecId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Prefix)
                .HasColumnName("NAME_PREFIX")
                .HasMaxLength(20);

            this.Property(t => t.FirstName)
                .HasColumnName("FIRST_NAME")
                .HasMaxLength(100);

            this.Property(t => t.MiddleName)
                .HasColumnName("MIDDLE_NAME")
                .HasMaxLength(40);

            this.Property(t => t.LastName)
                .HasColumnName("LAST_NAME")
                .HasMaxLength(100);

            this.Property(t => t.Suffix)
                .HasColumnName("NAME_SUFFIX")
                .HasMaxLength(20);

            this.Property(t => t.Credentials)
                .HasColumnName("NAME_CREDENTIALS")
                .HasMaxLength(40);

            this.Property(t => t.Title)
                .HasColumnName("JOB_TITLE")
                .HasMaxLength(250);

            this.Property(t => t.CustomerClass)
                .HasColumnName("INDIV_CUSTOMER_CLASS")
                .HasMaxLength(24);

            this.Property(t => t.PhoneCountryCode)
                .HasColumnName("INDIVIDUAL_PHONE_COUNTRY_CODE")
                .HasMaxLength(5);

            this.Property(t => t.PhoneAreaCode)
                .HasColumnName("INDIVIDUAL_PHONE_AREA_CODE")
                .HasMaxLength(5);

            this.Property(t => t.PhoneNumber)
                .HasColumnName("INDIVIDUAL_PHONE_NUMBER")
                .HasMaxLength(20);

            this.Property(t => t.PhoneExtension)
                .HasColumnName("INDIVIDUAL_PHONE_EXTENSION")
                .HasMaxLength(20);

            this.Property(t => t.FaxCountryCode)
                .HasColumnName("INDIVIDUAL_FAX_COUNTRY_CODE")
                .HasMaxLength(5);

            this.Property(t => t.FaxAreaCode)
                .HasColumnName("INDIVIDUAL_FAX_AREA_CODE")
                .HasMaxLength(5);

            this.Property(t => t.FaxNumber)
                .HasColumnName("INDIVIDUAL_FAX_NUMBER")
                .HasMaxLength(20);

            this.Property(t => t.FaxExtension)
                .HasColumnName("INDIVIDUAL_FAX_EXTENSION")
                .HasMaxLength(20);

            this.Property(t => t.Email)
                .HasColumnName("INDIVIDUAL_EMAIL")
                .HasMaxLength(100);

            this.Property(t => t.PrimaryFlag)
                .HasColumnName("PRIMARY_CONTACT")
                .HasMaxLength(1);

            this.Property(t => t.ProcessedFlag)
                .HasColumnName("PROCESSED_FLAG")
                .HasMaxLength(1);

            this.Property(t => t.ErrorFlag)
                .HasColumnName("ERROR_FLAG")
                .HasMaxLength(1);

            this.Property(t => t.RejectReason)
                .HasColumnName("REJECT_REASON")
                .HasMaxLength(1);

            this.Property(t => t.ConcurrencyId)
                .HasColumnName("CONCURRENCY_ID")
                .HasMaxLength(50);

            this.Property(t => t.LoadStatus)
                .HasColumnName("Load_Status")
                .HasMaxLength(50);

            this.Property(t => t.DAPP_UserId)
                .HasColumnName("DAPP_UserID");

            this.Property(t => t.LoadDate)
                .HasColumnName("Load_date");
        }
    }
}
