using asi.asicentral.model.timss;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.timss
{
    public class TIMSSCommunicationMap : EntityTypeConfiguration<TIMSSCommunication>
    {
        public TIMSSCommunicationMap()
        {
            this.ToTable("TIMSS_APPLICATION_INPUT_COMMUNICATION");
            this.HasKey(t => t.DAPP_UserId);

            // Properties
            this.Property(t => t.DAPP_UserId)
                .HasColumnName("DAPP_UserID");

            this.Property(t => t.Type)
                .HasColumnName("COMM_TYPE")
                .IsRequired()
                .HasMaxLength(24);

            this.Property(t => t.CountryCode)
                .HasColumnName("COUNTRY_CODE")
                .HasMaxLength(5);

            this.Property(t => t.AreaCode)
                .HasColumnName("AREA_CODE")
                .HasMaxLength(5);

            this.Property(t => t.Phone)
                .HasColumnName("PHONE_NUMBER")
                .HasMaxLength(1);

            this.Property(t => t.FormattedPhoneAddress)
                .HasColumnName("FORMATTED_PHONE_ADDRESS")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.FirstName)
                .HasColumnName("FIRST_NAME")
                .HasMaxLength(40);

            this.Property(t => t.LastName)
                .HasColumnName("LAST_NAME")
                .HasMaxLength(40);

            this.Property(t => t.ProcessedFlag)
                .HasColumnName("PROCESSED_FLAG")
                .HasMaxLength(50);

            this.Property(t => t.ProcessedDate)
                .HasColumnName("PROCESSED_DATE");
        }
    }
}
