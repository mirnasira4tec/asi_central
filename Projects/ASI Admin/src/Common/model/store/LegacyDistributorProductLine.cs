using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class LegacyDistributorProductLine
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string MemberTypeRole { get; set; }
        public string SubCode { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public virtual ICollection<LegacyDistributorMembershipApplication> DistributorApplications { get; set; }
    }
}
