using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
   public class CompanyProfileData
    {
        public int Id { get; set; }
        public int CompanyProfileId { get; set; }
        public int ProfileOptionId { get; set; }
        public string OriginalValue { get; set; }
        public string UpdateValue { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
        public virtual CompanyProfile CompanyProfile { get; set; }
        public virtual ProfileOption ProfileOption { get; set; }
    }
}
