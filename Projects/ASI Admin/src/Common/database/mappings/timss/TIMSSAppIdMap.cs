using asi.asicentral.model.timss;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.timss
{
    public class TIMSSAppIdMap : EntityTypeConfiguration<TIMSSAppId>
    {
        public TIMSSAppIdMap()
        {
            this.ToTable("TIMSS_APPLICATION_INPUT_APPIDs");
            this.HasKey(t => new { DAPP_APPID = t.DAPP_AppId, DAPP_UserID = t.DAPP_UserId });

            // Properties
            this.Property(t => t.DAPP_AppId)
                .HasColumnName("DAPP_APPID");

            this.Property(t => t.DAPP_UserId)
                .HasColumnName("DAPP_UserID");
        }
    }
}
