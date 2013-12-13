using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;

namespace asi.asicentral.database.mappings.asiemailblast
{

    public class ClosedCampaignDateMap : EntityTypeConfiguration<ClosedCampaignDate>
    {
        public ClosedCampaignDateMap()
        {
            this.ToTable("BLST_ClosedCampaignDates_CCDS");
            this.HasKey(t => t.ID);
            Property(t => t.ID)
                .HasColumnName("CCDS_ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.Reactivated).IsRequired();

            Property(t => t.Date)
                .HasColumnName("CCDS_Date");
            Property(t => t.Reactivated)
                .HasColumnName("CCDS_Reactivated");
            Property(t => t.CreateDate)
                .HasColumnName("CCDS_CreateDate");
            Property(t => t.CreateSource)
                .HasColumnName("CCDS_CreateSource");
            Property(t => t.Updatedate)
                .HasColumnName("CCDS_Updatedate");
            Property(t => t.UpdateSource)
                .HasColumnName("CCDS_UpdateSource");        }
    }
}
