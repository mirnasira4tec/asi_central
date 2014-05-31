using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.timss
{
    public class PersonifyMapping
    {
        public PersonifyMapping()
        {
            ItemCount = 1;
        }

        public Guid Identifier { get; set; }
        public int? StoreContext { get; set; }
        public int StoreProduct { get; set; }
        public string StoreOption { get; set; }
        public int PersonifyProduct { get; set; }
        public string PersonifyRateCode { get; set; }
        public string PersonifyRateStructure { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
        public int ItemCount { get; set; }
    }
}
