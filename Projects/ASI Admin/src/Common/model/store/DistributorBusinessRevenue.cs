using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class LegacyDistributorBusinessRevenue
    {
        public const string BUSINESSREVENUE_SIGNS = "Signs";
        public const string BUSINESSREVENUE_TROPHYAWARDS = "Trophy and Awards";
        public const string BUSINESSREVENUE_PRINTING = "Printing";
        public const string BUSINESSREVENUE_SCREENPRINTING = "Screen Printing";
        public const string BUSINESSREVENUE_PROMOTIONALPRODUCTS = "Promotional Products";

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
