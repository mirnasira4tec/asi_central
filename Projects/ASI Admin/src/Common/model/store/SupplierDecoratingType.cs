using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class SupplierDecoratingType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SupplierMembershipApplication> SupplierApplications { get; set; }
    }
}
