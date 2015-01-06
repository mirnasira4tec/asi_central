using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.util.store.catalogadvertising
{
    public class CatalogAdvertisingTieredProductPricing
    {
        private static IDictionary<int, List<int>> ProductToTieredPricesMap;
        private static IList<CatalogAdvertisingTieredProductPricing> TieredProducts;

        public int Id { get; set; }
        public string AdSizeName { get; set; }
        public decimal Price { get; set; }
        public int Sequence { get; set; }

        static CatalogAdvertisingTieredProductPricing()
        {
            ProductToTieredPricesMap = new Dictionary<int, List<int>>()
            {
                {84,new List<int>(){0,1}},
            };

            TieredProducts = new List<CatalogAdvertisingTieredProductPricing>()
            {
                new CatalogAdvertisingTieredProductPricing()
                {
                    Id = 0,
                    AdSizeName = "One Advertorial Spread",
                    Price = 3000.00m,
                    Sequence = 1
                },
                new CatalogAdvertisingTieredProductPricing()
                {
                    Id = 1,
                    AdSizeName = "Two Advertorial Spread",
                    Price = 5000.00m,
                    Sequence = 2
                },
            };
        }

        public static IList<CatalogAdvertisingTieredProductPricing> GetTieredProductPricing(int productId)
        {
            var items = ProductToTieredPricesMap[productId];
            return TieredProducts.Where(item => items.Contains(item.Id)).OrderBy(item => item.Sequence).ToList();
        }

        public static decimal GetAdSizeValue(int productId, string adSizeName)
        {
            var prices = GetTieredProductPricing(productId);
            return prices.First(price => price.AdSizeName == adSizeName).Price;
        }
    }
}
