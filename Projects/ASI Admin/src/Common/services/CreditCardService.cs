using asi.asicentral.interfaces;
using asi.asicentral.model;
using asi.asicentral.model.store;
using asi.asicentral.services.PersonifyProxy;
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
            bool valid = false;
            ILogService log = null;
            try
            {
                log = LogService.GetLog(this.GetType());
                valid = PersonifyClient.ValidateCreditCard(creditCard);
            }
            catch (Exception ex)
            {
                log.Debug(string.Format("Error in accessing personify service for validation: {0}.", ex.Message));
                valid = false;
            }
            return valid;
        }

        public virtual string Store(CreditCard creditCard)
        {
            string result = null;
            ILogService log = null;
            try
            {
                log = LogService.GetLog(this.GetType());
                result = PersonifyClient.SaveCreditCard(creditCard);
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
