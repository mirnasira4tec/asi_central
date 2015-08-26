using asi.asicentral.model.show;
using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.models.show
{
    public class AddressModel
    {

        public int AdreessId { get; set; }

        public int CompanyId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsNonUSAddress")]
        public bool IsNonUSAddress { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Address1")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength100")]
        public string Address1 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Address2")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength100")]
        public string Address2 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "City")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(75, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string City { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "State")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(40, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string State { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "InternationalState")]
        [StringLength(40, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string InternationalState { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Zip")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(15, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string Zip { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Country")]
        public string Country { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Phone")]
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidPhoneNumber", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PhoneAreaCode")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        //[RegularExpression(@"^[2-9][0-9]\d$", ErrorMessageResourceName = "FieldInvalidPhoneAreaCode", ErrorMessageResourceType = typeof(Resource))]
        public string PhoneAreaCode { get; set; }

        //[RegularExpression(@"^[2-9]{1}[0-9]{6,6}$", ErrorMessageResourceName = "FieldInvalidFax", ErrorMessageResourceType = typeof(Resource))]
        public string Fax { get; set; }

        //[RegularExpression(@"^[2-9][0-9]\d$", ErrorMessageResourceName = "FieldInvalidFaxAreaCode", ErrorMessageResourceType = typeof(Resource))]
        public string FaxAreaCode { get; set; }

       
    }
}