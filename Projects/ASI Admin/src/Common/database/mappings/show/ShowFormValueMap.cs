using asi.asicentral.model.show;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.show
{
    public class ShowFormValueMap : EntityTypeConfiguration<ShowFormPropertyValue>
    {
        public ShowFormValueMap()
        {
            this.ToTable("FRM_PropertyValue");
            this.HasKey(t => t.PropertyValueId);

            //Properties
            this.Property(t => t.PropertyValueId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.FormInstanceId)
                .HasColumnName("InstanceId");

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");
        }
    }
}
