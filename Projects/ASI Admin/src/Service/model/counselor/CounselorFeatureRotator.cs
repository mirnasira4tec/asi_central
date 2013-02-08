using System;
using System.Collections.Generic;

namespace asi.asicentral.model.counselor
{
    public class CounselorFeatureRotator
    {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public bool Active { get; set; }
        public string SmallImage { get; set; }
        public string LargeImage { get; set; }
        public virtual CounselorContent Content { get; set; }
    }
}
