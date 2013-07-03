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
    public class StoreOrderMap : EntityTypeConfiguration<StoreOrder>
    {
        public StoreOrderMap()
        {
            this.ToTable("STOR_Order");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("OrderId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.ApprovedDate)
                .HasColumnName("ApprovedDateUTC");

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            // Relationships
            HasOptional(order => order.CreditCard)
                .WithMany()
                .Map(order => order.MapKey("CreditCardId"));

            HasOptional(order => order.Company)
                .WithMany()
                .Map(order => order.MapKey("CompanyId"));

            HasOptional(order => order.BillingIndividual)
                .WithMany()
                .Map(order => order.MapKey("BillingIndividualId"));
        }
    }
}
