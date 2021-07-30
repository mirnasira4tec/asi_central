using asi.asicentral.model.asicentral;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.asicentral
{
    class AsicentralFormQuestionMap : EntityTypeConfiguration<AsicentralFormQuestion>
    {
        public AsicentralFormQuestionMap()
        {
            ToTable("USR_FormQuestion");
            HasKey(t => t.Id);

            //Properties
            Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            HasRequired(q => q.FormType)
                .WithMany(t => t.FormQuestions)
                .HasForeignKey(q => q.FormTypeId)
                .WillCascadeOnDelete();
        }
    }
}
