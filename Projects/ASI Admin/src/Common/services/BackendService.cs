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

        public bool ValidateCreditCard(CreditCard info)
        {
            return m_personifyClient.ValidateCreditCard(info);
        }

        public bool SaveCreditCard(CreditCard info, StoreOrder storeOrder)
        {
            return m_personifyClient.SaveCreditCard(info, storeOrder);
        }

        public IEnumerable<ASICustomerCreditCard> GetCreditCardInfos(StoreOrder storeOrder)
        {
            return m_personifyClient.GetCreditCardInfos(storeOrder);
        }

        public CustomerInfo AddCompanyInfo(StoreOrder storeOrder, IList<LookSendMyAdCountryCode> countryCodes)
        {
            return m_personifyClient.AddCompanyInfo(storeOrder, countryCodes);
        }

        public CustomerInfo GetCompanyInfo(string companyName)
        {
            return m_personifyClient.GetCompanyInfo(companyName);
        }

        public IEnumerable<CustomerInfo> AddIndividualInfos(StoreOrder storeOrder, IList<LookSendMyAdCountryCode> countryCodes)
        {
            return m_personifyClient.AddIndividualInfos(storeOrder, countryCodes);
        }

        public CustomerInfo GetIndividualInfo(string firstName, string lastName)
        {
            return m_personifyClient.GetIndividualInfo(firstName, lastName);
        }

        public bool PlaceOrder(StoreOrder storeOrder)
        {
            return true;
        }
    }
}