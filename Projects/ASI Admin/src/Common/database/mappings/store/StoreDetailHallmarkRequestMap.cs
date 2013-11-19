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
    public class StoreDetailHallmarkRequestMap : EntityTypeConfiguration<StoreDetailHallmarkRequest>
    {
        public StoreDetailHallmarkRequestMap()
        {
            this.ToTable("STOR_Hallmark");
            this.HasKey(t => t.OrderDetailId);

            //Properties
            this.Property(t => t.OrderDetailId)
                .HasColumnName("OrderDetailId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");
        }
    }
}
