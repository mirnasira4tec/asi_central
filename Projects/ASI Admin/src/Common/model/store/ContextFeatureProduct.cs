using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class ContextFeatureProduct
    {
        public Guid ContextFeatureProductId { get; set; }
        public string Label { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public ContextProduct Product { get; set; }

        public override string ToString()
        {
            return string.Format("Feature Product: {0} - {1}", ContextFeatureProductId, Label);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            ContextFeatureProduct featureProduct = obj as ContextFeatureProduct;
            if (featureProduct != null) equals = featureProduct.ContextFeatureProductId == ContextFeatureProductId;
            return equals;
        }

        public override int GetHashCode()
        {
            return ContextFeatureProductId.GetHashCode();
        }
    }
}
