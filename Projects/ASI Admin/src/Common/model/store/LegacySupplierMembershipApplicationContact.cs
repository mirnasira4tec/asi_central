using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public class LegacySupplierMembershipApplicationContact : LegacyMembershipApplicationContact
    {
        public Nullable<int> SalesId { get; set; }
        public virtual LegacySupplierMembershipApplication SupplierApplication { get; set; }
    }
}
