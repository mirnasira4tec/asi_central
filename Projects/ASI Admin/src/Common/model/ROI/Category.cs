using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.ROI
{
    public class Category
    {
        public string Name { get; set; }
        public int SupplierCount { get; set; }
        public Company Requestor { get; set; }
        public Company[] Results { get; set; }
    }
}
