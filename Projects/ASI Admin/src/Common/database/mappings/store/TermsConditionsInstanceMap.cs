using asi.asicentral.model.store;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.TermCondition
{
    public class TermsConditionsInstanceMap: EntityTypeConfiguration<TermsConditionsInstance>
    {
        public TermsConditionsInstanceMap()
        {
            this.ToTable("TERM_Instance");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("InstanceId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.GUID)
                .HasColumnName("GUID");

            this.Property(t => t.CustomerName)
                .HasColumnName("CustomerName")
                .IsRequired();

            this.Property(t => t.CustomerEmail)
                .HasColumnName("CustomerEmail")
                .IsRequired();

            this.Property(t => t.CompanyName)
                .HasColumnName("CompanyName");

            this.Property(t => t.IPAddress)
                .HasColumnName("IPAddress");

            this.Property(t => t.OrderId)
                .HasColumnName("OrderId");

            this.Property(t => t.TypeId)
                .HasColumnName("TypeId")
                .IsRequired();

            this.Property(t => t.DateAgreedOn)
                .HasColumnName("DateAgreedOn");

            this.Property(t => t.CreatedBy)
                .HasColumnName("CreatedBy");

            this.Property(t => t.LastUpdatedBy)
                .HasColumnName("LastUpdatedBy");

            this.Property(t => t.NotificationEmail)
                .HasColumnName("NotificationEmail");

            this.Property(t => t.Messages)
                .HasColumnName("Messages");

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            this.Property(t => t.UpdateSource)
                .HasColumnName("UpdateSource");

            //relationships
            HasRequired(t => t.TermsAndConditions)
                .WithMany()
                .HasForeignKey(t => t.TypeId);

            HasOptional(t => t.StoreOrder)
                .WithMany(order => order.TermsConditionsInstance)
                .HasForeignKey(t => t.OrderId)
                .WillCascadeOnDelete();
        }    
    }
}
