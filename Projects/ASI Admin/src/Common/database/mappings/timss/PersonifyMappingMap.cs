using asi.asicentral.model.timss;
using System.Data.Entity.ModelConfiguration;


namespace asi.asicentral.database.mappings.timss
{
    public class PersonifyMappingMap : EntityTypeConfiguration<PersonifyMapping>
    {
        public PersonifyMappingMap()
        {
            this.ToTable("PERS_PRODUCT_MAPPING");
            this.HasKey(t => new { t.Identifier });
            this.Ignore(t => t.ItemCount);
            this.Ignore(t => t.Quantity);

            // Properties
            this.Property(t => t.Identifier)
                .HasColumnName("Identifier")
                .IsRequired();

            this.Property(t => t.StoreContext)
                .HasColumnName("Store_Context");

            this.Property(t => t.StoreProduct)
                .HasColumnName("Store_Product")
                .IsRequired();

            this.Property(t => t.StoreOption)
                .HasColumnName("Store_Option")
                .IsRequired();

            this.Property(t => t.PersonifyProduct)
                .HasColumnName("Pers_Product")
                .IsRequired();

            this.Property(t => t.PersonifyRateCode)
                .HasColumnName("Pers_RateCode")
                .IsRequired();

            this.Property(t => t.PersonifyRateStructure)
                .HasColumnName("Pers_RateStructure")
                .IsRequired();

            this.Property(t => t.CreateDateUTC)
                .HasColumnName("CreateDateUTC")
                .IsRequired();

            this.Property(t => t.UpdateDateUTC)
                .HasColumnName("UpdateDateUTC")
                .IsRequired();

            this.Property(t => t.UpdateSource)
                .HasColumnName("UpdateSource")
                .IsRequired();
        }
    }
}
