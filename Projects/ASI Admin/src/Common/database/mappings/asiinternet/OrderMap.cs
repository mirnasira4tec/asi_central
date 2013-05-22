using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.asiinternet
{
    internal class LegacyOrderMap : EntityTypeConfiguration<Order>
    {
        public LegacyOrderMap()
        {
            this.ToTable("STOR_Orders_ORDR");
            this.HasKey(order => order.Id);

            // Properties
            this.Property(order => order.Id)
                .HasColumnName("ORDR_OrderID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(order => order.UserId)
                .HasColumnName("ORDR_UserID");

            this.Property(order => order.TransId).
                HasColumnName("ORDR_TransID");

            this.Property(order => order.DateCreated)
                .HasColumnName("ORDR_DateCreated");

            this.Property(order => order.BillFirstName)
                .HasColumnName("ORDR_BillFirstName")
                .HasMaxLength(150);

            this.Property(order => order.BillLastName)
                .HasColumnName("ORDR_BillLastName")
                .HasMaxLength(150);

            this.Property(order => order.BillStreet1)
                .HasColumnName("ORDR_BillStreet1")
                .HasMaxLength(250);

            this.Property(order => order.BillStreet2)
                .HasColumnName("ORDR_BillStreet2")
                .HasMaxLength(250);

            this.Property(order => order.BillCity)
                .HasColumnName("ORDR_BillCity")
                .HasMaxLength(150);

            this.Property(order => order.BillState)
                .HasColumnName("ORDR_BillState")
                .HasMaxLength(50);

            this.Property(order => order.BillZip)
                .HasColumnName("ORDR_BillZip")
                .HasMaxLength(25);

            this.Property(order => order.BillCountry)
                .HasColumnName("ORDR_BillCountry")
                .HasMaxLength(15);

            this.Property(order => order.BillPhone)
                .HasColumnName("ORDR_BillPhone")
                .HasMaxLength(25);

            this.Property(order => order.IPAdd)
                .HasColumnName("ORDR_IPAdd")
                .HasMaxLength(35);

            this.Property(t => t.Status)
                .HasColumnName("ORDR_Status");

            this.Property(t => t.OrderTypeId)
                .HasColumnName("ORTY_OrderTypeID");

            this.Property(t => t.ExternalReference)
                .HasColumnName("ORDR_ExternalReference")
                .HasMaxLength(150);

            this.Property(t => t.Campaign)
                .HasColumnName("ORDR_Campaign")
                .HasMaxLength(150);

            this.Property(t => t.ProcessStatus)
                .HasColumnName("ORDR_ProcessStatus");

            this.Property(t => t.CompletedStep)
                .HasColumnName("ORDR_CompletedStep");

            this.Property(t => t.ContextId)
                .HasColumnName("ORDR_ContextID");

            // Relationships
            this.HasOptional(order => order.CreditCard)
                .WithRequired();

            this.HasOptional(order => order.Membership)
                .WithMany()
                .HasForeignKey(order => order.UserId);
        }
    }
}
