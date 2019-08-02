using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.services;
using asi.asicentral.web.model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            IList<string> messages = new List<string>();
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
            
            //Accessing the ASIInternet database
            try
            {
                LegacyOrder order = ObjectService.GetAll<LegacyOrder>().FirstOrDefault();
                if (order != null) messages.Add("Successfully accessed the Order database");
                else messages.Add("Error accessing the Order database");
            }
            catch (Exception exception)
            {
                messages.Add("Error could not access order database: " + exception.Message);
            }
            //Accessing the Media Server
            try
            {
                MediaFolderModel model = new MediaFolderModel();
                model.BasePath = ConfigurationManager.AppSettings["MediaPath"];
                if (string.IsNullOrEmpty(model.BasePath)) throw new Exception("The media properties need to be setup");
                if (!Directory.Exists(model.BasePath)) throw new Exception("The media properties seem to be incorrect, could not find '" + model.BasePath + "'");
            }
            catch (Exception exception)
            {
                messages.Add("Error could not access the media server: " + exception.Message);
            }
            return View("Diagnostic", messages);
        }
    }
}
