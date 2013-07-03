using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class ContextFeatureProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Label { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public ContextProduct Product { get; set; }

        public override string ToString()
        {
            return string.Format("Feature Product: Label({0}) - {1}", Label, Product.Name);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            ContextFeatureProduct featureProduct = obj as ContextFeatureProduct;
            if (featureProduct != null) 
                equals = (
                    featureProduct.Id == Id &&
                    ProductId == featureProduct.ProductId);
            return equals;
        }

        public override int GetHashCode()
        {
            return new { ContextFeatureProductId = Id, ProductId }.GetHashCode();
        }
    }
}
