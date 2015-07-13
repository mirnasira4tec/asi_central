﻿using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.store
{
    public class TermsConditionsTypeMap : EntityTypeConfiguration<TermsConditionsType>
    {
        public TermsConditionsTypeMap()
        {
            this.ToTable("TERM_Type");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("TypeId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            this.Property(t => t.UpdateSource)
                .HasColumnName("UpdateSource");
        }    
    }
}
