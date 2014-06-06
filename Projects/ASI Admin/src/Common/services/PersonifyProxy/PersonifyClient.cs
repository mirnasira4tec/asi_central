using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using asi.asicentral.util.store.companystore;
using System.Threading.Tasks;
using asi.asicentral.model;
using asi.asicentral.PersonifyDataASI;
using PersonifySvcClient;

namespace asi.asicentral.services.PersonifyProxy
{
    public static class PersonifyClient
    {
        private const string ADDED_OR_MODIFIED_BY = "ASI_Store";
        private const string COMMUNICATION_INPUT_PHONE = "PHONE";
        private const string COMMUNICATION_INPUT_FAX = "FAX";
        private const string COMMUNICATION_INPUT_WEB = "WEB";
        private const string COMMUNICATION_INPUT_EMAIL = "EMAIL";
        private const string COMMUNICATION_LOCATION_CODE_CORPORATE = "CORPORATE";
        private const string COMMUNICATION_LOCATION_CODE_WORK = "WORK";
        private const string CUSTOMER_CLASS_INDIV = "INDIV";
        private const string RECORD_TYPE_INDIVIDUAL = "I";
        private const string RECORD_TYPE_CORPORATE = "C";
        private const int PHONE_NUMBER_LENGTH = 10;

        private static readonly Dictionary<string, string> CreditCardType =
            new Dictionary<string, string>(4, StringComparer.InvariantCultureIgnoreCase)
            {
                { "AMEX", "AMEX" }, { "DISCOVER", "DISCOVER" }, { "MASTERCARD", "MC" }, { "VISA", "VISA" }
            };

        public static CreateOrderOutput CreateOrder(StoreOrder storeOrder,
            CustomerInfo companyInfo,
            CustomerInfo contactInfo,
            long billToAddressId,
            long shiptoAddressId,
            IList<CreateOrderLineInput> lineItems)
        {
            if (companyInfo == null)
                throw new ArgumentException("You need to pass the company information");

            var orderLineInputs = new DataServiceCollection<CreateOrderLineInput>(null, TrackingMode.None);
            foreach (var lineItem in lineItems)
                orderLineInputs.Add(lineItem);

            var createOrderInput = new CreateOrderInput()
            {
                BillMasterCustomerID = companyInfo.MasterCustomerId,
                BillSubCustomerID = Convert.ToInt16(companyInfo.SubCustomerId),
                BillAddressID = Convert.ToInt32(billToAddressId),
                ShipMasterCustomerID = contactInfo.MasterCustomerId,
                ShipSubCustomerID = Convert.ToInt16(contactInfo.SubCustomerId),
                ShipAddressID = Convert.ToInt32(shiptoAddressId),
                OrderLines = orderLineInputs,
                AddedOrModifiedBy = ADDED_OR_MODIFIED_BY,
            };
            var orderOutput = SvcClient.Post<CreateOrderOutput>("CreateOrder", createOrderInput);
            return orderOutput;
        }

        public static decimal GetOrderTotal(string orderNumber)
        {
            decimal total = 0;
            IList<WebOrderBalanceView> oOrdBalInfo = SvcClient.Ctxt.WebOrderBalanceViews
                .Where(o => o.OrderNumber == orderNumber).ToList();

            if (oOrdBalInfo.Count > 0)
            {
                total = oOrdBalInfo.Sum(o => Convert.ToDecimal(o.ActualBalanceAmount));
            }
            return total;
        }

