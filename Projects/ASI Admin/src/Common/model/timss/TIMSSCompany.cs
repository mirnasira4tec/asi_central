using System;
using System.Collections.Generic;

namespace asi.asicentral.model.timss
{
    public class TIMSSCompany
    {
        public decimal Sequence { get; set; }
        public System.Guid DAPP_UserId { get; set; }
        public string ASINumber { get; set; }
        public string Name { get; set; }
        public string CustomerClass { get; set; }
        public string ProcessedFlag { get; set; }
        public Nullable<System.DateTime> ProcessedDate { get; set; }
    }
}
