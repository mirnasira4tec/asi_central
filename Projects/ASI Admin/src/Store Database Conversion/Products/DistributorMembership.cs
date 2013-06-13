using asi.asicentral.database;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store_Database_Conversion.Products
{
    class DistributorMembership : BaseProductConvert
    {
        public override void Convert(StoreOrderDetail newOrderDetail, LegacyOrderDetail detail, StoreContext storeContext, ASIInternetContext asiInternetContext)
        {
            //retrieve the current application
            LegacyDistributorMembershipApplication application = asiInternetContext.DistributorMembershipApplications.Where(app => app.UserId == detail.Order.UserId).SingleOrDefault();
            if (application == null && !IgnoreOrderIssues(detail))
            {
                ILogService logService = LogService.GetLog(this.GetType());
                logService.Error("Expected to find and application and could not retrieve it: " + detail.Order.Id);
            }
            else if (application != null)
            {
                //creating a new application
                StoreDetailDistributorMembership newMembership = new StoreDetailDistributorMembership()
                {
                    OrderDetailId = newOrderDetail.Id,
                    AnnualSalesVolume = application.AnnualSalesVolume,
                    AnnualSalesVolumeASP = application.AnnualSalesVolumeASP,
                    AppStatusId = application.ApplicationStatusId,
                    ASIContactName = application.ASIContact,
                    Custom1 = application.Custom1,
                    Custom2 = application.Custom2,
                    Custom5 = application.Custom5,
                    EstablishedDate = application.EstablishedDate,
                    IsCorporateOfficer = application.CorporateOfficer,
                    IsForProfit = application.IsForProfit,
                    IsMajorForResale = application.IsMajorForResale,
                    IsMajorityDistributeForResale = application.IsMajorityDistributeForResale,
                    IsSolelyWork = application.IsSolelyWork,
                    HasRecSpecials = application.AgreeReceivePromotionalProducts,
                    LegacyApplicationId = application.Id.ToString(),
                    NumberOfEmployee = application.NumberOfEmployee,
                    NumberOfSalesEmployee = application.NumberOfSalesEmployee,
                    OtherBusinessRevenue = application.OtherBusinessRevenue,
                    SolelyWorkName = application.SolelyWorkName,
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.UpdateDate,
                    UpdateSource = newOrderDetail.UpdateSource,
                };
                storeContext.StoreDetailDistributorMemberships.Add(newMembership);
                if (application.PrimaryBusinessRevenueId.HasValue && application.PrimaryBusinessRevenueId.Value > 0)
                {
                    LookDistributorRevenueType revenue = storeContext.LookDistributorRevenueTypes.Where(t => t.Id == application.PrimaryBusinessRevenueId.Value).FirstOrDefault();
                    //newMembership.PrimaryBusinessRevenue = revenue;
                }
                //update the product line - ids match between legacy records and new db ones
                foreach (var product in application.ProductLines)
                {
                    LookProductLine newProduct = storeContext.LookProductLines.Where(t => t.Id == product.Id).First();
                    newMembership.ProductLines.Add(newProduct);
                }
                //update the account type - ids match between legacy records and new db ones
                foreach (var accountType in application.AccountTypes)
                {
                    LookDistributorAccountType account = storeContext.LookDistributorAccountTypes.Where(t => t.Id == accountType.Id).First();
                    newMembership.AccountTypes.Add(account);
                }
                //update primary business revenue - ids should match
                if (application.PrimaryBusinessRevenue != null)
                {
                    LookDistributorRevenueType revenue = storeContext.LookDistributorRevenueTypes.Where(t => t.Id == application.PrimaryBusinessRevenue.Id).First();
                    newMembership.PrimaryBusinessRevenue = revenue;
                }
                //always create a new company for transfer
                StoreCompany company = new StoreCompany()
                {
                    Name = application.Company,
                    MemberType = "Distributor",
                    Phone = application.Phone,
                    WebURL = application.BillingWebUrl,
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.UpdateDate,
                    UpdateSource = newOrderDetail.UpdateSource,                    
                };
                newOrderDetail.Order.Company = company;
                storeContext.StoreCompanies.Add(company); //leave this here, EF gets confused with the objects if this is moved
                //need to assign the company to the billing individual
                if (newOrderDetail.Order.BillingIndividual != null) newOrderDetail.Order.BillingIndividual.Company = company;

                //we have 3 addresses, they might all be the same. We already have a billing address stored with the order through billing individual
                StoreAddress reference = null;
                if (newOrderDetail.Order.BillingIndividual != null && newOrderDetail.Order.BillingIndividual.Address != null) reference = newOrderDetail.Order.BillingIndividual.Address;
                //add billing address
                StoreAddress companyAddress = new StoreAddress()
                {
                    Street1 = application.BillingAddress1,
                    Street2 = application.BillingAddress2,
                    City = application.BillingCity,
                    Zip = application.BillingZip,
                    State = application.BillingState,
                    Country = application.BillingCountry,
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.UpdateDate,
                    UpdateSource = newOrderDetail.UpdateSource,
                };
                if (newOrderDetail.Order.BillingIndividual != null) company.Individuals.Add(newOrderDetail.Order.BillingIndividual);
                StoreCompanyAddress newCompanyAddress = new StoreCompanyAddress()
                {
                    IsBilling = true,
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.UpdateDate,
                    UpdateSource = newOrderDetail.UpdateSource,
                };
                if (reference != null && reference.AreEquivalent(companyAddress)) newCompanyAddress.Address = reference;
                else newCompanyAddress.Address = companyAddress;
                if (newCompanyAddress.Address.IsValid)
                {
                    storeContext.StoreCompanyAddresses.Add(newCompanyAddress);
                    company.Addresses.Add(newCompanyAddress);
                    reference = newCompanyAddress.Address;
                }
                //add company address
                companyAddress = new StoreAddress()
                {
                    Street1 = application.Address1,
                    Street2 = application.Address2,
                    City = application.City,
                    Zip = application.Zip,
                    State = application.State,
                    Country = application.Country,
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.UpdateDate,
                    UpdateSource = newOrderDetail.UpdateSource,                    
                };
                newCompanyAddress = new StoreCompanyAddress()
                {
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.UpdateDate,
                    UpdateSource = newOrderDetail.UpdateSource,                                        
                };
                if (reference != null && reference.AreEquivalent(companyAddress)) newCompanyAddress.Address = reference;
                else newCompanyAddress.Address = companyAddress;
                if (newCompanyAddress.Address.IsValid)
                {
                    storeContext.StoreCompanyAddresses.Add(newCompanyAddress);
                    company.Addresses.Add(newCompanyAddress);
                    reference = newCompanyAddress.Address;
                }
                //add shipping address
                companyAddress = new StoreAddress()
                {
                    Street1 = application.ShippingStreet1,
                    Street2 = application.ShippingStreet2,
                    City = application.ShippingCity,
                    Zip = application.ShippingZip,
                    State = application.ShippingState,
                    Country = application.ShippingCountry,
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.UpdateDate,
                    UpdateSource = newOrderDetail.UpdateSource,
                };
                newCompanyAddress = new StoreCompanyAddress()
                {
                    IsShipping = true,
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.UpdateDate,
                    UpdateSource = newOrderDetail.UpdateSource,
                };
                
                if (reference != null && reference.AreEquivalent(companyAddress)) newCompanyAddress.Address = reference;
                else newCompanyAddress.Address = companyAddress;
                if (newCompanyAddress.Address.IsValid)
                {
                    storeContext.StoreCompanyAddresses.Add(newCompanyAddress);
                    company.Addresses.Add(newCompanyAddress);
                }
                //need to add the contacts - avoid duplicates
                foreach (var existingContact in application.Contacts)
                {
                    StoreIndividual contact = company.Individuals
                        .Where(t => t.FirstName == StoreIndividual.GetFirstName(existingContact.Name) && t.LastName == StoreIndividual.GetLastName(existingContact.Name))
                        .FirstOrDefault();
                    if (contact == null)
                    {
                        StoreIndividual individual = new StoreIndividual()
                        {
                            Company = company,
                            FirstName = StoreIndividual.GetFirstName(existingContact.Name),
                            LastName = StoreIndividual.GetLastName(existingContact.Name),
                            IsPrimary = existingContact.IsPrimary,
                            Department = existingContact.Department,
                            Title = existingContact.Title,
                            Email = existingContact.Email,
                            Fax = existingContact.Fax,
                            Phone = existingContact.Phone,
                            CreateDate = newOrderDetail.CreateDate,
                            UpdateDate = newOrderDetail.UpdateDate,
                            UpdateSource = newOrderDetail.UpdateSource,
                        };
                        company.Individuals.Add(individual);
                    }
                    else
                    {
                        if (contact.Company == null) contact.Company = company;
                        if (contact.Title == null) contact.Title = existingContact.Title;
                        if (contact.Department == null) contact.Department = existingContact.Department;
                        if (contact.Email == null) contact.Email = existingContact.Email;
                        if (contact.Fax == null) contact.Fax = existingContact.Fax;
                        if (contact.Phone == null) contact.Phone = existingContact.Phone;
                        if (!contact.IsPrimary && existingContact.IsPrimary) contact.IsPrimary = true;
                    }
                }
            }
        }
    }
}
