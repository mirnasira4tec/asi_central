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
        private LogService log = null;
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
            IList<LookSendMyAdCountryCode> countryCodes = storeService.GetAll<LookSendMyAdCountryCode>(true).ToList();
            if (order == null || order.Company == null || countryCodes == null)
                throw new ArgumentException("You must pass a valid order and the country codes");
            try
            {
                var companyInfo = PersonifyClient.ReconcileCompany(order.Company, countryCodes);
                IDictionary<AddressType, AddressInfo> addresses = PersonifyClient.AddCompanyAddresses(order.Company, companyInfo, countryCodes);
                StoreIndividual primaryContact = order.GetContact();
                IEnumerable<CustomerInfo> individualInfos = PersonifyClient.AddIndividualInfos(order, countryCodes, companyInfo);
                PersonifyClient.AddIndividualAddresses(individualInfos, addresses);
                CustomerInfo primaryContactInfo = individualInfos.FirstOrDefault(c =>
                    string.Equals(c.FirstName, primaryContact.FirstName, StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(c.LastName, primaryContact.LastName, StringComparison.InvariantCultureIgnoreCase));
                var lineItems = GetPersonifyLineInputs(order, addresses[AddressType.Shipping].CustomerAddressId);
                var orderOutput = PersonifyClient.CreateOrder(order, companyInfo, primaryContactInfo, addresses[AddressType.Billing].CustomerAddressId, addresses[AddressType.Shipping].CustomerAddressId, lineItems);
                order.ExternalReference = orderOutput.OrderNumber;
                decimal orderTotal = PersonifyClient.GetOrderTotal(orderOutput.OrderNumber);
                PersonifyClient.PayOrderWithCreditCard(orderOutput.OrderNumber, orderTotal, order.CreditCard.ExternalReference, addresses[AddressType.Billing], companyInfo);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error in adding order to personify: {0}", ex.Message));
                throw ex;
            }
        }

        public virtual bool IsProcessUsingBackend(StoreOrderDetail orderDetail)
        {
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
            return processUsingBackend;
        }

        public virtual bool ValidateCreditCard(CreditCard creditCard)
        {
            return PersonifyClient.ValidateCreditCard(creditCard);
        }

        public virtual string SaveCreditCard(StoreCompany company, CreditCard creditCard)
        {
            //assuming credit card is valid already
            if (company == null || creditCard == null) throw new ArgumentException("Invalid parameters");
            IList<LookSendMyAdCountryCode> countryCodes = storeService.GetAll<LookSendMyAdCountryCode>(true).ToList();
            //create company if not already there
            var companyInfo = PersonifyClient.ReconcileCompany(company, countryCodes);
            PersonifyClient.AddCompanyAddresses(company, companyInfo, countryCodes);
            //Add credit card to the company
            string profile = PersonifyClient.GetCreditCardProfileId(companyInfo, creditCard);
            if (profile == string.Empty) profile = PersonifyClient.SaveCreditCard(companyInfo, creditCard);
            return profile;
        }

        private IList<CreateOrderLineInput> GetPersonifyLineInputs(StoreOrder order, long shipAddressId)
        {
            var lineItems = new List<CreateOrderLineInput>();
            foreach (var orderDetail in order.OrderDetails)
            {
                switch (orderDetail.Product.Id)
                {
                    case 77: //supplier specials
                        var startDate = (orderDetail.DateOption.HasValue ? orderDetail.DateOption.Value : DateTime.Now).ToString("MM/dd/yyyy");
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

        public virtual SaveCustomerOutput AddCompanyByNameAndMemberTypeId(string companyName, int memberTypeId)
        {
            var company = PersonifyClient.AddCompanyByNameAndMemberTypeId(companyName, memberTypeId);
            return company;
        }

        public virtual CustomerInfo GetCompanyInfoByAsiNumber(string asiNumber)
        {
            var company = PersonifyClient.GetCompanyInfoByAsiNumber(asiNumber);
            return company;
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
                    log.Dispose();
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