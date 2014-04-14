using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.PersonifyDataASI;
using asi.asicentral.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.services.PersonifyProxy;

namespace asi.asicentral.services
{

    public class BackendService : asi.asicentral.services.IBackendService
    {

        private PersonifyClient m_personifyClient = new PersonifyClient();

     
        public bool SaveCreditCard(CreditCard info, StoreOrder storeOrder)
        {
            return m_personifyClient.SaveCreditCard(info, storeOrder);
        }

        public bool PlaceOrder(StoreOrder storeOrder, IList<LookSendMyAdCountryCode> countryCodes)
        {
            bool result = false;
            try
            {
                StoreCompany storeCompany = storeOrder.Company;
                CustomerInfo companyInfo = m_personifyClient.AddCompanyInfo(storeOrder, countryCodes);
                IEnumerable<CustomerInfo> CustomerInfos = m_personifyClient.AddIndividualInfos(storeOrder, countryCodes);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}