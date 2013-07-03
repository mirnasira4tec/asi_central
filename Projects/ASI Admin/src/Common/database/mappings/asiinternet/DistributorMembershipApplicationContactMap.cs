using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class DistributorMembershipApplicationContactMap : EntityTypeConfiguration<LegacyDistributorMembershipApplicationContact>
    {
        public DistributorMembershipApplicationContactMap()
        {
            this.ToTable("CENT_DistJoinAppContact_DAPP");
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasColumnName("DAPP_ContactID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Name)
                .HasColumnName("DAPP_ContactName")
                .HasMaxLength(200);

            this.Property(t => t.Title)
                .HasColumnName("DAPP_ContactTitle")
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .HasColumnName("DAPP_ContactEmail")
                .HasMaxLength(50);

            this.Property(t => t.Phone)
                .HasColumnName("DAPP_ContactPhone")
                .HasMaxLength(50);

            this.Property(t => t.Fax)
                .HasColumnName("DAPP_ContactFax")
                .HasMaxLength(50);

            this.Property(t => t.AppplicationId)
                .HasColumnName("DAPP_AppID");

            this.Property(t => t.Department)
                .HasColumnName("DAPP_Department");

            this.Property(t => t.IsPrimary)
                .HasColumnName("DAPP_Primary");

            // Relationships
            this.HasOptional(t => t.DistributorApplication)
                .WithMany(t => t.Contacts)
                .HasForeignKey(d => d.AppplicationId);
        }
    }
}
