using asi.asicentral.model.DM_memberDemogr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.database.mappings.DM_memberDemogr
{
    public class CompanyASIRepMap : EntityTypeConfiguration<CompanyASIRep>
    {
        public CompanyASIRepMap()
        {
            this.ToTable("vwCompanyASIRep");
            this.HasKey(t => new { t.CompanyID, t.IndividualID, t.IndividualRoleCode });

            this.Property(t => t.CompanyID)
                .HasColumnName("Company_ID");

            this.Property(t => t.IndividualID)
                .HasColumnName("Individual_ID");

            this.Property(t => t.CustomerStatusCode)
                .HasColumnName("CustomerStatus_CD");

            this.Property(t => t.MasterCustomerID)
                .HasColumnName("MasterCustomer_ID");

            this.Property(t => t.SubCustomerID)
                .HasColumnName("Sub_Customer_ID");

            this.Property(t => t.FirstName)
                .HasColumnName("First_NM");

            this.Property(t => t.MidleName)
                .HasColumnName("Middle_NM");

            this.Property(t => t.LastName)
                .HasColumnName("Last_NM");

            this.Property(t => t.NickName)
                .HasColumnName("Nick_NM");

            this.Property(t => t.SalutationFormal)
                .HasColumnName("Salutation_Formal");

            this.Property(t => t.SalutationInformal)
                .HasColumnName("Salutation_Informal");

            this.Property(t => t.IndividualRoleCode)
                .HasColumnName("IndividualRole_CD");

            this.Property(t => t.RoleDescription)
                .HasColumnName("Role_Desc");

            this.Property(t => t.PrimaryEmail)
               .HasColumnName("Primary_Email");

            this.Property(t => t.PrimaryAreaCode)
                .HasColumnName("Primary_Area_Code");

            this.Property(t => t.PrimaryPhoneBase)
                .HasColumnName("Primary_Phone_Base");

            this.Property(t => t.PrimaryPhoneExtension)
               .HasColumnName("Primary_Phone_Extn");

            this.Property(t => t.PrimaryPhoneFormatted)
                .HasColumnName("Primary_Phone_Formatted");


        }
    }
}
