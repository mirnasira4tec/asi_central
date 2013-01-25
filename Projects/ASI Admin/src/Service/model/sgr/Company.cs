﻿using System;
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

        [Display(ResourceType = typeof(Resource), Name = "CompanyID")]
        [Required]
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanyName")]
        [Required]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanySummary")]
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