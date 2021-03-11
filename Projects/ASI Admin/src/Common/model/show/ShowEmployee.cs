using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public class ShowEmployee
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string LoginEmail { get; set; }
        public string EPhoneAreaCode { get; set; }
        public string EPhone { get; set; }
        public int? AddressId { get; set; }
        public int? CompanyId { get; set; }
        public virtual ShowAddress Address { get; set; }
        public virtual ShowCompany Company { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
