using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public class SupplierMembershipApplicationContact : MembershipApplicationContact
    {
        public virtual SupplierMembershipApplication SupplierApplication { get; set; }
    }
}
