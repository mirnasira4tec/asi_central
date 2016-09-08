using System;

namespace asi.asicentral.model.excit
{
    public class SupUpdateField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public bool? IsObsolete { get; set; }
    }
}
