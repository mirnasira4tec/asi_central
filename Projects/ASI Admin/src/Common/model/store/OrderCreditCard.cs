using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public class OrderCreditCard
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
        public string ExpMonth { get; set; }
        public string ExpYear { get; set; }
        public string ExternalReference { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public virtual Order Order { get; set; }
    }
}