        public static CustomerInfo ReconcileCompany(StoreCompany company, IList<LookSendMyAdCountryCode> countryCodes)
        {
            CustomerInfo companyInfo = null;

            if (company == null || string.IsNullOrWhiteSpace(company.Name))
            {
                throw new Exception("Store company is not valid.");
            }
            if (!string.IsNullOrEmpty(company.ExternalReference))
            {
                string[] references = company.ExternalReference.Split(';');
                int subCustomerId = Int32.Parse(references[1]);
                companyInfo = GetCompanyInfo(references[0], subCustomerId);
            }
            else
            {
                //look company by ASI#
                if (!string.IsNullOrEmpty(company.ASINumber))
                {
                    companyInfo = GetCompanyInfoByAsiNumber(company.ASINumber);
                }
            }
            if (companyInfo == null)
            {
                //company not already there, create a new one
                StoreAddress companyAddress = company.GetCompanyAddress();
                bool isUsaAddress = countryCodes.IsUSAAddress(companyAddress.Country);
                string countryCode = countryCodes.Alpha3Code(companyAddress.Country);
                var saveCustomerInput = new SaveCustomerInput { LastName = company.Name, CustomerClassCode = "UNKNOWN" };
                AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_PHONE, company.Phone,
                    COMMUNICATION_LOCATION_CODE_CORPORATE, countryCode, isUsaAddress);
                AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_FAX, company.Fax,
                    COMMUNICATION_LOCATION_CODE_CORPORATE, countryCode, isUsaAddress);
                AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_EMAIL, company.Email,
                    COMMUNICATION_LOCATION_CODE_CORPORATE);
                AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_WEB, company.WebURL,
                    COMMUNICATION_LOCATION_CODE_CORPORATE);
                var result = SvcClient.Post<SaveCustomerOutput>("CreateCompany", saveCustomerInput);
                if (result != null && string.IsNullOrWhiteSpace(result.WarningMessage))
                {
                    var subCustomerId = result.SubCustomerId.HasValue ? result.SubCustomerId.Value : 0;
                    //try update status - not caring so much whether it works or not
                    //@todo this does not seem to be working for updating the status
                    var q =
                        SvcClient.Ctxt.ASICustomers.Where(
                            p => p.MasterCustomerId == result.MasterCustomerId && p.SubCustomerId == subCustomerId).Select(o => o);
                    var customers = new DataServiceCollection<ASICustomer>(q, TrackingMode.None);
                    if (customers.Count > 0)
                    {
                        ASICustomer customer = customers[0];
						customer.UserDefinedMemberStatusString = "ASICENTRAL";
                        SvcClient.Save<ASICustomer>(customer);
                    }
                    companyInfo = GetCompanyInfo(result.MasterCustomerId, subCustomerId);
                }
            }
            if (companyInfo != null) company.ExternalReference = companyInfo.MasterCustomerId + ";" + companyInfo.SubCustomerId;

            return companyInfo;
        }

        public static IDictionary<AddressType, AddressInfo> AddCompanyAddresses(StoreCompany storeCompany,
            CustomerInfo companyInfo,
            IList<LookSendMyAdCountryCode> countryCodes)
        {
            if (storeCompany == null || storeCompany.Addresses == null || companyInfo == null)
            {
                throw new Exception("Store address information and company infomation in personify are required");
            }
            IDictionary<AddressType, long> addressesAdded = null;
            StoreAddress companyAddress = storeCompany.GetCompanyAddress();
            StoreAddress billToAddress = storeCompany.GetCompanyBillingAddress();
            StoreAddress shipToAddress = storeCompany.GetCompanyShippingAddress();
            AddressInfo companyAddressInfo = null;
            AddressInfo billToAddressInfo = null;
            AddressInfo shipToAddressInfo = null;
            AddressInfo primaryAddressInfo = null;

            List<AddressInfo> addressInfos = SvcClient.Ctxt.AddressInfos.Where(
                    a => a.MasterCustomerId == companyInfo.MasterCustomerId).ToList();
            if (addressInfos.Any())
            {
                companyAddressInfo = addressInfos.FirstOrDefault(a =>
                   a.Address1 == companyAddress.Street1 && a.PostalCode == companyAddress.Zip);
                billToAddressInfo = addressInfos.FirstOrDefault(a =>
                    a.Address1 == billToAddress.Street1 && a.PostalCode == billToAddress.Zip);
                shipToAddressInfo = addressInfos.FirstOrDefault(a =>
                    a.Address1 == shipToAddress.Street1 && a.PostalCode == shipToAddress.Zip);
                primaryAddressInfo = addressInfos.FirstOrDefault(a => a.PrioritySeq == 0);
            }

            addressesAdded = new Dictionary<AddressType, long>();

            if (companyAddressInfo == null)
            {
                var countryCode = countryCodes.Alpha3Code(companyAddress.Country);
                var result = AddCompanyAddress(companyAddress,
                    primaryAddressInfo,
                    companyInfo,
                    storeCompany.Name,
                    countryCode,
                    companyAddress.Equals(billToAddress) || !addressInfos.Any(),
                    companyAddress.Equals(shipToAddress) || !addressInfos.Any());
                if (result != null && result.CusAddressId.HasValue)
                {
                    addressesAdded[AddressType.Primary] = result.CusAddressId.Value;
                    if (primaryAddressInfo == null)
                        primaryAddressInfo = addressInfos.FirstOrDefault(a => a.PrioritySeq == 0);
                }
            }
            else
            {
                addressesAdded[AddressType.Primary] = companyAddressInfo.CustomerAddressId;
            }
            addressesAdded[AddressType.Billing] = addressesAdded[AddressType.Primary];
            addressesAdded[AddressType.Shipping] = addressesAdded[AddressType.Primary];

            if (billToAddress != null && !billToAddress.Equals(companyAddress))
            {
                if (billToAddressInfo == null)
                {

                    var countryCode = countryCodes.Alpha3Code(billToAddress.Country);
                    var result = AddCompanyAddress(billToAddress, primaryAddressInfo, companyInfo,
                        storeCompany.Name, countryCode, true, billToAddress.Equals(shipToAddress));
                    if (result != null && result.CusAddressId.HasValue)
                    {
                        addressesAdded[AddressType.Billing] = result.CusAddressId.Value;
                    }
                }
                else
                {
                    addressesAdded[AddressType.Billing] = billToAddressInfo.CustomerAddressId;
                }
            }
            if (billToAddress != null && billToAddress.Equals(shipToAddress))
                addressesAdded[AddressType.Shipping] = addressesAdded[AddressType.Billing];

            if (shipToAddress != null && !shipToAddress.Equals(companyAddress) && !shipToAddress.Equals(billToAddress))
            {
                if (shipToAddressInfo == null)
                {
                    var countryCode = countryCodes.Alpha3Code(shipToAddress.Country);
                    var result = AddCompanyAddress(shipToAddress, primaryAddressInfo, companyInfo,
                        storeCompany.Name, countryCode, false, true);
                    if (result != null && result.CusAddressId.HasValue)
                    {
                        addressesAdded[AddressType.Shipping] = result.CusAddressId.Value;
                    }
                }
                else
                {
                    addressesAdded[AddressType.Shipping] = shipToAddressInfo.CustomerAddressId;
                }
            }

            var addressInfoAdded = new Dictionary<AddressType, AddressInfo>();
            List<AddressInfo> companyAddressInfos = SvcClient.Ctxt.AddressInfos.Where(
               a => a.MasterCustomerId == companyInfo.MasterCustomerId && a.SubCustomerId == companyInfo.SubCustomerId).ToList();
            foreach (var pair in addressesAdded)
            {
                AddressInfo addressInfo = companyAddressInfos.SingleOrDefault(a => a.CustomerAddressId == pair.Value);
                addressInfoAdded[pair.Key] = addressInfo;
            }
            return addressInfoAdded;
        }

        private static SaveAddressOutput AddCompanyAddress(StoreAddress address, AddressInfo existingPrimaryAddress,
            CustomerInfo companyInfo, string companyName, string countryCode, bool billTo, bool shipTo)
        {
            var newCustomerAddress = new SaveAddressInput()
            {
                MasterCustomerId = companyInfo.MasterCustomerId,
                SubCustomerId = companyInfo.SubCustomerId,
                AddressTypeCode = COMMUNICATION_LOCATION_CODE_CORPORATE,
                Address1 = address.Street1,
                Address2 = address.Street2,
                City = address.City,
                State = address.State,
                PostalCode = address.Zip,
                CountryCode = countryCode,
                BillToFlag = billTo,
                ShipToFlag = shipTo,
                DirectoryFlag = true,
                CompanyName = companyName,
                WebMobileDirectory = false,
                CreateNewAddressIfOrdersExist = true,
                OverrideAddressValidation = true,
                AddedOrModifiedBy = ADDED_OR_MODIFIED_BY,
            };
            if (existingPrimaryAddress == null)
            {
                newCustomerAddress.PrioritySeq = 0;
            }
            else
            {
                newCustomerAddress.OwnerAddressId = Convert.ToInt32(existingPrimaryAddress.CustomerAddressId);
            }
            return SvcClient.Post<SaveAddressOutput>("CreateOrUpdateAddress", newCustomerAddress);
        }

        public static SaveCustomerOutput AddCompanyByNameAndMemberTypeId(string companyName, int memberTypeId)
        {
            MemberData memberData = MemberTypeIDToCD.Data[memberTypeId];
            if (string.IsNullOrWhiteSpace(companyName) || memberData == null)
            {
                throw new Exception("Company name or member type id is not valid.");
            }
            var saveCustomerInput = new SaveCustomerInput
            {
                LastName = companyName,
                CustomerClassCode = memberData.MemberTypeCD,
            };

            var result = SvcClient.Post<SaveCustomerOutput>("CreateCompany", saveCustomerInput);
            if (!string.IsNullOrWhiteSpace(result.WarningMessage))
            {
                result = null;
            }
            return result;
        }

        public static CustomerInfo GetCompanyInfo(string masterCustomerId, int subCustomerId)
        {
            CustomerInfo customerInfo = null;
            //First() or Single() are not supported
            var customerList = SvcClient.Ctxt.CustomerInfos.Where(
                    company => company.MasterCustomerId == masterCustomerId &&
                    company.SubCustomerId == subCustomerId &&
                    company.RecordType == RECORD_TYPE_CORPORATE).ToList();

            if (customerList.Count == 1) customerInfo = customerList[0];
            return customerInfo;
        }

        public static CustomerInfo GetCompanyInfoByAsiNumber(string asiNumber)
        {
            CustomerInfo customerInfo = null;
            if (!string.IsNullOrWhiteSpace(asiNumber))
            {
                IList<ASICustomerInfo> customerinfos = SvcClient.Ctxt.ASICustomerInfos
                    .Where(c => c.UserDefinedAsiNumber == asiNumber).ToList();
                if (customerinfos.Count > 0)
                {
                    IList<CustomerInfo> customerinfos2 = SvcClient.Ctxt.CustomerInfos.Where(
                                c => c.MasterCustomerId == customerinfos[0].MasterCustomerId).ToList();
                    customerInfo = customerinfos2.Count == 0 ? null : customerinfos2.FirstOrDefault();
                }
            }
            return customerInfo;
        }

        public static IEnumerable<CustomerInfo> AddIndividualInfos(StoreOrder storeOrder,
            IList<LookSendMyAdCountryCode> countryCodes,
            CustomerInfo companyInfo)
        {
            if (storeOrder == null || storeOrder.Company == null)
            {
                throw new Exception("Order or company can't be null.");
            }
            StoreCompany storeCompany = storeOrder.Company;
            StoreAddress companyAddress = storeCompany.GetCompanyAddress();
            IEnumerable<StoreIndividual> storeIndividuals = storeCompany.Individuals;
            bool isUsaAddress = countryCodes.IsUSAAddress(companyAddress.Country);
            string countryCode = countryCodes.Alpha3Code(companyAddress.Country);

            IEnumerable<Task<CustomerInfo>> storeIndividualTasks = storeCompany.Individuals.Select(
                storeIndividual =>
                    Task.Run<CustomerInfo>(
                        () => GetIndividualInfo(storeIndividual.FirstName, storeIndividual.LastName, companyInfo)));
            Task<CustomerInfo[]> result1 = Task.WhenAll(storeIndividualTasks);
            IEnumerable<CustomerInfo> storeInfos = result1.Result.Where(storeInfo => storeInfo != null);
            storeIndividuals =
                storeCompany.Individuals.Where(
                    storeIndividual => !storeInfos.Any(
                        storeInfo =>
                            string.Equals(storeInfo.FirstName, storeIndividual.FirstName,
                                StringComparison.InvariantCultureIgnoreCase)
                            &&
                            string.Equals(storeInfo.LastName, storeIndividual.LastName,
                                StringComparison.InvariantCultureIgnoreCase)));

            IEnumerable<Task<CustomerInfo>> saveCustomerOutputs = storeIndividuals
                .Select<StoreIndividual, SaveCustomerInput>(
                    storeIndividual
                        =>
                    {
                        var customerInfo = new SaveCustomerInput
                        {
                            FirstName = storeIndividual.FirstName,
                            LastName = storeIndividual.LastName,
                            CustomerClassCode = CUSTOMER_CLASS_INDIV
                        };
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
            var result2 = Task.WhenAll(saveCustomerOutputs);
            IEnumerable<CustomerInfo> storeInfos2 = result2.Result;
            return storeInfos.Union(storeInfos2);
        }

        public static CustomerInfo GetIndividualInfo(string firstName, string lastName, CustomerInfo companyInfo)
        {
            CustomerInfo customerInfo = null;

            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName)
                && !string.IsNullOrWhiteSpace(companyInfo.MasterCustomerId))
            {
                List<CusRelationship> oCusRltnshps = SvcClient.Ctxt.CusRelationships
                    .Where(a => a.RelatedName == string.Format("{0}, {1}", lastName, firstName)
                                && a.MasterCustomerId == companyInfo.MasterCustomerId
                                && a.SubCustomerId == companyInfo.SubCustomerId).ToList();
                CusRelationship oCusRltnshp = null;
                if (oCusRltnshps.Count > 0)
                {
                    if (oCusRltnshps.Count > 1)
                    {
                        oCusRltnshp = oCusRltnshps.FirstOrDefault(
                                rltnshp => rltnshp.PrimaryContactFlag.HasValue && rltnshp.PrimaryContactFlag == true
                             || rltnshp.PrimaryEmployerFlag.HasValue && rltnshp.PrimaryEmployerFlag == true);
                        oCusRltnshp = oCusRltnshp ?? oCusRltnshps.First();
                    }
                    else
                    {
                        oCusRltnshp = oCusRltnshp ?? oCusRltnshps.First();
                    }
                    List<CustomerInfo> oCusInfo = SvcClient.Ctxt.CustomerInfos.Where(
                        a => a.MasterCustomerId == oCusRltnshp.RelatedMasterCustomerId && a.RecordType == RECORD_TYPE_INDIVIDUAL)
                        .ToList();
                    if (oCusInfo.Count > 0)
                    {
                        customerInfo = oCusInfo.FirstOrDefault();
                    }
                }
            }
            return customerInfo;
        }

        public static IEnumerable<SaveAddressOutput> AddIndividualAddresses(
            IEnumerable<CustomerInfo> contactInfos,
            IDictionary<AddressType, AddressInfo> addresseInfos)
        {
            IList<Task<SaveAddressOutput[]>> resultTasks = new List<Task<SaveAddressOutput[]>>();
            foreach (var contactInfo in contactInfos)
            {
                var addressesToAdd = AddressesToBeAdded(contactInfo, addresseInfos);
                IEnumerable<Task<SaveAddressOutput>> individualAddressTasks = addressesToAdd.Select(
                    addr => Task.Run<SaveAddressOutput>(() => AddIndividualAddress(contactInfo, addr.Key, addr.Value)));
                resultTasks.Add(Task.WhenAll(individualAddressTasks));
            }
            IEnumerable<SaveAddressOutput> results = new List<SaveAddressOutput>();
            resultTasks.ToList().ForEach(t => results = results.Union(t.Result));
            return results;
        }

        private static IDictionary<AddressType, AddressInfo> AddressesToBeAdded(CustomerInfo contactInfo,
            IDictionary<AddressType, AddressInfo> addressInfos)
        {
            IEnumerable<AddressInfo> existingAddressInfos = SvcClient.Ctxt.AddressInfos.Where(
                        a => a.MasterCustomerId == contactInfo.MasterCustomerId
                          && a.SubCustomerId == contactInfo.SubCustomerId).ToList();
			if (!existingAddressInfos.Any(info => (info.BillToFlag != null && info.BillToFlag.Value) || (info.ShipToFlag != null && info.ShipToFlag.Value)))
	        {
		        addressInfos[AddressType.Primary].BillToFlag = true;
				addressInfos[AddressType.Primary].ShipToFlag = true;
			}
            var comparer = new AddressInfoEqualityComparer();
            return addressInfos.Where(item => !existingAddressInfos.Contains(item.Value, comparer))
                .ToDictionary(item => item.Key, item => item.Value);
        }

        public static SaveAddressOutput AddIndividualAddress(CustomerInfo contactInfo, AddressType addressType, AddressInfo addressInfo)
        {
            SaveAddressOutput result = null;
            if (addressInfo != null)
            {
                var saveAddressInput = new SaveAddressInput()
                {
                    MasterCustomerId = contactInfo.MasterCustomerId,
                    SubCustomerId = contactInfo.SubCustomerId,
                    AddressTypeCode = "OFFICE",
                    Address1 = addressInfo.Address1,
                    City = addressInfo.City,
                    State = addressInfo.State,
                    PostalCode = addressInfo.PostalCode,
                    CountryCode = addressInfo.CountryCode,
                    DirectoryFlag = true,
                    WebMobileDirectory = false,
                    CreateNewAddressIfOrdersExist = true,
                    OverrideAddressValidation = true,
					ShipToFlag = addressInfo.ShipToFlag,
					BillToFlag = addressInfo.BillToFlag,
                    AddedOrModifiedBy = ADDED_OR_MODIFIED_BY,
                };
                switch (addressType)
                {
                    case AddressType.Primary:
                        saveAddressInput.PrioritySeq = 0;
                        break;
                }
                result = SvcClient.Post<SaveAddressOutput>("CreateOrUpdateAddress", saveAddressInput);
            }
            return result;
        }

        private static SaveCustomerInput LinkIndividualAddress(SaveCustomerInput customerInfo, StoreIndividual storeIndividual, CustomerInfo companyInfo)
        {
            if (companyInfo == null)
            {
                throw new Exception("Company information is not available in Personify.");
            }
            List<AddressInfo> companyAddressInfos = SvcClient.Ctxt.AddressInfos.Where(
               a => a.MasterCustomerId == companyInfo.MasterCustomerId).ToList();
            if (companyAddressInfos == null || companyAddressInfos.Count <= 0)
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
                CreateRelationshipRecord = true,
                SetRelationshipAsPrimary = true,
                EndOldPrimaryRelationship = false,
                WebMobileDirectory = false,
                AddedOrModifiedBy = ADDED_OR_MODIFIED_BY
            };
            var addresses = new DataServiceCollection<SaveAddressInput>(null, TrackingMode.None);
            addresses.Add(saveAddressInput);
            customerInfo.Addresses = addresses;
            return customerInfo;
        }

        private static SaveCustomerInput AddCusCommunicationInput(
         SaveCustomerInput customerInfo,
            string key,
            string value,
            string communitionLocationCode,
            string countryCode = null,
            bool isUSA = true)
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                if ((key == COMMUNICATION_INPUT_PHONE || key == COMMUNICATION_INPUT_FAX) && countryCode != null)
                {
                    value = new string(value.Where(Char.IsDigit).ToArray());
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
                            ActiveFlag = true,
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

        public static CusCommunication AddPhoneNumber(string phoneNumber, string countryCode, CustomerInfo companyInfo)
        {
            CusCommunication respSave = null;
            string[] phoneNumberTypes = new string[] { "BUSINESS", "CORPORATE" };
            IList<CusCommunication> oCusComms = SvcClient.Ctxt.CusCommunications
                   .Where(c => c.MasterCustomerId == companyInfo.MasterCustomerId
                               && c.CommTypeCodeString == "PHONE").ToList();
            IEnumerable<string> commTypes1 = oCusComms.Where(c => !string.IsNullOrWhiteSpace(c.PhoneNumber))
                                                      .Select(c => c.CommLocationCodeString.ToUpper());
            IEnumerable<string> commTypes2 = phoneNumberTypes.Where(c => !commTypes1.Contains(c.ToUpper()));
            if (commTypes2.Any())
            {
                try
                {
                    phoneNumber = new string(phoneNumber.Where(Char.IsDigit).ToArray());
                    if (string.Equals(countryCode, "USA", StringComparison.InvariantCultureIgnoreCase)
                        && phoneNumber.Length == PHONE_NUMBER_LENGTH
                        && !IsPhoneExist(phoneNumber, companyInfo))
                    {
                        var respCreate = SvcClient.Create<CusCommunication>();
                        respCreate.MasterCustomerId = companyInfo.MasterCustomerId;
                        respCreate.SubCustomerId = companyInfo.SubCustomerId;
                        respCreate.CommLocationCodeString = commTypes2.First();
                        respCreate.CommTypeCodeString = COMMUNICATION_INPUT_PHONE;
                        respCreate.PhoneAreaCode = phoneNumber.Substring(0, 3);
                        respCreate.PhoneNumber = phoneNumber.Substring(3, 7);
                        respCreate.CountryCode = countryCode;
                        respCreate.PrimaryFlag = false;
                        respSave = SvcClient.Save<CusCommunication>(respCreate);
                    }
                }
                catch (Exception ex)
                {
                    respSave = null;
                }
            }
            return respSave;
        }

        private static bool IsPhoneExist(string phoneNumber, CustomerInfo companyInfo)
        {
            IEnumerable<CusCommunication> cc = SvcClient.Ctxt.CusCommunications
                .Where(c => c.MasterCustomerId == companyInfo.MasterCustomerId && c.SearchPhoneAddress == phoneNumber);
            return cc.Any(c => string.Equals(c.CommTypeCodeString, "PHONE", StringComparison.InvariantCultureIgnoreCase));
        }

        public static CustomerInfo GetIndividualInfo(string masterCustomerId)
        {
            List<CustomerInfo> oCusInfo = SvcClient.Ctxt.CustomerInfos.Where(
                a => a.MasterCustomerId == masterCustomerId && a.RecordType == "I").ToList();
            if (oCusInfo.Count == 0)
            {
                return null;
            }
            return oCusInfo.FirstOrDefault();
        }

        #region Credit Card Handling

        public static bool ValidateCreditCard(CreditCard info)
        {
            var asiValidateCreditCardInput = new ASIValidateCreditCardInput()
            {
                ReceiptType = CreditCardType[info.Type.ToUpper()],
                CreditCardNumber = info.Number
            };
            var resp = SvcClient.Post<ASIValidateCreditCardOutput>("ASIValidateCreditCard", asiValidateCreditCardInput);
            return resp.IsValid ?? false;
        }

        public static string SaveCreditCard(CustomerInfo companyInfo, CreditCard creditCard)
        {
            var customerCreditCardInput = new CustomerCreditCardInput()
            {
                MasterCustomerId = companyInfo.MasterCustomerId,
                SubCustomerId = companyInfo.SubCustomerId,
                ReceiptType = CreditCardType[creditCard.Type.ToUpper()],
                CreditCardNumber = creditCard.Number,
                ExpirationMonth = (short)creditCard.ExpirationDate.Month,
                ExpirationYear = (short)creditCard.ExpirationDate.Year,
                NameOnCard = creditCard.CardHolderName,
                BillingAddressStreet = creditCard.Address,
                BillingAddressCity = creditCard.City,
                BillingAddressState = creditCard.State,
                BillingAddressPostalCode = creditCard.PostalCode,
                BillingAddressCountryCode = creditCard.CountryCode,
                DefaultFlag = true,
                CompanyNumber = "1",
                AddedOrModifiedBy = ADDED_OR_MODIFIED_BY
            };
            var resp = SvcClient.Post<CustomerCreditCardOutput>("AddCustomerCreditCard", customerCreditCardInput);
            return resp.Success ?? false ? resp.CreditCardProfileId : null;
        }

        public static string GetCreditCardProfileId(CustomerInfo companyInfo, CreditCard creditCard)
        {
            if (companyInfo == null) throw new Exception("Could not find a company to assign the credit card to");
            IEnumerable<ASICustomerCreditCard> oCreditCards = SvcClient.Ctxt.ASICustomerCreditCards
                .Where(c => c.MasterCustomerId == companyInfo.MasterCustomerId
                         && c.SubCustomerId == companyInfo.SubCustomerId
                         && c.ReceiptTypeCodeString == CreditCardType[creditCard.Type]);
            long? profileId = null;
            if (oCreditCards.Any())
            {
                string ccReference = string.Format("{0}{1}{2}", creditCard.Number.Substring(0, 6),
                    new string(Enumerable.Repeat('*', creditCard.Number.Length - 10).ToArray()),
                    creditCard.Number.Substring(creditCard.Number.Length - 4));
                profileId = oCreditCards.Where(c => c.CCReference == ccReference).Select(c => c.CustomerCreditCardProfileId).FirstOrDefault();
            }
            return profileId == null ? string.Empty : profileId.ToString();
        }

        public static ASICustomerCreditCard GetCreditCardByProfileId(CustomerInfo companyInfo, string profileId)
        {
            if (companyInfo == null || string.IsNullOrWhiteSpace(profileId))
            {
                throw new Exception("Company information and profile id are required.");
            }
            IEnumerable<ASICustomerCreditCard> oCreditCards = SvcClient.Ctxt.ASICustomerCreditCards
                .Where(c => c.MasterCustomerId == companyInfo.MasterCustomerId
                         && c.SubCustomerId == companyInfo.SubCustomerId
                         && c.CustomerCreditCardProfileId == Convert.ToInt64(profileId));
            ASICustomerCreditCard result = null;
            if (oCreditCards.Any())
            {
                result = oCreditCards.First();
            }
            return result;
        }

        public static PayOrderOutput PayOrderWithCreditCard(
            string orderNumber,
            decimal amount,
            string ccProfileid,
            AddressInfo billToAddressInfo,
            CustomerInfo companyInfo)
        {
            if (billToAddressInfo == null || companyInfo == null)
            {
                throw new ArgumentException("Billto address and company information are required.");
            }
            ASICustomerCreditCard credirCard = GetCreditCardByProfileId(companyInfo, ccProfileid);
            string orderLineNumbers = GetOrderLinesByOrderId(orderNumber);
            var payOrderInput = new PayOrderInput()
            {
                OrderNumber = orderNumber,
                OrderLineNumbers = orderLineNumbers,
                Amount = amount,
                AcceptPartialPayment = true,
                CurrencyCode = "USD",
                MasterCustomerId = companyInfo.MasterCustomerId,
                SubCustomerId = Convert.ToInt16(companyInfo.SubCustomerId),
                BillMasterCustomerId = companyInfo.MasterCustomerId,
                BillSubCustomerId = Convert.ToInt16(companyInfo.SubCustomerId),
                BillingAddressStreet = billToAddressInfo.Address1,
                BillingAddressCity = billToAddressInfo.City,
                BillingAddressState = billToAddressInfo.State,
                BillingAddressCountryCode = billToAddressInfo.CountryCode,
                BillingAddressPostalCode = billToAddressInfo.PostalCode,
                UseCreditCardOnFile = true,
                CCProfileId = ccProfileid,
                CompanyNumber = credirCard.UserDefinedCompanyNumber
            };
            var resp = SvcClient.Post<PayOrderOutput>("PayOrder", payOrderInput);
            return resp;
        }

        public static string GetOrderLinesByOrderId(string orderId)
        {
            IEnumerable<OrderDetailInfo> oOrderLines =
                SvcClient.Ctxt.OrderDetailInfos.Where(c => c.OrderNumber == orderId).ToList();
            string result = null;
            if (oOrderLines.Any())
            {
                result = string.Join(",", oOrderLines.Select(o => o.OrderLineNumber));
            }
            return result;
        }
        #endregion Credit Card Handling
    }

    public enum AddressType
    {
        Primary,
        Shipping,
        Billing
    }
}