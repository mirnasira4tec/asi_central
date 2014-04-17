using asi.asicentral.model.store;
using asi.asicentral.PersonifyDataASI;
using asi.asicentral.model;
using System;
using System.Collections.Generic;
using asi.asicentral.services.PersonifyProxy;

namespace asi.asicentral.services
{

    public class PersonifyService : asi.asicentral.services.IBackendService, IDisposable
    {

        private PersonifyClient m_personifyClient = new PersonifyClient();

        private LogService m_log = null;

        private bool m_disposed = false;

        public PersonifyService()
        {
            m_log = LogService.GetLog(this.GetType());
        }

        public bool PlaceOrder(StoreOrder storeOrder, IList<LookSendMyAdCountryCode> countryCodes)
        {
            bool result = false;
            try
            {
                StoreCompany storeCompany = storeOrder.Company;
                CustomerInfo companyInfo = m_personifyClient.AddCompanyInfo(storeOrder, countryCodes);
                m_personifyClient.AddIndividualInfos(storeOrder, countryCodes);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                m_log.Error(string.Format("Error in adding order to personify: {0}", ex.Message));
            }
            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    m_log.Dispose();
                }
            }
            m_disposed = true;
        }

        ~PersonifyService()
        {
            Dispose(false);
        }
    }
}