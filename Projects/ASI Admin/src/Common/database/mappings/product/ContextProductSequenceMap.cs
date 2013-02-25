﻿using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.product
{
    public class ContextProductSequenceMap : EntityTypeConfiguration<ContextProductSequence>
    {
        public ContextProductSequenceMap()
        {
            ToTable("PROD_ContextProduct");
            HasKey(prodSequence => prodSequence.ContextProductSequenceId);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //Relationships
            HasRequired(ctxProduct => ctxProduct.Product)
                .WithOptional()
                .Map(m => m.MapKey("ProductId"));
        }
    }
}
