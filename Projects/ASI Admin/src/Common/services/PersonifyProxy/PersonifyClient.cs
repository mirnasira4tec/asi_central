using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using asi.asicentral.util.store.companystore;
using System.Threading.Tasks;
using asi.asicentral.model;
using asi.asicentral.PersonifyDataASI;

namespace asi.asicentral.services.PersonifyProxy
{

    public class PersonifyClient
    {

        private const string ADDRESS_ADDED_OR_MODIFIED_BY = "ASI_Store";

        private const string COMMUNICATION_INPUT_PHONE = "PHONE";

        private const string COMMUNICATION_INPUT_FAX = "FAX";

        private const string COMMUNICATION_INPUT_WEB = "WEB";

        private const string COMMUNICATION_INPUT_EMAIL = "EMAIL";

        private const string COMMUNICATION_LOCATION_CODE_CORPORATE = "CORPORATE";

        private const string COMMUNICATION_LOCATION_CODE_WORK = "WORK";

        private const string COMMUNICATION_LOCATION_CODE_UNV = "UNV";

        private const string CUSTOMER_CLASS_INDIV = "INDIV";

        private const string RECORD_TYPE_INDIVIDUAL = "I";

        private const string RECORD_TYPE_CORPORATE = "C";

        private const int PHONE_NUMBER_LENGTH = 10;

        private static readonly Dictionary<string, string> CreditCardType =
            new Dictionary<string, string>(4) { { "AMEX", "AMEX" }, { "DISCOVER", "DISCOVER" }, { "MASTERCARD", "MC" }, { "VISA", "VISA" } };

        public bool ValidateCreditCard(CreditCard info)
        {
            var aSIValidateCreditCardInput = new ASIValidateCreditCardInput()
            {
                ReceiptType = info.Type,
                CreditCardNumber = info.Number
            };
            ASIValidateCreditCardOutput resp = SvcClient.Post<ASIValidateCreditCardOutput>("ASIValidateCreditCard", aSIValidateCreditCardInput);
            return resp.IsValid ?? false;
        }

        public bool SaveCreditCard(CreditCard info, StoreOrder storeOrder)
        {
            CustomerInfo companyInfo = GetCompanyInfo(storeOrder.Company.Name);
            var customerCreditCardInput = new CustomerCreditCardInput()
            {
                MasterCustomerId = companyInfo.MasterCustomerId,
                SubCustomerId = companyInfo.SubCustomerId,
                ReceiptType = CreditCardType[info.Type.ToUpper()],
                CreditCardNumber = info.Number,
                ExpirationMonth = (short)info.ExpirationDate.Month,
                ExpirationYear = (short)info.ExpirationDate.Year,
                NameOnCard = info.CardHolderName,
                BillingAddressStreet = info.Address,
                BillingAddressCity = info.City,
                BillingAddressState = info.State,
                BillingAddressPostalCode = info.PostalCode,
                BillingAddressCountryCode = info.CountryCode,
                DefaultFlag = true,
                CompanyNumber = "1",
                AddedOrModifiedBy = ADDRESS_ADDED_OR_MODIFIED_BY
            };
            CustomerCreditCardOutput resp = SvcClient.Post<CustomerCreditCardOutput>("AddCustomerCreditCard", customerCreditCardInput);
            return resp.Success ?? false;
        }

