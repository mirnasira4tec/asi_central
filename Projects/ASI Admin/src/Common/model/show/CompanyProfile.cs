using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
   public class CompanyProfile
    {
        public int Id { get; set; }
        public string UserReference { get; set; }
        public int? CompanyId { get; set; }
        public int? ShowId { get; set; }
        public string Status { get; set; }
        public string SubmitBy { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
        public virtual ShowCompany ShowCompany { get; set; }
        public virtual ShowASI Show { get; set; }
        public virtual IList<CompanyProfileData> CompanyProfileData { get; set; }
    }
}
