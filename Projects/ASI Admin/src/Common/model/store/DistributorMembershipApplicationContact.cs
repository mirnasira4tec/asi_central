using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public class LegacyDistributorMembershipApplicationContact : LegacyMembershipApplicationContact
    {
        public virtual LegacyDistributorMembershipApplication DistributorApplication { get; set; }
    }
}
