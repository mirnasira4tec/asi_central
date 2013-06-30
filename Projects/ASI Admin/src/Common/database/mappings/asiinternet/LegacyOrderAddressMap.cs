using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class LegacyOrderAddressMap : EntityTypeConfiguration<LegacyOrderAddress>
    {
        public LegacyOrderAddressMap()
        {
            // Primary Key
            this.HasKey(t => t.SPAD_AddressID);

            // Properties
            this.Property(t => t.SPAD_FirstName)
                .HasMaxLength(150);

            this.Property(t => t.SPAD_LastName)
                .HasMaxLength(150);

            this.Property(t => t.SPAD_Street1)
                .HasMaxLength(250);

            this.Property(t => t.SPAD_Street2)
                .HasMaxLength(250);

            this.Property(t => t.SPAD_City)
                .HasMaxLength(250);

            this.Property(t => t.SPAD_StateID)
                .HasMaxLength(15);

            this.Property(t => t.SPAD_Zip)
                .HasMaxLength(25);

            this.Property(t => t.SPAD_Email)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("STOR_SPAddresses_SPAD");
            this.Property(t => t.SPAD_AddressID).HasColumnName("SPAD_AddressID");
            this.Property(t => t.SPAD_FirstName).HasColumnName("SPAD_FirstName");
            this.Property(t => t.SPAD_LastName).HasColumnName("SPAD_LastName");
            this.Property(t => t.SPAD_Street1).HasColumnName("SPAD_Street1");
            this.Property(t => t.SPAD_Street2).HasColumnName("SPAD_Street2");
            this.Property(t => t.SPAD_City).HasColumnName("SPAD_City");
            this.Property(t => t.SPAD_StateID).HasColumnName("SPAD_StateID");
            this.Property(t => t.SPAD_Zip).HasColumnName("SPAD_Zip");
            this.Property(t => t.SPAD_Email).HasColumnName("SPAD_Email");
        }
    }
}
