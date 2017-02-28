using System.Collections.Generic;

namespace asi.asicentral.util.store.emailmarketing
{
    public class EmailMarketingHelper
    {
        public static readonly Dictionary<int, List<string>> EmailMarketingOptions = 
            new Dictionary<int,List<string>>() {
                {1, new List<string>() { "500", "20.00", "0.04"}},
                {2, new List<string>() { "1000", "35.00", "0.04"}},
                {3, new List<string>() { "2500", "75.00", "0.03"}},
                {4, new List<string>() { "5000", "115.00", "0.02"}},
                {5, new List<string>() { "1000", "150.00", "0.02"}}
            };
    }
}
