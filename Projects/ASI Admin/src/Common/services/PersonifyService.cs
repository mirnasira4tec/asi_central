using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.PersonifyDataASI;
using asi.asicentral.web.CreditCardService;
using asi.asicentral.web.Services.PersonifyProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.services
{

    public class PersonifyService
    {

        public static bool ValidateCreditCard(CreditCard info)
        {
            return PersonifyClient.ValidateCreditCard(info);
        }

        public static bool SaveCreditCard(CreditCard info, string orderId, IStoreService storeService)
        {
            return PersonifyClient.SaveCreditCard(info, orderId, storeService);
        }

        public static IEnumerable<ASICustomerCreditCard> GetCreditCardInfos(string orderId, IStoreService storeService)
        {
            return PersonifyClient.GetCreditCardInfos(orderId, storeService);
        }

        public static CustomerInfo AddCompanyInfo(StoreOrderDetail storeOrderDetail, IStoreService storeService)
        {
            return PersonifyClient.AddCompanyInfo(storeOrderDetail, storeService);
        }

        public static CustomerInfo GetCompanyInfo(string companyName)
        {
            return PersonifyClient.GetCompanyInfo(companyName);
        }

        public static IEnumerable<CustomerInfo> AddIndividualInfos(StoreOrder storeOrder, IStoreService storeService)
        {
            return PersonifyClient.AddIndividualInfos(storeOrder, storeService);
        }

        public static CustomerInfo GetIndividualInfo(string firstName, string lastName)
        {
            return PersonifyClient.GetIndividualInfo(firstName, lastName);
        }
    }
}