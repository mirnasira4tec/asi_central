﻿using asi.asicentral.model.show;
using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.models.show
{
    public class CompanyInformation : AddressModel
    {
        public string Name { get; set; }

        public int Id { get; set; }
        public IList<ShowCompanyAddress> CompanyAddress { set; get; }
        public int EmployeeId { get; set; }
        public IList<ShowAddress> Address { set; get; }
        public IList<ShowEmployee> Employee { set; get; }
        public bool HasAddress { get; set; }
         [Display(ResourceType = typeof(Resource), Name = "FirstName")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength100")]
        public string FirstName { get; set; }

         [Display(ResourceType = typeof(Resource), Name = "LastName")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength100")]
        public string LastName { get; set; }
        
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength50")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email")]
        [Remote("IsValidEmail", "ShowCompany", HttpMethod = "POST", ErrorMessage = "Email already exists. Please enter a different Email.")]
        public string Email { get; set; }

        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email")]
        [Remote("IsValidLoginEmail", "ShowCompany", HttpMethod = "POST", ErrorMessage = "Login Email already exists. Please enter a different Login Email.")]
        public string LoginEmail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Phone")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength50")]
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidPhoneNumber", ErrorMessageResourceType = typeof(Resource))]
        public string EPhone { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PhoneAreaCode")]
        [RegularExpression(@"^[2-9][0-9]\d$", ErrorMessageResourceName = "FieldInvalidPhoneAreaCode", ErrorMessageResourceType = typeof(Resource))]
        public string EPhoneAreaCode { get; set; }
    }
}