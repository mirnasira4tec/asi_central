using System;
using asi.asicentral.model.show.form;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.show.form
{
    public class SHW_FormQuestionMap : EntityTypeConfiguration<SHW_FormQuestion>
    {
        public SHW_FormQuestionMap()
        {
            this.ToTable("SHW_FormQuestion");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("FormQuestionId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            HasRequired(q => q.FormType)
                .WithMany(t => t.FormQuestions)
                .HasForeignKey(q => q.FormTypeId)
                .WillCascadeOnDelete();
        }
    }
}