        public IEnumerable<ASICustomerCreditCard> GetCreditCardInfos(StoreOrder storeOrder)
        {
            CustomerInfo companyInfo = GetCompanyInfo(storeOrder.Company.Name);
            IEnumerable<ASICustomerCreditCard> oCreditCards = GetCreditCardInfos(companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
            return oCreditCards;
        }

        public CustomerInfo AddCompanyInfo(StoreOrder storeOrder, IList<LookSendMyAdCountryCode> countryCodes)
        {
            CustomerInfo companyInfo = null;
            StoreCompany storeCompany = storeOrder.Company;
            if (storeCompany == null || string.IsNullOrWhiteSpace(storeCompany.Name))
            {
                throw new Exception("Store company is not valid.");
            }
            companyInfo = GetCompanyInfo(storeCompany.Name);
            if (companyInfo == null)
            {
                StoreAddress companyAddress = storeCompany.GetCompanyAddress();
                bool isUsaAddress = countryCodes.IsUSAAddress(companyAddress.Country);
                string countryCode = countryCodes.Alpha3Code(companyAddress.Country);
                var saveCustomerInput = CreateCompanyCustomerInfoInput(storeOrder);
                AddCompanyAddresses(saveCustomerInput, storeCompany, countryCodes);
                AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_PHONE, storeCompany.Phone, COMMUNICATION_LOCATION_CODE_CORPORATE, countryCode, isUsaAddress);
                AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_FAX, storeCompany.Fax, COMMUNICATION_LOCATION_CODE_CORPORATE, countryCode, isUsaAddress);
                AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_EMAIL, storeCompany.Email, COMMUNICATION_LOCATION_CODE_CORPORATE);
                AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_WEB, storeCompany.WebURL, COMMUNICATION_LOCATION_CODE_CORPORATE);
                SaveCustomerOutput Rslts = SvcClient.Post<SaveCustomerOutput>("CreateCompany", saveCustomerInput);
                if (string.IsNullOrWhiteSpace(Rslts.WarningMessage))
                {
                    companyInfo = GetCompanyInfo(storeCompany.Name);
                }
            }
            return companyInfo;
        }

        public CustomerInfo GetCompanyInfo(string companyName)
        {
            CustomerInfo customerInfo = null;
            if (companyName != null)
            {
                List<CustomerInfo> oCusInfo = SvcClient.Ctxt.CustomerInfos.Where(
                    a => a.LabelName == companyName && a.RecordType == RECORD_TYPE_CORPORATE).ToList();
                customerInfo = oCusInfo.Count == 0 ? null : oCusInfo.FirstOrDefault();
            }
            return customerInfo;
        }

        public IEnumerable<CustomerInfo> AddIndividualInfos(StoreOrder storeOrder, IList<LookSendMyAdCountryCode> countryCodes)
        {
            if (storeOrder == null || storeOrder.Company == null)
            {
                throw new Exception("Order or company can't be null.");
            }
            IEnumerable<CustomerInfo> customerInfos = null;
            try
            {
                StoreCompany storeCompany = storeOrder.Company;
                StoreAddress companyAddress = storeCompany.GetCompanyAddress();
                bool isUsaAddress = countryCodes.IsUSAAddress(companyAddress.Country);
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
                        {
                            SaveCustomerInput customerInfo = CreateIndividualCustomerInfoInput(storeIndividual);
                            AddIndividualAddress(customerInfo, storeIndividual, storeCompany);
                            AddCusCommunicationInput(customerInfo, COMMUNICATION_INPUT_PHONE, storeIndividual.Phone, COMMUNICATION_LOCATION_CODE_WORK, countryCode, isUsaAddress);
                            AddCusCommunicationInput(customerInfo, COMMUNICATION_INPUT_EMAIL, storeIndividual.Email, COMMUNICATION_LOCATION_CODE_WORK);
                            return customerInfo;
                        }
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

        public CustomerInfo GetIndividualInfo(string firstName, string lastName)
        {
            CustomerInfo customerInfo = null;
            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
            {
                List<CustomerInfo> oCusInfo = SvcClient.Ctxt.CustomerInfos.Where(
                    a => a.FirstName == firstName && a.LastName == lastName && a.RecordType == RECORD_TYPE_INDIVIDUAL).ToList();
                if (oCusInfo.Count != 0)
                {
                    customerInfo = oCusInfo.FirstOrDefault();
                }
            }
            return customerInfo;
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

        private SaveCustomerInput CreateIndividualCustomerInfoInput(StoreIndividual storeIndividual)
        {
            var customerInfo = new SaveCustomerInput
            {
                FirstName = storeIndividual.FirstName,
                LastName = storeIndividual.LastName,
                CustomerClassCode = CUSTOMER_CLASS_INDIV
            };
            return customerInfo;
        }

        private SaveCustomerInput AddIndividualAddress(SaveCustomerInput customerInfo, StoreIndividual storeIndividual, StoreCompany storeCompany)
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
                AddressTypeCode = COMMUNICATION_LOCATION_CODE_CORPORATE,
                JobTitle = storeIndividual.Title,
                OwnerMasterCustomer = companyInfo.MasterCustomerId,
                OwnerSubCustomer = companyInfo.SubCustomerId,
                LinkToOwnersAddress = true,
                OwnerAddressStatusCode = companyAddressInfo.AddressStatusCode,
                OwnerAddressId = companyAddressInfo.CustomerAddressId,
                OwnerRecordType = companyInfo.RecordType,
                OwnerCompanyName = companyInfo.LastName,
                OverrideAddressValidation = true,
                CreateNewAddressIfOrdersExist = true,
                CreateRelationshipRecord = false,
                SetRelationshipAsPrimary = false,
                EndOldPrimaryRelationship = false,
                WebMobileDirectory = false,
                AddedOrModifiedBy = ADDRESS_ADDED_OR_MODIFIED_BY
            };
            var addresses = new DataServiceCollection<SaveAddressInput>(null, TrackingMode.None);
            addresses.Add(saveAddressInput);
            customerInfo.Addresses = addresses;
            return customerInfo;
        }

        private static SaveCustomerInput CreateCompanyCustomerInfoInput(StoreOrder storeOrder)
        {
            var customerInfo = new SaveCustomerInput
            {
                LastName = storeOrder.Company.Name,
                CustomerClassCode = storeOrder.OrderRequestType.ToUpper()
            };
            return customerInfo;
        }

        private static SaveCustomerInput AddCompanyAddresses(SaveCustomerInput customerInfo, StoreCompany storeCompany, IList<LookSendMyAdCountryCode> countryCodes)
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
                long prioritySeq = companyAddress.Equals(address.Address) ? 0 : 1;
                var saveAddressInput = new SaveAddressInput
                {
                    LinkToOwnersAddress = false,
                    AddressStatusCode = "BAD",
                    OverrideAddressValidation = true,
                    CreateNewAddressIfOrdersExist = true,
                    SetRelationshipAsPrimary = false,
                    EndOldPrimaryRelationship = false,
                    WebMobileDirectory = false,
                    AddressTypeCode = COMMUNICATION_LOCATION_CODE_CORPORATE,
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
                    AddedOrModifiedBy = ADDRESS_ADDED_OR_MODIFIED_BY,
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
         SaveCustomerInput customerInfo, string key, string value, string communitionLocationCode, string countryCode = null, bool isUSA = true)
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                if ((key == COMMUNICATION_INPUT_PHONE || key == COMMUNICATION_INPUT_FAX) && countryCode != null)
                {
                    value = new string(value.Where(c => Char.IsDigit(c)).ToArray());
                    value = value.Substring(0, Math.Min(value.Substring(0).Length, PHONE_NUMBER_LENGTH));
                    if (value.Length == PHONE_NUMBER_LENGTH && isUSA)
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
                if ((key != COMMUNICATION_INPUT_PHONE || key != COMMUNICATION_INPUT_FAX) && countryCode == null)
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
    }
}