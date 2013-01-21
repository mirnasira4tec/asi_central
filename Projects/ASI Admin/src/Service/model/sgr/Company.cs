using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.sgr
{
    public class Company
    {
        public Company()
        {
            if (this.GetType() == typeof(Category))
            {
                Products = new List<Product>();
                Categories = new List<Category>();
            }
        }

        [Display(Name="Id")]
        [Required]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Summary")]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        [Display(Name = "ASI Number")]
        public string ASINumber { get; set; }

        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

        [Display(Name = "BusinessType")]
        public string BusinessType { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "ProductionTime")]
        public string ProductionTime { get; set; }

        [Display(Name = "NumberOfEmployees")]
        public string NumberOfEmployees { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "URL")]
        public string URL { get; set; }

        [Display(Name = "AnnualSalesTurnover")]
        public string AnnualSalesTurnover { get; set; }

        [Display(Name = "CountryCode")]
        public string CountryCode { get; set; }

        [Display(Name = "PhoneAreaCode")]
        public string PhoneAreaCode { get; set; }

        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Display(Name = "PhoneExtension")]
        public string PhoneExtension { get; set; }

        [Display(Name = "FaxCountryCode")]
        public string FaxCountryCode { get; set; }

        [Display(Name = "FaxAreaCode")]
        public string FaxAreaCode { get; set; }

        [Display(Name = "Fax")]
        public string Fax { get; set; }

        [Display(Name = "FaxExtension")]
        public string FaxExtension { get; set; }

        [Display(Name = "ContactName")]
        public string ContactName { get; set; }

        [Display(Name = "ContactEmail")]
        public string ContactEmail { get; set; }

        [Display(Name = "YearEstablished")]
        public string YearEstablished { get; set; }

        public virtual IList<Product> Products { get; set; }

        public virtual IList<Category> Categories { get; set; }

        public override string ToString()
        {
            return string.Format("Company: {0} - {1}", Id, Name);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            Company company = obj as Company;
            if (company != null) equals = company.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
