using System.Configuration;
using asi.asicentral.interfaces;
using asi.asicentral.model;
using asi.asicentral.model.store;
using System;
using asi.asicentral.util.store;

namespace asi.asicentral.services
{

    public class CreditCardService : ICreditCardService
    {
	    private readonly IBackendService backendService;

        public CreditCardService(IBackendService backendService)
        {
	        this.backendService = backendService;
        }

        public virtual bool Validate(CreditCard creditCard)
        {
            bool valid = false;
			ILogService log = LogService.GetLog(this.GetType());
	        try
	        {
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

		public virtual string Store(StoreOrder order, CreditCard creditCard, bool backendIntegration)
        {
            if ( creditCard != null && !string.IsNullOrEmpty(creditCard.ExternalReference))
                return creditCard.ExternalReference;
            
            string result;
            ILogService log = null;
			//check for ASI #
            var companyExist = order.Company != null && (!string.IsNullOrEmpty(order.Company.ASINumber) || order.Company.HasExternalReference() );
            
            //save the credit card in personify if we have an asi number but not if we have an external reference (already comes from personify)
            if ( backendIntegration || companyExist )
	        {
				//put the credit card in back office directly
		        try
		        {
			        log = LogService.GetLog(this.GetType());
			        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["svcUri"]))
			        {
				        result = backendService.SaveCreditCard(order, creditCard);
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
						log.Debug(string.Format("Error in saving credit card to personify: {0}.", ex.Message));
                    throw ex;
		        }
	        }
	        else
	        {
				//store the credit card through the service, it will be transferred to back office when approving the order
				var webCreditCard = new web.CreditCardService.CreditCard()
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
                if (!string.IsNullOrEmpty(creditCard.ExternalReference))
                    webCreditCard.Token = Int32.Parse(creditCard.ExternalReference);
				web.CreditCardService.CreditCardServiceClient cardServiceClient = GetClient();
		        var identifier = cardServiceClient.StoreCreditCard(order.GetASICompany(), webCreditCard);
				if (creditCard.Number.Length >= 4) creditCard.MaskedPAN = "****" + creditCard.Number.Substring(creditCard.Number.Length - 4, 4);
		        result = identifier.ToString();
	        }
	        return result;
        }

		private web.CreditCardService.CreditCardServiceClient GetClient()
		{
			var cardServiceClient = new web.CreditCardService.CreditCardServiceClient();
			cardServiceClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["CreditCardServiceUsername"];
			cardServiceClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["CreditCardServicePassword"];
			return cardServiceClient;
		}
    }
}
