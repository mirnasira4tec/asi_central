using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.asiinternet
{
    public class ASPNetUserMap : EntityTypeConfiguration<ASPNetUser>
    {
        public ASPNetUserMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.LoweredUserName)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.MobileAlias)
                .HasMaxLength(16);

            // Table & Column Mappings
            this.ToTable("aspnet_Users");
            this.Property(t => t.ApplicationId).HasColumnName("ApplicationId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.LoweredUserName).HasColumnName("LoweredUserName");
            this.Property(t => t.MobileAlias).HasColumnName("MobileAlias");
            this.Property(t => t.IsAnonymous).HasColumnName("IsAnonymous");
            this.Property(t => t.LastActivityDate).HasColumnName("LastActivityDate");
        }
    }
}
