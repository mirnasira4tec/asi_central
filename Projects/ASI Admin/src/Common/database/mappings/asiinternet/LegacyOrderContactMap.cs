using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class LegacyOrderContactMap : EntityTypeConfiguration<LegacyOrderContact>
    {
        public LegacyOrderContactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.FirstName)
                .HasMaxLength(250);

            this.Property(t => t.LastName)
                .HasMaxLength(250);

            this.Property(t => t.Title)
                .HasMaxLength(250);

            this.Property(t => t.Company)
                .HasMaxLength(500);

            this.Property(t => t.ASINo)
                .HasMaxLength(35);

            this.Property(t => t.Phone)
                .HasMaxLength(50);

            this.Property(t => t.Fax)
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .HasMaxLength(250);

            this.Property(t => t.ShowContact)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("STOR_OrderContact_OCNT");
            this.Property(t => t.Id).HasColumnName("OCNT_ContactID");
            this.Property(t => t.OrderId).HasColumnName("ORDR_OrderID");
            this.Property(t => t.ProdID).HasColumnName("PROD_ProdID");
            this.Property(t => t.FirstName).HasColumnName("OCNT_FName");
            this.Property(t => t.LastName).HasColumnName("OCNT_LName");
            this.Property(t => t.Title).HasColumnName("OCNT_Title");
            this.Property(t => t.Company).HasColumnName("OCNT_Company");
            this.Property(t => t.ASINo).HasColumnName("OCNT_ASINo");
            this.Property(t => t.Phone).HasColumnName("OCNT_Phone");
            this.Property(t => t.Fax).HasColumnName("OCNT_Fax");
            this.Property(t => t.Email).HasColumnName("OCNT_Email");
            this.Property(t => t.ShowContact).HasColumnName("OCNT_ShowContact");
        }
    }
}
