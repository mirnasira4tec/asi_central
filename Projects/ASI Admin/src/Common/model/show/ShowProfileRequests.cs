using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public enum ProfileRequestStatus
    {
        Created = 0,
        Pending = 1,
        AttendeeUpdate = 2,
        Updated = 3,
        PreApproved = 4,
        Approved = 5,
        Cancelled = 6
    }
   public class ShowProfileRequests
    {
        public int Id { get; set; }
        public int? AttendeeId { get; set; }
        public int? EmployeeAttendeeId { get; set; }
        public string RequestedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string RequestReference { get; set; }
        public ProfileRequestStatus Status{ get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public virtual ShowAttendee Attendee { get; set; }
        public virtual ShowEmployeeAttendee EmployeeAttendee { get; set; }
        public virtual IList<ShowProfileOptionalDetails> ProfileRequestOptionalDetails { get; set; }
        public virtual IList<ShowProfileSupplierData> ProfileSupplierData { get; set; }
        public virtual IList<ShowProfileDistributorData> ProfileDistributorData { get; set; }
    }
}
