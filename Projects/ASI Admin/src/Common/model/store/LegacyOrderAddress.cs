using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public partial class LegacyOrderAddress
    {
        public LegacyOrderAddress()
        {
        }

        public int SPAD_AddressID { get; set; }
        public string SPAD_FirstName { get; set; }
        public string SPAD_LastName { get; set; }
        public string SPAD_Street1 { get; set; }
        public string SPAD_Street2 { get; set; }
        public string SPAD_City { get; set; }
        public string SPAD_StateID { get; set; }
        public string SPAD_Zip { get; set; }
        public string SPAD_Email { get; set; }
    }
}
