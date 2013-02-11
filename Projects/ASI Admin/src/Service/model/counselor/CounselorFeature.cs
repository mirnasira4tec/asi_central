using System;
using System.Collections.Generic;

namespace asi.asicentral.model.counselor
{
    public class CounselorFeature
    {
        public int Id { get; set; }
        public Nullable<int> ContentId { get; set; }
        public virtual CounselorContent Content { get; set; }
    }
}
