using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreCompanyAddress
    {
        public int Id { get; set; }
        public bool IsShipping { get; set; }
        public bool IsBilling { get; set; }
        public StoreAddress Address { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
