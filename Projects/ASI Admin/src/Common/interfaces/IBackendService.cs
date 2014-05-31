using asi.asicentral.interfaces;
using asi.asicentral.model;
using asi.asicentral.model.store;
using asi.asicentral.PersonifyDataASI;
using System;
using System.Collections.Generic;

namespace asi.asicentral.interfaces
{
    public interface IBackendService
    {
        void PlaceOrder(StoreOrder storeOrder);

        SaveCustomerOutput AddCompanyByNameAndMemberTypeId(string companyName, int memberTypeId);

        CustomerInfo GetCompanyInfoByAsiNumber(string asiNumber);
    }
}
