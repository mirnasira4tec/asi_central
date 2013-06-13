using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class TaxRate
    {
        public int Id { get; set; }
        public string State { get; set; }
        public int? Zip { get; set; }
        public string County { get; set; }
        public decimal Rate { get; set; }
        public System.DateTime CreateDateUTC { get; set; }
        public System.DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }


        public override string ToString()
        {
            return "TaxRate (" + Id + ")";
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            TaxRate tax = obj as TaxRate;
            if (tax != null) equals = tax.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
