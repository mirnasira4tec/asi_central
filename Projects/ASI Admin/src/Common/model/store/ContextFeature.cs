using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class ContextFeature
    {
        public ContextFeature()
        {
            if (this.GetType() == typeof(ContextFeature))
            {
                ChildFeatures = new List<ContextFeature>();
                AssociatedProducts = new List<ContextFeatureProduct>();
            }
        }

        public int ContextFeatureId { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public virtual ICollection<ContextFeature> ChildFeatures { get; set; }
        public virtual ICollection<ContextFeatureProduct> AssociatedProducts { get; set; }
        
        /// <summary>
        /// Returns a label for the feature/product configuration, empty if feature is not associated with product, true if associated with no label, 
        /// actual label if feature is associated with product and a label was defined
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public string GetProductLabel(ContextProduct product)
        {
            string productLabel = string.Empty;
            ContextFeatureProduct featProd = this.AssociatedProducts
                .Where(ctxFeatProd => ctxFeatProd.ProductId == product.ProductId).SingleOrDefault();
            if (featProd != null) productLabel = string.IsNullOrEmpty(featProd.Label) ? "True" : featProd.Label;
            return productLabel;
        }

        public override string ToString()
        {
            return string.Format("Feature: {0} - {1}", ContextFeatureId, Name);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            ContextFeature feature = obj as ContextFeature;
            if (feature != null) equals = (feature.ContextFeatureId == ContextFeatureId && feature.Name  == Name);
            return equals;
        }

        public override int GetHashCode()
        {
            return new { A = ContextFeatureId, B = Name }.GetHashCode();
        }
    }
}
