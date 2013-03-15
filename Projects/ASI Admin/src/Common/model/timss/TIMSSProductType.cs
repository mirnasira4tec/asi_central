using System;
using System.Collections.Generic;

namespace asi.asicentral.model.timss
{
    public partial class TIMSSProductType
    {
        public System.Guid ApplicationId { get; set; }
        public System.Guid DAPP_UserId { get; set; }
        public string Description { get; set; }
        public string SubCode { get; set; }
        public string LoadStatus { get; set; }
        public Nullable<System.DateTime> LoadDate { get; set; }
    }
}
