using asi.asicentral.model.show;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.show
{
    class ShowProfileDistributorDataMap : EntityTypeConfiguration<ShowProfileDistributorData>
    {
        public ShowProfileDistributorDataMap()
        {
            this.ToTable("ATT_ProfileDistributorData");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("ProfileDistributorDataId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            this.Property(t => t.UpdateSource)
                .HasColumnName("UpdateSource");

            HasRequired(x => x.ProfileRequests)
           .WithMany(x => x.ProfileDistributorData)
           .HasForeignKey(x=>x.ProfileRequestId);

        }  
    }
}
