using asi.asicentral.model.call;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.call
{
    public class CallQueueMap : EntityTypeConfiguration<CallQueue>
    {
        public CallQueueMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(250);

            this.Property(t => t.CreateSource)
                .HasMaxLength(128);

            this.Property(t => t.UpdateSource)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("Cnfg_Queues");
            this.Property(t => t.Id).HasColumnName("Queues_ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.IsForcedClosed).HasColumnName("ForcedClosed");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateSource).HasColumnName("CreateSource");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdateSource).HasColumnName("UpdateSource");
        }
    }
}
