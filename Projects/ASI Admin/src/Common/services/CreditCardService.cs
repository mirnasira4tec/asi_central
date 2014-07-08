using System.Configuration;
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
		        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["svcUri"]))
		        {
			        valid = backendService.ValidateCreditCard(creditCard);
		        }
		        else
		        {
					log.Debug("By-passing personify, not validating the credit card");
			        valid = true;
		        }
	        }
            catch (Exception ex)
            {
                log.Debug(string.Format("Error in accessing personify service for validation: {0}.", ex.Message));
                valid = false;
            }
            return valid;
        }

        public virtual string Store(StoreCompany company, CreditCard creditCard, bool backendIntegration)
        {
            string result;
            ILogService log = null;
	        if (backendIntegration)
	        {
		        try
		        {
			        log = LogService.GetLog(this.GetType());
			        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["svcUri"]))
			        {
				        result = backendService.SaveCreditCard(company, creditCard);
			        }
			        else
			        {
				        log.Debug("By-passing personify, not validating the credit card");
				        result = "Backend Not used";
			        }
			        if (creditCard.Number.Length >= 4)
				        creditCard.MaskedPAN = "****" + creditCard.Number.Substring(creditCard.Number.Length - 4, 4);
		        }
		        catch (Exception ex)
		        {
			        if (log != null)
			        {
                    log.Debug(string.Format("Error in saving credit card to personify: {0}.", ex.Message));
			        }
			        result = null;
                    throw;
		        }
	        }
	        else
	        {
				asi.asicentral.web.CreditCardService.CreditCard webCreditCard = new web.CreditCardService.CreditCard()
				{
					CardHolderName = creditCard.CardHolderName,
					Type = creditCard.Type,
					Number = creditCard.Number,
					ExpirationDate = creditCard.ExpirationDate,
					Address = creditCard.Address,
					City = creditCard.City,
					Country = creditCard.Country,
					State = creditCard.State,
					PostalCode = creditCard.PostalCode,
					CountryCode = creditCard.CountryCode,
				};
				asi.asicentral.web.CreditCardService.CreditCardServiceClient cardServiceClient = GetClient();
				Guid identifier = cardServiceClient.Store(webCreditCard);
				if (creditCard.Number.Length >= 4) creditCard.MaskedPAN = "****" + creditCard.Number.Substring(creditCard.Number.Length - 4, 4);
		        result = identifier.ToString();
	        }
	        return result;
        }

		private asi.asicentral.web.CreditCardService.CreditCardServiceClient GetClient()
		{
			asi.asicentral.web.CreditCardService.CreditCardServiceClient cardServiceClient = new web.CreditCardService.CreditCardServiceClient();
			EncryptionService encryptionService = new EncryptionService();
			cardServiceClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["CreditCardServiceUsername"];
			cardServiceClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["CreditCardServicePassword"];
			return cardServiceClient;
		}
    }
}
