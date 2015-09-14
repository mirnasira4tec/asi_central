using asi.asicentral.model.show;
using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;

namespace asi.asicentral.web.models.show
{
    public class CompanyModel 
    {

        public int Id { get; set; }

        public int AddressId { get; set; }

        public const String TAB_COMPANYNAME = "companyName";
        public String CompanyTab { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsNonUSAddress")]
        public bool IsNonUSAddress { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanyName")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength100")]
        [Remote("IsValidCompany", "ShowCompany", HttpMethod = "POST", ErrorMessage = "The Name already exists.")]
        public string Name { get; set; }

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
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength100")]
        public string Country { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Phone")]
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidPhoneNumber", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PhoneAreaCode")]
        [RegularExpression(@"^[2-9][0-9]\d$", ErrorMessageResourceName = "FieldInvalidPhoneAreaCode", ErrorMessageResourceType = typeof(Resource))]
        public string PhoneAreaCode { get; set; }

        [RegularExpression(@"^[2-9]{1}[0-9]{6,6}$", ErrorMessageResourceName = "FieldInvalidFax", ErrorMessageResourceType = typeof(Resource))]
        public string Fax { get; set; }

        
        [RegularExpression(@"^[2-9][0-9]\d$", ErrorMessageResourceName = "FieldInvalidFaxAreaCode", ErrorMessageResourceType = typeof(Resource))]
        public string FaxAreaCode { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CompanyURL", Prompt = "CompanyURLPrompt")]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string Url { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ASINumber")]
        [RegularExpression(@"^[1-9][0-9]{3,5}$", ErrorMessageResourceName = "FieldInvalidASINumber", ErrorMessageResourceType = typeof(Resource))]
        public string ASINumber { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "MemberType")]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public string MemberType { set; get; }
       
        public IList<ShowCompany> company { set; get; }
        public static IList<SelectListItem> GetMemberTypes()
        {
            IList<SelectListItem> selItems = new List<SelectListItem>();
            selItems.Add(new SelectListItem() { Selected = true, Text = "Select ", Value ="" });
            selItems.Add(new SelectListItem() { Selected = false, Text = "Distributor", Value = "Distributor" });
            selItems.Add(new SelectListItem() { Selected = false, Text = "Supplier", Value = "Supplier" });
            selItems.Add(new SelectListItem() { Selected = false, Text = "Non-Member", Value = "Non-Member" });
            return selItems;
        }
    }
}