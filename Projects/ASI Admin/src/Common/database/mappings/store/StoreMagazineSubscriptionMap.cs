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
    public class StoreMagazineSubscriptionMap : EntityTypeConfiguration<StoreMagazineSubscription>
    {
        public StoreMagazineSubscriptionMap()
        {
            this.ToTable("STOR_MagazineSubscription");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("MagazineSubscriptionId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //relationships
            HasOptional(t => t.Contact)
                .WithMany()
                .Map(t => t.MapKey("IndividualId"));
        }
    }
}
