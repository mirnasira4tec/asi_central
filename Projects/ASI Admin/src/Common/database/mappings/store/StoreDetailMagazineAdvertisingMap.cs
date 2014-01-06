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

    class StoreMagazineAdvertisingItemMap : EntityTypeConfiguration<StoreMagazineAdvertisingItem>
    {

        public StoreMagazineAdvertisingItemMap()
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
                .Map(m => m.MapKey("MagazineId"));
            HasRequired(t => t.Position)
               .WithMany()
               .Map(m => m.MapKey("PositionId"));
            HasRequired(t => t.Size)
                .WithMany()
                .Map(m => m.MapKey("SizeId"));
        }
    }

    class MagazineIssueMap : EntityTypeConfiguration<MagazineIssue>
    {

        public MagazineIssueMap()
        {
            this.ToTable("Stor_MagazineAdvertisingIssue");
            this.HasKey(t => t.Id);
            Property(t => t.Id)
                .HasColumnName("MagazineIssueId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }

    class AdPositionMap : EntityTypeConfiguration<AdPosition>
    {

        public AdPositionMap()
        {
            this.ToTable("STOR_MagazineAdvertisingAdPosition");
            this.HasKey(t => t.Id);
            Property(t => t.Id)
                .HasColumnName("MagazineAdPositionId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Relationships
            HasRequired(t => t.Issue)
                .WithMany()
                .Map(m => m.MapKey("MagazineId"));
        }
    }

    class AdSizeMap : EntityTypeConfiguration<AdSize>
    {

        public AdSizeMap()
        {
            this.ToTable("STOR_MagazineAdSize");
            this.HasKey(t => t.Id);
            Property(t => t.Id)
                .HasColumnName("MagazineAdSizeId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Relationships
            HasRequired(t => t.Issue)
                .WithMany()
                .Map(m => m.MapKey("MagazineId"));
        }
    }
}