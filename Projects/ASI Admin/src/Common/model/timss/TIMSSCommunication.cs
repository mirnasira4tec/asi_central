using System;
using System.Collections.Generic;

namespace asi.asicentral.model.timss
{
    public class TIMSSCommunication
    {
        public System.Guid DAPP_UserId { get; set; }
        public string Type { get; set; }
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        public string Phone { get; set; }
        public string FormattedPhoneAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProcessedFlag { get; set; }
        public Nullable<System.DateTime> ProcessedDate { get; set; }
    }
}
