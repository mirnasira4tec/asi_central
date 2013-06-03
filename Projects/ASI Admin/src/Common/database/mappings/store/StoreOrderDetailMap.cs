﻿using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.store
{
    public class StoreOrderDetailMap : EntityTypeConfiguration<StoreOrderDetail>
    {
        public StoreOrderDetailMap()
        {
            this.ToTable("STOR_OrderDetail");
            this.HasKey(t => t.StoreOrderDetailId);

            //Properties
            this.Property(t => t.StoreOrderDetailId)
                .HasColumnName("OrderDetailId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //Relationships
            this.HasRequired(detail => detail.Order)
                .WithMany(order => order.OrderDetails)
                .Map(detail => detail.MapKey("OrderId"))
                .WillCascadeOnDelete();

            HasOptional(detail => detail.Product)
                .WithMany()
                .Map(detail => detail.MapKey("ProductId"));
        }
    }
}
