using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.Resources;

namespace asi.asicentral.model.store
{
    public class LegacyMembershipApplicationContact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        public string Fax { get; set; }
        
        public Nullable<System.Guid> AppplicationId { get; set; }
        public string Department { get; set; }
        public bool IsPrimary { get; set; }
    }
}
