using asi.asicentral.model.news;
using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.product
{
    public class ContextFeatureMap : EntityTypeConfiguration<ContextFeature>
    {
        public ContextFeatureMap()
        {
            this.ToTable("PROD_Feature");
            this.HasKey(t => t.ContextFeatureId);

            //Properties
            this.Property(t => t.ContextFeatureId)
                .HasColumnName("FeatureId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.IsOffer)
                .HasColumnName("Offer");

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //Relationships
            HasMany(feature => feature.ChildFeatures)
                .WithOptional()
                .Map(m => m.MapKey("ParentFeatureId"));

            HasMany(feature => feature.AssociatedProducts)
                .WithOptional()
                .Map(m => { m.MapKey("FeatureId"); });
        }
    }
}
