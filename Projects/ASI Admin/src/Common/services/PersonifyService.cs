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
                IDictionary<AddressType, long> addresses = PersonifyClient.AddCompanyAddresses(order.Company, companyInfo, countryCodes);
                //@todo AddIndividualInfos needs to return CustomerInfo class for the primary contact
                StoreIndividual primaryContact = order.GetContact();
                IEnumerable<CustomerInfo> individualInfos = PersonifyClient.AddIndividualInfos(order, countryCodes, companyInfo);
                CustomerInfo primaryContactInfo = individualInfos.FirstOrDefault(c =>
                    string.Equals(c.FirstName, primaryContact.FirstName, StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(c.LastName, primaryContact.LastName, StringComparison.InvariantCultureIgnoreCase));
                var lineItems = GetPersonifyLineInputs(order, addresses[AddressType.Shipping]);
                //@todo pass the primary contact information
                var orderOutput = PersonifyClient.CreateOrder(order, companyInfo, primaryContactInfo, addresses[AddressType.Billing], addresses[AddressType.Shipping], lineItems);
                order.ExternalReference = orderOutput.OrderNumber;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error in adding order to personify: {0}", ex.Message));
	            throw ex;
            }
        }

        public bool IsProcessUsingBackend(StoreOrderDetail orderDetail)
        {
            bool processUsingBackend = false;
            if (orderDetail != null && orderDetail.Product != null)
            {
                processUsingBackend = orderDetail.Product.HasBackEndIntegration;
                if (orderDetail.Product.Id == 61)
                {
                    //not all email express products are to be integrated
                    StoreDetailEmailExpress emailexpressdetails = storeService.GetAll<StoreDetailEmailExpress>(true).SingleOrDefault(details => details.OrderDetailId == orderDetail.Id);
                    processUsingBackend = (emailexpressdetails != null && (emailexpressdetails.ItemTypeId == 1 || emailexpressdetails.ItemTypeId == 2));
                }
            }
            return processUsingBackend;
        }

		public bool ValidateCreditCard(CreditCard creditCard)
		{
			return PersonifyClient.ValidateCreditCard(creditCard);
		}

		public string SaveCreditCard(StoreCompany company, CreditCard creditCard)
		{
			//assuming credit card is valid already
			if (company == null || creditCard == null) throw new System.ArgumentException("Invalid parameters");
			IList<LookSendMyAdCountryCode> countryCodes = storeService.GetAll<LookSendMyAdCountryCode>(true).ToList();
			//create company if not already there
			var companyInfo = PersonifyClient.ReconcileCompany(company, countryCodes);
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
                var items = LookupLineItem(orderDetail);
                //one order detail may be more than one item in personify
                //membership has one item for membership and one item for application fee
                foreach (var item in items)
                {
                    //each line item may have to be repeated more than once, for instance
                    //when purchasing more than one email express, each one is a line item
                    for (int i = 0; i < item.ItemCount; i++)
                    {
                        var lineItem = new CreateOrderLineInput
                        {
                            ProductId = item.PersonifyProduct,
                            RateCode = item.PersonifyRateCode,
                            RateStructure = item.PersonifyRateStructure,
                            ShipAddressID = Convert.ToInt32(shipAddressId),
                            Quantity = Convert.ToInt16(orderDetail.Quantity),
                            
                        };
                        lineItems.Add(lineItem);
                    }
                }
		    }
		    return lineItems;
	    }

        private IList<PersonifyMapping> LookupLineItem(StoreOrderDetail orderDetail)
        {
            var mappings = new List<PersonifyMapping>();
            switch (orderDetail.Product.Id)
            {
                case 77: //supplier specials
                    string option = orderDetail.OptionId.ToString();
                    PersonifyMapping mapping = storeService.GetAll<PersonifyMapping>(true).Single(map => Equals(map.StoreContext, orderDetail.Order.ContextId) && 
                        map.StoreProduct == orderDetail.Product.Id &&
                        map.StoreOption == option);
                    mappings.Add(mapping);
                    mapping.Quantity = orderDetail.Quantity;
                    break;
                case 61: //email express
                    StoreDetailEmailExpress emailexpressdetails = storeService.GetAll<StoreDetailEmailExpress>(true).Single(details => details.OrderDetailId == orderDetail.Id);
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
                    mappings.Add(mapping);
                    //need to create a new line item for each one rather than one for all quantity
                    mapping.ItemCount = orderDetail.Quantity;
                    mapping.Quantity = 1;
                    break;
            }
            return mappings;
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