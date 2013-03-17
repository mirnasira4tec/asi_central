using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public class SupplierMembershipApplicationContact : MembershipApplicationContact
    {
        public Nullable<int> SalesId { get; set; }
        public virtual SupplierMembershipApplication SupplierApplication { get; set; }
    }
}
