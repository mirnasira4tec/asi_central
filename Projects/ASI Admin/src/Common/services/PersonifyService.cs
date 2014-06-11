using System.Linq;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.PersonifyDataASI;
using asi.asicentral.model;
using System;
using System.Collections.Generic;
using asi.asicentral.services.PersonifyProxy;
using asi.asicentral.model.timss;

namespace asi.asicentral.services
{
    public class PersonifyService : IBackendService, IDisposable
    {
        private ILogService log = null;
        private readonly IStoreService storeService;
        private bool disposed = false;

        public PersonifyService()
        {
            log = LogService.GetLog(this.GetType());
        }

        public PersonifyService(IStoreService storeService)
        {
            log = LogService.GetLog(this.GetType());
            this.storeService = storeService;
        }

        public virtual void PlaceOrder(StoreOrder order)
        {
            log.Debug(string.Format("Place order Start : {0}", order));
            IList<LookSendMyAdCountryCode> countryCodes = storeService.GetAll<LookSendMyAdCountryCode>(true).ToList();
            if (order == null || order.Company == null || countryCodes == null)
                throw new ArgumentException("You must pass a valid order and the country codes");
            try
            {
                var companyInfo = PersonifyClient.ReconcileCompany(order.Company, "UNKNOWN", countryCodes);
                log.Debug(string.Format("Reconciled company '{1}' to order '{0}'.", order, companyInfo.MasterCustomerId + ";" + companyInfo.SubCustomerId));
                
                IEnumerable<StoreAddressInfo> addresses = PersonifyClient.AddCustomerAddresses(order.Company, companyInfo, countryCodes);
                log.Debug(string.Format("Added addresses to '{1}' to order '{0}'.", order, companyInfo.MasterCustomerId + ";" + companyInfo.SubCustomerId));

                
                IList<CustomerInfo> individualInfos = PersonifyClient.AddIndividualInfos(order, countryCodes, companyInfo).ToList();
                log.Debug(string.Format("Added individuals to company '{1}' to order '{0}'.", order, companyInfo.MasterCustomerId + ";" + companyInfo.SubCustomerId));

                IList<StoreAddressInfo> addresses2 = PersonifyClient.AddIndividualAddresses(order.Company, individualInfos, countryCodes).ToList();
                log.Debug(string.Format("Address added to individuals to the order '{0}'.", order));
                
                StoreIndividual primaryContact = order.GetContact();
                CustomerInfo primaryContactInfo = individualInfos.FirstOrDefault(c =>
                    string.Equals(c.FirstName, primaryContact.FirstName, StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(c.LastName, primaryContact.LastName, StringComparison.InvariantCultureIgnoreCase));

                var shipToAddr = GetAddressInfo(addresses2, AddressType.Shipping, order);
                var billToAddr = GetAddressInfo(addresses2, AddressType.Billing, order);
                var lineItems = GetPersonifyLineInputs(order, shipToAddr.PersonifyAddr.CustomerAddressId);
                log.Debug(string.Format("Retrieved the line items to the order '{0}'.", order.ToString()));
                var orderOutput = PersonifyClient.CreateOrder(
                    order, 
                    companyInfo, 
                    primaryContactInfo, 
                    billToAddr.PersonifyAddr.CustomerAddressId, 
                    shipToAddr.PersonifyAddr.CustomerAddressId, 
                    lineItems);
                log.Debug(string.Format("The order '{0}' has been created in Personify.", order));


                order.ExternalReference = orderOutput.OrderNumber;
                decimal orderTotal = PersonifyClient.GetOrderTotal(orderOutput.OrderNumber);
                log.Debug(string.Format("Got the order total for the order '{0}'.", order));
                try
                {
                    PersonifyClient.PayOrderWithCreditCard(orderOutput.OrderNumber, orderTotal, order.CreditCard.ExternalReference, billToAddr.PersonifyAddr, companyInfo);
                    log.Debug(string.Format("Payed the order '{0}'.", order));
                    log.Debug(string.Format("Place order End: {0}", order));
                }
                catch (Exception e)
                {
                    log.Error(string.Format("Failed to pay the order '{0}'. Error is {2}{1}", order, e.StackTrace, e.Message));
                    //@todo send email order failed to be charged
                    log.Debug(string.Format("Place order End: {0}", order));
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Unknown Error while adding order to personify: {0}{1}", ex.Message, ex.StackTrace));
                log.Debug(string.Format("Place order End: {0}", order));
                throw ex;
            }
        }

        private StoreAddressInfo GetAddressInfo(IList<StoreAddressInfo> addresses, AddressType type, StoreOrder order)
        {
            var addr = addresses.FirstOrDefault(a =>
            {
                if (type == AddressType.Shipping) return a.StoreIsShipping && !a.StoreIsPrimary;
                if (type == AddressType.Billing) return a.StoreIsBilling && !a.StoreIsPrimary;
                return false;
            });
            if (addr == null)
            {
                addr = addresses.FirstOrDefault(a =>
                {
                    if (type == AddressType.Shipping) return a.StoreIsShipping;
                    if (type == AddressType.Billing) return a.StoreIsBilling;
                    return false;
                });
            };
       
            if (addr == null || addr.PersonifyAddr == null)
            {
                string s = string.Format("Shipping and billing personify customer addresses are required for order {0}.", order.ToString());
                throw new Exception(s);
                log.Debug(s);
            }
            return addr;
        }

        public virtual bool IsProcessUsingBackend(StoreOrderDetail orderDetail)
        {
            log.Debug(string.Format("Check if {0} is processed using backend.", orderDetail.ToString()));
            bool processUsingBackend = false;
            if (orderDetail != null && orderDetail.Product != null)
            {
                processUsingBackend = orderDetail.Product.HasBackEndIntegration;
                if (processUsingBackend && orderDetail.Product.Id == 61)
                {
                    //not all email express products are to be integrated
                    StoreDetailEmailExpress emailexpressdetails = storeService.GetAll<StoreDetailEmailExpress>(true).SingleOrDefault(details => details.OrderDetailId == orderDetail.Id);
                    processUsingBackend = (emailexpressdetails != null && (emailexpressdetails.ItemTypeId == 1 || emailexpressdetails.ItemTypeId == 2));
                }
            }
            log.Debug(string.Format("Processing {0} is using the backend: {1}", orderDetail.ToString(), processUsingBackend));
            return processUsingBackend;
        }

        public virtual bool ValidateCreditCard(CreditCard creditCard)
        {

            log.Debug(string.Format("Validate credit card {0} ({1}).", creditCard.MaskedPAN, creditCard.Type));
            var result = PersonifyClient.ValidateCreditCard(creditCard);
            log.Debug(string.Format("Credit card {0} ({1}) is {2}.", creditCard.MaskedPAN, creditCard.Type, result ? "valid" : "invalid"));
            return result;
        }

        public virtual string SaveCreditCard(StoreCompany company, CreditCard creditCard)
        {
            log.Debug(string.Format("Save credit of {0} ({1})", creditCard.MaskedPAN, company.Name));
            //assuming credit card is valid already
            if (company == null || creditCard == null) throw new ArgumentException("Invalid parameters");
            IList<LookSendMyAdCountryCode> countryCodes = storeService.GetAll<LookSendMyAdCountryCode>(true).ToList();
            //create company if not already there
            var companyInfo = PersonifyClient.ReconcileCompany(company, "UNKNOWN", countryCodes);
            PersonifyClient.AddCustomerAddresses(company, companyInfo, countryCodes);
            //Add credit card to the company
            string profile = PersonifyClient.GetCreditCardProfileId(companyInfo, creditCard);
            if (profile == string.Empty) profile = PersonifyClient.SaveCreditCard(companyInfo, creditCard);
            log.Debug(string.IsNullOrWhiteSpace(profile)?
                "Fail to save the credit.":string.Format("Saved credit profile id : {0}",profile));
            return profile;
        }

        private IList<CreateOrderLineInput> GetPersonifyLineInputs(StoreOrder order, long shipAddressId)
        {
            log.Debug(string.Format("Create personify order line input for order {0} with shipping address id {1}", order.ToString(), shipAddressId));
            var lineItems = new List<CreateOrderLineInput>();
            foreach (var orderDetail in order.OrderDetails)
            {
                switch (orderDetail.Product.Id)
                {
                    case 77: //supplier specials
                        var startDate = (orderDetail.DateOption.HasValue ? orderDetail.DateOption.Value : DateTime.Now).ToString("MM/dd/yyyy");
						var endDate = (orderDetail.DateOption.HasValue ? orderDetail.DateOption.Value : DateTime.Now).AddMonths(1).ToString("MM/dd/yyyy");
                        var option = orderDetail.OptionId.ToString();
                        var mapping = storeService.GetAll<PersonifyMapping>(true).Single(map => Equals(map.StoreContext, orderDetail.Order.ContextId) &&
                            map.StoreProduct == orderDetail.Product.Id &&
                            map.StoreOption == option);
                        mapping.Quantity = orderDetail.Quantity;
                        var lineItem = new CreateOrderLineInput
                        {
                            ProductId = mapping.PersonifyProduct,
                            RateCode = mapping.PersonifyRateCode,
                            RateStructure = mapping.PersonifyRateStructure,
                            ShipAddressID = Convert.ToInt32(shipAddressId),
                            Quantity = Convert.ToInt16(orderDetail.Quantity),
                            BeginDate = startDate,
							EndDate = endDate,
                        };
                        lineItems.Add(lineItem);
                        break;
                    case 61: //email express
                        var emailexpressdetails = storeService.GetAll<StoreDetailEmailExpress>(true).Single(details => details.OrderDetailId == orderDetail.Id);
                        option = emailexpressdetails.ItemTypeId.ToString();
                        if (option == "1" || option == "2")
                        {
                            option += ";";
                            if (orderDetail.Quantity >= 120) option += "120X";
                            else if (orderDetail.Quantity >= 52) option += "52X";
                            else if (orderDetail.Quantity >= 26) option += "26X";
                            else if (orderDetail.Quantity >= 12) option += "12X";
                            else if (orderDetail.Quantity >= 6) option += "6X";
                            else if (orderDetail.Quantity >= 3) option += "3X";
                            else option += "1X";
                        }
                        mapping = storeService.GetAll<PersonifyMapping>(true).Single(map => Equals(map.StoreContext, orderDetail.Order.ContextId) &&
                            map.StoreProduct == orderDetail.Product.Id &&
                            map.StoreOption == option);
                        //need to create a new line item for each one rather than one for all quantity
                        for (int i = 0; i < orderDetail.Quantity; i++)
                        {
                            lineItem = new CreateOrderLineInput
                            {
                                ProductId = mapping.PersonifyProduct,
                                RateCode = mapping.PersonifyRateCode,
                                RateStructure = mapping.PersonifyRateStructure,
                                ShipAddressID = Convert.ToInt32(shipAddressId),
                                Quantity = 1,
                            };
                            lineItems.Add(lineItem);
                        }
                        mapping.ItemCount = orderDetail.Quantity;
                        mapping.Quantity = 1;
                        break;
                    default:
                        mapping = storeService.GetAll<PersonifyMapping>(true).Single(map => Equals(map.StoreContext, orderDetail.Order.ContextId) &&
                            map.StoreProduct == orderDetail.Product.Id);

                        lineItem = new CreateOrderLineInput
                        {
                            ProductId = mapping.PersonifyProduct,
                            RateCode = mapping.PersonifyRateCode,
                            RateStructure = mapping.PersonifyRateStructure,
                            ShipAddressID = Convert.ToInt32(shipAddressId),
                            Quantity = Convert.ToInt16(orderDetail.Quantity),
                        };
                        lineItems.Add(lineItem);
                        break;
                }
            }
            return lineItems;
        }

        public virtual CompanyInformation AddCompany(CompanyInformation companyInformation, int memberType)
        {
            //create equivalent store objects
            StoreCompany company = new StoreCompany
            {
                Name = companyInformation.Name,
            };
            StoreAddress address = new StoreAddress
            {
                Street1 = companyInformation.Street1,
                Street2 = companyInformation.Street2,
                City = companyInformation.City,
                State = companyInformation.State,
                Country = companyInformation.Country,
                Zip = companyInformation.Zip,
            };
            company.Addresses.Add(new StoreCompanyAddress
            {
                Address = address,
                IsBilling = true,
                IsShipping = true,
            });
            //@todo convert membertype
            //create company if not already there
            var companyInfo = PersonifyClient.ReconcileCompany(company, "UNKNOWN", null);
            PersonifyClient.AddCustomerAddresses(company, companyInfo, null);
            return companyInformation;
        }

	    public virtual CompanyInformation GetCompanyInfoByAsiNumber(string asiNumber)
	    {
            var companyInfo = PersonifyClient.GetCompanyInfoByASINumber(asiNumber);
            return companyInfo;
        }

        public virtual CompanyInformation GetCompanyInfoByIdentifier(int companyIdentifier)
        {
            var companyInfo = PersonifyClient.GetCompanyInfoByIdentifier(companyIdentifier);
            return companyInfo;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
            }
            disposed = true;
        }

        ~PersonifyService()
        {
            Dispose(false);
        }
    }
}