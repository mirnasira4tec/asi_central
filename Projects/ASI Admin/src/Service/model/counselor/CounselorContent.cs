using System;
using System.Collections.Generic;

namespace asi.asicentral.model.counselor
{
    public class CounselorContent
    {
        public CounselorContent()
        {
            this.Categories = new List<CounselorCategory>();
            this.Features = new List<CounselorFeature>();
            this.FeatureRotators = new List<CounselorFeatureRotator>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Author { get; set; }
        public string TagLine { get; set; }
        public string Teaser { get; set; }
        public string Content { get; set; }
        public Nullable<bool> Active { get; set; }
        public string SmallImage { get; set; }
        public string LargeImage { get; set; }
        public virtual ICollection<CounselorCategory> Categories { get; set; }
        public virtual ICollection<CounselorFeature> Features { get; set; }
        public virtual ICollection<CounselorFeatureRotator> FeatureRotators { get; set; }
    }
}
