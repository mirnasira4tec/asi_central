using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public partial class LegacyOrderDistributorAddress
    {
        public LegacyOrderDistributorAddress()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string WebAdd { get; set; }
    }
}
