using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.model.store;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace asi.asicentral.database.mappings.store
{
    class StoreDetailMagazineAdvertisingItemMap : EntityTypeConfiguration<StoreDetailMagazineAdvertisingItem>
    {
        public StoreDetailMagazineAdvertisingItemMap()
        {
            this.ToTable("STOR_MagazineAdvertisingItem");
            this.HasKey(t => t.Id);
            Property(t => t.Id)
                .HasColumnName("AdItemId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.OrderDetailId).IsRequired();
            
            //Relationships
            HasRequired(t => t.Issue)
                .WithMany()
                .Map(m => m.MapKey("MagazineIssueId"));

            HasRequired(t => t.Position)
               .WithMany()
               .Map(m => m.MapKey("PositionId"));

            HasRequired(t => t.Size)
                .WithMany()
                .Map(m => m.MapKey("SizeId"));
        }
    }   
}