using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreCompany
    {
        public StoreCompany()
        {
            if (this.GetType() == typeof(StoreCompany))
            {
                Addresses = new List<StoreCompanyAddress>();
                Individuals = new List<StoreIndividual>();
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string MemberType { get; set; }
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        [Display(ResourceType = typeof(Resource), Name = "Phone")]
        public string Phone { get; set; }
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        public string Fax { get; set; }
        public string Email { get; set; }
        public string WebURL { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public virtual IList<StoreCompanyAddress> Addresses { get; set; }
        public virtual IList<StoreIndividual> Individuals { get; set; }

        public override string ToString()
        {
            return Id + " - " + Name;
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            StoreAddress company = obj as StoreAddress;
            if (company != null) equals = company.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
