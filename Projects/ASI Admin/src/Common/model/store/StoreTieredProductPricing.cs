using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreTieredProductPricing
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public decimal Cost { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public override bool Equals(object obj)
        {
            bool equals = false;

            StoreTieredProductPricing productPricing = obj as StoreTieredProductPricing;
            if (productPricing != null) equals = (productPricing.Id == Id);
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
