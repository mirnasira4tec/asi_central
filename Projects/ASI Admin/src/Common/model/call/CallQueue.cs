using System;
using System.Collections.Generic;

namespace asi.asicentral.model.call
{
    public class CallQueue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<byte> IsForcedClosed { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateSource { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public bool Enabled
        {
            get
            {
                return !IsForcedClosed.HasValue || IsForcedClosed.Value == 0;
            }
        }
    }
}
