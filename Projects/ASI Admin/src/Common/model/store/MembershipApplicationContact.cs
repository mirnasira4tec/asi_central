using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class MembershipApplicationContact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public Nullable<System.Guid> AppplicationId { get; set; }
        public string Department { get; set; }
        public bool IsPrimary { get; set; }
    }
}
