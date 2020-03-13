﻿using asi.asicentral.model;
using asi.asicentral.model.personify;
using asi.asicentral.model.store;
using asi.asicentral.PersonifyDataASI;
using System.Collections.Generic;
using System.Xml.Linq;
using asi.asicentral.services.PersonifyProxy;

namespace asi.asicentral.interfaces
{
    public enum Activity
    {
        Order = 0,
        Exception = 1
    }

    public interface IBackendService
    {
        CompanyInformation PlaceOrder(StoreOrder storeOrder, IEmailService emailService, string url);

        /// <summary>
        /// Used to identify where the order detail is processed through backend
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        bool IsProcessUsingBackend(StoreOrderDetail orderDetail);

        CompanyInformation UpdateCompanyStatus(StoreCompany storeCompany, asi.asicentral.oauth.StatusCode status);

		string SaveCreditCard(StoreOrder order, CreditCard creditCard);

        void SaveCreditCardInfo(StoreOrder order);

        /// <summary>
        /// Used to get a list of credit cards for an existing company
        /// </summary>
        /// <param name="company">The store company record to get the list for</param>
        /// <param name="asiCompany">The company code the card is for (ASI, ASI Show, ASI Canada</param>
        /// <returns></returns>
        IEnumerable<StoreCreditCard> GetCompanyCreditCards(StoreCompany company, string asiCompany);

        CompanyInformation GetCompanyInfoByAsiNumber(string asiNumber);

        CompanyInformation GetCompanyInfoByASICompAccountId(string accountId);

		CompanyInformation GetCompanyInfoByIdentifier(int companyIdentifier);

        CompanyInformation GetIndividualByEmail(string email, bool validate);
        List<CompanyInformation> AddIndividualInfos(StoreCompany storeCompany, string companyMasterId, int companySubId);

        CompanyInformation AddCompany(User curUser);

        CompanyInformation CreateCompany(StoreCompany storeCompany, string storeType);

        CompanyInformation FindCompanyInfo(StoreCompany company, ref List<string> matchList);

        string GetCompanyStatus(string masterCustomerId, int subCustomerId);
        string GetCompanyAsiNumber(string masterCustomerId, int subCustomerId);

        void AddActivity(StoreCompany company, string activityText, Activity activityType);
        bool ValidateRateCode(string groupName, string rateStructure, string rateCode, ref int persProductId, bool isCanada);

        CompanyInformation AddEEXSubscription(User user, bool isBusinessAddress);
        PersonifyStatus OptOutEmailSubscription(string email, List<string> usageCodes);

        StoreDetailApplication GetDemographicData(StoreOrderDetail orderDetail);

        List<XElement> GetASICOMPData(string masterId);
        void UpdateASICompData(List<string> parameters);

        bool UpdateEMSSSO(string masterId, int subCustomerId, string ssoId);
        List<CompanyInformation> GetASIRep(string asiNumber, string relationshipCode);
    }
}
