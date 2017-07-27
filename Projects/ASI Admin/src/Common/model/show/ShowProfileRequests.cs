using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public enum ProfileRequestStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Cancelled = 3,
        Updated = 4,
        PreApproved =5
    }
   public class ShowProfileRequests
    {
        public int Id { get; set; }
        public int? AttendeeId { get; set; }
        public int? EmployeeAttendeeId { get; set; }
        public string RequestedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string RequestReference { get; set; }
        public ProfileRequestStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public virtual ShowAttendee Attendee { get; set; }
        public virtual ShowEmployeeAttendee EmployeeAttendee { get; set; }
        public virtual IList<ShowProfileRequestOptionalDetails> ProfileRequestOptionalDetails { get; set; }
        public virtual IList<ShowProfileRequiredData> ProfileRequiredData { get; set; }
    }
}
