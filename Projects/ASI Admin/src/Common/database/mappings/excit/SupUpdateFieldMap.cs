using asi.asicentral.model.excit;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.excit
{
    class SupUpdateFieldMap: EntityTypeConfiguration<SupUpdateField>
    {
        public SupUpdateFieldMap()
        {
            this.ToTable("EXCT_SupUpdateField");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("SupUpdateFieldId")
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
