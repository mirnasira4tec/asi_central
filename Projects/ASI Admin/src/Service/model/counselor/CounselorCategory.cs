using System;
using System.Collections.Generic;

namespace asi.asicentral.model.counselor
{
    public class CounselorCategory
    {
        public CounselorCategory()
        {
            this.Contents = new List<CounselorContent>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public virtual ICollection<CounselorContent> Contents { get; set; }
    }
}
