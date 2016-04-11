using asi.asicentral.model.personify;
using System.Data.Entity.ModelConfiguration;


namespace asi.asicentral.database.mappings.personify
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
                .HasColumnName("IDENTIFIER")
                .IsRequired();

            this.Property(t => t.StoreContext)
                .HasColumnName("STORE_CONTEXT");

            this.Property(t => t.StoreProduct)
                .HasColumnName("STORE_PRODUCT")
                .IsRequired();

            this.Property(t => t.StoreOption)
                .HasColumnName("STORE_OPTION")
                .IsRequired();

            this.Property(t => t.ClassCode)
                .HasColumnName("CUSTOMER_CLASS_CODE");

            this.Property(t => t.SubClassCode)
                .HasColumnName("CUSTOMER_CLASS_SUBCODE");

            this.Property(t => t.PersonifyProduct)
                .HasColumnName("PRODUCT_ID")
                .IsRequired();

            this.Property(t => t.PersonifyBundle)
                .HasColumnName("BUNDLE");

            this.Property(t => t.PersonifyRateCode)
                .HasColumnName("RATE_CODE")
                .IsRequired();

            this.Property(t => t.PersonifyRateStructure)
                .HasColumnName("RATE_STRUCTURE")
                .IsRequired();
            
            this.Property(t => t.CreateDateUTC)
                .HasColumnName("CREATE_DATE_UTC")
                .IsRequired();

            this.Property(t => t.UpdateDateUTC)
                .HasColumnName("UPDATE_DATE_UTC")
                .IsRequired();

            this.Property(t => t.UpdateSource)
                .HasColumnName("UPDATE_SOURCE")
                .IsRequired();
        }
    }
}
