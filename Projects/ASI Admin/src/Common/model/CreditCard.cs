using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model
{
    public class CreditCard
    {
        public string Type { get; set; }
        public string Number { get; set; }
        public string MaskedPAN { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public string CardHolderName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
