using asi.asicentral.interfaces;
using asi.asicentral.model;
using asi.asicentral.model.store;
using asi.asicentral.PersonifyDataASI;
using System;
using System.Collections.Generic;

namespace asi.asicentral.services
{

    interface IBackendService
    {

        CustomerInfo AddCompanyInfo(StoreOrder storeOrder, IList<LookSendMyAdCountryCode> countryCodes);

        IEnumerable<CustomerInfo> AddIndividualInfos(StoreOrder storeOrder, IList<LookSendMyAdCountryCode> countryCodes);

        CustomerInfo GetCompanyInfo(string companyName);

        IEnumerable<ASICustomerCreditCard> GetCreditCardInfos(StoreOrder storeOrder);

        CustomerInfo GetIndividualInfo(string firstName, string lastName);

        bool SaveCreditCard(CreditCard info, StoreOrder storeOrder);

        bool ValidateCreditCard(CreditCard info);

        bool PlaceOrder(StoreOrder storeOrder);
    }
}
