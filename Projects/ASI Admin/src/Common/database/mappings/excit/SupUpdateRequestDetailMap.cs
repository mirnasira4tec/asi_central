using asi.asicentral.model.excit;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.excit
{
    public class SupUpdateRequestDetailMap: EntityTypeConfiguration<SupUpdateRequestDetail>
    {
        public SupUpdateRequestDetailMap()
        {
            this.ToTable("EXCT_SupUpdateRequestDetail");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("SupUpdateRequestDetailId")
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
