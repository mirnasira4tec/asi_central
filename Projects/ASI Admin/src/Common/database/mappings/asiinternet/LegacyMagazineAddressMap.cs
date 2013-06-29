using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class LegacyMagazineAddressMap : EntityTypeConfiguration<LegacyMagazineAddress>
    {
        public LegacyMagazineAddressMap()
        {
            // Primary Key
            this.HasKey(t => t.MAGA_SubscribeID);

            // Properties
            this.Property(t => t.MAGA_ASINo)
                .HasMaxLength(25);

            this.Property(t => t.MAGA_FName)
                .HasMaxLength(250);

            this.Property(t => t.MAGA_LName)
                .HasMaxLength(250);

            this.Property(t => t.MAGA_Title)
                .HasMaxLength(250);

            this.Property(t => t.MAGA_Company)
                .HasMaxLength(250);

            this.Property(t => t.MAGA_Street1)
                .HasMaxLength(250);

            this.Property(t => t.MAGA_Street2)
                .HasMaxLength(250);

            this.Property(t => t.MAGA_City)
                .HasMaxLength(250);

            this.Property(t => t.MAGA_Zip)
                .HasMaxLength(20);

            this.Property(t => t.MAGA_State)
                .HasMaxLength(15);

            this.Property(t => t.MAGA_Country)
                .HasMaxLength(15);

            this.Property(t => t.MAGA_Phone)
                .HasMaxLength(50);

            this.Property(t => t.MAGA_Fax)
                .HasMaxLength(50);

            this.Property(t => t.MAGA_Email)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("STOR_MagSbcrLong_MAGA");
            this.Property(t => t.MAGA_SubscribeID).HasColumnName("MAGA_SubscribeID");
            this.Property(t => t.MAGA_ASINo).HasColumnName("MAGA_ASINo");
            this.Property(t => t.MAGA_FName).HasColumnName("MAGA_FName");
            this.Property(t => t.MAGA_LName).HasColumnName("MAGA_LName");
            this.Property(t => t.MAGA_Title).HasColumnName("MAGA_Title");
            this.Property(t => t.MAGA_Company).HasColumnName("MAGA_Company");
            this.Property(t => t.MAGA_Street1).HasColumnName("MAGA_Street1");
            this.Property(t => t.MAGA_Street2).HasColumnName("MAGA_Street2");
            this.Property(t => t.MAGA_City).HasColumnName("MAGA_City");
            this.Property(t => t.MAGA_Zip).HasColumnName("MAGA_Zip");
            this.Property(t => t.MAGA_State).HasColumnName("MAGA_State");
            this.Property(t => t.MAGA_Country).HasColumnName("MAGA_Country");
            this.Property(t => t.MAGA_Phone).HasColumnName("MAGA_Phone");
            this.Property(t => t.MAGA_Fax).HasColumnName("MAGA_Fax");
            this.Property(t => t.MAGA_Email).HasColumnName("MAGA_Email");
            this.Property(t => t.MAGA_Digital).HasColumnName("MAGA_Digital");
        }
    }
}
