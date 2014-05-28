using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.Resources;
namespace asi.asicentral.model.store
{
    public class StoreAddress
    {
        public StoreAddress()
        {
            Country = "USA";
        }
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
        [Display(ResourceType = typeof(Resource), Name = "Phone")]
        public string Phone { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        /// <summary>
        /// Used to try to re-use existing addresses and remove duplicate entries
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool AreEquivalent(StoreAddress address)
        {
            bool areEquivalent = Street1 == address.Street1 && City == address.City && Zip == address.Zip && Country == address.Country;
            return areEquivalent;
        }

        /// <summary>
        /// Check whether this address is a valid one
        /// </summary>
        /// <returns></returns>
        public bool IsValid
        {
            get
            {
                return Street1 != null && City != null;
            }
        }


        public override string ToString()
        {
            return Id + " (" + Street1 != null ? Street1 : "no street" + City != null ? City : "no city" + Country != null ? Country : "no country)";
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            StoreAddress address = obj as StoreAddress;
	        if (address != null)
	        {
		        if (Id != 0 || address.Id != 0)
		        {
			        equals = address.Id == Id;
		        }
		        else
		        {
			        equals = ToString().Equals(address.ToString());
		        }
	        }
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
