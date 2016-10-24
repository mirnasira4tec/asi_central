using asi.asicentral.oauth;
using asi.asicentral.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace asi.asicentral.model
{
	public class CompanyInformation
	{
		public string MasterCustomerId { get; set; }
		public int SubCustomerId { get; set; }
		public int CompanyId { get; set; }
		public string Name { get; set; }
		public string MemberType { get; set; }  // member type for store
        public string CustomerClassCode { get; set; }  // class code in personidy
        public string SubClassCode { get; set; }
		public string MemberStatus { get; set; }
		public int MemberTypeNumber { get; set; }
		public string ASINumber { get; set; }
        public bool DNSFlag { get; set; }
		[Display(ResourceType = typeof(Resource), Name = "Street1")]
		public string Street1 { get; set; }
		[Display(ResourceType = typeof(Resource), Name = "Street2")]
		public string Street2 { get; set; }
		[Display(ResourceType = typeof(Resource), Name = "City")]
		public string City { get; set; }
		[Display(ResourceType = typeof(Resource), Name = "State")]
		public string State { get; set; }
		[Display(ResourceType = typeof(Resource), Name = "Zipcode")]
		public string Zip { get; set; }
		[Display(ResourceType = typeof(Resource), Name = "Country")]
		public string Country { get; set; }
		[Display(ResourceType = typeof(Resource), Name = "Phone")]
		public string Phone { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "Email")]
        public string Email { get; set; }

        public bool IsTerminated() { return MemberStatus == StatusCode.TERMINATED.ToString() || MemberStatus == StatusCode.TRMN.ToString(); }
	}
}
