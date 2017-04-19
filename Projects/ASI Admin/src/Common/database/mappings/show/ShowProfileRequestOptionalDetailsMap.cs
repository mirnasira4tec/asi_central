using asi.asicentral.model.show;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.show
{
    class ShowProfileRequestOptionalDetailsMap : EntityTypeConfiguration<ShowProfileRequestOptionalDetails>
    {
        public ShowProfileRequestOptionalDetailsMap()
        {
            this.ToTable("ATT_ProfileRequestOptionalDetails");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("ProfileRequestOptionalDetailId")
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
