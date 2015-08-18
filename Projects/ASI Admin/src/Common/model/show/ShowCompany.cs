using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public class ShowCompany
    {
        public ShowCompany()
        {
            if (this.GetType() == typeof(ShowCompany))
            {
                CompanyAddress = new List<ShowCompanyAddress>();
                Employee = new List<ShowEmployee>();
                Attendee = new List<ShowAttendee>();
            }
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string WebUrl { get; set; }
        public string MemberType { get; set; }
        public string ASINumber { get; set; }
        public virtual List<ShowCompanyAddress> CompanyAddress { get; set; }
        public virtual List<ShowEmployee> Employee { get; set; }
        public virtual List<ShowAttendee> Attendee { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
