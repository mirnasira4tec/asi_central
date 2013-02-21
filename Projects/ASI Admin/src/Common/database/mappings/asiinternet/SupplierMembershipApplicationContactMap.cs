using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class SupplierMembershipApplicationContactMap : EntityTypeConfiguration<SupplierMembershipApplicationContact>
    {
        public SupplierMembershipApplicationContactMap()
        {
            this.ToTable("CENT_SuppJoinAppContact_SAPP");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("SAPP_ContactID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Name)
                .HasColumnName("SAPP_ContactName")
                .HasMaxLength(50);

            this.Property(t => t.Title)
                .HasColumnName("SAPP_ContactTitle")
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .HasColumnName("SAPP_ContactEmail")
                .HasMaxLength(50);

            this.Property(t => t.Phone)
                .HasColumnName("SAPP_ContactPhone")
                .HasMaxLength(50);

            this.Property(t => t.Fax)
                .HasColumnName("SAPP_ContactFax")
                .HasMaxLength(50);

            this.Property(t => t.SalesId)
                .HasColumnName("SAPP_RepID");

            this.Property(t => t.AppplicationId)
                .HasColumnName("SAPP_AppID");

            this.Property(t => t.Department)
                .HasColumnName("SAPP_Department");

            // Relationships
            this.HasOptional(t => t.SupplierApplication)
                .WithMany(t => t.Contacts)
                .HasForeignKey(d => d.AppplicationId);
        }
    }
}
