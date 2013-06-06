using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreAddress
    {
        public int Id { get; set; }
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
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
