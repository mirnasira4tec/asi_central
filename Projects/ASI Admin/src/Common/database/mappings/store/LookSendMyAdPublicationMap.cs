using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.store
{

    public class LookSendMyAdPublicationMap : EntityTypeConfiguration<LookSendMyAdPublication>
    {

        public LookSendMyAdPublicationMap()
        {
            this.ToTable("LOOK_SendMyAdPublication");
            this.HasKey(t => t.Id);
            Property(t => t.Id)
                .HasColumnName("SendMyAdPublicationId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //Relationships
            HasRequired(t => t.MagazineIssue)
                .WithMany()
                .Map(m => m.MapKey("MagazineIssueId"));
        }
    }
}