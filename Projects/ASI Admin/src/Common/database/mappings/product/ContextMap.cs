using asi.asicentral.model.news;
using asi.asicentral.model.product;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.internet
{
    public class ContextMap : EntityTypeConfiguration<Context>
    {
        public ContextMap()
        {
            this.ToTable("PROD_Context");
            this.HasKey(t => t.ContextId);

            //Properties
            this.Property(t => t.ContextId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");
        }
    }
}
