using asi.asicentral.model.timss;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.timss
{
    public class TIMSSProductTypeMap : EntityTypeConfiguration<TIMSSProductType>
    {
        public TIMSSProductTypeMap()
        {
            this.ToTable("TIMSS_APPLICATION_INPUT_ProdType");
            this.HasKey(t => new { DAPP_AppID = t.ApplicationId, DAPP_UserID = t.DAPP_UserId, ProdTypeDesc = t.Description });

            // Properties
            this.Property(t => t.ApplicationId)
                .HasColumnName("DAPP_AppID");

            this.Property(t => t.DAPP_UserId)
                .HasColumnName("DAPP_UserID");

            this.Property(t => t.Description)
                .HasColumnName("ProdTypeDesc")
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.SubCode)
                .HasColumnName("ProdType_SUBCODE")
                .HasMaxLength(24);

            this.Property(t => t.LoadStatus)
                .HasColumnName("LoadStatus")
                .HasMaxLength(20);

            this.Property(t => t.LoadDate)
                .HasColumnName("LoadDate");
        }
    }
}
