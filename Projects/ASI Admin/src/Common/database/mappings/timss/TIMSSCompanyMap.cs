using asi.asicentral.model.timss;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.timss
{
    public class TIMSSCompanyMap : EntityTypeConfiguration<TIMSSCompany>
    {
        public TIMSSCompanyMap()
        {
            this.ToTable("TIMSS_APPLICATION_INPUT_COMPANY");
            this.HasKey(t => t.DAPP_UserId);

            // Properties
            this.Property(t => t.Sequence)
                .HasColumnName("SEQ_NO")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.DAPP_UserId)
                .HasColumnName("DAPP_UserID");

            this.Property(t => t.ASINumber)
                .HasColumnName("ASI_NUMBER")
                .HasMaxLength(12);

            this.Property(t => t.Name)
                .HasColumnName("COMPANY_NAME")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CustomerClass)
                .HasColumnName("COMPANY_CUSTOMER_CLASS")
                .IsRequired()
                .HasMaxLength(24);

            this.Property(t => t.ProcessedFlag)
                .HasColumnName("PROCESSED_FLAG")
                .HasMaxLength(50);

            this.Property(t => t.ProcessedDate)
                .HasColumnName("PROCESSED_DATE");
        }
    }
}
