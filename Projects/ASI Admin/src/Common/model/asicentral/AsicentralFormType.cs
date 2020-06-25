using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.asicentral
{
    public class AsicentralFormType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TermsAndConditions { get; set; }
        public string NotificationEmails { get; set; }
        public bool IsObsolete { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
