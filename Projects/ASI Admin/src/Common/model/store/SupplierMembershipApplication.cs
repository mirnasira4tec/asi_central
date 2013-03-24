using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace asi.asicentral.model.store
{
    public class SupplierMembershipApplication : OrderDetailApplication
    {
        public SupplierMembershipApplication()
        {
            if (this.GetType() == typeof(SupplierMembershipApplication))
            {
                Contacts = new List<SupplierMembershipApplicationContact>();
                DecoratingTypes = new List<SupplierDecoratingType>();
            }
        }

        [Display(ResourceType = typeof(Resource), Name = "ApplicationStatus")]
        public Nullable<int> ApplicationStatusId { get; set; }

        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        [Display(ResourceType = typeof(Resource), Name = "BillingTollPhone")]
        public string BillingTollFree { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ContactName")]
        public string ContactName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ContactTitle")]
        public string ContactTitle { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(ResourceType = typeof(Resource), Name = "ContactEmail")]
        public string ContactEmail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ContactPhone")]
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        public string ContactPhone { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "LineNames")]
        public string LineNames { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "MinorityOwned")]
        public Nullable<bool> LineMinorityOwned { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SalesVolume")]
        public string SalesVolume { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "YearEstablished")]
        public Nullable<int> YearEstablished { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "YearEnteredAdvertising")]
        public Nullable<int> YearEnteredAdvertising { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "OfficeHour")]
        public string OfficeHourStart { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "OfficeHourEnd")]
        public string OfficeHourEnd { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NumberOfEmployee")]
        public string NumberOfEmployee { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NumberOfSalesEmployee")]
        public string NumberOfSalesEmployee { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsImprinterVsDecorator")]
        public Nullable<bool> IsImprinterVsDecorator { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsImporter")]
        public Nullable<bool> IsImporter { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsManufacturer")]
        public Nullable<bool> IsManufacturer { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsRetailer")]
        public Nullable<bool> IsRetailer { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsWholesaler")]
        public Nullable<bool> IsWholesaler { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "FedTaxId")]
        public Nullable<int> FedTaxId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SellToEndUsers")]
        public Nullable<bool> SellToEndUsers { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SellThruDistributors")]
        public Nullable<bool> SellThruDistributors { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SellThruInternet")]
        public Nullable<bool> SellThruInternet { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SellThruDirectMarketing")]
        public Nullable<bool> SellThruDirectMarketing { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SellThruRetail")]
        public Nullable<bool> SellThruRetail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SellThruAffiliate")]
        public Nullable<bool> SellThruAffiliate { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AffiliateCompanyName")]
        public string AffiliateCompanyName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AffiliateASINumber")]
        public Nullable<int> AffiliateASINumber { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsUnionMade")]
        public Nullable<bool> IsUnionMade { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ProductionTime")]
        public string ProductionTime { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsRushServiceAvailable")]
        public Nullable<bool> IsRushServiceAvailable { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "OtherDec")]
        public string OtherDec { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsUPSAvailable")]
        public Nullable<bool> IsUPSAvailable { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "UPSAddress")]
        public string UPSAddress { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "UPSCity")]
        public string UPSCity { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "UPSState")]
        public string UPSState { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "UPSZip")]
        public string UPSZip { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "UPSShippingNumber")]
        public string UPSShippingNumber { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AuthorizeUPSNewAccount")]
        public Nullable<bool> AuthorizeUPSNewAccount { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AgreeUPSTermsAndConditions")]
        public Nullable<bool> AgreeUPSTermsAndConditions { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AgreeASITermsAndConditions")]
        public Nullable<bool> AgreeASITermsAndConditions { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsWomanOwned")]
        public Nullable<bool> WomanOwned { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "HasAmericanProducts")]
        public Nullable<bool> HasAmericanProducts { get; set; }
        
        [Display(ResourceType = typeof(Resource), Name = "BusinessHours")]
        public string BusinessHours { get; set; }

        public virtual IList<SupplierMembershipApplicationContact> Contacts { get; set; }
        public virtual ICollection<SupplierDecoratingType> DecoratingTypes { get; set; }

        public void CopyTo(SupplierMembershipApplication target)
        {
            target.HasBillAddress = HasBillAddress;
            target.HasShipAddress = HasShipAddress;
            target.DecoratingTypes = DecoratingTypes;

            if (target.Contacts == null) target.Contacts = Contacts;
            else
            {
                //got through the target contacts and update
                for (int i = Contacts.Count - 1; i >= 0; i--)
                {
                    SupplierMembershipApplicationContact viewContact = Contacts[i];
                    SupplierMembershipApplicationContact targetContact = target.Contacts.Where(theContact => theContact.Id == viewContact.Id).SingleOrDefault();
                    if (targetContact != null)
                    {
                        //contact already there, update it
                        targetContact.Name = viewContact.Name;
                        targetContact.Title = viewContact.Title;
                        targetContact.Email = viewContact.Email;
                        targetContact.Phone = viewContact.Phone;
                        targetContact.Fax = viewContact.Fax;
                        targetContact.IsPrimary = viewContact.IsPrimary;
                    }
                    else
                    {
                        //target is missing a contact
                        target.Contacts.Add(new SupplierMembershipApplicationContact()
                        {
                            Email = targetContact.Email,
                            Fax = targetContact.Fax,
                            IsPrimary = targetContact.IsPrimary,
                            Name = targetContact.Name,
                            Department = targetContact.Name,
                            Phone = targetContact.Phone,
                            SalesId = targetContact.SalesId,
                            Title = targetContact.Title,
                        });
                    }
                }
                for (int i = target.Contacts.Count - 1; i >= 0; i--)
                {
                    SupplierMembershipApplicationContact targetContact = Contacts[i];
                    SupplierMembershipApplicationContact viewContact = Contacts.Where(theContact => theContact.Id == targetContact.Id).SingleOrDefault();
                    if (viewContact == null) target.Contacts.Remove(targetContact);
                }
            }

            target.AffiliateASINumber = AffiliateASINumber;
            target.AffiliateCompanyName = AffiliateCompanyName;
            target.AgreeASITermsAndConditions = AgreeASITermsAndConditions;
            target.AgreeUPSTermsAndConditions = AgreeUPSTermsAndConditions;
            target.ApplicationStatusId = ApplicationStatusId;
            target.AuthorizeUPSNewAccount = AuthorizeUPSNewAccount;
            target.BillingAddress1 = BillingAddress1;
            target.BillingAddress2 = BillingAddress2;
            target.BillingCity = BillingCity;
            target.BillingEmail = BillingEmail;
            target.BillingFax = BillingFax;
            target.BillingPhone = BillingPhone;
            target.BillingState = BillingState;
            target.BillingCountry = BillingCountry;
            target.BillingTollFree = BillingTollFree;
            target.BillingWebUrl = BillingWebUrl;
            target.BillingZip = BillingZip;
            target.Company = Company;
            target.ContactEmail = ContactEmail;
            target.ContactName = ContactName;
            target.ContactPhone = ContactPhone;
            target.ContactTitle = ContactTitle;
            target.FedTaxId = FedTaxId;
            target.Id = Id;
            target.BusinessHours = BusinessHours;
            target.HasAmericanProducts = HasAmericanProducts;
            target.IsImporter = IsImporter;
            target.IsImprinterVsDecorator = IsImprinterVsDecorator;
            target.IsManufacturer = IsManufacturer;
            target.IsRetailer = IsRetailer;
            target.IsRushServiceAvailable = IsRushServiceAvailable;
            target.IsUnionMade = IsUnionMade;
            target.IsUPSAvailable = IsUPSAvailable;
            target.IsWholesaler = IsWholesaler;
            target.LineMinorityOwned = LineMinorityOwned;
            target.LineNames = LineNames;
            target.NumberOfEmployee = NumberOfEmployee;
            target.NumberOfSalesEmployee = NumberOfSalesEmployee;
            target.OfficeHourEnd = OfficeHourEnd;
            target.OfficeHourStart = OfficeHourStart;
            target.OtherDec = OtherDec;
            target.ProductionTime = ProductionTime;
            target.SalesVolume = SalesVolume;
            target.SellThruAffiliate = SellThruAffiliate;
            target.SellThruDirectMarketing = SellThruDirectMarketing;
            target.SellThruDistributors = SellThruDistributors;
            target.SellThruInternet = SellThruInternet;
            target.SellThruRetail = SellThruRetail;
            target.SellToEndUsers = SellToEndUsers;
            target.ShippingStreet1 = ShippingStreet1;
            target.ShippingStreet2 = ShippingStreet2;
            target.ShippingCity = ShippingCity;
            target.ShippingState = ShippingState;
            target.ShippingZip = ShippingZip;
            target.ShippingCountry = ShippingCountry;
            target.UPSAddress = UPSAddress;
            target.UPSCity = UPSCity;
            target.UPSShippingNumber = UPSShippingNumber;
            target.UPSState = UPSState;
            target.UPSZip = UPSZip;
            target.UserId = UserId;
            target.WomanOwned = WomanOwned;
            target.YearEnteredAdvertising = YearEnteredAdvertising;
            target.YearEstablished = YearEstablished;
            target.Address1 = Address1;
            target.Address2 = Address2;
            target.City = City;
            target.State = State;
            target.Zip = Zip;
            target.Country = Country;
        }
    }
}
