using asi.asicentral.model.show;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.show
{
    public class ShowFormTypeMap : EntityTypeConfiguration<ShowFormType>
    {
        public ShowFormTypeMap()
        {
            this.ToTable("FRM_Type");
            this.HasKey(t => t.TypeId);

            //Properties
            this.Property(t => t.TypeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");
        }
    }
}
