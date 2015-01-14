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
                {86,new List<int>(){4,5,6}},
                {87,new List<int>(){4,5,6}},
                {90,new List<int>(){11,12,13,14,15,16}}
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
                new CatalogAdvertisingTieredProductPricing()
                {
                    Id = 4,
                    AdSizeName = "Full Page",
                    Price = 2819.00m,
                    Sequence = 1
                },
                new CatalogAdvertisingTieredProductPricing()
                {
                    Id = 5,
                    AdSizeName = "Spread",
                    Price = 4155.00m,
                    Sequence = 2
                },
                new CatalogAdvertisingTieredProductPricing()
                {
                    Id = 6,
                    AdSizeName = "Single-Product Page",
                    Price = 1000.00m,
                    Sequence = 3
                },
                new CatalogAdvertisingTieredProductPricing()
                {
                    Id = 11,
                    AdSizeName = "Full Page",
                    Price = 2600.00m,
                    Sequence = 1
                },
                new CatalogAdvertisingTieredProductPricing()
                {
                    Id = 12,
                    AdSizeName = "Full Pages 2-3",
                    Price = 2340.00m,
                    Sequence = 2
                },
                new CatalogAdvertisingTieredProductPricing()
                {
                    Id = 13,
                    AdSizeName = "Full Pages 4+",
                    Price = 2080.00m,
                    Sequence = 3
                },
                new CatalogAdvertisingTieredProductPricing()
                {
                    Id = 14,
                    AdSizeName = "Cover 4",
                    Price = 3590.00m,
                    Sequence = 4
                },
                new CatalogAdvertisingTieredProductPricing()
                {
                    Id = 15,
                    AdSizeName = "Cover 3",
                    Price =3120.00m,
                    Sequence = 5
                },
                new CatalogAdvertisingTieredProductPricing()
                {
                    Id = 16,
                    AdSizeName = "Cover 2",
                    Price = 3120.00m,
                    Sequence = 6
                }
            };
        }

        public static IList<CatalogAdvertisingTieredProductPricing> GetTieredProductPricing(int productId)
        {
            var items = ProductToTieredPricesMap[productId];
            return TieredProducts.Where(item => items.Contains(item.Id)).OrderBy(item => item.Sequence).ToList();
        }

        public static decimal GetDefaultProductPrice(int productId)
        {
            decimal result = 0m;
            var items = ProductToTieredPricesMap[productId];
            switch (productId)
            {
                case 84:
                    result = TieredProducts.Where(item => items.Contains(item.Id) && item.AdSizeName == "One Advertorial Spread").Select(item => item.Price).Single();
                    break;
                case 86:
                case 87:
                case 90:
                    result = TieredProducts.Where(item => items.Contains(item.Id) && item.AdSizeName == "Full Page").Select(item => item.Price).Single();
                    break;
                default:
                    break;
            }
            return result;
        }

        public static decimal GetAdSizeValue(int productId, string adSizeName)
        {
            var prices = GetTieredProductPricing(productId);
            return prices.First(price => price.AdSizeName == adSizeName).Price;
        }
    }
}
