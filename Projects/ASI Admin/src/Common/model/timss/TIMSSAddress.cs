using System;
using System.Collections.Generic;

namespace asi.asicentral.model.timss
{
    public partial class TIMSSAddress
    {
        public System.Guid DAPP_UserId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string BillingPerson { get; set; }
        public string ShipToFlag { get; set; }
        public string BillToFlag { get; set; }
        public string ProcessedFlag { get; set; }
        public Nullable<System.DateTime> ProcessedDate { get; set; }
        public string PrimaryFlag { get; set; }
    }
}
