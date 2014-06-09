using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.Resources;

namespace asi.asicentral.model
{
	public class CompanyInformation
	{
		public string MasterCustomerId { get; set; }
		public int SubCustomerId { get; set; }
		public int CompanyId { get; set; }
		public string Name { get; set; }
		public string MemberType { get; set; }
		public string ASINumber { get; set; }
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
	}
}
