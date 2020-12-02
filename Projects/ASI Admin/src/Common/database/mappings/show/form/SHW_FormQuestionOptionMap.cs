using asi.asicentral.model.show.form;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace asi.asicentral.database.mappings.show.form
{
    class SHW_FormQuestionOptionMap : EntityTypeConfiguration<SHW_FormQuestionOption>
    {
        public SHW_FormQuestionOptionMap()
        {
            this.ToTable("SHW_FormQuestionOption");
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Id)
                .HasColumnName("FormQuestionOptionId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CreateDate)
                .HasColumnName("CreateDateUTC");

            this.Property(t => t.UpdateDate)
                .HasColumnName("UpdateDateUTC");

            HasRequired(q => q.FormQuestion)
                .WithMany(t => t.QuestionOptions)
                .HasForeignKey(q => q.FormQuestionId);                
        }
    }
}
