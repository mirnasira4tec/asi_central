using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class CENTUserProfilesPROF
    {
        public Guid PROF_UserID { get; set; }
        public string PROF_ASINo { get; set; }
        public string PROF_MemberTypeID { get; set; }
        public string PROF_FirstName { get; set; }
        public string PROF_LastName { get; set; }
        public string PROF_Title { get; set; }
        public string PROF_Company { get; set; }
        public string PROF_Phone { get; set; }
        public string PROF_Fax { get; set; }
        public string PROF_Street1 { get; set; }
        public string PROF_Street2 { get; set; }
        public string PROF_City { get; set; }
        public string PROF_StateID { get; set; }
        public string PROF_Zip { get; set; }
        public string PROF_CountryID { get; set; }
        public DateTime PROF_LastUpdatedDate { get; set; }
        int PROF_ProfileVersion { get; set; }
        public virtual ASPNetUser Users { get; set; }
    }
}
