using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.EntityModel;

namespace asi.asicentral.model.store
{
    public class StoreDetailCatalogAdvertisingItem : IDateUTCAndSource//, IValidatableObject
    {
        public int Id { get; set; }
        public int OrderDetailId { get; set; }

        public string AdSize { get; set; }
        public string ProductDescription { get; set; }
        public string ProductPricing { get; set; }

        public int Sequence { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public IList<string> GetSummaryDetails(int productId)
        {
            IList<string> result = new List<string>();
            switch (productId)
            {
                case 84:
                    result.Add(string.Format("Ad Size: {0}", AdSize));
                    if (!string.IsNullOrWhiteSpace(ProductDescription))
                    {
                        result.Add(string.Format("Product Description: {0}", ProductDescription));
                    }
                    if (!string.IsNullOrWhiteSpace(ProductPricing))
                    {
                        result.Add(string.Format("Product Pricing: {0}", ProductPricing));
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
