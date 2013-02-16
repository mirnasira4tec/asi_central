using System;
using System.Collections.Generic;

namespace asi.asicentral.model.news
{
    public class News
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> DateEntered { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<System.DateTime> LiveDate { get; set; }
        public Nullable<int> Duration { get; set; }
        public bool Post { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public Nullable<int> SourceId { get; set; }
        public virtual NewsRotator NewsRotator { get; set; }
        public virtual NewsSource Source { get; set; }
    }
}
