using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public class ShowEmployeeAttendee
    {
        public int Id { get; set; }
        public int AttendeeId { get; set; }
        public int EmployeeId { get; set; }
        public virtual ShowAttendee Attendee { get; set; }
        public virtual ShowEmployee Employee { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public bool? HasTravelForm { get; set; }
        public virtual IList<ShowProfileRequests> ProfileRequests { get; set; }
        public virtual IList<ShowFormInstance> TravelForms { get; set; }
        public int? PriorityOrder { get; set; }
        public bool? HasReviewedGuidelines { get; set; }
    }
}
