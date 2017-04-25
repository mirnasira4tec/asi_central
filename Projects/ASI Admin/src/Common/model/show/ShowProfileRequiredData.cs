using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public class ShowProfileRequiredData
    {
        public int Id { get; set; }
        public int ProfileRequestId { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string ASINumber { get; set; }
        public string AttendeeName { get; set; }
        public string AttendeeTitle { get; set; }
        public string AttendeeCommEmail { get; set; }
        public string AttendeeCellPhone { get; set; }
        public string AttendeeWorkPhone { get; set; }
        public string CorporateAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string CompanyWebsite { get; set; }
        public string ProductSummary { get; set; }
        public string TrustFromDistributor { get; set; }
        public string SpecialServices { get; set; }
        public string LoyaltyPrograms { get; set; }
        public string Samples { get; set; }
        public string ProductSafety { get; set; }
        public string FactAboutCompany { get; set; }
        public Boolean flag { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public virtual ShowProfileRequests ProfileRequests { get; set; }
    }
}
