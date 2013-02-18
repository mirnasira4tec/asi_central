using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public class SupplierMembershipApplicationContact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public Nullable<int> SalesId { get; set; }
        public Nullable<System.Guid> AppplicationId { get; set; }
        public virtual SupplierMembershipApplication SupplierApplication { get; set; }
    }
}
