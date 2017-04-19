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
        Cancelled = 3
    }
   public class ShowProfileRequests
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int EventId { get; set; }
        public string RequestedBy { get; set; }
        public string ApprovedBy { get; set; }
        public ProfileRequestStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public virtual IList<ShowProfileRequestOptionalDetails> RequestOptionalDetails { get; set; }
    }
}
