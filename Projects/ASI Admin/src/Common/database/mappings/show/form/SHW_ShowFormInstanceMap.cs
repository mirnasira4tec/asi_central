using asi.asicentral.model.show.form;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.show.form
{
    class SHW_ShowFormInstanceMap : EntityTypeConfiguration<SHW_ShowFormInstance>
    {
        public SHW_ShowFormInstanceMap()
        {
            this.ToTable("SHW_ShowFormInstance");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("ShowFormInstanceId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            HasRequired(m => m.FormInstance)
                .WithMany(i => i.ShowFormInstances)
                .HasForeignKey(si => si.FormInstanceId);

            HasRequired(m => m.Show)
                .WithMany(i => i.ShowFormInstances)
                .HasForeignKey(si => si.ShowId);
        }
    }
}
