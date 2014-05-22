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
        private LogService log = null;

        private bool disposed = false;

        public PersonifyService()
        {
            log = LogService.GetLog(this.GetType());
        }

        public bool PlaceOrder(StoreOrder storeOrder, IList<LookSendMyAdCountryCode> countryCodes)
        {
            bool result = false;
            try
            {
                StoreCompany storeCompany = storeOrder.Company;
                CustomerInfo companyInfo = PersonifyClient.AddCompanyInfo(storeOrder, countryCodes);
                PersonifyClient.AddIndividualInfos(storeOrder, countryCodes);
                PersonifyClient.CreateOrder(storeOrder);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                log.Error(string.Format("Error in adding order to personify: {0}", ex.Message));
            }
            return result;
        }

        public SaveCustomerOutput AddCompanyByNameAndMemberTypeId(string companyName, int memberTypeId)
        {
            return PersonifyClient.AddCompanyByNameAndMemberTypeId(companyName, memberTypeId);
        }

        public CustomerInfo GetCompanyInfoByAsiNumber(string asiNumber)
        {
            return PersonifyClient.GetCompanyInfoByAsiNumber(asiNumber);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    log.Dispose();
                }
            }
            disposed = true;
        }

        ~PersonifyService()
        {
            Dispose(false);
        }
    }
}