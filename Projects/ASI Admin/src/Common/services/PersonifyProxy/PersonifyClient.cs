using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.services;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Web;
using asi.asicentral.util.store.companystore;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using asi.asicentral.web.CreditCardService;
using asi.asicentral.services.PersonifyProxy;
using asi.asicentral.PersonifyDataASI;

namespace asi.asicentral.web.Services.PersonifyProxy
{

    public static class PersonifyClient
    {

        private const string AddressAddedOrModifiedBy = "ASI_Store";

        private const string CommunicationInputPhone = "PHONE";

        private const string CommunicationInputFax = "FAX";

        private const string CommunicationInputWeb = "WEB";

        private const string CommunicationInputEmail = "EMAIL";

        private const string CommunicationLocationCodeCorporate = "CORPORATE";

        private const string CommunicationLocationCodeWork = "WORK";

        private const string CommunicationLocationCodeUnv = "UNV";

        private const string CustomerClassIndiv = "INDIV";

        private const string RecordTypeIndividual = "I";

        private const string RecordTypeCorporate = "C";

        private const int PhoneNumberLength = 10;

        private static IList<LookSendMyAdCountryCode> CountryCodes = null;

        private static readonly Dictionary<string, string> CreditCardType =
            new Dictionary<string, string>(4) { { "AMEX", "AMEX" }, { "DISCOVER", "DISCOVER" }, { "MASTERCARD", "MC" }, { "VISA", "VISA" } };

        public static bool ValidateCreditCard(CreditCard info)
        {
            var aSIValidateCreditCardInput = new ASIValidateCreditCardInput()
            {
                ReceiptType = info.Type,
                CreditCardNumber = info.Number
            };
            ASIValidateCreditCardOutput resp = SvcClient.Post<ASIValidateCreditCardOutput>("ASIValidateCreditCard", aSIValidateCreditCardInput);
            return resp.IsValid ?? false;
        }

        public static bool SaveCreditCard(CreditCard info, string orderId, IStoreService storeService)
        {
            StoreOrder storeOrder = GetOrder(orderId, storeService);
            CustomerInfo companyInfo = PersonifyClient.GetCompanyInfo(storeOrder.Company.Name);
            IList<LookSendMyAdCountryCode> countryCodes = GetCountryCodes(storeService);
            StoreAddress companyAddress = storeOrder.Company.GetCompanyAddress();
            string countryCode = countryCodes.Alpha3Code(companyAddress.Country);
            var customerCreditCardInput = new CustomerCreditCardInput()
            {
                MasterCustomerId = companyInfo.MasterCustomerId,
                SubCustomerId = companyInfo.SubCustomerId,
                ReceiptType = CreditCardType[info.Type],
                CreditCardNumber = info.Number,
                ExpirationMonth = (short)info.ExpirationDate.Month,
                ExpirationYear = (short)info.ExpirationDate.Year,
                NameOnCard = info.CardHolderName,
                BillingAddressStreet = info.Address,
                BillingAddressCity = info.City,
                BillingAddressState = info.State,
                BillingAddressPostalCode = info.PostalCode,
                BillingAddressCountryCode = countryCode,
                DefaultFlag = true,
                CompanyNumber = "1",
                AddedOrModifiedBy = AddressAddedOrModifiedBy
            };
            CustomerCreditCardOutput resp = SvcClient.Post<CustomerCreditCardOutput>("AddCustomerCreditCard", customerCreditCardInput);
            return resp.Success ?? false;
        }

