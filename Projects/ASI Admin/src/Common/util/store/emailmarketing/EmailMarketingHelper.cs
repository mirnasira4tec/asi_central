using System.Collections.Generic;

namespace asi.asicentral.util.store.emailmarketing
{
    public class EmailMarketingHelper
    {
        public static readonly Dictionary<int, List<string>> EmailMarketingOptions = 
            new Dictionary<int,List<string>>() {
                {1, new List<string>() { "250", "10.00"}},
                {2, new List<string>() { "500", "20.00"}},
                {3, new List<string>() { "1000", "35.00"}},
                {4, new List<string>() { "5000", "150.00"}},
                {5, new List<string>() { "10000", "250.00"}}
            };
    }
}
