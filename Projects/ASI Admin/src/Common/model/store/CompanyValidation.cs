using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class CompanyValidation
    {
        public const string END_BUYERS_KEYWORDS = "End Buyer Keywords";
        public const string REGISTERED_TRADEMARKS = "Registered Trademarks";
        public const string EMAIL_DOMAINS = "Email Domains";
        public const string FORBIDDEN_WORDS = "Forbidden Words";

        public int Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
