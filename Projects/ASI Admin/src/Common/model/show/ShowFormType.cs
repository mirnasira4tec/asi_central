using System;
using System.Collections.Generic;

namespace asi.asicentral.model.show
{
    public class ShowFormType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NotificationEmails { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }        
    }
}
