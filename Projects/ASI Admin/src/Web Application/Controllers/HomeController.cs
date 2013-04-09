using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.model.timss;
using asi.asicentral.services;
using asi.asicentral.web.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Application.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IObjectService ObjectService { get; set; }

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

        public virtual ActionResult Diagnostic()
        {
            //Check access to the credit card service
            IList<string> messages = new List<string>();
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["CreditCardServicePassword"]) || string.IsNullOrEmpty(ConfigurationManager.AppSettings["CreditCardServiceUsername"]))
            {
                messages.Add("Error the config variables for the web service are not set properly");
            }
            else
            {
                try
                {
                    asi.asicentral.web.CreditCardService.CreditCardServiceClient cardServiceClient = new asi.asicentral.web.CreditCardService.CreditCardServiceClient();
                    cardServiceClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["CreditCardServiceUsername"];
                    cardServiceClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["CreditCardServicePassword"];
                    cardServiceClient.Ping();
                    messages.Add("Successfully pinged the Web Service");
                }
                catch (Exception exception)
                {
                    messages.Add("Error could not connect to the web service: " + exception.Message);
                }
            }
            //Accessing MySQL database hosting the contexts
            try
            {
                Context context = ObjectService.GetAll<Context>().FirstOrDefault();
                if (context != null) messages.Add("Successfully retrieved context records");
                else messages.Add("Error could not retrieve a single context record");
            }
            catch (Exception exception)
            {
                messages.Add("Error could not access context database: " + exception.Message);
            }
            //Accessing the database hosting TIMSS tables
            try
            {
                ObjectService.GetAll<TIMSSCompany>().FirstOrDefault();
                messages.Add("Successfully accessed the TIMSS database");
            }
            catch (Exception exception)
            {
                messages.Add("Error could not access timss database: " + exception.Message);
            }
            //Accessing the ASIInternet database
            try
            {
                Order order = ObjectService.GetAll<Order>().FirstOrDefault();
                if (order != null) messages.Add("Successfully accessed the Order database");
                else messages.Add("Error accessing the Order database");
            }
            catch (Exception exception)
            {
                messages.Add("Error could not access order database: " + exception.Message);
            }
            return View("Diagnostic", messages);
        }

        [HttpPost]
        public virtual ActionResult CreditCard(CreditCardModel creditCardModel)
        {
            IEncryptionService encryptionService = new EncryptionService();
            if (!string.IsNullOrEmpty(creditCardModel.Username) && string.IsNullOrEmpty(creditCardModel.ServiceUrl))
            {
                creditCardModel.Password = encryptionService.Encrypt("Credit Card Service", creditCardModel.Username);
            }
            if (!string.IsNullOrEmpty(creditCardModel.ServiceUrl) && !string.IsNullOrEmpty(creditCardModel.Username))
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
