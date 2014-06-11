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

        /// <summary>
        /// Used to identify where the order detail is processed through backend
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        bool IsProcessUsingBackend(StoreOrderDetail orderDetail);

	    bool ValidateCreditCard(CreditCard creditCard);

	    string SaveCreditCard(StoreCompany company, CreditCard creditCard);

        CompanyInformation GetCompanyInfoByAsiNumber(string asiNumber);
        CompanyInformation GetCompanyInfoByIdentifier(int companyIdentifier);

        CompanyInformation AddCompany(CompanyInformation companyInfo, int memberType);
    }
}
