using System;
using System.Collections.Generic;

namespace asi.asicentral.model.excit
{
    public enum SupRequestStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }

    public class SupUpdateRequest
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string RequestedBy { get; set; }
        public string ApprovedBy { get; set; }
        public SupRequestStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public virtual IList<SupUpdateRequestDetail > RequestDetails { get; set; }
    }
}
