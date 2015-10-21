using asi.asicentral.interfaces;
using asi.asicentral.model;
using asi.asicentral.model.personify;
using asi.asicentral.model.store;
using asi.asicentral.oauth;
using asi.asicentral.PersonifyDataASI;
using asi.asicentral.util.store;
using asi.asicentral.util.store.companystore;
using PersonifySvcClient;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace asi.asicentral.services.PersonifyProxy
{
	public enum AddressType
	{
		Primary,
		Shipping,
		Billing
	}
	
	public static class PersonifyClient
    {
        private static ILogService _log = LogService.GetLog(typeof(PersonifyClient));

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
        private const string CUSTOMER_INFO_STATUS_DUPLICATE = "DUPL";
        private const int PHONE_NUMBER_LENGTH = 10;
        private const string DNS_FLAG_TAG = "USR_DNS_FLAG";
        private const string SP_SEARCH_BY_CUSTOMER_ID = "USR_TEST_CUSTOMER_SEARCH_PROC";
        private const string SP_SEARCH_BY_ASI_NUMBER = "USR_EASI_CUSTOMER_SEARCH_ASI_NO_PROC";
        private const string SP_SEARCH_BY_COMPANY_NAME = "USR_EASI_CUSTOMER_SEARCH_COMPANY_NAME_PROC";
        private const string SP_SEARCH_BY_COMMUNICATION = "USR_EASI_CUSTOMER_SEARCH_COMMUNICATION_PROC";
        private const string SP_SEARCH_BY_COMPANY_IDENTIFIER = "USR_EASI_CUSTOMER_SEARCH_CUSTOMER_NO_PROC";
        private const string SP_UPDATE_CUSTOMER_CLASS = "USR_EASI_CUSTOMER_UPDATE_CLASS";

		private static readonly IDictionary<string, string> ASICreditCardType = new Dictionary<string, string>(4, StringComparer.InvariantCultureIgnoreCase) { { "AMEX", "AMEX" }, { "DISCOVER", "DISCOVER" }, { "MASTERCARD", "MC" }, { "VISA", "VISA" } };
		private static readonly IDictionary<string, string> ASIShowCreditCardType = new Dictionary<string, string>(4, StringComparer.InvariantCultureIgnoreCase) { { "AMEX", "SHOW AE" }, { "DISCOVER", "SHOW DISC" }, { "MASTERCARD", "SHOW MS" }, { "VISA", "SHOW VS" } };
		private static readonly IDictionary<string, string> ASICanadaCreditCardType = new Dictionary<string, string>(4, StringComparer.InvariantCultureIgnoreCase) { { "AMEX", "CAN AMEX" }, { "DISCOVER", "CAN DISC" }, { "MASTERCARD", "CAN MC" }, { "VISA", "CAN VISA" } };
		private static readonly IDictionary<string, IDictionary<string, string>> CreditCardType = new Dictionary<string, IDictionary<string, string>>(3, StringComparer.InvariantCultureIgnoreCase) { { "ASI", ASICreditCardType }, { "ASI Show", ASIShowCreditCardType }, { "ASI Canada", ASICanadaCreditCardType } };
		private static readonly IDictionary<string, string> CompanyNumber = new Dictionary<string, string>(3, StringComparer.InvariantCultureIgnoreCase) { { "ASI", "1" }, { "ASI Show", "2" }, { "ASI Canada", "4" } };
        private static readonly IDictionary<Activity, IList<string>> ActivityCodes = new Dictionary<Activity, IList<string>>() { { Activity.Exception, new List<string>(){ "EXCEPTION", "VALIDATION" } }, 
                                                                                                                                 { Activity.Order, new List<string>(){ "ACTIVITY", "ORDER" } } };
        private static readonly IDictionary<string, List<string>> PERSONIFY_STORED_PROCEDURE = new Dictionary<string, List<string>>()
        {
            {SP_SEARCH_BY_CUSTOMER_ID, new List<string>() { "@ip_master_customer_id", "@ip_sub_customer_id" }},
            {SP_SEARCH_BY_ASI_NUMBER, new List<string>() { "@ip_asi_number" }},
            {SP_SEARCH_BY_COMPANY_NAME, new List<string>() { "@ip_label_name" }},
            {SP_SEARCH_BY_COMMUNICATION, new List<string>() { "@ip_search_phone_address" }},
            {SP_SEARCH_BY_COMPANY_IDENTIFIER, new List<string>() { "@ip_customer_number" }},
            {SP_UPDATE_CUSTOMER_CLASS, new List<string>(){"@ip_master_customer_id", "@ip_sub_customer_id", 
                                                          "@upd_class_code", "@upd_sub_class", "@upd_user" }}
        };

        public static CreateOrderOutput CreateOrder(StoreOrder storeOrder,
            string companyMasterCustomerId,
            int companySubCustomerId,
            string contactMasterCustomerId,
            int contactSubCustomerId,
            long billToAddressId,
            long shiptoAddressId,
            IList<CreateOrderLineInput> lineItems)
        {
            if (string.IsNullOrEmpty(companyMasterCustomerId) || string.IsNullOrEmpty(contactMasterCustomerId))
                throw new ArgumentException("You need to pass the company information and contact information");
            if (lineItems == null || !lineItems.Any())
            {
                throw new Exception("lineItems can't be null.");
            }
            var orderLineInputs = new DataServiceCollection<CreateOrderLineInput>(null, TrackingMode.None);
            foreach (var lineItem in lineItems)
                orderLineInputs.Add(lineItem);

            var createOrderInput = new CreateOrderInput()
            {
                BillMasterCustomerID = companyMasterCustomerId,
                BillSubCustomerID = Convert.ToInt16(companySubCustomerId),
                BillAddressID = Convert.ToInt32(billToAddressId),
                ShipMasterCustomerID = contactMasterCustomerId,
                ShipSubCustomerID = Convert.ToInt16(contactSubCustomerId),
                ShipAddressID = Convert.ToInt32(shiptoAddressId),
                OrderLines = orderLineInputs,
                AddedOrModifiedBy = ADDED_OR_MODIFIED_BY,
            };
            var orderOutput = SvcClient.Post<CreateOrderOutput>("CreateOrder", createOrderInput);
            return orderOutput;
        }

        public static void CreateBundleOrder(StoreOrder storeOrder, PersonifyMapping mapping, CompanyInformation companyInfo,                                             
                                             string contactMasterCustomerId, int contactSubCustomerId, 
                                             AddressInfo billToAddress, AddressInfo shipToAddress,
                                             bool waiveAppFee, bool firstMonthFree)
        {          
            _log.Debug(string.Format("CreateBundleOrder - start: order {0} ", storeOrder));
            DateTime startTime = DateTime.Now;

            if( mapping == null)
            {
                throw new Exception("Error getting personify bundle in mapping table");
            }
            else if (storeOrder == null || string.IsNullOrEmpty(companyInfo.MasterCustomerId) ||
                     string.IsNullOrEmpty(contactMasterCustomerId) || billToAddress == null || shipToAddress == null)
            {
                throw new Exception("Error processing personify bunddle order, one of the parameters is null!");
            }

            UpdatePersonifyCompany(companyInfo, mapping);

            // create bundle
            var bundleOrderInput = new ASICreateBundleOrderInput()
            {
                ShipMasterCustomerID = contactMasterCustomerId,
                ShipSubCustomerID = (short)contactSubCustomerId,
                ShipAddressID = (int)shipToAddress.CustomerAddressId,
                ShipAddressTypeCode = "CORPORATE",
                BillMasterCustomerID = companyInfo.MasterCustomerId,
                BillSubCustomerID = (short)companyInfo.SubCustomerId,
                BillAddressID = (int)billToAddress.CustomerAddressId,
                BillAddressTypeCode = "CORPORATE",
                RateStructure = mapping.PersonifyRateStructure,

                RateCode = mapping.PersonifyRateCode, //"FY_DISTMEM", "FP_ESPPMDLMORD14", //
                BundleGroupName = mapping.PersonifyBundle //"DIST_MEM", "ESPP-MD-LM-ORD" //
            };

            var bOutput = SvcClient.Post<ASICreateBundleOrderOutput>("ASICreateBundleOrder", bundleOrderInput);
            storeOrder.BackendReference = bOutput.ASIBundleOrderNumber;

            //payment schedule for bundle line items
            if (!firstMonthFree)
            {
                var orderLineItems = SvcClient.Ctxt.OrderDetailInfos
                                                   .Where(c => c.OrderNumber == bOutput.ASIBundleOrderNumber && c.BaseTotalAmount > 0)
                                                   .ToList();
                if (orderLineItems.Any())
                {
                    var item = orderLineItems[0];
                    var iPaySchedual = new ASICreatePayScheduleInput()
                    {
                        OrderNumber = item.OrderNumber,
                        OrderLineNumber = (short)item.RelatedLineNumber,
                        PayFrequency = "MONTHLY",
                        PayStartDate = DateTime.Now,
                        PayMethodCode = "CC",
                        CCProfileId = Int32.Parse(storeOrder.CreditCard.ExternalReference),
                        SyncPayScheduleFlag = true
                    };

                    SvcClient.Post<ASICreatePayScheduleOutput>("ASICreatePaySchedule", iPaySchedual);
                }
            }

            //add membership application fee
            var classCode = mapping.ClassCode;
            if( !string.IsNullOrEmpty(classCode) && !string.IsNullOrEmpty(mapping.SubClassCode) && mapping.SubClassCode == "DECORATOR")
            {
                classCode = classCode.ToUpper();
                if (classCode == "DISTRIBUTOR" || (classCode == "SUPPLIER" && !string.IsNullOrEmpty(companyInfo.ASINumber) && companyInfo.ASINumber.Length < 8) )
                {  // existing supplier/distributor membership
                    classCode = "DECORATOR";
                }
            }

            if (Helper.APPLICATION_FEE_IDS.Keys.Contains(classCode))
            {
                long? applicationFeeId = Helper.APPLICATION_FEE_IDS[classCode];
                var linePriceInput = new ASIAddOrderLinewithPriceInput()
                {
                    OrderNumber = bOutput.ASIBundleOrderNumber,
                    ProductID = applicationFeeId,
                    Quantity = 1,
                    UserDefinedBoltOn = true,
                    RateStructure = "MEMBER",
                    RateCode = waiveAppFee ? "SPECIAL" : "STD"
                };

                SvcClient.Post<OrderNumberParam>("ASIAddOrderLinewithPrice", linePriceInput);

                //one time application fee payment if not waived
                if (!waiveAppFee)
                {
                    var appFeeLines = SvcClient.Ctxt.OrderDetailInfos.Where(c => c.OrderNumber == linePriceInput.OrderNumber &&
                                                                                 c.ProductId == applicationFeeId).ToList();
                    if (appFeeLines.Any() && appFeeLines[0].BaseTotalAmount > 0)
                    {
                        ASICustomerCreditCard creditCard = GetCreditCardByProfileId(companyInfo.MasterCustomerId,
                                                                                    companyInfo.SubCustomerId,
                                                                                    storeOrder.CreditCard.ExternalReference);

                        var payOrderInput = new PayOrderInput()
                        {
                            OrderNumber = linePriceInput.OrderNumber,
                            OrderLineNumbers = appFeeLines[0].OrderLineNumber.ToString(),
                            Amount = appFeeLines[0].ActualTotalAmount,
                            AcceptPartialPayment = true,
                            CurrencyCode = "USD",
                            MasterCustomerId = companyInfo.MasterCustomerId,
                            SubCustomerId = Convert.ToInt16(companyInfo.SubCustomerId),
                            BillMasterCustomerId = companyInfo.MasterCustomerId,
                            BillSubCustomerId = Convert.ToInt16(companyInfo.SubCustomerId),
                            BillingAddressStreet = billToAddress.Address1,
                            BillingAddressCity = billToAddress.City,
                            BillingAddressState = billToAddress.State,
                            BillingAddressCountryCode = billToAddress.CountryCode,
                            BillingAddressPostalCode = billToAddress.PostalCode,
                            UseCreditCardOnFile = true,
                            CCProfileId = storeOrder.CreditCard.ExternalReference,
                            CompanyNumber = creditCard.UserDefinedCompanyNumber
                        };
                        var resp = SvcClient.Post<PayOrderOutput>("PayOrder", payOrderInput);
                        if (!(resp.Success ?? false))
                        {
                            throw new Exception(resp.ErrorMessage ?? "Error in paying order");
                        }
                    }
                }
            }

            _log.Debug(string.Format("CreateBundleOrder - end: order {0} ({1})", storeOrder, DateTime.Now.Subtract(startTime).TotalMilliseconds));
        }

        public static decimal GetOrderBalanceTotal(string orderNumber)
        {
            decimal total = 0;
            IList<WebOrderBalanceView> oOrdBalInfo = SvcClient.Ctxt.WebOrderBalanceViews
                .Where(o => o.OrderNumber == orderNumber).ToList();
            if (oOrdBalInfo.Any())
            {
                total = oOrdBalInfo.Sum(o => Convert.ToDecimal(o.ActualBalanceAmount));
            }
            return total;
        }

        public static CompanyInformation ReconcileCompany(StoreCompany company, string customerClassCode, IList<LookSendMyAdCountryCode> countryCodes, 
                                                          ref IEnumerable<StoreAddressInfo> storeAddress, bool update = false)
        {
            List<string> masterIdList = null;
            var customerInfo = FindCustomerInfo(company, ref masterIdList);
            if (customerInfo == null)
            {
                customerInfo = CreateCompany(company, customerClassCode, countryCodes);
            }
            else
            {
                company.ExternalReference = customerInfo.MasterCustomerId + ";" + customerInfo.SubCustomerId;
                if (update)
                {
                    StoreAddress companyAddress = company.GetCompanyAddress();
                    string countryCode = countryCodes != null ? countryCodes.Alpha3Code(companyAddress.Country) : companyAddress.Country;
                    AddPhoneNumber(company.Phone, countryCode, customerInfo.MasterCustomerId, customerInfo.SubCustomerId);
                    storeAddress = AddCustomerAddresses(company, customerInfo.MasterCustomerId, customerInfo.SubCustomerId, countryCodes);
                }
            }

            return customerInfo;
        }

        public static CompanyInformation CreateCompany(StoreCompany storeCompany, string storeType, IList<LookSendMyAdCountryCode> countryCodes)
        {
            var startTime = DateTime.Now;
            _log.Debug(string.Format("CreateCompany - start: company name {0}", storeCompany.Name));

            PersonifyCustomerInfo customerInfo = null;
            StoreAddress companyAddress = storeCompany.GetCompanyAddress();
            string countryCode = countryCodes != null ? countryCodes.Alpha3Code(companyAddress.Country) : companyAddress.Country;
            if (!string.IsNullOrEmpty(storeType) && storeType.ToUpper() == "EQUIPMENT")
                storeType = "SUPPLIER";

            var saveCustomerInput = new SaveCustomerInput { LastName = storeCompany.Name, CustomerClassCode = storeType  };
            AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_PHONE, storeCompany.Phone,
                COMMUNICATION_LOCATION_CODE_CORPORATE, countryCode);
            AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_FAX, storeCompany.Fax,
                COMMUNICATION_LOCATION_CODE_CORPORATE, countryCode);
            AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_EMAIL, storeCompany.Email,
                COMMUNICATION_LOCATION_CODE_CORPORATE);
            AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_WEB, storeCompany.WebURL,
                COMMUNICATION_LOCATION_CODE_CORPORATE);

            var result = SvcClient.Post<SaveCustomerOutput>("CreateCompany", saveCustomerInput);
            if (result != null && string.IsNullOrWhiteSpace(result.WarningMessage))
            {
                var subCustomerId = result.SubCustomerId.HasValue ? result.SubCustomerId.Value : 0;
                //try update status, non critical but should be working
                var customers = SvcClient.Ctxt.ASICustomers.Where(
                        p => p.MasterCustomerId == result.MasterCustomerId && p.SubCustomerId == subCustomerId).ToList();
                if (customers.Count > 0)
                {
                    ASICustomer customer = customers[0];
                    customer.UserDefinedMemberStatusString = "ASICENTRAL";
                    SvcClient.Save<ASICustomer>(customer);
                }

                customerInfo = GetPersonifyCompanyInfo(result.MasterCustomerId, subCustomerId);
                storeCompany.ExternalReference = customerInfo.MasterCustomerId + ";" + customerInfo.SubCustomerId;
                AddCustomerAddresses(storeCompany, customerInfo.MasterCustomerId, customerInfo.SubCustomerId, countryCodes);
            }

            _log.Debug(string.Format("CreateCompany - end: ({0})", DateTime.Now.Subtract(startTime).TotalMilliseconds));
            return GetCompanyInfo(customerInfo);
        }

        public static IEnumerable<StoreAddressInfo> AddCustomerAddresses(
            StoreCompany storeCompany,
            string masterCustomerId,
            int subCustomerId,
            IEnumerable<LookSendMyAdCountryCode> countryCodes)
        {
            if (storeCompany == null || storeCompany.Addresses == null || string.IsNullOrEmpty(masterCustomerId) )
            {
                throw new Exception("Store company and addresses, customer personify information and country codes are required");
            }

            var existingAddressInfos = SvcClient.Ctxt.AddressInfos.Where(
                a => a.MasterCustomerId == masterCustomerId && a.SubCustomerId == subCustomerId).ToList();

            var storeCompanyAddresses = ProcessStoreAddresses(storeCompany, countryCodes);
            storeCompanyAddresses = ProcessPersonifyAddresses(storeCompanyAddresses, existingAddressInfos);

            if (storeCompanyAddresses.Any(a => a.PersonifyAddr == null))
            {
                var existingPrimaryAddress = existingAddressInfos.FirstOrDefault(a => a.PrioritySeq == 0);
                if (existingPrimaryAddress == null)
                {
                    storeCompanyAddresses = storeCompanyAddresses.Select(addr =>
                    {
                        if (addr.PrioritySeq.HasValue && addr.PrioritySeq.Value == 0)
                        {
                            AddCustomerAddress(addr, null, masterCustomerId, subCustomerId, storeCompany.Name);
                            existingPrimaryAddress = addr.PersonifyAddr;
                        }
                        return addr;
                    }).ToList();
                }
                storeCompanyAddresses = storeCompanyAddresses.Select(addr =>
                {
                    if (addr.PersonifyAddr == null && !addr.IsAdded)
                        AddCustomerAddress(addr, existingPrimaryAddress, masterCustomerId, subCustomerId, storeCompany.Name);
                    return addr;
                }).ToList();
            }

            return storeCompanyAddresses;
        }

        private static IEnumerable<StoreAddressInfo> ProcessStoreAddresses(StoreCompany storeCompany, IEnumerable<LookSendMyAdCountryCode> countryCodes)
        {
            var storeCompanyAddresses = new List<StoreAddressInfo>(3);
            var primaryAddress = storeCompany.GetCompanyAddress();
            var billToAddress = storeCompany.GetCompanyBillingAddress();
            var shipToAdress = storeCompany.GetCompanyShippingAddress();

            var primaryIsBilling = primaryAddress.Equals(billToAddress);
            var primaryIsShipping = primaryAddress.Equals(shipToAdress);
            var billingIsShipping = billToAddress.Equals(shipToAdress);

            storeCompanyAddresses.Add(new StoreAddressInfo()
            {
                StoreAddr = primaryAddress,
                StoreIsPrimary = true,
                PersonifyIsBilling = primaryIsBilling,
                StoreIsBilling = primaryIsBilling,
                PersonifyIsShipping = primaryIsShipping,
                StoreIsShipping = primaryIsShipping
            });

            if (!primaryIsBilling)
            {
                storeCompanyAddresses.Add(new StoreAddressInfo()
                {
                    StoreAddr = billToAddress,
                    PersonifyIsBilling = true,
                    StoreIsBilling = true,
                    PersonifyIsShipping = billingIsShipping,
                    StoreIsShipping = billingIsShipping
                });
            }

            if (!primaryIsShipping && !billingIsShipping)
            {
                storeCompanyAddresses.Add(new StoreAddressInfo()
                                            {
                                                StoreAddr = shipToAdress,
                                                PersonifyIsShipping = true,
                                                StoreIsShipping = true
                                            });
            }

            return storeCompanyAddresses.Select(a =>
                        {
                            a.CountryCode = countryCodes != null ? countryCodes.Alpha3Code(a.StoreAddr.Country) : a.StoreAddr.Country;
                            return a;
                        });
        }

        private static IEnumerable<StoreAddressInfo> ProcessPersonifyAddresses(
                                                    IEnumerable<StoreAddressInfo> customerAddresses, 
                                                    IList<AddressInfo> existingAddressInfos)
        {
            if (existingAddressInfos.Any(a => a.BillToFlag.HasValue && a.BillToFlag.Value))
            {
                customerAddresses = customerAddresses.Select(a => { a.PersonifyIsBilling = false; return a; }).ToList();
            }
            if (existingAddressInfos.Any(a => a.ShipToFlag.HasValue && a.ShipToFlag.Value))
            {
                customerAddresses = customerAddresses.Select(a => { a.PersonifyIsShipping = false; return a; }).ToList();
            }
            if (!existingAddressInfos.Any() || existingAddressInfos.Any() && existingAddressInfos.All(a => a.PrioritySeq != 0))
            {
                customerAddresses = customerAddresses.Select(a =>
                {
                    if (a.StoreIsPrimary)
                    {
                        a.PrioritySeq = 0;
                        a.PersonifyIsBilling = true;
                        a.PersonifyIsShipping = true;
                    }
                    return a;
                });
            }
            customerAddresses = customerAddresses.Select(addr =>
            {
                addr.PersonifyAddr =
                    existingAddressInfos.FirstOrDefault(
                        a => a.Address1 == addr.StoreAddr.Street1 && a.PostalCode == addr.StoreAddr.Zip);
                return addr;
            });

            return customerAddresses;
        }

        private static SaveAddressOutput AddCustomerAddress(
            StoreAddressInfo storeAddressInfo,
            AddressInfo existingPrimaryAddress,
            string masterCustomerId,
            int subCustomerId,
            string companyName)
        {
            var newCustomerAddress = new SaveAddressInput()
            {
                MasterCustomerId = masterCustomerId,
                SubCustomerId = subCustomerId,
                AddressTypeCode = COMMUNICATION_LOCATION_CODE_CORPORATE,
                Address1 = storeAddressInfo.StoreAddr.Street1,
                Address2 = storeAddressInfo.StoreAddr.Street2,
                City = storeAddressInfo.StoreAddr.City,
                State = storeAddressInfo.StoreAddr.State,
                PostalCode = storeAddressInfo.StoreAddr.Zip,
                CountryCode = storeAddressInfo.CountryCode,
                BillToFlag = storeAddressInfo.PersonifyIsBilling,
                ShipToFlag = storeAddressInfo.PersonifyIsShipping,
                DirectoryFlag = true,
                CompanyName = companyName,
                WebMobileDirectory = false,
                CreateNewAddressIfOrdersExist = true,
                OverrideAddressValidation = true,
                AddedOrModifiedBy = ADDED_OR_MODIFIED_BY,
            };
            if (storeAddressInfo.PrioritySeq.HasValue && storeAddressInfo.PrioritySeq.Value == 0)
            {
                newCustomerAddress.PrioritySeq = storeAddressInfo.PrioritySeq.Value;
            }
            else if (existingPrimaryAddress != null)
            {
                newCustomerAddress.OwnerAddressId = Convert.ToInt32(existingPrimaryAddress.CustomerAddressId);
            }
            var result = SvcClient.Post<SaveAddressOutput>("CreateOrUpdateAddress", newCustomerAddress);
            storeAddressInfo.IsAdded = true;
            storeAddressInfo.PersonifyAddr = SvcClient.Ctxt.AddressInfos.Where(
                   a => a.MasterCustomerId == masterCustomerId
                     && a.SubCustomerId == subCustomerId
                     && a.CustomerAddressId == result.CusAddressId).ToList().FirstOrDefault();
            return result;
        }

        #region Getting company information

        public static CompanyInformation GetCompanyInfoByIdentifier(int companyIdentifier)
        {
            CompanyInformation companyInfo = null;
            var customerInfos = GetCustomerInfoFromSP(SP_SEARCH_BY_COMPANY_IDENTIFIER, new List<string>() { companyIdentifier.ToString() });
            if (customerInfos.Any())
            {
                companyInfo = GetCompanyInfo(customerInfos[0]);
            };

            return companyInfo;
        }

        public static string GetCompanyStatus(string masterCustomerId, int subCustomerId)
        {
            string status = string.Empty;
            var asiCustomers = SvcClient.Ctxt.ASICustomers.Where(c => c.MasterCustomerId == masterCustomerId && c.SubCustomerId == subCustomerId).ToList();
            if( asiCustomers.Any() )
                status = asiCustomers.ElementAt(0).UserDefinedMemberStatusString;

            return status;
        }

        public static string GetCompanyAsiNumber(string masterCustomerId, int subCustomerId)
        {
            var company = GetPersonifyCompanyInfo(masterCustomerId, subCustomerId);
            return company != null ? company.AsiNumber : string.Empty;
        }

        public static CompanyInformation GetCompanyInfo(PersonifyCustomerInfo customerInfo)
        {
            CompanyInformation company = null;
            if (customerInfo != null)
            {
                company = new CompanyInformation
                {
                    ASINumber = customerInfo.AsiNumber,
                    Name = customerInfo.LabelName,
                    MasterCustomerId = customerInfo.MasterCustomerId,
                    SubCustomerId = customerInfo.SubCustomerId,
                    MemberType = customerInfo.CustomerClassCode,
                    CustomerClassCode = customerInfo.CustomerClassCode,
                    SubClassCode = customerInfo.SubClassCode,
                    MemberStatus = customerInfo.MemberStatus,
                    Phone = customerInfo.PrimaryPhone,
                    DNSFlag = customerInfo.DNSFlag,
                    CompanyId = customerInfo.CustomerNumber
                };

                if (!string.IsNullOrEmpty(company.ASINumber) && company.MemberType == "SUPPLIER" &&
                    int.Parse(company.ASINumber) > 10000 && int.Parse(company.ASINumber) < 20000)
                {
                    company.MemberType = "EQUIPMENT";
                }

                //get the company primary address
                var companyAddressInfos = SvcClient.Ctxt.AddressInfos.Where(
                   a => a.MasterCustomerId == customerInfo.MasterCustomerId && a.SubCustomerId == customerInfo.SubCustomerId && a.PrioritySeq == 0).ToList();
                if (companyAddressInfos.Count > 0)
                {
                    var address = companyAddressInfos[0];
                    company.Street1 = address.Address1;
                    company.Street2 = address.Address2;
                    company.State = address.State;
                    company.City = address.City;
                    company.Country = address.CountryCode;
                    company.Zip = address.PostalCode;
                }
            }

            return company;
        }

        public static PersonifyCustomerInfo GetPersonifyCompanyInfo(string masterCustomerId, int subCustomerId)
        {
            var customerInfos = GetCustomerInfoFromSP(SP_SEARCH_BY_CUSTOMER_ID, new List<string>() { masterCustomerId, subCustomerId.ToString() });
            return customerInfos.Any() ? customerInfos.FirstOrDefault(c => c.RecordType == RECORD_TYPE_CORPORATE) : null;
        }

		public static PersonifyCustomerInfo GetCompanyInfoByASINumber(string asiNumber)
		{
            var customerInfos = GetCustomerInfoFromSP(SP_SEARCH_BY_ASI_NUMBER, new List<string>() { asiNumber });
            return customerInfos.Any() ? customerInfos[0]: null;
		}

        public static CompanyInformation FindCustomerInfo(StoreCompany company, ref List<string> matchList)
	    {
            var startTime = DateTime.Now;
            PersonifyCustomerInfo companyInfo = null;
            _log.Debug(string.Format("FindCustomerInfo - start: company {0} ", company.Name));
            if (company == null || string.IsNullOrWhiteSpace(company.Name)) throw new Exception("Store company is not valid.");
			if (!string.IsNullOrEmpty(company.ExternalReference))
			{
                // company exists in personify
                if ( company.HasExternalReference() )
                {
				    string[] references = company.ExternalReference.Split(';');
				    int subCustomerId = Int32.Parse(references[1]);
                    companyInfo = GetPersonifyCompanyInfo(references[0], subCustomerId);
                }
			}
			else if (!string.IsNullOrEmpty(company.ASINumber))
			{  //look company by ASI#
                companyInfo = GetCompanyInfoByASINumber(company.ASINumber);
			}
            else            
            {  // find matching company by phone, email or name
                companyInfo = FindMatchingCompany(company, ref matchList);
            }
            _log.Debug(string.Format("FindCustomerInfo - end: ({0})", DateTime.Now.Subtract(startTime).TotalMilliseconds));

            return companyInfo != null ? GetCompanyInfo(companyInfo) : null;
	    }
	    #endregion Getting company information

        public static void AddActivity(StoreCompany company, string activityText, Activity activityType)
        {
            if (!string.IsNullOrEmpty(company.ExternalReference))
            {
                string[] references = company.ExternalReference.Split(';');
                int subCustomerId = Int32.Parse(references[1]);
                var masterCustomerId = references[0];
                var curActivity = ActivityCodes.Keys.Contains(activityType) ? ActivityCodes[activityType] : ActivityCodes[Activity.Exception];
               
                var activity = SvcClient.Create<CusActivity>();
                activity.MasterCustomerId = masterCustomerId;
                activity.SubCustomerId = subCustomerId;
                activity.ActivityCode = "CONTACTTRACKING";
                activity.CallTopicCode = curActivity[0];
                activity.CallTopicSubcode = curActivity[1];
                activity.CallTypeCode = "STORE"; 
                activity.ActivityDate = DateTime.Now;
                activity.Subsystem = "MRM";
                activity.ResolvedFlag = true;
                activity.ResolvedDate = DateTime.Now;

                if (!string.IsNullOrEmpty(company.MatchingCompanyIds))
                {
                    activity.ActivityText = string.Format("{0} We also potentially matched the operation with those other companies: {1}", activityText, company.MatchingCompanyIds);
                }
                else
                    activity.ActivityText = activityText;

                SvcClient.Save<CusActivity>(activity);
            }
        }

        #region matching company with name, email or phone
        private static PersonifyCustomerInfo FindMatchingCompany(StoreCompany company, ref List<string> matchIdList)
        {
            var startTime = DateTime.Now;
            _log.Debug(string.Format("FindMatchingCompany - start: company name {0} ", company.Name));
            
            PersonifyCustomerInfo matchCompany = null;
            List<PersonifyCustomerInfo> nameMatchList = null;
            var phoneMatchList = new List<PersonifyCustomerInfo>();
            var emailMatchList = new List<PersonifyCustomerInfo>();
            var matchCompanyList = new List<PersonifyCustomerInfo>();

            var primaryContact = company.Individuals.Where(c => c.IsPrimary && !string.IsNullOrEmpty(c.Email)).FirstOrDefault();
            var email = primaryContact != null ? primaryContact.Email.Trim() : string.Empty;
            var phoneFilter = string.IsNullOrEmpty(company.Phone) ? string.Empty : Regex.Replace(company.Phone, "[^0-9.]", "");
            bool isSupplier = string.Equals(company.MemberType, "SUPPLIER", StringComparison.InvariantCultureIgnoreCase) ||
                              string.Equals(company.MemberType, "EQUIPMENT", StringComparison.InvariantCultureIgnoreCase);

            var tasks = new Task[3]
            {
                Task.Factory.StartNew(() => MatchCompanyName(company, out nameMatchList)),
                Task.Factory.StartNew(() => MatchPhoneEmail(phoneFilter, ref phoneMatchList)),
                Task.Factory.StartNew(() => MatchPhoneEmail(email, ref emailMatchList))
            };

            // Find company matching name
            Task.WaitAny(tasks[0]);
            if (nameMatchList != null && nameMatchList.Count == 1 && isSupplier)
            {
                matchCompany = nameMatchList[0];
            }
            else
            {
                // Get company matching phone/email
                Task.WaitAll(tasks[1], tasks[2]);
                var matchPhoneOrEmail = phoneMatchList.Union(emailMatchList);

                if (isSupplier)
                {
                    if (nameMatchList != null && nameMatchList.Count > 1)
                    {
                        // find companies match both name and phone/email
                        if (matchPhoneOrEmail.Count() > 0)
                        {
                            var matchBoth = nameMatchList.Intersect(matchPhoneOrEmail);
                            if (matchBoth.Count() > 0)
                            {
                                matchCompanyList.AddRange(matchBoth);
                            }
                        }

                        // no company matches both, get match-name list only
                        if (matchCompanyList.Count < 1)
                            matchCompanyList.AddRange(nameMatchList);
                    }
                    else if (matchPhoneOrEmail.Count() > 0)
                        matchCompanyList.AddRange(matchPhoneOrEmail);
                }
                else
                {  // non-supplier: match Name and Phone or Name and Email first, otherwise match phone and email
                    if (nameMatchList != null && nameMatchList.Count > 0 && matchPhoneOrEmail.Count() > 0)
                    {
                        var matchBoth = nameMatchList.Intersect(matchPhoneOrEmail);
                        if (matchBoth.Count() > 0)
                        {
                            matchCompanyList.AddRange(matchBoth);
                        }
                    }

                    // no match found through name, match phone and email
                    if (matchCompanyList.Count < 1 && phoneMatchList.Count > 0 && emailMatchList.Count > 0)
                    {
                        var matchPhoneAndEmail = phoneMatchList.Intersect(emailMatchList);
                        if (matchPhoneAndEmail.Count() > 0)
                            matchCompanyList.AddRange(matchPhoneAndEmail);
                    }
                }
            }

            if (matchCompany == null && matchCompanyList.Count > 0)
            {
                matchCompany = GetCompanyWithLatestNote(matchCompanyList);

                if( matchIdList == null )
                    matchIdList = new List<string>();

                matchIdList.AddRange(matchCompanyList.Select(m => m.MasterCustomerId));
                matchIdList.Remove(matchCompany.MasterCustomerId);
                matchIdList = matchIdList.Distinct().ToList();
            }

            _log.Debug(string.Format("FindMatchingCompany - end: ({0})", DateTime.Now.Subtract(startTime).TotalMilliseconds));

            return matchCompany;
        }

        private static void MatchCompanyName(StoreCompany company, out List<PersonifyCustomerInfo> companyList)
        {
            var startTime = DateTime.Now;
            _log.Debug(string.Format("MatchCompanyName - start: company name {0}", company.Name));

            companyList = GetCustomerInfoFromSP(SP_SEARCH_BY_COMPANY_NAME, new List<string>() { company.Name });

            if (companyList.Count < 1)
            {
                var nameWithoutSpecialChars = IgnoreSpecialChars(company.Name);
                if( nameWithoutSpecialChars != company.Name )
                    companyList = GetCustomerInfoFromSP(SP_SEARCH_BY_COMPANY_NAME, new List<string>() { nameWithoutSpecialChars });
            }

            _log.Debug(string.Format("MatchCompanyName - end: ({0})", DateTime.Now.Subtract(startTime).TotalMilliseconds));
        }

        private static void MatchPhoneEmail(string filter, ref List<PersonifyCustomerInfo> companyList)
        {
            var startTime = DateTime.Now;
            _log.Debug(string.Format("MatchPhoneEmail - start: filter {0}", filter));

            if( companyList == null )
                companyList = new List<PersonifyCustomerInfo>();

            if (!string.IsNullOrEmpty(filter))
            {
                var matchList = GetCustomerInfoFromSP(SP_SEARCH_BY_COMMUNICATION, new List<string>() { filter });

                var condition = string.Empty;
                foreach (var match in matchList)
                {
                    if (match.RecordType == RECORD_TYPE_CORPORATE)
                    {
                        companyList.Add(match);
                    }
                    else
                    { // get company from contact phone/email
                        if (condition == string.Empty)
                            condition = string.Format("(SubCustomerId eq 0 and RelatedMasterCustomerId eq '{0}' and RelatedSubCustomerId eq {1} and RelationshipType eq 'EMPLOYMENT')",
                                                        match.MasterCustomerId, match.SubCustomerId);
                        else
                            condition = string.Format("{0} or (SubCustomerId eq 0 and RelatedMasterCustomerId eq '{1}' and RelatedSubCustomerId eq {2} and RelationshipType eq 'EMPLOYMENT')",
                                                       condition, match.MasterCustomerId, match.SubCustomerId);
                    }
                }

                if (condition != string.Empty)
                {
                    var cusRelations = SvcClient.Ctxt.CusRelationships.AddQueryOption("$filter", condition);
                    foreach (var r in cusRelations)
                    {
                        matchList = GetCustomerInfoFromSP(SP_SEARCH_BY_CUSTOMER_ID, new List<string>() { r.MasterCustomerId, r.SubCustomerId.ToString() });
                        companyList.AddRange(matchList.FindAll(m => m.RecordType == RECORD_TYPE_CORPORATE));
                    }
                }
            }

            _log.Debug(string.Format("MatchPhoneEmail - end: ({0})", DateTime.Now.Subtract(startTime).TotalMilliseconds));
        }

        private static PersonifyCustomerInfo GetCompanyWithLatestNote(List<PersonifyCustomerInfo> matchCompanyList)
        {
            _log.Debug("GetCompanyWithLatestNote - start: ");
            var startTime = DateTime.Now;

            PersonifyCustomerInfo companyInfo = null;
            if (matchCompanyList != null && matchCompanyList.Count > 0)
            {
                if (matchCompanyList.Count == 1)
                {
                    companyInfo = matchCompanyList[0];
                }
                else
                {
                    var searchList = matchCompanyList;
                    var leadCompanys = matchCompanyList.FindAll(m => m.MemberStatus != null && (m.MemberStatus.ToUpper() == "ASICENTRAL" || m.MemberStatus.ToUpper() == "LEAD"));

                    if (leadCompanys.Count > 1)
                    {  // find company from Lead companys only
                        searchList = leadCompanys;
                    }
                    else if ( leadCompanys.Count == 1)
                    {  // one lead company, no more searching
                        companyInfo = leadCompanys[0];
                    }

                    if (companyInfo == null && searchList.Count > 0 )
                    {
                        string condition = null;
                        foreach (var com in searchList)
                        {
                            if (condition == null)
                                condition = string.Format("(MasterCustomerId eq '{0}' and SubCustomerId eq 0)", com.MasterCustomerId);
                            else
                                condition = string.Format("{0} or (MasterCustomerId eq '{1}' and SubCustomerId eq 0)",
                                                           condition, com.MasterCustomerId);
                        }

                        var cusActivities = SvcClient.Ctxt.CusActivities.AddQueryOption("$filter", condition).ToList();
                        var result = cusActivities.OrderByDescending(c => c.ActivityDate).FirstOrDefault();
                        if (result != null)
                        {
                            companyInfo = searchList.Find(m => m.MasterCustomerId == result.MasterCustomerId);
                        }
                        else
                        { // none have activites, get the latest one
                            companyInfo = searchList.OrderByDescending(c => c.MasterCustomerId).FirstOrDefault();
                        }
                    }
                }
            }

            _log.Debug(string.Format("GetCompanyWithLatestNote - end: ({0})", DateTime.Now.Subtract(startTime).TotalMilliseconds));
            return companyInfo;
        }

        private static string IgnoreSpecialChars(string input)
        {
            return Regex.Replace(input.Trim(), @"[\.\$@&#\?,!]*", "");
        }

        private static void UpdatePersonifyCompany(CompanyInformation companyInfo, PersonifyMapping mapping)
        {
            // update company status from Delisted to Active
            if (companyInfo.MemberStatus == StatusCode.DELISTED.ToString())
            {
                var customers = SvcClient.Ctxt.ASICustomers.Where(
                        p => p.MasterCustomerId == companyInfo.MasterCustomerId && p.SubCustomerId == 0).ToList();
                if (customers.Count > 0)
                {
                    ASICustomer customer = customers[0];
                    if (customer.UserDefinedMemberStatusString == StatusCode.DELISTED.ToString())
                    {
                        customer.UserDefinedMemberStatusString = StatusCode.ACTIVE.ToString();
                        SvcClient.Save<ASICustomer>(customer);
                    }
                }
            }
            // update company class/subclass if they are different from mapping table
            else if (!string.IsNullOrEmpty(companyInfo.MemberStatus) && (companyInfo.MemberStatus.ToUpper() == "LEAD" || companyInfo.MemberStatus.ToUpper() == "ASICENTRAL")) 
            {
                var mappedClassCode = !string.IsNullOrEmpty(mapping.ClassCode) ? mapping.ClassCode.Trim().ToUpper() : string.Empty;
                var mappedSubClassCode = !string.IsNullOrEmpty(mapping.SubClassCode) ?  mapping.SubClassCode.Trim().ToUpper() : string.Empty;
                var classCode = !string.IsNullOrEmpty(companyInfo.CustomerClassCode) ? companyInfo.CustomerClassCode.Trim().ToUpper() : string.Empty;
                var subClassCode = !string.IsNullOrEmpty(companyInfo.SubClassCode) ? companyInfo.SubClassCode.Trim().ToUpper() : string.Empty;

                if( mappedClassCode != classCode || ( mappedClassCode == "SUPPLIER" && mappedSubClassCode != subClassCode ) )
                {
                    ExecutePersonifySP(SP_UPDATE_CUSTOMER_CLASS, new List<string>{ companyInfo.MasterCustomerId,
                                                                                   companyInfo.SubCustomerId.ToString(),
                                                                                   mapping.ClassCode,
                                                                                   mapping.SubClassCode,
                                                                                   "WEBUSER"});

                }
            }
        }

        private static StoredProcedureOutput ExecutePersonifySP(string spName, List<string> parameters)
        {
            _log.Debug(string.Format("ExecutePersonifySP - start: StoreProcedure name - {0})", spName));
            var startTime = DateTime.Now;

            StoredProcedureOutput response = null;

            if (PERSONIFY_STORED_PROCEDURE.Keys.Contains(spName))
            {
                var spParams = PERSONIFY_STORED_PROCEDURE[spName];
                if (spParams.Count == parameters.Count)
                {
                    var ipSPParameterList = new DataServiceCollection<StoredProcedureParameter>(null, TrackingMode.None);

                    for (int i = 0; i < parameters.Count; i++)
                    {
                        var parameter = new StoredProcedureParameter() { Name = spParams[i], Value = parameters[i], Direction = 1 };
                        ipSPParameterList.Add(parameter);
                    }

                    var spRequest = new StoredProcedureRequest()
                    {
                        StoredProcedureName = spName,
                        IsUserDefinedFunction = false,
                        SPParameterList = ipSPParameterList
                    };

                    response = SvcClient.Post<StoredProcedureOutput>("GetStoredProcedureDataXML", spRequest);
                }
            }

            _log.Debug(string.Format("ExecutePersonifySP - StoreProcedure end: ({0})", DateTime.Now.Subtract(startTime).TotalMilliseconds));

            return response;
        }

        private static List<PersonifyCustomerInfo> GetCustomerInfoFromSP(string spName, List<string> parameters)
        {
            var companyInfoList = new List<PersonifyCustomerInfo>();

            var response = ExecutePersonifySP(spName, parameters);
            if (response != null && !string.IsNullOrEmpty(response.Data) && response.Data != "No Data Found")
            {
                var xml = XDocument.Parse(response.Data);

                var list = xml.Root.Elements("Table").ToList();
                foreach (var com in list)
                {
                    var customerInfo = new PersonifyCustomerInfo();
                    foreach (var element in com.Elements())
                    {
                        var value = element.Value;
                        switch (element.Name.ToString())
                        {
                            case "USR_ASI_NUMBER":
                                customerInfo.AsiNumber = value;
                                break;
                            case "USR_CUSTOMER_NUMBER":
                                customerInfo.CustomerNumber = Int32.Parse(value);
                                break;
                            case "MASTER_CUSTOMER_ID":
                                customerInfo.MasterCustomerId = value;
                                break;
                            case "SUB_CUSTOMER_ID":
                                customerInfo.SubCustomerId = Int32.Parse(value);
                                break;
                            case "RECORD_TYPE":
                                customerInfo.RecordType = value;
                                break;
                            case "LAST_NAME":
                                customerInfo.LastName = value;
                                break;
                            case "FIRST_NAME":
                                customerInfo.FirstName = value;
                                break;
                            case "LABEL_NAME":
                                customerInfo.LabelName = value;
                                break;
                            case "CUSTOMER_STATUS_CODE":
                                customerInfo.CustomerStatusCode = value;
                                break;
                            case "USR_MEMBER_STATUS":
                                customerInfo.MemberStatus = value;
                                break;
                            case "PRIMARY_EMAIL_ADDRESS":
                                customerInfo.PrimaryEmail = value;
                                break;
                            case "PRIMARY_PHONE":
                                customerInfo.PrimaryPhone = value;
                                break;
                            case "CUSTOMER_CLASS_CODE":
                                customerInfo.CustomerClassCode = value;
                                break;
                            case "USR_SUB_CLASS":
                                customerInfo.SubClassCode = value;
                                break;
                            case "USR_DNS_FLAG":
                                customerInfo.DNSFlag = value.StartsWith("Y");
                                break;
                        }
                    }

                    if (customerInfo.RecordType == null || customerInfo.RecordType != RECORD_TYPE_CORPORATE ||
                        customerInfo.CustomerStatusCode == null || customerInfo.CustomerStatusCode != CUSTOMER_INFO_STATUS_DUPLICATE)
                    {
                        companyInfoList.Add(customerInfo);
                    }
                }
            }

            return companyInfoList;
        }

  	    #endregion matching company

        public static IEnumerable<PersonifyCustomerInfo> AddIndividualInfos(StoreCompany storeCompany, 
                                                                      IList<LookSendMyAdCountryCode> countryCodes,
                                                                      string companyMasterId, int companySubId)
        {
            if (storeCompany == null || storeCompany.Individuals == null)
            {
                throw new Exception("Company and compnay contact can't be null.");
            }
            if (string.IsNullOrEmpty(companyMasterId))
            {
                throw new Exception("Company information is needed.");
            }

            StoreAddress companyAddress = storeCompany.GetCompanyAddress();
            if (companyAddress == null)
            {
                throw new Exception("Company address is required");
            }
            string countryCode = countryCodes != null ? countryCodes.Alpha3Code(companyAddress.Country) : companyAddress.Country;
            if (string.IsNullOrEmpty(countryCode))
            {
                throw new Exception("Country code is required");
            }
            else if (countryCode.Trim().Length != 3)
            {
                throw new Exception("Invalid Country code: " + countryCode);
            }
            var allCustomers = new List<PersonifyCustomerInfo>();

            foreach (var storeIndividual in storeCompany.Individuals)
            {
                //check if individual already exist based on email
                var customerInfo = GetIndividualInfoByEmail(storeIndividual.Email);
                
                //check if contact belong to company
                if (customerInfo != null)
                {
                    try
                    {
                        List<CusRelationship> relationships = SvcClient.Ctxt.CusRelationships
                            .Where(rel => rel.MasterCustomerId == customerInfo.MasterCustomerId
                                          && rel.RelatedMasterCustomerId == companyMasterId
                                          && rel.RelatedSubCustomerId == companySubId).ToList();
                        if (relationships.Count == 0)
                        {
                            //also link this user to the company
                            AddRelationship(customerInfo.MasterCustomerId, customerInfo.SubCustomerId, companyMasterId, companySubId);
                        }
                    }
                    catch (Exception ex)
                    {
                        string s = string.Format("customerInfo.MasterCustomerId = {0}", customerInfo.MasterCustomerId)
                                   + string.Format("\ncompanyInfo.MasterCustomerId = {0}", companyMasterId)
                                   + string.Format("\ncompanyInfo.SubCustomerId = {0}\n", companySubId)
                                   + ex.Message
                                   + ex.StackTrace;
                        ILogService log = LogService.GetLog(typeof(PersonifyClient));
                        log.Error(string.Format("Error in adding individuals: {0}", s));
                        throw new Exception(s, ex);
                    }
                }
                else
                {
                    //check if there is a contact with same name for same company already
                    customerInfo = GetIndividualInfo(storeIndividual.FirstName, storeIndividual.LastName, companyMasterId, companySubId);
                    if (customerInfo == null)
                    {
                        //could not find him, add him
                        var customerInfoInput = new SaveCustomerInput
                        {
                            FirstName = storeIndividual.FirstName,
                            LastName = storeIndividual.LastName,
                            CustomerClassCode = CUSTOMER_CLASS_INDIV,
                        };
                        AddCusCommunicationInput(customerInfoInput, COMMUNICATION_INPUT_PHONE, storeIndividual.Phone, COMMUNICATION_LOCATION_CODE_WORK, countryCode);
                        AddCusCommunicationInput(customerInfoInput, COMMUNICATION_INPUT_EMAIL, storeIndividual.Email, COMMUNICATION_LOCATION_CODE_WORK);
                        var customerInfoOutput = SvcClient.Post<SaveCustomerOutput>("CreateIndividual", customerInfoInput);
                        if (customerInfoOutput != null )
                        {
                            customerInfo = GetIndividualInfo(customerInfoOutput.MasterCustomerId);
                            if (customerInfo != null)
                                AddRelationship(customerInfo.MasterCustomerId, customerInfo.SubCustomerId, companyMasterId, companySubId);
                        }
                    }
                    else
                    {
                        //already there but not with the email specified
                        //@todo add the email?
                        //@todo check the phone?
                    }
                }
                if (customerInfo != null) allCustomers.Add(customerInfo);
            }
            return allCustomers;
        }

        private static void AddRelationship(string contactMasterId, int contactSubId, string companyMasterId, int companySubId)
        {
            if ( string.IsNullOrEmpty(contactMasterId) || string.IsNullOrEmpty(companyMasterId))
            {
                throw new Exception("To add a relation between individual and company, information from both sides is required");
            }
            var cusRelationship = SvcClient.Create<CusRelationship>();
            cusRelationship.AddedBy = ADDED_OR_MODIFIED_BY;
            //Provide values and Save
            cusRelationship.MasterCustomerId = contactMasterId;
            cusRelationship.SubCustomerId = contactSubId;

            cusRelationship.RelationshipType = "EMPLOYMENT";
            cusRelationship.RelationshipCode = "Employee";

            cusRelationship.RelatedMasterCustomerId = companyMasterId;
            cusRelationship.RelatedSubCustomerId = companySubId;
            cusRelationship.ReciprocalCode = "Employer";
            cusRelationship.BeginDate = DateTime.Now.AddDays(-1);
            SvcClient.Save<CusRelationship>(cusRelationship);
        }

        private static PersonifyCustomerInfo GetIndividualInfo(string firstName, string lastName, string companyMasterId, int companySubId)
        {
            PersonifyCustomerInfo customerInfo = null;

            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName)
                && !string.IsNullOrWhiteSpace(companyMasterId))
            {
                List<CusRelationship> oCusRltnshps = SvcClient.Ctxt.CusRelationships
                    .Where(a => a.RelatedName == string.Format("{0}, {1}", lastName, firstName)
                                && a.MasterCustomerId == companyMasterId
                                && a.SubCustomerId == companySubId).ToList();
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

                    customerInfo = GetIndividualInfo(oCusRltnshp.RelatedMasterCustomerId);
                }
            }
            return customerInfo;
        }

        public static PersonifyCustomerInfo GetIndividualInfo(string masterCustomerId, int subCustomerId = 0)
        {
            var individualList = GetCustomerInfoFromSP(SP_SEARCH_BY_CUSTOMER_ID, new List<string>() { masterCustomerId, subCustomerId.ToString() });
            return individualList.Any() ? individualList.Find(c => c.RecordType.StartsWith("I")): null;
        }

        public static PersonifyCustomerInfo GetIndividualInfoByEmail(string emailAddress)
        {
            var individuals = GetCustomerInfoFromSP(SP_SEARCH_BY_COMMUNICATION, new List<string>() { emailAddress });
            PersonifyCustomerInfo customerInfo = individuals.Any() ? customerInfo = individuals.Find(i => i.RecordType.StartsWith(RECORD_TYPE_INDIVIDUAL)) : null;
            return customerInfo;
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

        private static SaveCustomerInput AddCusCommunicationInput(
            SaveCustomerInput customerInfo,
            string key,
            string value,
            string communitionLocationCode,
            string countryCode = null)
        {
            if (customerInfo == null)
            {
                throw new Exception("To add communiaction, customer information is required");
            }
            if (customerInfo == null)
            {
                throw new Exception("To add communiaction, country code is required");
            }
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                if ( key == COMMUNICATION_INPUT_PHONE || key == COMMUNICATION_INPUT_FAX )
                {
                    value = new string(value.Where(Char.IsDigit).ToArray());
                    value = value.Substring(0, Math.Min(value.Length, PHONE_NUMBER_LENGTH));
                    var comm = new CusCommunicationInput()
                        {
                            CommLocationCode = communitionLocationCode,
                            CommTypeCode = key,
                            ActiveFlag = true,
                        };
                    if (value.Length == PHONE_NUMBER_LENGTH && (string.Equals(countryCode, "USA", StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(countryCode, "CAN", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        comm.CountryCode = countryCode;
                        comm.PhoneAreaCode = value.Substring(0, 3);
                        comm.PhoneNumber = value.Substring(3, 7);
                    }
                    else
                    {
                        comm.CountryCode = "[ALL]";
                        comm.PhoneAreaCode = "";
                        comm.PhoneNumber = value;
                    }
                    customerInfo.Communication.Add(comm);
                }
                // (key != COMMUNICATION_INPUT_PHONE || key != COMMUNICATION_INPUT_FAX) is always true, should be &&
                // then we could use "else if"
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

        public static CusCommunication AddPhoneNumber(string phoneNumber, string countryCode, string masterCustomerId, int subCustomerId)
        {
            CusCommunication respSave = null;
            if (string.IsNullOrEmpty(phoneNumber)) return respSave;
            phoneNumber = new string(phoneNumber.Where(Char.IsDigit).ToArray());
            IList<CusCommunication> oCusComms = GetCusCommunications(masterCustomerId, subCustomerId, COMMUNICATION_INPUT_PHONE);
            respSave = oCusComms.FirstOrDefault(c => c.SearchPhoneAddress == phoneNumber);
            if (respSave != null)
            {
                return respSave;
            }
            var phoneNumberTypes = new string[] { "BUSINESS", "CORPORATE" };
            IEnumerable<string> commTypes1 = oCusComms.Where(c => !string.IsNullOrWhiteSpace(c.SearchPhoneAddress))
                                                      .Select(c => c.CommLocationCodeString.ToUpper());
            IEnumerable<string> commTypes2 = phoneNumberTypes.Where(c => !commTypes1.Contains(c.ToUpper())).ToList();
            if (commTypes2.Any())
            {
                try
                {
                    var respCreate = SvcClient.Create<CusCommunication>();
                    respCreate.MasterCustomerId = masterCustomerId;
                    respCreate.SubCustomerId = subCustomerId;
                    respCreate.CommLocationCodeString = commTypes2.First();
                    respCreate.CommTypeCodeString = COMMUNICATION_INPUT_PHONE;
                    respCreate.PrimaryFlag = false;
                    if (!string.IsNullOrWhiteSpace(countryCode)
                        && (string.Equals(countryCode, "USA", StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(countryCode, "CAN", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        if (phoneNumber.Length == PHONE_NUMBER_LENGTH )
                        {
                            bool isPhoneExist = GetCusCommunications(masterCustomerId, subCustomerId, COMMUNICATION_INPUT_PHONE).Any(c => c.SearchPhoneAddress == phoneNumber);
                            if (!isPhoneExist)
                            {
                                respCreate.CountryCode = countryCode;
                                respCreate.PhoneAreaCode = phoneNumber.Substring(0, 3);
                                respCreate.PhoneNumber = phoneNumber.Substring(3, 7);
                            }
                        }
                    }
                    else
                    {
                        respCreate.CountryCode = "[ALL]";
                        respCreate.PhoneAreaCode = "";
                        respCreate.PhoneNumber = phoneNumber;
                    }
                    respSave = SvcClient.Save<CusCommunication>(respCreate);
                }
                catch
                {
                    respSave = null;
                }
            }
            return respSave;
        }

         private static IList<CusCommunication> GetCusCommunications(string masterCustomerId, int subCustomerId, string cusCommType)
        {
            IEnumerable<CusCommunication> cc = SvcClient.Ctxt.CusCommunications
                .Where(c => c.MasterCustomerId == masterCustomerId
                         && c.SubCustomerId == subCustomerId);
            return cc.Where(c => string.Equals(c.CommTypeCodeString, cusCommType, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        #region Credit Card Handling

	    public static IEnumerable<StoreCreditCard> GetCompanyCreditCards(StoreCompany company, string asiCompany)
	    {
		    var creditCards = new List<StoreCreditCard>();
			if (company == null || string.IsNullOrWhiteSpace(company.Name)) throw new Exception("Store company is not valid.");
		    string masterCustomerId = string.Empty;
		    int subCustomerId = 0;

			if (!string.IsNullOrEmpty(company.ExternalReference))
			{
				string[] references = company.ExternalReference.Split(';');
				subCustomerId = Int32.Parse(references[1]);
				masterCustomerId = references[0];
			}
			else
			{
				//look company by ASI#
				if (!string.IsNullOrEmpty(company.ASINumber))
				{
					var customers = SvcClient.Ctxt.ASICustomerInfos.Where(p => p.UserDefinedAsiNumber == company.ASINumber).ToList();
					if (customers.Any())
					{
						masterCustomerId = customers[0].MasterCustomerId;
						subCustomerId = customers[0].SubCustomerId;
					}
				}
			}
		    if (!string.IsNullOrEmpty(masterCustomerId))
		    {
				IEnumerable<ASICustomerCreditCard> personifyCreditCards = SvcClient.Ctxt.ASICustomerCreditCards
								.Where(c => c.MasterCustomerId == masterCustomerId && c.SubCustomerId == subCustomerId && 
									c.UserDefinedCompanyNumber == CompanyNumber[asiCompany]);

			    foreach (var personifyCreditCard in personifyCreditCards.OrderByDescending( cc => cc.AddedOn))
			    {
				    var card = new StoreCreditCard()
				    {
						CardNumber = "****" + personifyCreditCard.CCReference.Substring(personifyCreditCard.CCReference.Length - 4),
						ExternalReference = personifyCreditCard.CustomerCreditCardProfileId.ToString(),
						CardType = personifyCreditCard.ReceiptTypeCodeString,
				    };
					//convert credit card type
				    var creditCardType = ASICreditCardType.Union(ASIShowCreditCardType).Union(ASICanadaCreditCardType).FirstOrDefault(c => c.Value == card.CardType);
				    if (creditCardType.Key != null) card.CardType = creditCardType.Key;
					//make sure we do not add a duplicate card reference
					if (!creditCards.Any(cc => cc.CardNumber == card.CardNumber && cc.CardType == card.CardType))
						creditCards.Add(card);
			    }
		    }
			return creditCards;
	    }

        public static bool ValidateCreditCard(CreditCard info)
        {
			//when validating the credit card we can use ASI for the company
	        string creditCardType = CreditCardType["ASI"][info.Type.ToUpper()];
            var asiValidateCreditCardInput = new ASIValidateCreditCardInput()
            {
                ReceiptType = creditCardType,
                CreditCardNumber = info.Number
            };
            var resp = SvcClient.Post<ASIValidateCreditCardOutput>("ASIValidateCreditCard", asiValidateCreditCardInput);
            return resp.IsValid ?? false;
        }

        public static string SaveCreditCard(string asiCompany, string masterCustomerId, int subCustomerId, CreditCard creditCard)
        {
	        if (string.IsNullOrEmpty(asiCompany)) asiCompany = "ASI";
	        string creditCardType = CreditCardType[asiCompany][creditCard.Type.ToUpper()];
            var customerCreditCardInput = new CustomerCreditCardInput()
            {
                MasterCustomerId = masterCustomerId,
                SubCustomerId = subCustomerId,
                ReceiptType = creditCardType,
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
				CompanyNumber = CompanyNumber[asiCompany],
                AddedOrModifiedBy = ADDED_OR_MODIFIED_BY
            };
            var resp = SvcClient.Post<CustomerCreditCardOutput>("AddCustomerCreditCard", customerCreditCardInput);
            if (!(resp.Success?? false))
            {
                var m = string.Format("Error in saving credit {0} to Personify", GetCreditCardReference(creditCard.Number));
                if (resp.AddCustomerCreditCardVI.Any())
                {
                    m = string.Format("{0}\n{1}", m, resp.AddCustomerCreditCardVI[0].Message);
                }
                throw new Exception(m);
            }
            return resp.CreditCardProfileId;
        }

        public static ASICustomerCreditCard GetCreditCardByProfileId(string masterCustomerId, int subCustomerId, string profileId)
        {
            if( string.IsNullOrEmpty(masterCustomerId) || string.IsNullOrWhiteSpace(profileId))
            {
                throw new Exception("Company information and profile id are required.");
            }
            IEnumerable<ASICustomerCreditCard> oCreditCards = SvcClient.Ctxt.ASICustomerCreditCards
                .Where(c => c.MasterCustomerId == masterCustomerId
                         && c.SubCustomerId == subCustomerId
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
            string masterCustomerId,
            int subCustomerId)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                throw new Exception("Personify order id is null");
            }
            if (string.IsNullOrWhiteSpace(ccProfileid))
            {
                throw new Exception(string.Format("Creadit card profile id is null for order {0}", orderNumber));
            }
            if (billToAddressInfo == null || string.IsNullOrEmpty(masterCustomerId))
            {
                throw new ArgumentException(
                    string.Format("Billto address and company information are required for order {0}", orderNumber));
            }
            ASICustomerCreditCard credirCard = GetCreditCardByProfileId(masterCustomerId, subCustomerId, ccProfileid);
            string orderLineNumbers = GetOrderLinesByOrderId(orderNumber);
            var payOrderInput = new PayOrderInput()
            {
                OrderNumber = orderNumber,
                OrderLineNumbers = orderLineNumbers,
                Amount = amount,
                AcceptPartialPayment = true,
                CurrencyCode = "USD",
                MasterCustomerId = masterCustomerId,
                SubCustomerId = Convert.ToInt16(subCustomerId),
                BillMasterCustomerId = masterCustomerId,
                BillSubCustomerId = Convert.ToInt16(subCustomerId),
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
            if (!(resp.Success ?? false))
            {
                throw new Exception(resp.ErrorMessage ?? "Error in paying order");
            }
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

        private static string GetCreditCardReference(string num)
        {
            string result = string.Empty;
            if (num.Length >= 10)
            {
                result = string.Format("{0}{1}{2}", num.Substring(0, 6),
                    new string(Enumerable.Repeat('*', num.Length - 10).ToArray()),
                    num.Substring(num.Length - 4));
            }
            return result;
        }

        #endregion Credit Card Handling
    }
}