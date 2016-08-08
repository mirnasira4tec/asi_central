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
                .HasColumnName("STORE_PRODUCT");

            this.Property(t => t.StoreOption)
                .HasColumnName("STORE_OPTION");

            this.Property(t => t.ClassCode)
                .HasColumnName("CUSTOMER_CLASS_CODE");

            this.Property(t => t.SubClassCode)
                .HasColumnName("CUSTOMER_CLASS_SUBCODE");

            this.Property(t => t.PersonifyProduct)
                .HasColumnName("PRODUCT_ID");

            this.Property(t => t.ProductCode)
                .HasColumnName("PRODUCT_CODE");

            this.Property(t => t.PersonifyBundle)
                .HasColumnName("BUNDLE");

            this.Property(t => t.PersonifyRateCode)
                .HasColumnName("RATE_CODE");

            this.Property(t => t.PersonifyRateStructure)
                .HasColumnName("RATE_STRUCTURE")
                .IsRequired();

            this.Property(t => t.ESBSendGlag)
                .HasColumnName("ESB_SEND_FLAG");

            this.Property(t => t.NewAsiNumFlag)
                .HasColumnName("NEW_ASI_NUM_FLAG");

            this.Property(t => t.NotifyByEmailFlag)
                .HasColumnName("NOTIFY_BY_EMAIL_FLAG");

            this.Property(t => t.CreateUserUTC)
                .HasColumnName("CREATE_USER_UTC");

            this.Property(t => t.PaySchedule)
                .HasColumnName("PAYSCHEDULE");
            
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
