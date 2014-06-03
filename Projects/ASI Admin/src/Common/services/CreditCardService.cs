using asi.asicentral.interfaces;
using asi.asicentral.model;
using asi.asicentral.model.store;
using asi.asicentral.services.PersonifyProxy;
using System;

namespace asi.asicentral.services
{

    public class CreditCardService : ICreditCardService
    {
	    private IBackendService backendService;

        public CreditCardService(IBackendService backendService)
        {
	        this.backendService = backendService;
        }

        public virtual bool Validate(CreditCard creditCard)
        {
            bool valid = false;
            ILogService log = null;
            try
            {
                log = LogService.GetLog(this.GetType());
                valid = backendService.ValidateCreditCard(creditCard);
            }
            catch (Exception ex)
            {
                log.Debug(string.Format("Error in accessing personify service for validation: {0}.", ex.Message));
                valid = false;
            }
            return valid;
        }

        public virtual string Store(StoreCompany company, CreditCard creditCard)
        {
            string result = null;
            ILogService log = null;
            try
            {
                log = LogService.GetLog(this.GetType());
                result = backendService.SaveCreditCard(company, creditCard);
                if (creditCard.Number.Length >= 4) creditCard.MaskedPAN = "****" + creditCard.Number.Substring(creditCard.Number.Length - 4, 4);
            }
            catch (Exception ex)
            {
                if (log != null)
                {
                    log.Debug(string.Format("Error in accessing personify service for validation: {0}.", ex.Message));
                }
                result = null;
            }
            return result;
        }
    }
}
