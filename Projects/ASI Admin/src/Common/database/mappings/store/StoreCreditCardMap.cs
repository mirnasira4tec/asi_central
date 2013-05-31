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
    public class StoreCreditCardMap : EntityTypeConfiguration<StoreCreditCard>
    {
        public StoreCreditCardMap()
        {
            this.ToTable("STOR_CreditCard");
            this.HasKey(t => t.CreditCardId);

            //Properties
            this.Property(t => t.CreditCardId)
                .HasColumnName("CreditCardId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");
        }
    }
}
