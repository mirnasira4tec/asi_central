using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public class Employee
    {
        public int Id { get; set; }
         [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
         [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }
         [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public int? AddressId { get; set; }
        public int? CompanyId { get; set; }
        public Address Address { get; set; }
        public virtual ShowCompany Company { get; set; }
        //public virtual EmployeeAttendee EmployeeAttendee { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
