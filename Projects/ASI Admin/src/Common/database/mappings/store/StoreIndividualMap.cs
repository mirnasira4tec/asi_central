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
    public class StoreIndividualMap : EntityTypeConfiguration<StoreIndividual>
    {
        public StoreIndividualMap()
        {
            this.ToTable("STOR_Individual");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("IndividualId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //Relationships
            HasOptional(t => t.Address)
                .WithMany()
                .Map(order => order.MapKey("AddressId"));

            this.HasOptional(t => t.Company)
                .WithMany(t => t.Individuals)
                .Map(t => t.MapKey("CompanyId"));
        }
    }
}
