using System;
using System.Collections.Generic;

namespace Publications.Models
{
    public partial class COUN_Categories_CSCT
    {
        public COUN_Categories_CSCT()
        {
            this.COUN_Content_CSCO1 = new List<COUN_Content_CSCO>();
        }

        public int CatId_CSCT { get; set; }
        public string CatDesc_CSCT { get; set; }
        public Nullable<int> FeatureID_CSCT { get; set; }
        public virtual COUN_Content_CSCO COUN_Content_CSCO { get; set; }
        public virtual COUN_FeatureContent_COFC COUN_FeatureContent_COFC { get; set; }
        public virtual ICollection<COUN_Content_CSCO> COUN_Content_CSCO1 { get; set; }
    }
}