        public static IEnumerable<ASICustomerCreditCard> GetCreditCardInfos(string orderId, IStoreService storeService)
        {
            StoreOrder storeOrder = GetOrder(orderId, storeService);
            CustomerInfo companyInfo = PersonifyClient.GetCompanyInfo(storeOrder.Company.Name);
            IEnumerable<ASICustomerCreditCard> oCreditCards = GetCreditCardInfos(companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
            return oCreditCards;
        }

        public static CustomerInfo AddCompanyInfo(StoreOrderDetail storeOrderDetail, IStoreService storeService)
        {
            CustomerInfo companyInfo = null;
            StoreCompany storeCompany = storeOrderDetail.Order.Company;
            if (storeCompany == null || string.IsNullOrWhiteSpace(storeCompany.Name))
            {
                throw new Exception("Store company is not valid.");
            }
            companyInfo = GetCompanyInfo(storeCompany.Name);
            if (companyInfo == null)
            {
                IList<LookSendMyAdCountryCode> countryCodes = storeService.GetAll<LookSendMyAdCountryCode>(true).ToList();
                StoreAddress companyAddress = storeCompany.GetCompanyAddress();
                bool isUSAAddress = countryCodes.IsUSAAddress(companyAddress.Country);
                string countryCode = countryCodes.Alpha3Code(companyAddress.Country);
                var saveCustomerInput = CreateCompanyCustomerInfoInput(storeOrderDetail, storeCompany)
                    .AddCompanyAddresses(storeCompany, countryCodes)
                    .AddCusCommunicationInput(CommunicationInputPhone, storeCompany.Phone, CommunicationLocationCodeCorporate, countryCode, isUSAAddress)
                    .AddCusCommunicationInput(CommunicationInputFax, storeCompany.Fax, CommunicationLocationCodeCorporate, countryCode, isUSAAddress)
                    .AddCusCommunicationInput(CommunicationInputEmail, storeCompany.Email, CommunicationLocationCodeCorporate)
                    .AddCusCommunicationInput(CommunicationInputWeb, storeCompany.WebURL, CommunicationLocationCodeCorporate);
                SaveCustomerOutput Rslts = SvcClient.Post<SaveCustomerOutput>("CreateCompany", saveCustomerInput);
                if (string.IsNullOrWhiteSpace(Rslts.WarningMessage))
                {
                    companyInfo = GetCompanyInfo(storeCompany.Name);
                }
            }
            return companyInfo;
        }

        public static CustomerInfo GetCompanyInfo(string companyName)
        {
            CustomerInfo customerInfo = null;
            if (companyName != null)
            {
                List<CustomerInfo> oCusInfo = SvcClient.Ctxt.CustomerInfos.Where(
                    a => a.LabelName == companyName && a.RecordType == RecordTypeCorporate).ToList();
                customerInfo = oCusInfo.Count == 0 ? null : oCusInfo.FirstOrDefault();
            }
            return customerInfo;
        }

        public static IEnumerable<CustomerInfo> AddIndividualInfos(StoreOrder storeOrder, IStoreService storeService)
        {
            if (storeOrder == null || storeOrder.Company == null)
            {
                throw new Exception("Order or company can't be null.");
            }
            IEnumerable<CustomerInfo> customerInfos = null;
            try
            {
                IList<LookSendMyAdCountryCode> countryCodes = storeService.GetAll<LookSendMyAdCountryCode>(true).ToList();
                StoreCompany storeCompany = storeOrder.Company;
                StoreAddress companyAddress = storeCompany.GetCompanyAddress();
                bool isUSAAddress = countryCodes.IsUSAAddress(companyAddress.Country);
                string countryCode = countryCodes.Alpha3Code(companyAddress.Country);

                IEnumerable<Task<CustomerInfo>> storeIndividualTasks = storeCompany.Individuals.Select(
                    storeIndividual => Task.Run<CustomerInfo>(() => GetIndividualInfo(storeIndividual.FirstName, storeIndividual.LastName)));
                Task<CustomerInfo[]> result1 = Task.WhenAll(storeIndividualTasks);
                IEnumerable<CustomerInfo> storeInfos = result1.Result.Where(storeInfo => storeInfo != null);
                IEnumerable<StoreIndividual> storeIndividuals =
                    storeCompany.Individuals.Where(
                    storeIndividual => !storeInfos.Any(
                        storeInfo => string.Equals(storeInfo.FirstName, storeIndividual.FirstName, StringComparison.InvariantCultureIgnoreCase)
                                  && string.Equals(storeInfo.LastName, storeIndividual.LastName, StringComparison.InvariantCultureIgnoreCase)));
                IEnumerable<Task<CustomerInfo>> saveCustomerOutputs = storeIndividuals
                    .Select<StoreIndividual, SaveCustomerInput>(
                        storeIndividual
                            =>
                        CreateIndividualCustomerInfoInput(storeIndividual)
                        .AddIndividualAddress(storeIndividual, storeCompany)
                        .AddCusCommunicationInput(CommunicationInputPhone, storeIndividual.Phone, CommunicationLocationCodeWork, countryCode, isUSAAddress)
                        .AddCusCommunicationInput(CommunicationInputEmail, storeIndividual.Email, CommunicationLocationCodeWork)
                    )
                    .Select(
                        saveCustomerInput
                            =>
                        Task<SaveCustomerOutput>.Run(() => SvcClient.Post<SaveCustomerOutput>("CreateIndividual", saveCustomerInput))
                        .ContinueWith<CustomerInfo>(saveCustomerOutput => GetIndividualInfo(saveCustomerOutput.Result.MasterCustomerId)
                    )
                );
                Task<CustomerInfo[]> result2 = Task.WhenAll(saveCustomerOutputs);
                customerInfos = result2.Result;
            }
            catch
            {
                customerInfos = null;
            }
            return customerInfos;
        }

        public static CustomerInfo GetIndividualInfo(string firstName = null, string lastName = null)
        {
            List<CustomerInfo> oCusInfo = SvcClient.Ctxt.CustomerInfos.Where(
                a => a.FirstName == firstName && a.LastName == lastName && a.RecordType == RecordTypeIndividual).ToList();
            if (oCusInfo.Count == 0)
            {
                return null;
            }
            return oCusInfo.FirstOrDefault();
        }

        private static IEnumerable<ASICustomerCreditCard> GetCreditCardInfos(string masterCustomerId, int subCustomerId)
        {
            IEnumerable<ASICustomerCreditCard> oCreditCards = SvcClient.Ctxt.ASICustomerCreditCards
                .Where(c => c.MasterCustomerId == masterCustomerId && c.SubCustomerId == subCustomerId);
            if (oCreditCards.Count() == 0)
            {
                oCreditCards = null;
            }
            return oCreditCards;
        }

        private static SaveCustomerInput CreateIndividualCustomerInfoInput(StoreIndividual storeIndividual)
        {
            var customerInfo = new SaveCustomerInput
            {
                FirstName = storeIndividual.FirstName,
                LastName = storeIndividual.LastName,
                CustomerClassCode = CustomerClassIndiv
            };
            return customerInfo;
        }

        private static SaveCustomerInput AddIndividualAddress(this SaveCustomerInput customerInfo, StoreIndividual storeIndividual, StoreCompany storeCompany)
        {
            CustomerInfo companyInfo = GetCompanyInfo(storeCompany.Name);
            if (companyInfo == null)
            {
                throw new Exception("Company information is not available in Personify.");
            }
            List<AddressInfo> companyAddressInfos = SvcClient.Ctxt.AddressInfos.Where(
               a => a.MasterCustomerId == companyInfo.MasterCustomerId).ToList();
            if (companyAddressInfos.Count <= 0)
            {
                throw new Exception("Company address is not available in Personify.");
            }
            if (companyAddressInfos.Count > 1)
            {
                List<AddressInfo> companyAddressInfos1 = companyAddressInfos.Where(a => a.PrioritySeq == 0).ToList();
                if (companyAddressInfos1.Count >= 1) companyAddressInfos = companyAddressInfos1;
            }
            AddressInfo companyAddressInfo = companyAddressInfos[0];
            var customerAddressInput = new DataServiceCollection<SaveAddressInput>(null, TrackingMode.None);
            var saveAddressInput = new SaveAddressInput()
            {
                AddressTypeCode = CommunicationLocationCodeCorporate,
                JobTitle = storeIndividual.Title,
                OwnerMasterCustomer = companyInfo.MasterCustomerId,
                OwnerSubCustomer = companyInfo.SubCustomerId,
                LinkToOwnersAddress = true,
                OwnerAddressStatusCode = companyAddressInfo.AddressStatusCode,//companyInfo.Addresses.FirstOrDefault().AddressStatusCode,
                OwnerAddressId = companyAddressInfo.CustomerAddressId,//companyInfo.Addresses.FirstOrDefault().CustomerAddressId,
                OwnerRecordType = companyInfo.RecordType,
                OwnerCompanyName = companyInfo.LastName,
                OverrideAddressValidation = true,
                CreateNewAddressIfOrdersExist = true,
                CreateRelationshipRecord = false,
                SetRelationshipAsPrimary = false,
                EndOldPrimaryRelationship = false,
                WebMobileDirectory = false,
                AddedOrModifiedBy = AddressAddedOrModifiedBy
            };
            var addresses = new DataServiceCollection<SaveAddressInput>(null, TrackingMode.None);
            addresses.Add(saveAddressInput);
            customerInfo.Addresses = addresses;
            return customerInfo;
        }

        private static SaveCustomerInput CreateCompanyCustomerInfoInput(StoreOrderDetail storeOrderDetail, StoreCompany storeCompany)
        {
            var customerInfo = new SaveCustomerInput
            {
                LastName = storeCompany.Name,
                CustomerClassCode = storeOrderDetail.Order.OrderRequestType.ToUpper() // DISTRIBUTOR, DECORRATOR, SUPPLIER
            };
            return customerInfo;
        }

        private static SaveCustomerInput AddCompanyAddresses(this SaveCustomerInput customerInfo, StoreCompany storeCompany, IList<LookSendMyAdCountryCode> countryCodes)
        {
            if (customerInfo.Addresses == null)
            {
                customerInfo.Addresses = new DataServiceCollection<SaveAddressInput>(null, TrackingMode.None);
            }
            StoreAddress shippingAddress = storeCompany.GetCompanyShippingAddress();
            StoreAddress companyAddress = storeCompany.GetCompanyAddress();
            foreach (var address in storeCompany.Addresses)
            {
                string code = countryCodes.Alpha3Code(address.Address.Country);
                bool shipToFlag = shippingAddress.Equals(address.Address);
                long prioritySeq = address.Equals(address.Address) ? 1 : 0;
                var saveAddressInput = new SaveAddressInput
                {
                    LinkToOwnersAddress = false,
                    AddressStatusCode = "BAD",
                    OverrideAddressValidation = true,
                    CreateNewAddressIfOrdersExist = true,
                    SetRelationshipAsPrimary = false,
                    EndOldPrimaryRelationship = false,
                    WebMobileDirectory = false,
                    AddressTypeCode = CommunicationLocationCodeCorporate,
                    Address1 = address.Address.Street1,
                    Address2 = address.Address.Street2,
                    City = address.Address.City,
                    State = address.Address.State,
                    County = address.Address.Country,
                    PostalCode = address.Address.Zip,
                    CountryCode = code,
                    BillToFlag = address.IsBilling,
                    ShipToFlag = shipToFlag,
                    DirectoryFlag = false,
                    CompanyName = storeCompany.Name,
                    AddedOrModifiedBy = AddressAddedOrModifiedBy,
                    PrioritySeq = prioritySeq
                };
                if (customerInfo.Addresses == null)
                {
                    customerInfo.Addresses = new DataServiceCollection<SaveAddressInput>(null, TrackingMode.None);
                }
                customerInfo.Addresses.Add(saveAddressInput);
            }
            return customerInfo;
        }

        private static SaveCustomerInput AddCusCommunicationInput(
         this SaveCustomerInput customerInfo, string key, string value, string communitionLocationCode, string countryCode = null, bool isUSA = true)
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                if ((key == CommunicationInputPhone || key == CommunicationInputFax) && countryCode != null)
                {
                    value = new string(value.Where(c => Char.IsDigit(c)).ToArray());
                    value = value.Substring(0, Math.Min(value.Substring(0).Length, PhoneNumberLength));
                    if (value.Length == PhoneNumberLength && isUSA)
                    {
                        customerInfo.Communication.Add(new CusCommunicationInput
                        {
                            CommLocationCode = communitionLocationCode,
                            CommTypeCode = key,
                            CountryCode = countryCode,
                            PhoneAreaCode = value.Substring(0, 3),
                            PhoneNumber = value.Substring(3, 7),
                            ActiveFlag = true
                        });
                    }
                }
                if ((key != CommunicationInputPhone || key != CommunicationInputFax) && countryCode == null)
                {
                    customerInfo.Communication.Add(new CusCommunicationInput
                    {
                        CommLocationCode = communitionLocationCode,
                        CommTypeCode = key,
                        FormattedPhoneAddress = value,
                        ActiveFlag = true
                    });
                }
            }
            return customerInfo;
        }

