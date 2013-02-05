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
    internal class CompanyMap :  EntityTypeConfiguration<Company>
    {
        public CompanyMap()
        {
            ToTable("CENT_SGRInternCompany_SGRC");
            HasKey(company => company.Id);
            Property(company => company.Id)
                .HasColumnName("SGRC_SGRInternCompanyID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(company => company.Name)
                .HasColumnName("SGRC_SGRInternCompDesc");
            Property(company => company.Summary)
                .HasColumnName("SGRC_Intro");
            Property(company => company.ASINumber)
                .HasColumnName("SGRC_SGRInternASINo");
            Property(company => company.IsInactive)
                .HasColumnName("SGRC_SGRInternCoDeleted");
            Property(company => company.BusinessType)
                .HasColumnName("SGRC_BusinessType");
            Property(company => company.Address)
                .HasColumnName("SGRC_Address");
            Property(company => company.ProductionTime)
                .HasColumnName("SGRC_ProdTime");
            Property(company => company.NumberOfEmployees)
                .HasColumnName("SGRC_NoOfEmployees");
            Property(company => company.Email)
                .HasColumnName("SGRC_Email");
            Property(company => company.URL)
                .HasColumnName("SGRC_WebAddress");
            Property(company => company.AnnualSalesTurnover)
                .HasColumnName("SGRC_AnnualSalesTurnover");
            Property(company => company.CountryCode)
                .HasColumnName("SGRC_CountryCode");
            Property(company => company.PhoneAreaCode)
                .HasColumnName("SGRC_AreaCode");
            Property(company => company.Phone)
                .HasColumnName("SGRC_PhoneNo");
            Property(company => company.PhoneExtension)
                .HasColumnName("SGRC_ExtNo");
            Property(company => company.FaxCountryCode)
                .HasColumnName("SGRC_FaxCountryCode");
            Property(company => company.FaxAreaCode)
                .HasColumnName("SGRC_FaxAreaCode");
            Property(company => company.Fax)
                .HasColumnName("SGRC_FaxNo");
            Property(company => company.FaxExtension)
                .HasColumnName("SGRC_FaxExtNo");
            Property(company => company.ContactName)
                .HasColumnName("SGRC_CompanyContact");
            Property(company => company.ContactEmail)
                .HasColumnName("SGRC_EmailOfResponsiblePerson");
            Property(company => company.YearEstablished)
                .HasColumnName("SGRC_YearEst");
        }
    }
}
