﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
   public class Address
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Phone Area code is required")]
        public string PhoneAreaCode { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }
        public string Cell { get; set; }
        public string FaxAreaCode { get; set; }
        public string Fax { get; set; }
        public string Title { get; set; }
        [Required(ErrorMessage = "Street1 is required")]
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        [Required(ErrorMessage = "Zip is required")]
        public string Zip { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }
        [Required(ErrorMessage = "Country code is required")]
        public string CountryCode { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
        //public int CompanyId { get; set; }
        //public virtual ShowCompany Company { get; set; }

      //  public virtual Show Show { get; set; }
       // public virtual Employee Employee { get; set; }
       // public virtual Attendee Attendee { get; set; } 
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

    }
}
