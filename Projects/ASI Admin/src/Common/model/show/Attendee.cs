using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
  public class Attendee
    {
        public int Id { get; set; }
        public int ShowId { get; set; }
        public int AddressId { get; set; }
        public int CompanyId { get; set; }
        public Address Address { get; set; }
        public virtual Show show { get; set; }
        public virtual ShowCompany Company { get; set; }
        //public virtual EmployeeAttendee EmployeeAttendee { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
