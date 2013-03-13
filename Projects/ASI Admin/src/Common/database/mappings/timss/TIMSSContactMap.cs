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
            this.HasKey(t => t.DAPP_UserId);

            // Properties
            this.Property(t => t.DAPP_UserId)
                .HasColumnName("DAPP_UserID");

            this.Property(t => t.FirstName)
                .HasColumnName("FIRST_NAME")
                .IsRequired()
                .HasMaxLength(40);

            this.Property(t => t.LastName)
                .HasColumnName("LAST_NAME")
                .IsRequired()
                .HasMaxLength(40);

            this.Property(t => t.Title)
                .HasColumnName("TITLE")
                .HasMaxLength(80);

            this.Property(t => t.Department)
                .HasColumnName("DEPARTMENT")
                .HasMaxLength(60);

            this.Property(t => t.PrimaryFlag)
                .HasColumnName("PRIMARY_FLAG")
                .HasMaxLength(1);

            this.Property(t => t.ProcessedFlag)
                .HasColumnName("PROCESSED_FLAG")
                .HasMaxLength(50);

            this.Property(t => t.ProcessedDate)
                .HasColumnName("PROCESSED_DATE");
        }
    }
}
