using asi.asicentral.interfaces;
using asi.asicentral.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace asi.asicentral.services
{
    public class CreditCardService : ICreditCardService
    {
        public CreditCardService()
        {
            //nothing to do at this point
        }
        public virtual bool Validate(CreditCard creditCard)
        {
            ILogService log = LogService.GetLog(this.GetType());
            bool valid = true;
            string webAPIUrl = ConfigurationManager.AppSettings["ConnectUrl"];
            string webAPICredentials = ConfigurationManager.AppSettings["ConnectCredentials"];
            if (string.IsNullOrEmpty(webAPIUrl) || string.IsNullOrEmpty(webAPICredentials)) return valid;
            string[] credentials = webAPICredentials.Split(';');
            if (credentials.Length != 2) return valid;
            //get a token
            log.Debug("found entry in config file, calling web api at:" + webAPIUrl);
            //get the data from the web api
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(webAPIUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string loginData = string.Format(@"{{'asi':'33020','username':'{0}','password':'{1}'}}", credentials[0], credentials[1]);
                HttpResponseMessage response = client.PostAsJsonAsync("api/integration/login", loginData).Result;
                if (response.IsSuccessStatusCode)
                {
                    log.Debug("Retrieved a token from login services");
                    string token = Json.Decode(response.Content.ReadAsStringAsync().Result);
                    string parameters = string.Format("cardNo={0}&cardCVVCode=&cardExpMonth={1:00}&cardExpYear={2}", creditCard.Number, creditCard.ExpirationDate.Month, creditCard.ExpirationDate.Year.ToString().Substring(2, 2));
                    //now validate the credit card
                    client.DefaultRequestHeaders.Add("AuthToken", token);
                    response = client.GetAsync("api/Payflow/GetProcessedAuthTransaction?" + parameters).Result;  // Blocking call!
                    dynamic validationResponse = Json.Decode(response.Content.ReadAsStringAsync().Result);
                    string verified = validationResponse.RespMsg;
                    valid = (!string.IsNullOrEmpty(verified) && verified == "Approved");
                }
                else
                {
                    log.Debug("Failed to retrieve a token from login services");
                }
            }
            return valid;
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
