using asi.asicentral.model;
using asi.asicentral.model.store;
using asi.asicentral.PersonifyDataASI;
using System.Collections.Generic;

namespace asi.asicentral.interfaces
{
    public interface IBackendService
    {
        void PlaceOrder(StoreOrder storeOrder, IEmailService emailService, string url);

        /// <summary>
        /// Used to identify where the order detail is processed through backend
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        bool IsProcessUsingBackend(StoreOrderDetail orderDetail);

	    bool ValidateCreditCard(CreditCard creditCard);

		string SaveCreditCard(StoreOrder order, CreditCard creditCard);

		/// <summary>
		/// Used to get a list of credit cards for an existing company
		/// </summary>
		/// <param name="company">The store company record to get the list for</param>
		/// <param name="asiCompany">The company code the card is for (ASI, ASI Show, ASI Canada</param>
		/// <returns></returns>
	    IEnumerable<StoreCreditCard> GetCompanyCreditCards(StoreCompany company, string asiCompany);

        CompanyInformation GetCompanyInfoByAsiNumber(string asiNumber);

		CompanyInformation GetCompanyInfoByIdentifier(int companyIdentifier);

        CompanyInformation AddCompany(CompanyInformation companyInfo);

        CompanyInformation CreateCompany(StoreCompany storeCompany, string storeType);

        CompanyInformation FindCompanyInfo(StoreCompany company, ref List<string> matchList, ref bool dnsFlg);

        void AddActivity(StoreCompany company, string activityText, string activityCode);
    }
}
