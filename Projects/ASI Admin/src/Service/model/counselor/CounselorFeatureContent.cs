using System;
using System.Collections.Generic;

namespace Publications.Models
{
    public partial class COUN_FeatureContent_COFC
    {
        public int CatId_CSCT { get; set; }
        public Nullable<int> ID_CSCO { get; set; }
        public virtual COUN_Categories_CSCT COUN_Categories_CSCT { get; set; }
        public virtual COUN_Content_CSCO COUN_Content_CSCO { get; set; }
    }
}
