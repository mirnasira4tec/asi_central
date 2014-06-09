using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.PersonifyDataASI;

namespace asi.asicentral.services.PersonifyProxy
{
    class AddressInfoEqualityComparer:IEqualityComparer<AddressInfo>
    {
        public bool Equals(AddressInfo a, AddressInfo b)
        {
            return a.Address1 == b.Address1 && a.PostalCode == b.PostalCode
                && a.ShipToFlag == b.ShipToFlag && a.BillToFlag == b.BillToFlag;
        }

        public int GetHashCode(AddressInfo a)
        {
            return a.Address1.GetHashCode() + a.PostalCode.GetHashCode() 
                + a.ShipToFlag.GetHashCode() + a.BillToFlag.GetHashCode();
        }
    }
}
