using System;

namespace asi.asicentral.model.show
{
    public class ShowFormPropertyValue
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int FormInstanceId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
