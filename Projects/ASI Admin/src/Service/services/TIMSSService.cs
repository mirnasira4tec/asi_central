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
    //Issues for the conversion
    //Phone CountryCode?
    //Production time - Rush probably not needed
    //state for international - limited to 15 chars in TIMSS
    //DAPP_AnnSalesVol and DAPP_AnnSalesVolSAP expect numbers
    //supplier decorating other label cannot be stored
    public class TIMSSService : IFulfilmentService
    {
        IObjectService _objectService;

        public TIMSSService(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public virtual Guid Process(StoreOrder order, StoreDetailApplication application)
        {
            if (order == null || order.BillingIndividual == null || order.CreditCard == null || string.IsNullOrEmpty(order.CreditCard.ExternalReference)) throw new InvalidOperationException("You must pass a valid Order for this method");

            //Added code to support magazines
            bool isMagazineRequest = false;
            if(order != null && order.OrderDetails != null && order.OrderDetails.Count >0)
            {
                StoreOrderDetail orderDetail = order.OrderDetails.ElementAt(0);
                if(orderDetail != null && orderDetail.Product != null && orderDetail.Product.Type == "Magazine")
                    isMagazineRequest = true;
            }

            if (application == null && !isMagazineRequest) throw new InvalidOperationException("You must pass a valid Application or Magazine details for this method");
            TIMSSCompany company = new TIMSSCompany()
            {
                //@todo talk to gary about that column
                DAPP_UserId = Guid.NewGuid(),
                Name = order.Company.Name,
                MasterCustomerId = order.ExternalReference,
                BillAddress1 = order.BillingIndividual.Address.Street1,
                BillAddress2 = order.BillingIndividual.Address.Street2,
                BillCity = order.BillingIndividual.Address.City,
                BillPostalCode = order.BillingIndividual.Address.Zip,
                BillState = order.BillingIndividual.Address.State,
                BillCountryCode = order.BillingIndividual.Address.Country,
                Url = order.Company.WebURL,
                PhoneNumber = order.BillingIndividual.Phone,
            };
            //find the shipping information if we have it
            if (order.Company != null) {
                StoreCompanyAddress shippingCompanyAddress = order.Company.Addresses.Where(add => add.IsShipping).FirstOrDefault();
                if (shippingCompanyAddress != null)
                {
                    company.ShipAddress1 = shippingCompanyAddress.Address.Street1;
                    company.ShipAddress2 = shippingCompanyAddress.Address.Street2;
                    company.ShipCity = shippingCompanyAddress.Address.City;
                    company.ShipPostalCode = shippingCompanyAddress.Address.Zip;
                    company.ShipCountryCode = shippingCompanyAddress.Address.Country;
                    company.ShipState = shippingCompanyAddress.Address.State;
                }
            }
            _objectService.Add<TIMSSCompany>(company);
            TIMSSCreditInfo credit = new TIMSSCreditInfo()
            {
                DAPP_UserId = company.DAPP_UserId,
                Name = order.CreditCard.CardHolderName,
                FirstName = GetNamePart(order.CreditCard.CardHolderName, true),
                LastName = GetNamePart(order.CreditCard.CardHolderName, false),
                ExpirationMonth = order.CreditCard.ExpMonth,
                ExpirationYear = order.CreditCard.ExpYear,
                Type = order.CreditCard.CardType,
                Number = order.CreditCard.ExternalReference,
                Street1 = order.BillingIndividual.Address.Street1,
                Street2 = order.BillingIndividual.Address.Street2,
                Zip = order.BillingIndividual.Address.Zip,
                City = order.BillingIndividual.Address.City,
                State = order.BillingIndividual.Address.State,
                Country = order.BillingIndividual.Address.Country,
                ExternalReference = new Guid(order.CreditCard.ExternalReference),
                DateCreated = DateTime.Now,
            };
            _objectService.Add<TIMSSCreditInfo>(credit);
            //add the contacts
            if (order.Company != null && order.Company.Individuals.Count > 0)
            {
                foreach (StoreIndividual contact in order.Company.Individuals)
                {
                    TIMSSContact timssContact = new TIMSSContact()
                    {
                        DAPP_UserId = company.DAPP_UserId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        PhoneNumber = contact.Phone,
                        Title = contact.Title,
                        Email = contact.Email,
                        PrimaryFlag = contact.IsPrimary ? "Y" : "N",
                    };
                    _objectService.Add<TIMSSContact>(timssContact);
                }
            }
            if (application is StoreDetailSupplierMembership)
            {
                company.CustomerClass = "Supplier";
                //Needs to be there for FK to resolves themselves
                _objectService.SaveChanges();
                //add the contacts
                StoreDetailSupplierMembership supplierApplication = application as StoreDetailSupplierMembership;
                TIMSSAdditionalInfo additionalInformation = new TIMSSAdditionalInfo()
                {
                    DAPP_UserId = company.DAPP_UserId,
                    YearEstablished = supplierApplication.YearEstablished.HasValue ? supplierApplication.YearEstablished.Value : (int?)null,
                    YearEstablishedAsAdSpecialist = supplierApplication.YearEnteredAdvertising.HasValue ? supplierApplication.YearEnteredAdvertising.Value : (int?)null,
                    WomanOwned = supplierApplication.WomanOwned.HasValue ? (supplierApplication.WomanOwned.Value ? "Y" : "N") : null,
                    MinorityOwned = supplierApplication.IsMinorityOwned.HasValue ? (supplierApplication.IsMinorityOwned.Value ? "Y" : "N") : null,
                    NumberOfEmployees = supplierApplication.NumberOfEmployee,
                    HasAmericanProducts = supplierApplication.HasAmericanProducts.HasValue ? (supplierApplication.HasAmericanProducts.Value ? "Y" : "N") : null,
                    BusinessHours = supplierApplication.BusinessHours,
                    RushService = supplierApplication.IsRushServiceAvailable.HasValue ? (supplierApplication.IsRushServiceAvailable.Value ? "Y" : "N") : null,
                    Importer = supplierApplication.IsImporter.HasValue ? (supplierApplication.IsImporter.Value ? "Y" : "N") : null,
                    Manufacturer = supplierApplication.IsManufacturer.HasValue ? (supplierApplication.IsManufacturer.Value ? "Y" : "N") : null,
                    Retailer = supplierApplication.IsRetailer.HasValue ? (supplierApplication.IsRetailer.Value ? "Y" : "N") : null,
                    Wholesaler = supplierApplication.IsWholesaler.HasValue ? (supplierApplication.IsWholesaler.Value ? "Y" : "N") : null,
                    Imprinter = supplierApplication.IsImprinterVsDecorator.HasValue ? (supplierApplication.IsImprinterVsDecorator.Value ? "Y" : "N") : null,
                    LineName = supplierApplication.LineNames,
                };
                //try to convert different data types
                //ProductionTime = supplierApplication.ProductionTime, issue with data types clashing
                try { additionalInformation.ProductionTime = int.Parse(supplierApplication.ProductionTime); }
                catch (Exception) { }
                //need to store the decorating type - probably a switch all properties are in this class
                foreach (var decoratingAndImprinting in supplierApplication.DecoratingTypes)
                {
                    switch (decoratingAndImprinting.Description)
                    {
                        case LegacySupplierDecoratingType.DECORATION_ETCHING:
                            additionalInformation.Etching = "Y";
                            break;
                        case LegacySupplierDecoratingType.DECORATION_HOTSTAMPING:
                            additionalInformation.HotStamping = "Y";
                            break;
                        case LegacySupplierDecoratingType.DECORATION_SILKSCREEN:
                            additionalInformation.SilkScreen = "Y";
                            break;
                        case LegacySupplierDecoratingType.DECORATION_PADPRINT:
                            additionalInformation.PadPrinting = "Y";
                            break;
                        case LegacySupplierDecoratingType.DECORATION_DIRECTEMBROIDERY:
                            additionalInformation.DirectEmbroidery = "Y";
                            break;
                        case LegacySupplierDecoratingType.DECORATION_FOILSTAMPING:
                            additionalInformation.FoilStamping = "Y";
                            break;
                        case LegacySupplierDecoratingType.DECORATION_LITHOGRAPHY:
                            additionalInformation.Lithography = "Y";
                            break;
                        case LegacySupplierDecoratingType.DECORATION_SUBLIMINATION:
                            additionalInformation.Sublimation = "Y";
                            break;
                        case LegacySupplierDecoratingType.DECORATION_FOURCOLOR:
                            additionalInformation.FourColorProcess = "Y";
                            break;
                        case LegacySupplierDecoratingType.DECORATION_ENGRAVING:
                            additionalInformation.Engraving = "Y";
                            break;
                        case LegacySupplierDecoratingType.DECORATION_LASER:
                            additionalInformation.Laser = "Y";
                            break;
                        case LegacySupplierDecoratingType.DECORATION_OFFSET:
                            additionalInformation.Offset = "Y";
                            break;
                        case LegacySupplierDecoratingType.DECORATION_TRANSFER:
                            additionalInformation.Transfer = "Y";
                            break;
                        case LegacySupplierDecoratingType.DECORATION_FULLCOLOR:
                            additionalInformation.FullColorProcess = "Y";
                            break;
                        case LegacySupplierDecoratingType.DECORATION_DIESTAMP:
                            additionalInformation.DieStamp = "Y";
                            break;
                    }
                    //TODO imprint other value, nowhere to add the value stored in supplierApplication.OtherDec
                    if (!string.IsNullOrEmpty(supplierApplication.OtherDec)) additionalInformation.ImprintOther = "Y";
                }
                _objectService.Add<TIMSSAdditionalInfo>(additionalInformation);
            }
            else if (application is StoreDetailDistributorMembership)
            {
                company.CustomerClass = "Distributor";
                //Needs to be there for FK to resolves themselves
                _objectService.SaveChanges();
                StoreDetailDistributorMembership distributorApplication = application as StoreDetailDistributorMembership;
                //need to commit the data so far or the database will fail FK for Additional Information
                _objectService.SaveChanges();
                //create the Additional Information records
                TIMSSAdditionalInfo additionalInformation = new TIMSSAdditionalInfo()
                {
                    DAPP_UserId = company.DAPP_UserId,
                    NumberOfEmployees = distributorApplication.NumberOfEmployee.HasValue ? distributorApplication.NumberOfEmployee.Value.ToString() : null,
                    NumberOfSalesPeople = distributorApplication.NumberOfSalesEmployee,
                    YearEstablished = distributorApplication.EstablishedDate.HasValue ? distributorApplication.EstablishedDate.Value.Year : (int?)null,
                    BusinessRevenue = distributorApplication.PrimaryBusinessRevenue != null ? distributorApplication.PrimaryBusinessRevenue.Name : null,
                    BusinessRevenueOther = distributorApplication.OtherBusinessRevenue,
                };
                //try to convert different data types
                try { additionalInformation.AnnualSalesVol = int.Parse(distributorApplication.AnnualSalesVolume); }
                catch (Exception){}
                try { additionalInformation.AnnualSalesVolumeASP = int.Parse(distributorApplication.AnnualSalesVolumeASP); }
                catch (Exception){}

                _objectService.Add<TIMSSAdditionalInfo>(additionalInformation);
                //add the TIMSSAccountType description + subcode
                foreach (var accountType in distributorApplication.AccountTypes)
                {
                    TIMSSAccountType timssAccountType = new TIMSSAccountType()
                    {
                        DAPP_UserId = company.DAPP_UserId,
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
                        DAPP_UserId = company.DAPP_UserId,
                        SubCode = productLine.SubCode,
                        Description = productLine.Description,
                    };
                    _objectService.Add<TIMSSProductType>(timssProductLine);
                }
            }
            else if (isMagazineRequest)
            {
                //Todo: Code for Magazines
            }
            _objectService.SaveChanges();
            return company.DAPP_UserId;
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
