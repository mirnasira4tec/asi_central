using asi.asicentral.Resources;
using asi.asicentral.util.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreCompany
    {
        public StoreCompany()
        {
            if (this.GetType() == typeof(StoreCompany))
            {
                Addresses = new List<StoreCompanyAddress>();
                Individuals = new List<StoreIndividual>();
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string MemberType { get; set; }
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        [Display(ResourceType = typeof(Resource), Name = "Phone")]
        public string Phone { get; set; }
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        public string Fax { get; set; }
        public string Email { get; set; }
        public string WebURL { get; set; }
        public string ASINumber { get; set; }
	    public string ExternalReference { get; set; }
        public string BankName { get; set; }
        public string BankCity { get; set; }
        public string BankState { get; set; }
        public string MatchingCompanyIds { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public virtual IList<StoreCompanyAddress> Addresses { get; set; }
        public virtual IList<StoreIndividual> Individuals { get; set; }

        public bool HasExternalReference()
        {
            return !string.IsNullOrEmpty(ExternalReference) && ExternalReference != Helper.NOT_FOUND;
        }

        public override string ToString()
        {
            return Id + " - " + Name;
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            StoreAddress company = obj as StoreAddress;
            if (company != null) equals = company.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Gets the company address by going through all the addresses associated with the company
        /// </summary>
        /// <returns></returns>
        public StoreAddress GetCompanyAddress()
        {
            StoreAddress address = null;
            //find address which is neither shipping nor billing
            StoreCompanyAddress companyAddress = Addresses.Where(add => !add.IsShipping && !add.IsBilling).FirstOrDefault();
            if (companyAddress != null) address = companyAddress.Address;
            if (address == null)
            {
                //Try to use the shipping address for the company now
                companyAddress = Addresses.Where(add => add.IsShipping).FirstOrDefault();
                if (companyAddress != null) address = companyAddress.Address;
            }
            if (address == null)
            {
                //Try to use the billing address for the company now
                companyAddress = Addresses.Where(add => add.IsBilling).FirstOrDefault();
                if (companyAddress != null) address = companyAddress.Address;
            }
            return address;
        }

        /// <summary>
        /// Tries to find the a shipping address assigned to the company
        /// </summary>
        /// <returns></returns>
        public StoreAddress GetCompanyShippingAddress()
        {
            StoreAddress address = null;
            //Try to use the shipping address for the company now
            StoreCompanyAddress companyAddress = Addresses.Where(add => add.IsShipping).FirstOrDefault();
            if (companyAddress != null) address = companyAddress.Address;
            else address = GetCompanyAddress();
            return address;
        }

        public StoreAddress GetCompanyBillingAddress()
        {
            StoreAddress address = null;
            //Try to use the shipping address for the company now
            StoreCompanyAddress companyAddress = Addresses.Where(add => add.IsBilling).FirstOrDefault();
            if (companyAddress != null) address = companyAddress.Address;
            else address = GetCompanyAddress();
            return address;
        }

        public StoreIndividual GetCompanyContact()
        {
            StoreIndividual individual = null;
            //Try to use the primary individual for the company now
            individual = Individuals.Where(add => add.IsPrimary).FirstOrDefault();
            if (individual == null) individual = Individuals.FirstOrDefault();
            return individual;
        }

        public bool HasBillAddress
        {
            get
            {
                return Addresses.Where(add => add.IsBilling).Count() > 0;
            }
        }

        public bool HasShipAddress
        {
            get
            {
                return Addresses.Where(add => add.IsShipping).Count() > 0;
            }
        }
    }
}
