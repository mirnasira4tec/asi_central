using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using asi.asicentral.Resources;

namespace asi.asicentral.model.store
{
    public class LegacyDistributorMembershipApplication : LegacyOrderDetailApplication
    {
        public LegacyDistributorMembershipApplication()
        {
            if (this.GetType() == typeof(LegacyDistributorMembershipApplication))
            {
                Contacts = new List<LegacyDistributorMembershipApplicationContact>();
                AccountTypes = new List<LegacyDistributorAccountType>();
                ProductLines = new List<LegacyDistributorProductLine>();
            }
        }

        [Display(ResourceType = typeof(Resource), Name = "ApplicationStatus")]
        public Nullable<int> ApplicationStatusId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "LastName")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NumberOfEmployee")]
        public Nullable<int> NumberOfEmployee { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NumberOfSalesEmployee")]
        public Nullable<int> NumberOfSalesEmployee { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AnnualSalesVolume")]
        public string AnnualSalesVolume { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ASIContact")]
        public string ASIContact { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AnnualSalesVolumeASP")]
        public string AnnualSalesVolumeASP { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "CorporateOfficer")]
        public Nullable<bool> CorporateOfficer { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SignatureType")]
        public Nullable<int> SignatureType { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsMajorForResale")]
        public Nullable<bool> IsMajorForResale { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsForProfit")]
        public Nullable<bool> IsForProfit { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ProvideInvoiceOnDemand")]
        public Nullable<bool> ProvideInvoiceOnDemand { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsSolelyWork")]
        public Nullable<bool> IsSolelyWork { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "SolelyWorkName")]
        public string SolelyWorkName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "InformASIOfChange")]
        public Nullable<bool> InformASIOfChange { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ApplicantName")]
        public string ApplicantName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ApplicantEmail")]
        [DataType(DataType.EmailAddress)]
        public string ApplicantEmail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "TrueAnswers")]
        public Nullable<bool> TrueAnswers { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AgreeReceivePromotionalProducts")]
        public Nullable<bool> AgreeReceivePromotionalProducts { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "AgreeTermsAndConditions")]
        public Nullable<bool> AgreeTermsAndConditions { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsMajorityDistributeForResale")]
        public Nullable<bool> IsMajorityDistributeForResale { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IPAddress")]
        public string IPAddress { get; set; }

        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string Custom4 { get; set; }
        public string Custom5 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IPAddress")]
        public Nullable<int> PrimaryBusinessRevenueId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "OtherBusinessRevenue")]
        public string OtherBusinessRevenue { get; set; }

        public Nullable<DateTime> EstablishedDate { get; set; }

        public virtual LegacyDistributorBusinessRevenue PrimaryBusinessRevenue { get; set; }
        public virtual IList<LegacyDistributorMembershipApplicationContact> Contacts { get; set; }
        public virtual ICollection<LegacyDistributorAccountType> AccountTypes { get; set; }
        public virtual ICollection<LegacyDistributorProductLine> ProductLines { get; set; }

        public void CopyTo(LegacyDistributorMembershipApplication target)
        {
            // sync the collections
            SyncContactsWith(target);
            SyncAccountTypesWith(target);
            SyncProductLinesWith(target);

            target.AgreeReceivePromotionalProducts = AgreeReceivePromotionalProducts;
            target.AgreeTermsAndConditions = AgreeTermsAndConditions;
            target.AnnualSalesVolume = AnnualSalesVolume;
            target.AnnualSalesVolumeASP = AnnualSalesVolumeASP;
            target.ApplicantEmail = ApplicantEmail;
            target.ApplicantName = ApplicantName;
            target.ApplicationStatusId = ApplicationStatusId;
            target.ASIContact = ASIContact;
            target.Company = Company;
            target.Address1 = Address1;
            target.Address2 = Address2;
            target.City = City;
            target.State = State;
            target.Zip = Zip;
            target.Country = Country;
            target.HasBillAddress = HasBillAddress;
            if (HasBillAddress)
            {
                target.BillingEmail = BillingEmail;
                target.BillingPhone = BillingPhone;
                target.BillingAddress1 = BillingAddress1;
                target.BillingAddress2 = BillingAddress2;
                target.BillingState = BillingState;
                target.BillingCity = BillingCity;
                target.BillingZip = BillingZip;
                target.BillingCountry = BillingCountry;
                target.BillingWebUrl = BillingWebUrl;
            }
            else
            {
                target.BillingAddress1 = Address1;
                target.BillingAddress2 = Address2;
                target.BillingState = State;
                target.BillingCity = City;
                target.BillingZip = Zip;
                target.BillingCountry = Country;
            }
            target.HasShipAddress = HasShipAddress;
            if (target.HasShipAddress)
            {
                target.ShippingStreet1 = ShippingStreet1;
                target.ShippingStreet2 = ShippingStreet2;
                target.ShippingCity = ShippingCity;
                target.ShippingState = ShippingState;
                target.ShippingZip = ShippingZip;
                target.ShippingCountry = ShippingCountry;
            }
            else
            {
                target.ShippingStreet1 = Address1;
                target.ShippingStreet2 = Address2;
                target.ShippingCity = City;
                target.ShippingState = State;
                target.ShippingZip = Zip;
                target.ShippingCountry = Country;
            }
            target.CorporateOfficer = CorporateOfficer;
            target.Custom1 = Custom1;
            target.Custom2 = Custom2;
            target.Custom3 = Custom3;
            target.Custom4 = Custom4;
            target.Custom5 = Custom5;
            target.EstablishedDate = EstablishedDate;
            target.BillingFax = BillingFax;
            target.FirstName = FirstName;
            target.Id = Id;
            target.InformASIOfChange = InformASIOfChange;
            target.IPAddress = IPAddress;
            target.IsForProfit = IsForProfit;
            target.IsMajorForResale = IsMajorForResale;
            target.IsMajorityDistributeForResale = IsMajorityDistributeForResale;
            target.IsSolelyWork = IsSolelyWork;
            target.LastName = LastName;
            target.NumberOfEmployee = NumberOfEmployee;
            target.NumberOfSalesEmployee = NumberOfSalesEmployee;
            target.OtherBusinessRevenue = OtherBusinessRevenue;
            target.PrimaryBusinessRevenueId = PrimaryBusinessRevenueId;
            target.PrimaryBusinessRevenue = PrimaryBusinessRevenue;
            target.ProvideInvoiceOnDemand = ProvideInvoiceOnDemand;
            target.SignatureType = SignatureType;
            target.SolelyWorkName = SolelyWorkName;
            target.TrueAnswers = TrueAnswers;
            target.UserId = UserId;
        }

        private void SyncAccountTypesWith(LegacyDistributorMembershipApplication target)
        {
            // sync the account types
            if (target.AccountTypes == null || target.AccountTypes.Count == 0) target.AccountTypes = AccountTypes;
            else
            {
                for (int i = AccountTypes.Count - 1; i >= 0; i--)
                {
                    LegacyDistributorAccountType originalAcccountType = AccountTypes.ElementAt(i);
                    LegacyDistributorAccountType targetAccountType = target.AccountTypes.Where(theAccountType => theAccountType.Id == originalAcccountType.Id).SingleOrDefault();
                    if (targetAccountType == null)
                    {
                        // target is missing an account type
                        target.AccountTypes.Add(originalAcccountType);
                    }
                }
                for (int i = target.AccountTypes.Count - 1; i >= 0; i--)
                {
                    LegacyDistributorAccountType targetAccountType = target.AccountTypes.ElementAt(i);
                    LegacyDistributorAccountType originalAccountType = AccountTypes.Where(accountType => accountType.Id == targetAccountType.Id).SingleOrDefault();
                    if (originalAccountType == null) target.AccountTypes.Remove(targetAccountType);
                }
            }
        }

        private void SyncProductLinesWith(LegacyDistributorMembershipApplication target)
        {
            // sync the product lines
            if (target.ProductLines == null || target.ProductLines.Count == 0) target.ProductLines = ProductLines;
            else
            {
                for (int i = ProductLines.Count - 1; i >= 0; i--)
                {
                    LegacyDistributorProductLine originalProductLine = ProductLines.ElementAt(i);
                    LegacyDistributorProductLine targetProductLine = target.ProductLines.Where(productLine => productLine.Id == originalProductLine.Id).SingleOrDefault();
                    if (targetProductLine == null)
                    {
                        // update existing
                        target.ProductLines.Add(originalProductLine);
                    }
                }
                for (int i = target.ProductLines.Count - 1; i >= 0; i--)
                {
                    LegacyDistributorProductLine targetProductLine = target.ProductLines.ElementAt(i);
                    LegacyDistributorProductLine originalProductLine = ProductLines.Where(productLine => productLine.Id == targetProductLine.Id).SingleOrDefault();
                    if (originalProductLine == null) target.ProductLines.Remove(targetProductLine);
                }
            }
        }

        private void SyncContactsWith(LegacyDistributorMembershipApplication target)
        {
            //sync the contacts
            if (target.Contacts == null || target.Contacts.Count == 0) target.Contacts = Contacts;
            else
            {
                //got through the target contacts and update
                for (int i = Contacts.Count - 1; i >= 0; i--)
                {
                    LegacyDistributorMembershipApplicationContact originalContact = Contacts[i];
                    LegacyDistributorMembershipApplicationContact targetContact = target.Contacts.Where(theContact => theContact.Id == originalContact.Id).SingleOrDefault();
                    if (targetContact != null)
                    {
                        //contact already there, update it
                        targetContact.Name = originalContact.Name;
                        targetContact.Title = originalContact.Title;
                        targetContact.Email = originalContact.Email;
                        targetContact.Phone = originalContact.Phone;
                        targetContact.Fax = originalContact.Fax;
                        targetContact.IsPrimary = originalContact.IsPrimary;
                    }
                    else
                    {
                        //target is missing a contact
                        target.Contacts.Add(new LegacyDistributorMembershipApplicationContact()
                        {
                            Email = targetContact.Email,
                            Fax = targetContact.Fax,
                            IsPrimary = targetContact.IsPrimary,
                            Name = targetContact.Name,
                            Department = targetContact.Name,
                            Phone = targetContact.Phone,
                            Title = targetContact.Title,
                        });
                    }
                }
                for (int i = target.Contacts.Count - 1; i >= 0; i--)
                {
                    LegacyDistributorMembershipApplicationContact targetContact = target.Contacts[i];
                    LegacyDistributorMembershipApplicationContact originalContact = Contacts.Where(theContact => theContact.Id == targetContact.Id).SingleOrDefault();
                    if (originalContact == null) target.Contacts.Remove(targetContact);
                }
            }
        }
    }
}
