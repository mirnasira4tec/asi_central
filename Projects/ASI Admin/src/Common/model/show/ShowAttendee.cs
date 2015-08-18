using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
  public class ShowAttendee
    {
        public int Id { get; set; }
        public int ShowId { get; set; }
        public int AddressId { get; set; }
        public int CompanyId { get; set; }
        public bool IsAttending { get; set; }
        public bool IsSponsor { get; set; }
        public bool IsExhibitDay { get; set; }
        public string BoothNumber { get; set; }
        public ShowAddress Address { get; set; }
        public virtual ShowASI Show { get; set; }
        public virtual ShowCompany Company { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