        private static CustomerInfo GetIndividualInfo(string masterCustomerId)
        {
            List<CustomerInfo> oCusInfo = SvcClient.Ctxt.CustomerInfos.Where(
                a => a.MasterCustomerId == masterCustomerId && a.RecordType == "I").ToList();
            if (oCusInfo.Count == 0)
            {
                return null;
            }
            return oCusInfo.FirstOrDefault();
        }

        private static IList<LookSendMyAdCountryCode> GetCountryCodes(IStoreService storeService)
        {
            if (CountryCodes == null) CountryCodes = storeService.GetAll<LookSendMyAdCountryCode>(true).ToList();
            return CountryCodes;
        }

        private static bool Validate(string receiptType, string creditCardNum)
        {
            var aSIValidateCreditCardInput = new ASIValidateCreditCardInput()
            {
                ReceiptType = receiptType,
                CreditCardNumber = creditCardNum
            };
            ASIValidateCreditCardOutput resp = SvcClient.Post<ASIValidateCreditCardOutput>("ASIValidateCreditCard", aSIValidateCreditCardInput);
            return resp.IsValid ?? false;
        }

        private static StoreOrder GetOrder(string orderId, IStoreService storeService)
        {
            StoreOrder storeOrder = null;
            if (orderId != null)
            {
                storeOrder = storeService.GetAll<StoreOrder>(true).Where(order => order.UserReference == orderId).SingleOrDefault();
                if (storeOrder != null && storeOrder.ProcessStatus != OrderStatus.Pending) storeOrder = null;
            }
            return storeOrder;
        }
    }
}