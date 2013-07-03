using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
            HasKey(prodSequence => prodSequence.Id);

            this.Property(t => t.Id)
                .HasColumnName("ContextProductSequenceId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.PageNumber)
                .HasColumnName("PageNb");

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //Relationships
            HasRequired(ctxProduct => ctxProduct.Product)
                .WithMany()
                .Map(m => m.MapKey("ProductId"));
        }
    }
}
