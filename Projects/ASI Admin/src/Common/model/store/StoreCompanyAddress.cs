using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreCompanyAddress
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public bool IsShipping { get; set; }
        public bool IsBilling { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public virtual StoreAddress Address { get; set; }

        public override string ToString()
        {
            string description = Id + " - " ;
            return description;
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            StoreCompanyAddress address = obj as StoreCompanyAddress;
            if (address != null) equals = (address.Id == Id);
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
