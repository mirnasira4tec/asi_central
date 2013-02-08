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
    internal class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            ToTable("CENT_SGRInternSpecs_SGRS");
            HasKey(product => product.Id);
            HasRequired(product => product.Company)
                .WithMany(company => company.Products)
                .Map(company => company.MapKey("SGRC_SGRInternCompanyID"));

            Property(product => product.Id)
                .HasColumnName("SGRS_SpecsID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(product => product.Name)
                .HasColumnName("SGRS_ProdName");
            Property(product => product.ModelNumber)
                .HasColumnName("SGRS_ModelNo");
            Property(product => product.Price)
                .HasColumnName("SGRS_Price");
            Property(product => product.PriceCeiling)
                .HasColumnName("SGRS_PriceCeiling");
            Property(product => product.MinimumOrderQuantity)
                .HasColumnName("SGRS_MinOrderQuan");
            Property(product => product.PaymentTerms)
                .HasColumnName("SGRS_PaymentTerms");
            Property(product => product.KeySpecifications)
                .HasColumnName("SGRS_KeySpecs");
            Property(product => product.ImageSmall)
                .HasColumnName("SGRS_AdImgSm");
            Property(product => product.ImageLarge)
                .HasColumnName("SGRS_AdImgLg");
            Property(product => product.IsInactive)
                .HasColumnName("SGRS_AdDeleted");
        }
    }
}
