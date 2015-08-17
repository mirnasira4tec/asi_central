using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using asi.asicentral.model.show;

namespace asi.asicentral.database.mappings.show
{
   public class ShowMap : EntityTypeConfiguration<Show>
    {
       public ShowMap()
        {
            this.ToTable("Show");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("ShowId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.ShowTypeId)
                 .HasColumnName("TypeId");

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            //relationship  

            HasMany(t => t.Attendee)
                .WithOptional()
                .HasForeignKey(t => t.ShowId)
                .WillCascadeOnDelete();

            HasRequired(x => x.ShowType)
              .WithMany()
              .HasForeignKey(x => x.ShowTypeId);

        }
    }
}
