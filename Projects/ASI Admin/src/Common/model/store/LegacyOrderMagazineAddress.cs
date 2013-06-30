using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public partial class LegacyOrderMagazineAddress
    {
        public int OrderID { get; set; }
        public int SubscribeID { get; set; }
        public int ProdID { get; set; }
        public virtual LegacyMagazineAddress Address { get; set; }
    }
}
