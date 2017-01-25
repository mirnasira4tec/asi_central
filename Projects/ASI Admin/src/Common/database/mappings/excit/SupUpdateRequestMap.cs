using asi.asicentral.model.excit;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.excit
{
    public class SupUpdateRequestMap: EntityTypeConfiguration<SupUpdateRequest>
    {
        public SupUpdateRequestMap()
        {
            this.ToTable("EXCT_SupUpdateRequest");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("SupUpdateRequestId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            this.Property(t => t.UpdateSource)
                .HasColumnName("UpdateSource");
        }    
    }
}
