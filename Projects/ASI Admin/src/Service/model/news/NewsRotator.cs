using System;
using System.Collections.Generic;

namespace asi.asicentral.model.news
{
    public class NewsRotator
    {
        public int Id { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> CategoryPriority { get; set; }
        public string FollowLink { get; set; }
        public string VideoLink { get; set; }
        public string AudioLink { get; set; }
        public string MainImage { get; set; }
        public string SubImage { get; set; }
        public string ThumbnailImage { get; set; }
        public Nullable<bool> IsFeature { get; set; }
        public Nullable<bool> IsSubFeature { get; set; }
        public virtual News News { get; set; }
    }
}
