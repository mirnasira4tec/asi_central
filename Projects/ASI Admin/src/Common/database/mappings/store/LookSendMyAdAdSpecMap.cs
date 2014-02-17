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

    public class LookSendMyAdAdSpecMap : EntityTypeConfiguration<LookSendMyAdAdSpec>
    {

        public LookSendMyAdAdSpecMap()
        {
            this.ToTable("LOOK_SendMyAdAdSpec");
            this.HasKey(t => t.Id);
            Property(t => t.Id)
                .HasColumnName("SendMyAdAdSpecId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            //Relationships
            HasRequired(t => t.Size)
                .WithMany()
                .Map(m => m.MapKey("AdSizeId"));
        }
    }
}