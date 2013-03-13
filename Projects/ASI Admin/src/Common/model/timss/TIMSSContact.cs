using System;
using System.Collections.Generic;

namespace asi.asicentral.model.timss
{
    public class TIMSSContact
    {
        public System.Guid DAPP_UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string PrimaryFlag { get; set; }
        public string ProcessedFlag { get; set; }
        public Nullable<System.DateTime> ProcessedDate { get; set; }
    }
}
