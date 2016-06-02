using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.model.store;
using asi.asicentral.PersonifyDataASI;

namespace asi.asicentral.services.PersonifyProxy
{
    public class StoreAddressInfo
    {
        //public ASICustomerInfo CustuInfo { get; set; }
        public string CountryCode { get; set; }
        public StoreAddress StoreAddr { get; set; }
        public bool StoreIsPrimary { get; set; }
        public bool StoreIsBilling { get; set; }
        public bool StoreIsShipping { get; set; }
        public bool IsAdded { get; set; }
        public long? PrioritySeq { get; set; }
        public bool PersonifyIsBilling { get; set; }
        public bool PersonifyIsShipping { get; set; }
        public AddressInfo PersonifyAddr { get; set; }   
    }
}
