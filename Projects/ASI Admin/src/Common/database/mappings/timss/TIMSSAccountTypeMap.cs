using asi.asicentral.model.timss;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.timss
{
    public class TIMSSAccountTypeMap : EntityTypeConfiguration<TIMSSAccountType>
    {
        public TIMSSAccountTypeMap()
        {
            this.ToTable("TIMSS_APPLICATION_INPUT_AcctType");
            this.HasKey(t => new { t.DAPP_UserId, t.SubCode });

            // Properties
            this.Property(t => t.DAPP_UserId)
                .HasColumnName("DAPP_UserID");

            this.Property(t => t.Description)
                .HasColumnName("AcctTypeDesc")
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.SubCode)
                .HasColumnName("AcctType_SUBCODE")
                .HasMaxLength(24);

            this.Property(t => t.LoadStatus)
                .HasColumnName("LoadStatus")
                .HasMaxLength(20);

            this.Property(t => t.LoadDate)
                .HasColumnName("LoadDate");
        }
    }
}
