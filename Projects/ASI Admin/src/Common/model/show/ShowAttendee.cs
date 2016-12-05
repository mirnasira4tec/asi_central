using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
  public class ShowAttendee
    {
      public ShowAttendee()
      {
          if (this.GetType() == typeof(ShowAttendee))
          {
              EmployeeAttendees = new List<ShowEmployeeAttendee>();
          }
      }
        public int Id { get; set; }
        public int? ShowId { get; set; }
        public int? CompanyId { get; set; }
        public bool IsSponsor { get; set; }
        public bool IsExhibitDay { get; set; }
        public bool IsPresentation { get; set; }
        public bool IsRoundTable { get; set; }
        public bool IsExisting { get; set; }
        public bool IsCatalog { get; set; }
        public string BoothNumber { get; set; }
        public virtual ShowASI Show { get; set; }
        public virtual ShowCompany Company { get; set; }
        public virtual List<ShowDistShowLogo> DistShowLogo { get; set; }
        public virtual IList<ShowEmployeeAttendee> EmployeeAttendees { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
