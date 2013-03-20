using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.model.timss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    //questions for gary
    //  Phone CountryCode?
    //Production time - Rush probably not needed
    public class TIMSSService : IFulfilmentService
    {
        IObjectService _objectService;

        public TIMSSService(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public virtual void Process(model.store.Order order, model.store.OrderDetailApplication application)
        {
            if (order == null || !order.UserId.HasValue || order.CreditCard == null || string.IsNullOrEmpty(order.CreditCard.ExternalReference)) throw new InvalidOperationException("You must pass a valid Order for this method");
            if (application == null) throw new InvalidOperationException("You must pass a valid Application for this method");
            TIMSSCompany company = new TIMSSCompany()
            {
                DAPP_UserId = order.UserId.Value,
                Name = application.Company,
                MasterCustomerId = order.ExternalReference,
                BillAddress1 = application.BillingAddress1,
                BillAddress2 = application.BillingAddress2,
                BillCity = application.BillingCity,
                BillPostalCode = application.BillingZip,
                BillState = application.BillingState,
                BillCountryCode = application.BillingCountry,
                ShipAddress1 = application.ShippingStreet1,
                ShipAddress2 = application.ShippingStreet2,
                ShipCity = application.ShippingCity,
                ShipPostalCode = application.ShippingZip,
                ShipCountryCode = application.ShippingCountry,
                ShipState = application.ShippingState,
                Url = application.BillingWebUrl,
                PhoneNumber = application.Phone,
            };
            _objectService.Add<TIMSSCompany>(company);
            TIMSSCreditInfo credit = new TIMSSCreditInfo()
            {
                DAPP_UserId = order.UserId.Value,
                Name = order.CreditCard.Name,
                FirstName = GetNamePart(order.CreditCard.Name, true),
                LastName = GetNamePart(order.CreditCard.Name, false),
                ExpirationMonth = order.CreditCard.ExpMonth,
                ExpirationYear = order.CreditCard.ExpYear,
                Type = order.CreditCard.Type,
                Number = order.CreditCard.ExternalReference,
                Street1 = application.BillingAddress1,
                Street2 = application.BillingAddress2,
                Zip = application.BillingZip,
                City = application.BillingCountry,
                State = application.BillingState,
                Country = application.BillingCountry,
                ExternalReference = new Guid(order.CreditCard.ExternalReference),
                DateCreated = DateTime.Now,
            };
            _objectService.Add<TIMSSCreditInfo>(credit);
            if (application is SupplierMembershipApplication)
            {
                company.CustomerClass = "Supplier";
                //add the contacts
                SupplierMembershipApplication supplierApplication = application as SupplierMembershipApplication;
                foreach (SupplierMembershipApplicationContact contact in supplierApplication.Contacts)
                {
                    TIMSSContact timssContact = new TIMSSContact()
                    {
                        DAPP_UserId = order.UserId.Value,
                        FirstName = GetNamePart(contact.Name, true),
                        LastName = GetNamePart(contact.Name, false),
                        PhoneNumber = contact.Phone,
                        Title = contact.Title,
                        Email = contact.Email,
                        PrimaryFlag = contact.IsPrimary ? "Y" : "N",
                    };
                    _objectService.Add<TIMSSContact>(timssContact);
                }
                //need to commit the data so far or the database will fail FK for Additional Information
                _objectService.SaveChanges();
                TIMSSAdditionalInfo additionalInformation = new TIMSSAdditionalInfo()
                {
                    DAPP_UserId = order.UserId.Value,
                    YearEstablished = supplierApplication.YearEstablished.HasValue ? supplierApplication.YearEstablished.Value : (int?)null,
                    YearEstablishedAsAdSpecialist = supplierApplication.YearEnteredAdvertising.HasValue ? supplierApplication.YearEnteredAdvertising.Value : (int?)null,
                    WomanOwned = supplierApplication.WomanOwned.HasValue ? (supplierApplication.WomanOwned.Value ? "Y" : "N") : null,
                    MinorityOwned = supplierApplication.LineMinorityOwned.HasValue ? (supplierApplication.LineMinorityOwned.Value ? "Y" : "N") : null,
                    NumberOfEmployees = supplierApplication.NumberOfEmployee,
                    HasAmericanProducts = supplierApplication.HasAmericanProducts.HasValue ? (supplierApplication.HasAmericanProducts.Value ? "Y" : "N") : null,
                    BusinessHours = supplierApplication.BusinessHours,
                    //TODO ProductionTime = supplierApplication.ProductionTime, issue with data types clashing
                    RushService = supplierApplication.IsRushServiceAvailable.HasValue ? (supplierApplication.IsRushServiceAvailable.Value ? "Y" : "N") : null,
                    Importer = supplierApplication.IsImporter.HasValue ? (supplierApplication.IsImporter.Value ? "Y" : "N") : null,
                    Manufacturer = supplierApplication.IsManufacturer.HasValue ? (supplierApplication.IsManufacturer.Value ? "Y" : "N") : null,
                    Retailer = supplierApplication.IsRetailer.HasValue ? (supplierApplication.IsRetailer.Value ? "Y" : "N") : null,
                    Wholesaler = supplierApplication.IsWholesaler.HasValue ? (supplierApplication.IsWholesaler.Value ? "Y" : "N") : null,
                    Imprinter = supplierApplication.IsImprinterVsDecorator.HasValue ? (supplierApplication.IsImprinterVsDecorator.Value ? "Y" : "N") : null,
                };
                //need to store the decorating type - probably a switch all properties are in this class
                foreach (var decoratingAndImprinting in supplierApplication.DecoratingTypes)
                {
                    switch (decoratingAndImprinting.Name)
                    {
                        case "Etching":
                            additionalInformation.Etching = "Y";
                            break;
                        case "Hot Stamping":
                            additionalInformation.HotStamping = "Y";
                            break;
                        case "Silkscreen":
                            additionalInformation.SilkScreen = "Y";
                            break;
                        case "Pad Print":
                            additionalInformation.PadPrinting = "Y";
                            break;
                        case "Direct Embroidery":
                            additionalInformation.DirectEmbroidery = "Y";
                            break;
                        case "Foil Stamping":
                            additionalInformation.FoilStamping = "Y";
                            break;
                        case "Lithography":
                            additionalInformation.Lithography = "Y";
                            break;
                        case "Sublimation":
                            additionalInformation.Sublimation = "Y";
                            break;
                        case "Four Color Process":
                            additionalInformation.FourColorProcess = "Y";
                            break;
                        case "Engraving":
                            additionalInformation.Engraving = "Y";
                            break;
                        case "Laser":
                            additionalInformation.Laser = "Y";
                            break;
                        case "Offset":
                            additionalInformation.Offset = "Y";
                            break;
                        case "Transfer":
                            additionalInformation.Transfer = "Y";
                            break;
                        case "Full Color Process":
                            additionalInformation.FullColorProcess = "Y";
                            break;
                        case "Die Stamp":
                            additionalInformation.DieStamp = "Y";
                            break;
                    }
                    //TODO imprint other value, nowhere to add the value stored in supplierApplication.OtherDec
                }
                _objectService.Add<TIMSSAdditionalInfo>(additionalInformation);
            }
            else if (application is DistributorMembershipApplication)
            {
                company.CustomerClass = "Distributor";
                //add the contacts
                DistributorMembershipApplication distributorApplication = application as DistributorMembershipApplication;
                foreach (DistributorMembershipApplicationContact contact in distributorApplication.Contacts)
                {
                    TIMSSContact timssContact = new TIMSSContact()
                    {
                        DAPP_UserId = order.UserId.Value,
                        FirstName = GetNamePart(contact.Name, true),
                        LastName = GetNamePart(contact.Name, false),
                        PhoneNumber = contact.Phone,
                        Title = contact.Title,
                        Email = contact.Email,
                        PrimaryFlag = contact.IsPrimary ? "Y" : "N",
                    };
                    _objectService.Add<TIMSSContact>(timssContact);
                }
                //need to commit the data so far or the database will fail FK for Additional Information
                _objectService.SaveChanges();
                //create the Additional Information records
                TIMSSAdditionalInfo additionalInformation = new TIMSSAdditionalInfo()
                {
                    DAPP_UserId = order.UserId.Value,
                    NumberOfEmployees = distributorApplication.NumberOfEmployee.HasValue ? distributorApplication.NumberOfEmployee.Value.ToString() : null,
                    NumberOfSalesPeople = distributorApplication.NumberOfSalesEmployee.HasValue ? distributorApplication.NumberOfSalesEmployee.Value.ToString() : null,
                    AnnualSalesVol = distributorApplication.AnnualSalesVolume,
                    AnnualSalesVolumeASP = distributorApplication.AnnualSalesVolumeASP,
                    YearEstablished = distributorApplication.EstablishedDate.HasValue ? distributorApplication.EstablishedDate.Value.Year : (int?)null,
                    BusinessRevenue = distributorApplication.PrimaryBusinessRevenue != null ? distributorApplication.PrimaryBusinessRevenue.Name : null,
                    BusinessRevenueOther = distributorApplication.OtherBusinessRevenue,
                };
                _objectService.Add<TIMSSAdditionalInfo>(additionalInformation);
                //add the TIMSSAccountType description + subcode
                foreach (var accountType in distributorApplication.AccountTypes)
                {
                    TIMSSAccountType timssAccountType = new TIMSSAccountType()
                    {
                        DAPP_UserId = order.UserId.Value,
                        SubCode = accountType.SubCode,
                        Description = accountType.Description,
                    };
                    _objectService.Add<TIMSSAccountType>(timssAccountType);
                }
                //Add the TIMSSProductType description + subcode
                foreach (var productLine in distributorApplication.ProductLines)
                {
                    TIMSSProductType timssProductLine = new TIMSSProductType()
                    {
                        DAPP_UserId = order.UserId.Value,
                        SubCode = productLine.SubCode,
                        Description = productLine.Description,
                    };
                    _objectService.Add<TIMSSProductType>(timssProductLine);
                }
            }
            _objectService.SaveChanges();
        }

        private string GetNamePart(string name, bool first)
        {
            string[] nameParts = name.Split(' ');
            if (nameParts.Length > 0 && first) return nameParts[0].Trim();
            else if (!first && nameParts.Length > 0) return nameParts[nameParts.Length - 1].Trim();
            else return name;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            //no unmanaged resource to free at this point
        }

        #endregion IDisposable
    }
}
