using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public class DistributorMembershipApplicationContact : MembershipApplicationContact
    {
        public virtual DistributorMembershipApplication DistributorApplication { get; set; }
    }
}
