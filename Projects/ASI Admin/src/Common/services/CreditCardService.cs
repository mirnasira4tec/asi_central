using asi.asicentral.interfaces;
using asi.asicentral.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    public class CreditCardService : ICreditCardService
    {
        public CreditCardService()
        {
            //nothing to do at this point
        }

        public virtual string Store(CreditCard creditCard)
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
            };
            asi.asicentral.web.CreditCardService.CreditCardServiceClient cardServiceClient = GetClient();
            Guid identifier = cardServiceClient.Store(webCreditCard);
            if (creditCard.Number.Length >= 4) creditCard.MaskedPAN = "****" + creditCard.Number.Substring(creditCard.Number.Length - 4, 4);
            return identifier.ToString();
        }


        public virtual void Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new Exception("Invalid identifier for a credit card");
            Guid guid;
            Guid.TryParse(id, out guid);
            if (guid == Guid.Empty) throw new Exception("Invalid identifier for a credit card");
            asi.asicentral.web.CreditCardService.CreditCardServiceClient cardServiceClient = GetClient();
            cardServiceClient.Delete(guid);
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
