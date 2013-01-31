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
            if (this.GetType() == typeof(Company))
            {
                Products = new List<Product>();
                Categories = new List<Category>();
            }
        }

        [Display(ResourceType = typeof(Resource), Name = "CompanyID")]
        [Required]
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanyName")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanySummary")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ASINumber")]
        public string ASINumber { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanyIsActive")]
        public bool IsActive { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanyBusinessType")]
        public string BusinessType { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanyAddress")]
        public string Address { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanyProductionTime")]
        public string ProductionTime { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanyNumOfEmployees")]
        public string NumberOfEmployees { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanyURL")]
        public string URL { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanyAnnualSalesTurnover")]
        public string AnnualSalesTurnover { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CountryCode")]
        public string CountryCode { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PhoneAreaCode")]
        public string PhoneAreaCode { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Phone")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PhoneExt")]
        public string PhoneExtension { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "FaxCountryCode")]
        public string FaxCountryCode { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "FaxAreaCode")]
        public string FaxAreaCode { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Fax")]
        public string Fax { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "FaxExt")]
        public string FaxExtension { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanyContactName")]
        public string ContactName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanyContactEmail")]
        public string ContactEmail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "YearEst")]
        public string YearEstablished { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public void CopyTo(Company company)
        {
            if (company == null) throw new Exception("Cannot copy data to a null Company object");
            company.Id = Id;
            company.Name = Name;
            company.Summary = Summary;
            company.ASINumber = ASINumber;
            company.IsActive = IsActive;
            company.BusinessType = BusinessType;
            company.Address = Address;
            company.ProductionTime = ProductionTime;
            company.NumberOfEmployees = NumberOfEmployees;
            company.Email = Email;
            company.URL = URL;
            company.AnnualSalesTurnover = AnnualSalesTurnover;
            company.CountryCode = CountryCode;
            company.PhoneAreaCode = PhoneAreaCode;
            company.Phone = Phone;
            company.PhoneExtension = PhoneExtension;
            company.FaxCountryCode = FaxCountryCode;
            company.FaxAreaCode = FaxCountryCode;
            company.Fax = Fax;
            company.FaxExtension = FaxExtension;
            company.ContactName = ContactName;
            company.ContactEmail = ContactEmail;
            company.YearEstablished = YearEstablished;
            company.Products = Products;
            company.Categories = Categories;
        }

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