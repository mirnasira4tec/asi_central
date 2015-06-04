using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.store
{
    public class StoreOrderMap : EntityTypeConfiguration<StoreOrder>
    {
        public StoreOrderMap()
        {
            ToTable("STOR_Order");
            HasKey(t => t.Id);

            //Properties
            Property(t => t.Id)
                .HasColumnName("OrderId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.ApprovedDate)
                .HasColumnName("ApprovedDateUTC");

            Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            Property(t => t.UpdateDate)
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

            HasOptional(order => order.Context)
                .WithMany()
                .HasForeignKey(order => order.ContextId);
        }
    }
}
