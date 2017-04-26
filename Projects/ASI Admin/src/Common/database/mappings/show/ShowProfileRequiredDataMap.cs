using asi.asicentral.model.show;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.show
{
    class ShowProfileRequiredDataMap : EntityTypeConfiguration<ShowProfileRequiredData>
    {
        public ShowProfileRequiredDataMap()
        {
            this.ToTable("ATT_ProfileRequiredData");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("ProfileRequiredDataId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            this.Property(t => t.UpdateSource)
                .HasColumnName("UpdateSource");

            HasRequired(x => x.ProfileRequests)
           .WithMany(x => x.ProfileRequiredData)
           .HasForeignKey(x=>x.ProfileRequestId);

        }  
    }
}
