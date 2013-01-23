using asi.asicentral.model.sgr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings
{
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            ToTable("CENT_SGRInternCategoryDescription_SGCD");
            HasKey(category => category.Id);
            HasMany(category => category.Companies)
                .WithMany(company => company.Categories)
                .Map(category =>
                {
                    category.MapLeftKey("CategoryID");
                    category.MapRightKey("SGRC_SGRInternCompanyID");
                    category.ToTable("CENT_SGRInternCompanyCategorySICC");
                });
            HasMany(category => category.Products)
                .WithMany(product => product.Categories)
                .Map(category =>
                {
                    category.MapLeftKey("CategoryID");
                    category.MapRightKey("SGRS_SpecsID");
                    category.ToTable("CENT_SGRInternSpecsCatDescSGIC");
                });
            

            Property(category => category.Id)
                .HasColumnName("CategoryID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(category => category.Name)
                .HasColumnName("CategoryDescription");
        }
    }
}
