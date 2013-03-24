using asi.asicentral.interfaces;
using asi.asicentral.services;
using asi.asicentral.web.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Application.Controllers
{
    public class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            return View("Index");
        }

        public virtual ActionResult CreditCard()
        {
            CreditCardModel model = new CreditCardModel();
            model.ServiceUrl = "https://pc-2200.asinetwork.local/service/CreditCardService.svc";
            return View("CreditCard", model);
        }

        [HttpPost]
        public virtual ActionResult CreditCard(CreditCardModel creditCardModel)
        {
            IEncryptionService encryptionService = new EncryptionService();
            if (!string.IsNullOrEmpty(creditCardModel.Username) && string.IsNullOrEmpty(creditCardModel.ServiceUrl))
            {
                creditCardModel.Password = encryptionService.Encrypt("Credit Card Service", creditCardModel.Username);
            }
            if(!string.IsNullOrEmpty(creditCardModel.ServiceUrl) && !string.IsNullOrEmpty(creditCardModel.Username))
            {
                Guid recordId = Guid.Empty;
                asi.asicentral.web.CreditCardService.CreditCardServiceClient cardServiceClient = 
                    new asi.asicentral.web.CreditCardService.CreditCardServiceClient("CreditCardService", creditCardModel.ServiceUrl);
                cardServiceClient.ClientCredentials.UserName.UserName = creditCardModel.Username;
                cardServiceClient.ClientCredentials.UserName.Password = encryptionService.Encrypt("Credit Card Service", creditCardModel.Username);
                try
                {
                    cardServiceClient.Ping();
                    creditCardModel.SuccessMessages.Add("Successfully pinged the web service");
                }
                catch (Exception exception)
                {
                    creditCardModel.ErrorMessages.Add("Could not ping the web service: " + exception.Message);
                }
                try
                {
                    asi.asicentral.web.CreditCardService.CreditCard webCreditCard = new asi.asicentral.web.CreditCardService.CreditCard()
                    {
                        CardHolderName = "Admin Tool",
                        Type = "VISA",
                        Number = "4111111111111111",
                        ExpirationDate = DateTime.Now,
                        Address = "Address Field",
                        City = "City Field",
                        Country = "Country Field",
                        State = "State Field",
                        PostalCode = "Postal",
                    };
                    recordId = cardServiceClient.Store(webCreditCard);
                    creditCardModel.SuccessMessages.Add("Successfully stored the credit card");
                }
                catch (Exception exception)
                {
                    creditCardModel.ErrorMessages.Add("Could not store a credit card: " + exception.Message);
                }
                if (recordId != Guid.Empty)
                {
                    try
                    {
                        cardServiceClient.Delete(recordId);
                        creditCardModel.SuccessMessages.Add("Successfully removed a credit card record");
                    }
                    catch (Exception exception)
                    {
                        creditCardModel.ErrorMessages.Add("Could not remove a credit card: " + exception.Message);
                    }
                }
                else
                {
                    creditCardModel.ErrorMessages.Add("Do not have a record id to delete");
                }
            }
            return View("CreditCard", creditCardModel);
        }
    }
}
