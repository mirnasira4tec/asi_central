using asi.asicentral.model.news;
using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.product
{
    public class ContextProductMap : EntityTypeConfiguration<ContextProduct>
    {
        public ContextProductMap()
        {
            this.ToTable("PROD_Product");
            this.HasKey(t => t.ProductId);

            //Properties
            this.Property(t => t.ProductId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");
        }
    }
}
