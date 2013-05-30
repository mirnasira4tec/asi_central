using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class ContextProduct
    {
        public ContextProduct()
        {
            if (this.GetType() == typeof(ContextProduct))
            {
                Features = new List<ContextFeatureProduct>();
            }
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public decimal ApplicationCost { get; set; }
        public decimal ShippingCostUS { get; set; }
        public decimal ShippingCostOther { get; set; }
        public bool HasTax { get; set; }
        public bool IsSubscription { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public virtual ICollection<ContextFeatureProduct> Features { get; set; }

        public override string ToString()
        {
            return string.Format("Product: {0} - {1}", ProductId, Name);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            ContextProduct product = obj as ContextProduct;
            if (product != null) equals = product.ProductId == ProductId;
            return equals;
        }

        public override int GetHashCode()
        {
            return ProductId.GetHashCode();
        }
    }
}
