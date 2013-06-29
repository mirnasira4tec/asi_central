using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class LegacyOrderDistributorAddressMap : EntityTypeConfiguration<LegacyOrderDistributorAddress>
    {
        public LegacyOrderDistributorAddressMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(250);

            this.Property(t => t.Contact)
                .HasMaxLength(250);

            this.Property(t => t.Address)
                .HasMaxLength(250);

            this.Property(t => t.City)
                .HasMaxLength(150);

            this.Property(t => t.State)
                .HasMaxLength(15);

            this.Property(t => t.Zip)
                .HasMaxLength(50);

            this.Property(t => t.Phone)
                .HasMaxLength(50);

            this.Property(t => t.Fax)
                .HasMaxLength(50);

            this.Property(t => t.WebAdd)
                .HasMaxLength(350);

            // Table & Column Mappings
            this.ToTable("STOR_SPDistAdd_DADD");
            this.Property(t => t.Id).HasColumnName("DADD_SPDistAdd_ID");
            this.Property(t => t.Name).HasColumnName("DADD_SPDistAddName");
            this.Property(t => t.Contact).HasColumnName("DADD_SPDistContact");
            this.Property(t => t.Address).HasColumnName("DADD_SPDistAdd");
            this.Property(t => t.City).HasColumnName("DADD_SPDistCity");
            this.Property(t => t.State).HasColumnName("DADD_SPDistState");
            this.Property(t => t.Zip).HasColumnName("DADD_SPDistZip");
            this.Property(t => t.Phone).HasColumnName("DADD_SPDistPhone");
            this.Property(t => t.Fax).HasColumnName("DADD_SPDistFax");
            this.Property(t => t.WebAdd).HasColumnName("DADD_SPWebAdd");
        }
    }
}
