using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class LookProductShippingRate
    {
        public int Id { get; set; }
        public string Origin { get; set; }
        public string ShippingMethod { get; set; }
        public string Country { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal AmountOrPercent { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public override string ToString()
        {
            return "LookProductShippingRate - " + Id;
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            LookProductShippingRate lookup = obj as LookProductShippingRate;
            if (lookup != null) equals = lookup.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
