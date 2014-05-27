using System.Linq;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.PersonifyDataASI;
using asi.asicentral.model;
using System;
using System.Collections.Generic;
using asi.asicentral.services.PersonifyProxy;
using DotLiquid.Exceptions;

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
				throw new System.ArgumentException("You must pass a valid order and the country codes");
            try
            {
				//@todo probably need to get reference for addresses so they cna be assigned to order
		        var companyInfo = PersonifyClient.AddCompanyInfo(order, countryCodes);
				PersonifyClient.AddIndividualInfos(order, countryCodes, companyInfo);
	            var lineItems = GetPersonifyLineInputs(order);
                var orderOutput = PersonifyClient.CreateOrder(order, companyInfo, lineItems);
	            order.ExternalReference = orderOutput.OrderNumber;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error in adding order to personify: {0}", ex.Message));
	            throw ex;
            }
        }

	    private IList<CreateOrderLineInput> GetPersonifyLineInputs(StoreOrder order)
	    {
		    var lineItems = new List<CreateOrderLineInput>();
		    foreach (var orderDetail in order.OrderDetails)
		    {
			    //lookup mapping table
			    var lineItem = new CreateOrderLineInput()
			    {
					ProductId = 1566,
					Quantity = Convert.ToInt16(orderDetail.Quantity),
					RateCode = "1 Time Ad",
					RateStructure = "Member",
			    };
				lineItems.Add(lineItem);
		    }
		    return lineItems;
	    }

        public virtual SaveCustomerOutput AddCompanyByNameAndMemberTypeId(string companyName, int memberTypeId)
        {
            return PersonifyClient.AddCompanyByNameAndMemberTypeId(companyName, memberTypeId);
        }

        public virtual CustomerInfo GetCompanyInfoByAsiNumber(string asiNumber)
        {
            return PersonifyClient.GetCompanyInfoByAsiNumber(asiNumber);
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