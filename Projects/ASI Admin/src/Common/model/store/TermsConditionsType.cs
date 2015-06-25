using System;

namespace asi.asicentral.model.store
{
    public class TermsConditionsType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
