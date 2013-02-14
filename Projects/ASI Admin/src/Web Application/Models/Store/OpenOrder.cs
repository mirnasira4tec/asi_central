using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.Models.Store
{
    public class OpenOrder
    {
        public int OrderId { get; set; }
        public String Name { get; set; }
        public String Company { get; set; }
        public Nullable<int> Phone { get; set; }
        public String Product { get; set; }
        public String BillingAddress { get; set; }
        public DateTime DateOrerCreated { get; set; }
    }
}