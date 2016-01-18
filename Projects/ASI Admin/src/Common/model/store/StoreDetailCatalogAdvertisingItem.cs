using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.util.store.catalogadvertising;
using asi.asicentral.util.store.magazinesadvertising;
using ASI.EntityModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace asi.asicentral.model.store
{
    public class StoreDetailCatalogAdvertisingItem : IDateUTCAndSource, IValidatableObject
    {
        public static readonly string[] SUPPLIER_CATALOG_ADVERTISING_PRODUCT_NAMES = { "The Apparel Catalog", "Idea Showcase Schools", "Spectrum Celebration", "The Gift Book", "Spectrum", "Idea Showcase", "The Professional Buyer's Guide" };
        public static readonly string[] SUPPLIER_CATALOG_ADVERTISING_PRODUCT_PROGRAMMATIC_NAMES = { "TheApparelCatalogAdvertising", "IdeaShowcaseSchoolsAdvertising", "TheApparelCatalogAdvertising", "TheApparelCatalogAdvertising", "IdeaShowcaseSchoolsAdvertising", "IdeaShowcaseSchoolsAdvertising", "TheApparelCatalogAdvertising" };
        public static readonly int[] SUPPLIER_CATALOG_ADVERTISING_PRODUCT_IDS = { 84, 85, 86, 87, 88, 89, 90 };
        public static readonly int[] SUPPLIER_CATALOG_ADVERTISING_PRODUCT_1_IDS = { 84, 86, 87, 90 };
        public static readonly int[] SUPPLIER_CATALOG_ADVERTISING_PRODUCT_2_IDS = { 85, 88, 89 };

        public int Id { get; set; }
        public int OrderDetailId { get; set; }

        [NotMapped]
        public CatalogAdvertisingUpload ProductType { get; set; }

        public string ProductTypeStringValue
        { 
            get { return ProductType.ToString("d"); }
            set 
            {
                CatalogAdvertisingUpload type;
                if( Enum.TryParse<CatalogAdvertisingUpload>(value, out type) )
                    ProductType = type;
            }
        }

        public string AdSize { get; set; }
        public string ProductDescription { get; set; }
        public string ProductPricing { get; set; }

        public string Website { get; set; }
        public string ProductNumber { get; set; }
        public string ESPNumber { get; set; }
        public string ProductImage { get; set; }

        public int Sequence { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            if (string.IsNullOrWhiteSpace(Website) || string.IsNullOrWhiteSpace(ProductNumber))
            {
                if (!(string.IsNullOrWhiteSpace(Website) && string.IsNullOrWhiteSpace(ProductNumber)))
                {
                    errors.Add(new ValidationResult("Both website and product number have to be filled or be empty"));
                }
            }
            return errors;
        }

        public string ToString(int productId)
        {
            StringBuilder result = new StringBuilder();
            var dict = ToDictionary(productId);
            foreach (var item in dict)
            {
                if (result.Length > 0) result.Append(", ");
                result.Append(string.Format("{0}: {1}", item.Key, item.Value));
            }
            return result.ToString();
        }

        public static string GetProductName(int productId)
        {
            int i = Array.FindIndex(SUPPLIER_CATALOG_ADVERTISING_PRODUCT_IDS, item => item == productId);
            return string.Format("{0} {1}", SUPPLIER_CATALOG_ADVERTISING_PRODUCT_NAMES[i], "Advertising");
        }

        public string GetSummaryDetails(int productId)
        {
            var result = new StringBuilder();
            var dict = ToDictionary(productId);
            foreach (var item in dict)
            {
                if (result.Length > 0) result.Append(", ");
                result.Append(string.Format("{0}: {1}", item.Key, item.Value));
            }
            if (result.Length > 0) result.Append(".");
            return result.ToString();
        }

        private IDictionary<string, string> ToDictionary(int productId)
        {
            var result = new Dictionary<string, string>();
            result.Add("Ad Size", AdSize);
            switch (productId)
            {
                case 84:
                case 86:
                case 87:
                case 90:
                    if (SUPPLIER_CATALOG_ADVERTISING_PRODUCT_1_IDS.Contains(productId))
                    {
                        if (!string.IsNullOrWhiteSpace(ProductDescription))
                        {
                            result.Add("Product Description", ProductDescription);
                        }
                        if (!string.IsNullOrWhiteSpace(ProductPricing))
                        {
                            result.Add("Product Pricing", ProductPricing);
                        }
                    }
                    break;
                case 85:
                case 88:
                case 89:
                    if (SUPPLIER_CATALOG_ADVERTISING_PRODUCT_2_IDS.Contains(productId))
                    {
                        switch (ProductType)
                        {
                            case CatalogAdvertisingUpload.WebAndProductNumber:
                                if (!string.IsNullOrWhiteSpace(Website))
                                {
                                    result.Add("Website", Website);
                                }
                                if (!string.IsNullOrWhiteSpace(ProductNumber))
                                {
                                    result.Add("Product Number", ProductNumber);
                                }
                                break;
                            case CatalogAdvertisingUpload.ESPNumber:
                                if (!string.IsNullOrWhiteSpace(ESPNumber))
                                {
                                    result.Add("ESP Number", ESPNumber);
                                }
                                break;
                            case CatalogAdvertisingUpload.ProductImage:
                                if (!string.IsNullOrWhiteSpace(ProductImage))
                                {
                                    result.Add("Upload image of the product", CatalogAdvertisingHelper.GetOriginalFileName(ProductImage, OrderDetailId.ToString()));
                                }
                                break;
                            default:
                                throw new Exception("Product type is required");
                        }
                    }
                    break;
                default:
                    throw new Exception("Product id is required");
            }
            return result;
        }
    }

    public enum CatalogAdvertisingUpload
    {
        WebAndProductNumber,
        ESPNumber,
        ProductImage
    }
}
