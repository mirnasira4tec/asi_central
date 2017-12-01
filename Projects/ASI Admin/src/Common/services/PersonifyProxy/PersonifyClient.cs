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
using DocumentFormat.OpenXml.Drawing;

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
        private const string COMMUNICATION_LOCATION_CODE_BUSINESS = "BUSINESS";
        private const string COMMUNICATION_LOCATION_CODE_WORK = "WORK";
        private const string CUSTOMER_CLASS_INDIV = "INDIV";
        private const string RECORD_TYPE_INDIVIDUAL = "I";
        private const string RECORD_TYPE_CORPORATE = "C";
        private const string CUSTOMER_INFO_STATUS_DUPLICATE = "DUPL";
        private const int PHONE_NUMBER_LENGTH = 10;
        private const string SP_SEARCH_BY_CUSTOMER_ID = "USR_EASI_CUSTOMER_SEARCH_MASTERCUSTOMER_PROC";
        private const string SP_SEARCH_BY_ASI_NUMBER = "USR_EASI_CUSTOMER_SEARCH_ASI_NO_PROC";
        private const string SP_SEARCH_BY_COMPANY_NAME = "USR_EASI_CUSTOMER_SEARCH_COMPANY_NAME_PROC";
        private const string SP_SEARCH_BY_COMMUNICATION = "USR_EASI_CUSTOMER_SEARCH_COMMUNICATION_PROC";
        private const string SP_SEARCH_BY_COMPANY_IDENTIFIER = "USR_EASI_CUSTOMER_SEARCH_CUSTOMER_NO_PROC";
        private const string SP_UPDATE_CUSTOMER_CLASS = "USR_EASI_CUSTOMER_UPDATE_CLASS";
        private const string SP_GET_BUNDLE_PRODUCT_DETAILS = "ASI_GetBundleProductDetails_SP";
	    private const string SP_GET_PRODUCT_DETAILS = "USR_PRODUCT_VALIDATION_PROC";
        private const string SP_EEX_EMAIL_USAGE_UPDATE = "USR_EEX_EMAIL_USAGE_UPDATE";
	    private const string SP_GET_SUPPLIER_MEMBER_QUESTRIONS = "USR_ASI_CENTRAL_MQ_SELECT_SUPPLIER_PROC";
        private const string SP_GET_DISTRIBUTOR_MEMBER_QUESTRIONS = "USR_ASI_CENTRAL_MQ_SELECT_DISTRIBUTOR_PROC";
        private const string SP_GET_DECORATOR_MEMBER_QUESTRIONS = "USR_ASI_CENTRAL_MQ_SELECT_DECORATOR_PROC";
        private const string SP_UPDATE_DISTRIBUTOR_MEMBER_QUESTIONS = "USR_ASI_CENTRAL_MQ_UPDATE_DISTRIBUTOR_PROC";
        private const string SP_UPDATE_SUPPLIER_MEMBER_QUESTIONS = "USR_ASI_CENTRAL_MQ_UPDATE_SUPPLIER_PROC";
        private const string SP_UPDATE_DECORATOR_MEMBER_QUESTIONS = "USR_ASI_CENTRAL_MQ_UPDATE_DECORATOR_PROC";
        private const string SP_UPDATE_ASICOMP_DATA = "USR_CREATE_ASI_COMP_DATA";
        private const string SP_GET_ASICOMP_DATA = "USR_REPORT_ASI_COMP_DATA";

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
                                                          "@upd_class_code", "@upd_sub_class", "@upd_user" }},
            {SP_GET_BUNDLE_PRODUCT_DETAILS, new List<string>() { "@ip_Bundle_Group_Name ", "@ip_Rate_Structure", "@ip_Rate_Code" }},
            {SP_GET_PRODUCT_DETAILS, new List<string>() { "@ip_parent_product", "@ip_product_code", "@ip_Rate_Structure", "@ip_Rate_Code" }},
            {SP_EEX_EMAIL_USAGE_UPDATE, new List<string>() { "@email_address", "@usage_code", "@unsubscribe", "@is_globally_suppressed" }},
            {SP_GET_SUPPLIER_MEMBER_QUESTRIONS, new List<string>(){"@ip_master_customer_id", "@ip_sub_customer_id"}},
            {SP_GET_DISTRIBUTOR_MEMBER_QUESTRIONS, new List<string>(){"@ip_master_customer_id", "@ip_sub_customer_id"}},
            {SP_GET_DECORATOR_MEMBER_QUESTRIONS, new List<string>(){"@ip_master_customer_id", "@ip_sub_customer_id"}},
            {SP_UPDATE_DISTRIBUTOR_MEMBER_QUESTIONS, new List<string>{ "@ip_master_customer_id", 
                                                                        "@ip_sub_customer_id",
                                                                        "@ip_usr_year_established",
                                                                        "@ip_usr_facilitiesinformation",
                                                                        "@ip_usr_total_sales_force",
                                                                        "@ip_usr_specialty_sales_amount",
                                                                        "@ip_usr_company_sales_amount",
                                                                        "@ip_usr_pri_bus_rev_src",
                                                                        "@ip_usr_pri_bus_other_desc",
                                                                        "@ip_product_line",
                                                                        "@ip_types_accounts",
                                                                        "@ip_user" }},  
            {SP_UPDATE_SUPPLIER_MEMBER_QUESTIONS, new List<string> {"@ip_master_customer_id", 
                                                                    "@ip_sub_customer_id", 
                                                                    "@ip_usr_year_established", 
                                                                    "@ip_usr_selling_promo_products", 
                                                                    "@ip_usr_owner_gender", 
                                                                    "@ip_usr_minority_owned", 
                                                                    "@ip_usr_facilitiesinformation", 
                                                                    "@ip_usr_north_american_company", 
                                                                    "@ip_usr_office_hours", 
                                                                    "@ip_usr_prod_time_min", 
                                                                    "@ip_usr_prod_time_max", 
                                                                    "@ip_usr_rush_time", 
                                                                    "@ip_usr_imprinter", 
                                                                    "@ip_usr_retail", 
                                                                    "@ip_usr_importer", 
                                                                    "@ip_usr_wholesaler", 
                                                                    "@ip_usr_manufacturer", 
                                                                    "@ip_usr_etching", 
                                                                    "@ip_usr_pad_print", 
                                                                    "@ip_usr_lithography", 
                                                                    "@ip_usr_engraving", 
                                                                    "@ip_usr_transfer", 
                                                                    "@ip_usr_other_note", 
                                                                    "@ip_usr_hot_stamping", 
                                                                    "@ip_usr_direct_embroidery", 
                                                                    "@ip_usr_sublimation", 
                                                                    "@ip_usr_laser", 
                                                                    "@ip_usr_full_color_process", 
                                                                    "@ip_usr_silkscreen", 
                                                                    "@ip_usr_foil_stamping", 
                                                                    "@ip_usr_four_color_process", 
                                                                    "@ip_usr_offset", 
                                                                    "@ip_usr_die_stamp", 
                                                                    "@ip_user" }},
            {SP_UPDATE_DECORATOR_MEMBER_QUESTIONS, new List<string>{"@ip_master_customer_id", 
                                                                    "@ip_sub_customer_id",
                                                                    "@ip_usr_organization",
                                                                    "@ip_imprinting",
                                                                    "@ip_usr_union_label_flag",
                                                                    "@ip_user" }},

            {SP_UPDATE_ASICOMP_DATA, new List<string>(){ "@ip_USR_ACCOUNTID",
                                                         "@ip_MASTER_CUSTOMER_ID",
                                                         "@ip_SUB_CUSTOMER_ID",
                                                         "@ip_USR_PACKAGE",
                                                         "@ip_USR_CONTRACT",
                                                         "@ip_USR_CREDIT_STATUS",
                                                         "@ip_USR_ECOMMERCE",
                                                         "@ip_USR_ASI_SMARTBOOKS_EVAL",
                                                         "@ip_USR_NEWS",
                                                         "@ip_ADDED_BY" } },
            {SP_GET_ASICOMP_DATA, new List<string>(){ "@ip_MASTER_CUSTOMER_ID" } }
        };

        private static readonly IDictionary<string, List<string>> EEX_USAGE_CODES = new Dictionary<string, List<string>>()
        {
            {"DISTRIBUTOR", new List<string>() { "EEX_WEEKLY" }},
            {"SUPPLIER", new List<string>() { "EEX_SGR" }},
            {"DECORATOR", new List<string>() { "EEX_WEARABLES" }}
        };

        public static readonly int EMAIL_MARKETING_PRODUCT_ID = 126;

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

        public static void CreateBundleOrder(StoreOrder storeOrder, PersonifyMapping mapping, CompanyInformation companyInfo, string contactMasterCustomerId, 
                                             int contactSubCustomerId, AddressInfo billToAddress, AddressInfo shipToAddress)
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
            _log.Debug(string.Format("CreateBundleOrder - end: order {0} ({1})", storeOrder, DateTime.Now.Subtract(startTime).TotalMilliseconds));
        }

	    public static void AddLineItemToOrder(StoreOrder order, int productId, string rateStructure, string rateCode, 
                                                bool boltOn = true, int quantity = 1)
	    {
            var linePriceInput = new ASIAddOrderLinewithPriceInput()
            {
                OrderNumber = order.BackendReference,
                ProductID = productId,
                Quantity = quantity,
                UserDefinedBoltOn = boltOn,
                RateStructure = rateStructure,
                RateCode = rateCode
            };

            var output = SvcClient.Post<OrderNumberParam>("ASIAddOrderLinewithPrice", linePriceInput);
        }

	    public static bool ScheduleOrderPayment(StoreOrder storeOrder)
	    {
            var scheduleCreated = false;
	        if (storeOrder != null && !string.IsNullOrEmpty(storeOrder.BackendReference))
	        {
                var orderLineItems = SvcClient.Ctxt.OrderDetailInfos
                                       .Where(c => c.OrderNumber == storeOrder.BackendReference && c.BaseTotalAmount > 0)
                                       .ToList();
	            if (orderLineItems.Any())
	            {
	                var item = orderLineItems[0];
	                var iPaySchedual = new ASICreatePayScheduleInput()
	                {
	                    OrderNumber = item.OrderNumber,
	                    OrderLineNumber = (short) item.RelatedLineNumber,
	                    PayFrequency = "MONTHLY",
	                    PayStartDate = DateTime.Now,
	                    PayMethodCode = "CC",
	                    CCProfileId = Int32.Parse(storeOrder.CreditCard.ExternalReference),
	                    SyncPayScheduleFlag = true
	                };

	                var output = SvcClient.Post<ASICreatePayScheduleOutput>("ASICreatePaySchedule", iPaySchedual);
	                scheduleCreated = output.IsPaySchduleCreated ?? false;
	            }
	        }

            return scheduleCreated;
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
                    AddCompanyEmail(company, customerInfo);
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
            // add company email only if it is not the same as contact email
            if (!string.IsNullOrEmpty(storeCompany.Email) && 
                 (storeCompany.Individuals == null || !storeCompany.Individuals.Any() || string.IsNullOrEmpty(storeCompany.Individuals[0].Email) || 
                  string.Compare(storeCompany.Individuals[0].Email, storeCompany.Email, StringComparison.CurrentCultureIgnoreCase) != 0))
            {
                AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_EMAIL, storeCompany.Email,
                    COMMUNICATION_LOCATION_CODE_CORPORATE);
            }
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
                if (customerInfo != null)
                {
                    storeCompany.ExternalReference = customerInfo.MasterCustomerId + ";" + customerInfo.SubCustomerId;
                    storeCompany.MemberStatus = customerInfo.MemberStatus;
                    AddCustomerAddresses(storeCompany, customerInfo.MasterCustomerId, customerInfo.SubCustomerId, countryCodes);
                }
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
                    Email = customerInfo.PrimaryEmail,
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
                    int subCustomerId = references.Length > 1 ? Int32.Parse(references[1]) : 0;
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

        public static bool ValidateRateCode(string groupName, string rateStructure, string rateCode, ref int persProductId)
	    {
	        var isValid = false;
            if (!string.IsNullOrEmpty(groupName) && !string.IsNullOrEmpty(rateStructure) && !string.IsNullOrEmpty(rateCode))
	        {
                var response = ExecutePersonifySP(SP_GET_PRODUCT_DETAILS, new List<string>() { "ASI_PRODUCTS", groupName, rateStructure, rateCode });

	            if (response != null && !string.IsNullOrEmpty(response.Data) )
	            {
	                var responseData = response.Data.Trim();
	                if (responseData.ToUpper() != "NO DATA FOUND")
	                {
	                    var match = Regex.Match(responseData, @"<PRODUCT_ID>(.*?)</PRODUCT_ID>");
	                    if (match.Success && Int32.TryParse(match.Groups[1].Value.Trim(), out persProductId))
	                    {
	                        isValid = true;
	                    }
	                }
	            }
	        }

	        return isValid;
	    }

        public static List<XElement> GetASICOMPData(string masterId)
        {
            List<XElement> asicompData = null;
            var response = ExecutePersonifySP(SP_GET_ASICOMP_DATA, new List<string>() { masterId });
            if (response != null && !string.IsNullOrEmpty(response.Data) && response.Data.Trim().ToUpper() != "NO DATA FOUND")
            {
                var xml = XDocument.Parse(response.Data);

                asicompData = xml.Root.Elements("Table").ToList();                
            }

            return asicompData;
        }

        public static void UpdateASICompData(List<string> parameters)
        {
            var response = ExecutePersonifySP(SP_UPDATE_ASICOMP_DATA, parameters);
        }

        #region matching company with name, email or phone
        private static PersonifyCustomerInfo FindMatchingCompany(StoreCompany company, ref List<string> matchedList)
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
                matchCompany = GetMatchCompanyFromList(matchCompanyList, company.MemberType);
                matchedList = matchedList == null ? new List<string>() : matchedList;

                foreach (var c in matchCompanyList.Distinct())
                {
                    if (matchCompany == null || c.MasterCustomerId != matchCompany.MasterCustomerId )
                    {
                        matchedList.Add(string.Format("{0},{1},{2}", c.MasterCustomerId, c.MemberStatus, c.CustomerClassCode ));
                    }
                }
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

        private static PersonifyCustomerInfo GetMatchCompanyFromList(List<PersonifyCustomerInfo> matchCompanyList, string memberType)
        {
            _log.Debug("GetCompanyWithLatestNote - start: ");
            var startTime = DateTime.Now;

            PersonifyCustomerInfo companyInfo = null;
            if (matchCompanyList != null && matchCompanyList.Count > 0)
            {
                //do not match distirbutor type with other delisted types
                if (matchCompanyList.Count == 1 && 
                    (memberType.ToUpper() != "DISTRIBUTOR" ||
                        (matchCompanyList[0].CustomerClassCode.ToUpper() != "DISTRIBUTOR" && matchCompanyList[0].MemberStatus.ToUpper() != "DELISTED")))
                {
                    companyInfo = matchCompanyList[0];
                }
                else
                {
                    // find company from terminated/deliested/active companies, first the same memberType, then All MemberTypes
                    var searchList = SelectCompanies(matchCompanyList.FindAll(c => c.CustomerClassCode.ToUpper() == memberType.ToUpper()).ToList());

                    //for distributor do not try to match with other delisted member types, we will create a new record
                    if (searchList.Count < 1 && memberType.ToUpper() != "DISTRIBUTOR")
                        searchList = SelectCompanies(matchCompanyList);

                    // no match company is TERMINATED, ACTIVE and DELISTED, match from LEAD companies
                    if (searchList.Count < 1)
                    {
                        searchList = matchCompanyList.FindAll(m => m.MemberStatus != null && (m.MemberStatus.ToUpper() == "LEAD" || m.MemberStatus.ToUpper() == "ASICENTRAL"));
                    }

                    if (searchList.Count < 1)
                    {  // find company from the original list
                        if (memberType.ToUpper() != "DISTRIBUTOR") searchList = matchCompanyList;
                        else searchList = matchCompanyList.FindAll(m => m.MemberStatus != null && m.MemberStatus.ToUpper() != "DELISTED");
                    }
                    else if (searchList.Count == 1)
                    {  // one selected company, no more searching
                        companyInfo = searchList[0];
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

	    private static List<PersonifyCustomerInfo> SelectCompanies(List<PersonifyCustomerInfo> matchCompanyList)
	    {
	        var selectedCompanies = matchCompanyList;

	        if (selectedCompanies != null && selectedCompanies.Any())
	        {
                selectedCompanies = matchCompanyList.FindAll(m => m.MemberStatus != null && m.MemberStatus.ToUpper() == "TERMINATED");

                if (selectedCompanies.Count < 1)
                {
                    selectedCompanies = matchCompanyList.FindAll(m => m.MemberStatus != null && m.MemberStatus.ToUpper() == "DELISTED");
                }

                if (selectedCompanies.Count < 1)
                {
                    selectedCompanies = matchCompanyList.FindAll(m => m.MemberStatus != null && m.MemberStatus.ToUpper() == "ACTIVE");
                }	            
	        }

            return selectedCompanies;
	    }

        private static string IgnoreSpecialChars(string input)
        {
            return Regex.Replace(input.Trim(), @"[\.\$@&#\?,!]*", "");
        }

        public static CompanyInformation UpdateCompanyStatus(StoreCompany company, StatusCode status)
        {
            var matchList = new List<string>();
            var companyInfo = FindCustomerInfo(company, ref matchList);
            var customers = SvcClient.Ctxt.ASICustomers.Where(
                    p => p.MasterCustomerId == companyInfo.MasterCustomerId && p.SubCustomerId == 0).ToList();
            if (customers.Count > 0)
            {
                ASICustomer customer = customers[0];
                if (customer.UserDefinedMemberStatusString != status.ToString())
                {
                    customer.UserDefinedMemberStatusString = status.ToString();
                    SvcClient.Save<ASICustomer>(customer);

                    companyInfo.MemberStatus = customer.UserDefinedMemberStatusString;
                }
            }

            return companyInfo;
        }

        public static void UpdatePersonifyCompany(CompanyInformation companyInfo, PersonifyMapping mapping)
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

                        companyInfo.MemberStatus = customer.UserDefinedMemberStatusString;
                    }
                }
            }
            // update company class/subclass if they are different from mapping table
            else
            {
                var mappedClassCode = !string.IsNullOrEmpty(mapping.ClassCode) ? mapping.ClassCode.Trim().ToUpper() : string.Empty;
                var mappedSubClassCode = !string.IsNullOrEmpty(mapping.SubClassCode) ?  mapping.SubClassCode.Trim().ToUpper() : string.Empty;
                var classCode = !string.IsNullOrEmpty(companyInfo.CustomerClassCode) ? companyInfo.CustomerClassCode.Trim().ToUpper() : string.Empty;
                var subClassCode = !string.IsNullOrEmpty(companyInfo.SubClassCode) ? companyInfo.SubClassCode.Trim().ToUpper() : string.Empty;
                var needUpdate = false;

                if (!string.IsNullOrEmpty(companyInfo.MemberStatus) && (companyInfo.MemberStatus.ToUpper() == "LEAD" || companyInfo.MemberStatus.ToUpper() == "ASICENTRAL"))
                {
                    needUpdate = mappedClassCode != classCode || (mappedClassCode == "SUPPLIER" && mappedSubClassCode != subClassCode);
                }
                else
                {
                    needUpdate = mappedClassCode == "SUPPLIER" && mappedClassCode == classCode && string.IsNullOrEmpty(subClassCode) && !string.IsNullOrEmpty(mappedSubClassCode);
                }

                if( needUpdate )
                {
                    ExecutePersonifySP(SP_UPDATE_CUSTOMER_CLASS, new List<string>{ companyInfo.MasterCustomerId,
                                                                                   companyInfo.SubCustomerId.ToString(),
                                                                                   mapping.ClassCode,
                                                                                   mapping.SubClassCode,
                                                                                   "WEBUSER"});

                    companyInfo.CustomerClassCode = mapping.ClassCode;
                    companyInfo.SubClassCode = mapping.SubClassCode;
                }
            }
        }

	    public static CompanyInformation AddEEXSubscription(StoreCompany company, User user, IList<LookSendMyAdCountryCode> countryCodes, bool isBusinessAddress)
	    {
	        CompanyInformation companyInfo = null;
            IEnumerable<StoreAddressInfo> storeAddress = null;

	        if (user != null && company != null)
	        {
	            try
	            {
	                companyInfo = ReconcileCompany(company, user.MemberType_CD, countryCodes, ref storeAddress, false);

	                if (companyInfo != null)
	                {
	                    // Add contact to personify
	                    var customerInfos = AddIndividualInfos(company, countryCodes, companyInfo.MasterCustomerId,
	                        companyInfo.SubCustomerId);
	                    if (customerInfos != null && customerInfos.Any())
	                    {
	                        var indivInfo = customerInfos.ElementAt(0);
	                        // save address to personify
	                        if (indivInfo != null)
	                        {
	                            var countryCode = countryCodes != null
	                                ? countryCodes.Alpha3Code(user.Country)
	                                : user.Country;
	                            var saveAddressInput = new SaveAddressInput()
	                            {
	                                MasterCustomerId = indivInfo.MasterCustomerId,
	                                SubCustomerId = indivInfo.SubCustomerId,
	                                AddressTypeCode = isBusinessAddress ? "OFFICE" : "HOME",
	                                Address1 = user.Street1,
	                                City = user.City,
	                                State = user.State,
	                                PostalCode = user.Zip,
	                                CountryCode = countryCode,
	                                DirectoryFlag = true,
	                                WebMobileDirectory = false,
	                                CreateNewAddressIfOrdersExist = true,
	                                OverrideAddressValidation = true,
	                                ShipToFlag = true,
	                                BillToFlag = true,
	                                AddedOrModifiedBy = ADDED_OR_MODIFIED_BY
	                            };

	                            SvcClient.Post<SaveAddressOutput>("CreateOrUpdateAddress", saveAddressInput);
	                        }

	                        var memberType = user.MemberType_CD.ToUpper();
	                        if (EEX_USAGE_CODES.Keys.Contains(memberType))
	                        {
	                            foreach (var code in EEX_USAGE_CODES[memberType])
	                            {
	                                ExecutePersonifySP(SP_EEX_EMAIL_USAGE_UPDATE,
	                                    new List<string>() {user.Email, code, "N", "N"});
	                            }
	                        }
	                    }
	                }
	            }
	            catch (Exception ex)
	            {
	                _log.Debug("PersonifyClient.AddEEXSubscription - exception when update EmailExpress Subscription, messages " + ex.Message);
	            }
	        }

	        return companyInfo;
	    }

        public static PersonifyStatus OptOutEmailSubscription(string email, List<string> usageCodes)
        {
            var requestStatus = PersonifyStatus.PersonifyError;
            if (!string.IsNullOrEmpty(email) && usageCodes != null && usageCodes.Any())
            {
                try
                {
                    if (GetIndividualInfoByEmail(email) == null)
                    {
                        requestStatus = PersonifyStatus.NoRecordFound;
                    }
                    else
                    {
                        foreach (var code in usageCodes)
	                    {
                            ExecutePersonifySP(SP_EEX_EMAIL_USAGE_UPDATE, new List<string>() { email, code, "Y", "N" });
	                    }

                        requestStatus = PersonifyStatus.Success;
                    }
                }
                catch(Exception ex)
                {
                    _log.Debug(string.Format("OptOutEmailSubscription() Exception, message: {0}", ex.Message));
                }
            }

            return requestStatus;
        }

        public static StoreDetailApplication GetDemographicData(IStoreService storeService, StoreOrderDetail orderDetail)
        {
            var storeDetailApp = storeService.GetApplication(orderDetail);
            try
            {
                if (orderDetail.Order != null && orderDetail.Order.Company != null && orderDetail.Order.Company.HasExternalReference())
                {
                    var masterId = orderDetail.Order.Company.ExternalReference.Split(';')[0];
                    if (StoreDetailSupplierMembership.Identifiers.Contains(orderDetail.Product.Id))
                    {
                        storeDetailApp = GetSupplierDemograpic((StoreDetailSupplierMembership)storeDetailApp, storeService, masterId);
                    }
                    else if (StoreDetailDistributorMembership.Identifiers.Contains(orderDetail.Product.Id))
                    {
                        storeDetailApp = GetDistributorDemograpic((StoreDetailDistributorMembership)storeDetailApp, storeService, masterId);
                    }
                    else if(StoreDetailDecoratorMembership.Identifiers.Contains(orderDetail.Product.Id))
                    {
                        storeDetailApp = GetDecoratorDemograpic((StoreDetailDecoratorMembership)storeDetailApp, storeService, masterId);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Debug("PersonifyClient.GetDemographicData exception, message: " + ex.Message);
            }

            return storeDetailApp;
        }

        public static void UpdateDemographicData(IStoreService storeService, StoreOrderDetail orderDetail)
        {
            var storeDetailApp = storeService.GetApplication(orderDetail);
            if (storeDetailApp != null && orderDetail.Order != null && orderDetail.Order.Company != null && 
                orderDetail.Order.Company.HasExternalReference())
            {
                try
                {
                    var company = orderDetail.Order.Company;
                    var user = string.Empty;

                    if (company.Individuals != null && company.Individuals.Count > 0)
                    {
                        user = company.Individuals[0].Email;
                    }

                    if (string.IsNullOrEmpty(user))
                    {
                        user = ((System.Security.Principal.WindowsIdentity)System.Web.HttpContext.Current.User.Identity).Name;
                    }

                    if (StoreDetailSupplierMembership.Identifiers.Contains(orderDetail.Product.Id))
                    {
                        var memberData = (StoreDetailSupplierMembership)storeDetailApp;
                        var decoTypes = memberData.DecoratingTypes;
                        ExecutePersonifySP(SP_UPDATE_SUPPLIER_MEMBER_QUESTIONS, new List<string>
                        {
                            orderDetail.Order.ExternalReference, "0",
                            memberData.YearEstablished.HasValue ? memberData.YearEstablished.Value.ToString() : string.Empty,
                            memberData.YearEnteredAdvertising.HasValue ? memberData.YearEnteredAdvertising.Value.ToString() : string.Empty,
                            memberData.WomanOwned.HasValue && memberData.WomanOwned.Value ? "F" : "M",
                            memberData.IsMinorityOwned.HasValue && memberData.IsMinorityOwned.Value ? "1" : "0",
                            memberData.NumberOfEmployee,
                            memberData.HasAmericanProducts.HasValue && memberData.HasAmericanProducts.Value ? "1" : "0",
                            memberData.BusinessHours,
                            memberData.ProductionTime,
                            memberData.ProductionTime,
                            memberData.IsRushServiceAvailable.HasValue && memberData.IsRushServiceAvailable.Value ? "1" : "0",
                            memberData.IsImprinterVsDecorator.HasValue && memberData.IsMinorityOwned.Value ? "1" : "0",
                            memberData.IsRetailer.HasValue && memberData.IsRetailer.Value ? "1" : "0",
                            memberData.IsImporter.HasValue && memberData.IsImporter.Value ? "1" : "0",
                            memberData.IsWholesaler.HasValue && memberData.IsWholesaler.Value ? "1" : "0",
                            memberData.IsManufacturer.HasValue && memberData.IsMinorityOwned.Value ? "1" : "0",
                            decoTypes.FirstOrDefault(d => d.Description == LookSupplierDecoratingType.DECORATION_ETCHING) != null ? "1" : "0",
                            decoTypes.FirstOrDefault(d => d.Description == LookSupplierDecoratingType.DECORATION_PADPRINT) != null ? "1" : "0",
                            decoTypes.FirstOrDefault(d => d.Description == LookSupplierDecoratingType.DECORATION_LITHOGRAPHY) != null ? "1" : "0",
                            decoTypes.FirstOrDefault(d => d.Description == LookSupplierDecoratingType.DECORATION_ENGRAVING) != null ? "1" : "0",
                            decoTypes.FirstOrDefault(d => d.Description == LookSupplierDecoratingType.DECORATION_TRANSFER) != null ? "1" : "0",
                            memberData.OtherDec,
                            decoTypes.FirstOrDefault(d => d.Description == LookSupplierDecoratingType.DECORATION_HOTSTAMPING) != null ? "1" : "0",
                            decoTypes.FirstOrDefault(d => d.Description == LookSupplierDecoratingType.DECORATION_DIRECTEMBROIDERY) != null ? "1" : "0",
                            decoTypes.FirstOrDefault(d => d.Description == LookSupplierDecoratingType.DECORATION_SUBLIMINATION) != null ? "1" : "0",
                            decoTypes.FirstOrDefault(d => d.Description == LookSupplierDecoratingType.DECORATION_LASER) != null ? "1" : "0",
                            decoTypes.FirstOrDefault(d => d.Description == LookSupplierDecoratingType.DECORATION_FULLCOLOR) != null ? "1" : "0",
                            decoTypes.FirstOrDefault(d => d.Description == LookSupplierDecoratingType.DECORATION_SILKSCREEN) != null ? "1" : "0",
                            decoTypes.FirstOrDefault(d => d.Description == LookSupplierDecoratingType.DECORATION_FOILSTAMPING) != null ? "1" : "0",
                            decoTypes.FirstOrDefault(d => d.Description == LookSupplierDecoratingType.DECORATION_FOURCOLOR) != null ? "1" : "0",
                            decoTypes.FirstOrDefault(d => d.Description == LookSupplierDecoratingType.DECORATION_OFFSET) != null ? "1" : "0",
                            decoTypes.FirstOrDefault(d => d.Description == LookSupplierDecoratingType.DECORATION_DIESTAMP) != null ? "1" : "0",
                            user
                        });
                    }
                    else if (StoreDetailDistributorMembership.Identifiers.Contains(orderDetail.Product.Id))
                    {
                        var memberData = (StoreDetailDistributorMembership)storeDetailApp;
                        ExecutePersonifySP(SP_UPDATE_DISTRIBUTOR_MEMBER_QUESTIONS, new List<string>
                        {
                            orderDetail.Order.ExternalReference, "0",
                            memberData.EstablishedDate.HasValue ? memberData.EstablishedDate.Value.Year.ToString() : string.Empty,
                            memberData.NumberOfEmployee.ToString(),
                            memberData.NumberOfSalesEmployee.HasValue ? memberData.NumberOfSalesEmployee.Value.ToString() : string.Empty,
                            memberData.AnnualSalesVolumeASP,
                            memberData.AnnualSalesVolume,
                            memberData.PrimaryBusinessRevenue.Name.ToString(),
                            memberData.OtherBusinessRevenue,
                            string.Join("", memberData.ProductLines.Select(p => p.SubCode).ToList()),
                            string.Join("", memberData.AccountTypes.Select(p => p.SubCode).ToList()),
                            user
                        });
                    }
                    else if (StoreDetailDecoratorMembership.Identifiers.Contains(orderDetail.Product.Id))
                    {
                        var memberData = (StoreDetailDecoratorMembership)storeDetailApp;
                        var imprinting = string.Join("", memberData.ImprintTypes.Select(m => m.Id.ToString()));

                        ExecutePersonifySP(SP_UPDATE_DECORATOR_MEMBER_QUESTIONS, new List<string>
                                           { orderDetail.Order.ExternalReference, "0", 
                                            memberData.BestDescribesOption != null ?  memberData.BestDescribesOption.Value.ToString() : "", 
                                            imprinting, memberData.IsUnionMember ? "Y" : "N", user });
                    }
                }
                catch (Exception ex)
                {
                    _log.Debug("PersonifyClient.UpdateDemographicData exception: " + ex.Message);
                }
            }
        }

        private static StoreDetailDecoratorMembership GetDecoratorDemograpic(StoreDetailDecoratorMembership memberData, IStoreService storeService, string masterCustomerId)
        {
            if (memberData == null)
                memberData = new StoreDetailDecoratorMembership();

            var response = ExecutePersonifySP(SP_GET_DECORATOR_MEMBER_QUESTRIONS, new List<string>() { masterCustomerId, "0" });
            if (storeService != null && response != null && !string.IsNullOrEmpty(response.Data) && 
                response.Data.Trim().ToUpper() != "NO DATA FOUND")
            {
                var xml = XDocument.Parse(response.Data);
                var data = xml.Root.Elements("Table").FirstOrDefault();
                if (data != null)
                {
                    var imprintingTypes = storeService.GetAll<LookDecoratorImprintingType>().ToList();
                    LookDecoratorImprintingType imprintType = null;
                    foreach (var element in data.Elements())
                    {
                        int value = 0;
                        Int32.TryParse(element.Value, out value);
                        switch (element.Name.ToString())
                        {
                            case "USR_ORGANIZATION":
                                memberData.BestDescribesOption = value;
                                break;
                            case "Digitizing":
                                imprintType = imprintingTypes.FirstOrDefault(t => t.SubCode == "D");
                                break;
                            case "Embroidery":
                                imprintType = imprintingTypes.FirstOrDefault(t => t.SubCode == "A");
                                break;
                            case "Engraving":
                                imprintType = imprintingTypes.FirstOrDefault(t => t.SubCode == "E");
                                break;
                            case "Heat_x0020_Transfer":
                                imprintType = imprintingTypes.FirstOrDefault(t => t.SubCode == "C");
                                break;
                            case "Monogramming":
                                imprintType = imprintingTypes.FirstOrDefault(t => t.SubCode == "G");
                                break;
                            case "Screen_x0020_Printing":
                                imprintType = imprintingTypes.FirstOrDefault(t => t.SubCode == "B");
                                break;
                            case "Sublimation":
                                imprintType = imprintingTypes.FirstOrDefault(t => t.SubCode == "F");
                                break;
                            case "Union_LabeL_Flag":
                                memberData.IsUnionMember = true;
                                break;                        
                        }

                        if ( imprintType != null)
                        {
                            var existing = memberData.ImprintTypes.FirstOrDefault(p => p.Id == imprintType.Id);
                            if (value == 1 && existing == null)
                                memberData.ImprintTypes.Add(imprintType);
                            else if (element.Value == "0" && existing != null)
                                memberData.ImprintTypes.Remove(existing);
                        }
                    }
                }
            }

            return memberData;
        }

        private static StoreDetailSupplierMembership GetSupplierDemograpic(StoreDetailSupplierMembership memberData, IStoreService storeService, string masterCustomerId)
        {
            if (memberData == null)
                memberData = new StoreDetailSupplierMembership();

            var response = ExecutePersonifySP(SP_GET_SUPPLIER_MEMBER_QUESTRIONS, new List<string>() { masterCustomerId, "0" });
            if (response != null && !string.IsNullOrEmpty(response.Data) && response.Data.Trim().ToUpper() != "NO DATA FOUND")
            {
                var xml = XDocument.Parse(response.Data);
                var data = xml.Root.Elements("Table").FirstOrDefault();
                var productionTimeMin = string.Empty;
                var productionTimeMax = string.Empty;
                var decoratorTypes = storeService.GetAll<LookSupplierDecoratingType>().ToList();
                if (data != null)
                {
                    foreach (var element in data.Elements())
                    {
                        int intValue = 0;
                        Int32.TryParse(element.Value, out intValue);
                        LookSupplierDecoratingType type = null;
                        switch (element.Name.ToString())
                        {
                            case "USR_YEAR_ESTABLISHED":
                                memberData.YearEstablished = intValue;
                                break;
                            case "USR_SELLING_PROMO_PRODUCTS":
                                memberData.YearEnteredAdvertising = intValue;
                                break;
                            case "USR_OWNER_GENDER":
                                memberData.WomanOwned = element.Value == "F";
                                break;
                            case "USR_MINORITY_OWNED":
                                memberData.IsMinorityOwned = intValue == 1;
                                break;
                            case "USR_FACILITIESINFORMATION":
                                memberData.NumberOfEmployee = element.Value;
                                break;
                            case "USR_NORTH_AMERICAN_COMPANY":
                                memberData.HasAmericanProducts = intValue == 1;
                                break;
                            case "USR_OFFICE_HOURS":
                                memberData.BusinessHours = element.Value;
                                break;
                            case "USR_PROD_TIME_MIN":
                                productionTimeMin = element.Value;
                                break;
                            case "USR_PROD_TIME_MAX":
                                productionTimeMax = element.Value;
                                break;
                            case "USR_RUSH_TIME":
                                memberData.IsRushServiceAvailable = intValue == 1;
                                break;
                            case "USR_IMPRINTER":
                                memberData.IsImprinterVsDecorator = intValue == 1;
                                break;
                            case "USR_RETAIL":
                                memberData.IsRetailer = intValue == 1;
                                break;
                            case "USR_IMPORTER":
                                memberData.IsImporter = intValue == 1;
                                break;
                            case "USR_WHOLESALER":
                                memberData.IsWholesaler = intValue == 1;
                                break;
                            #region decorator types
                            case "USR_ETCHING":
                                type = decoratorTypes.FirstOrDefault(t => t.Description == LookSupplierDecoratingType.DECORATION_ETCHING);
                                break;
                            case "USR_PAD_PRINT":
                                type = decoratorTypes.FirstOrDefault(t => t.Description == LookSupplierDecoratingType.DECORATION_PADPRINT);
                                break;
                            case "USR_LITHOGRAPHY":
                                type = decoratorTypes.FirstOrDefault(t => t.Description == LookSupplierDecoratingType.DECORATION_LITHOGRAPHY);
                                break;
                            case "USR_ENGRAVING":
                                type = decoratorTypes.FirstOrDefault(t => t.Description == LookSupplierDecoratingType.DECORATION_ENGRAVING);
                                break;
                            case "USR_TRANSFER":
                                type = decoratorTypes.FirstOrDefault(t => t.Description == LookSupplierDecoratingType.DECORATION_TRANSFER);
                                break;
                            case "USR_HOT_STAMPING":
                                type = decoratorTypes.FirstOrDefault(t => t.Description == LookSupplierDecoratingType.DECORATION_HOTSTAMPING);
                                break;
                            case "USR_DIRECT_EMBROIDERY":
                                type = decoratorTypes.FirstOrDefault(t => t.Description == LookSupplierDecoratingType.DECORATION_DIRECTEMBROIDERY);
                                break;
                            case "USR_SUBLIMATION":
                                type = decoratorTypes.FirstOrDefault(t => t.Description == LookSupplierDecoratingType.DECORATION_SUBLIMINATION);
                                break;
                            case "USR_LASER":
                                type = decoratorTypes.FirstOrDefault(t => t.Description == LookSupplierDecoratingType.DECORATION_LASER);
                                break;
                            case "USR_FULL_COLOR_PROCESS":
                                type = decoratorTypes.FirstOrDefault(t => t.Description == LookSupplierDecoratingType.DECORATION_FULLCOLOR);
                                break;
                            case "USR_SILKSCREEN":
                                type = decoratorTypes.FirstOrDefault(t => t.Description == LookSupplierDecoratingType.DECORATION_SILKSCREEN);
                                break;
                            case "USR_FOIL_STAMPING":
                                type = decoratorTypes.FirstOrDefault(t => t.Description == LookSupplierDecoratingType.DECORATION_FOILSTAMPING);
                                break;
                            case "USR_FOUR_COLOR_PROCESS":
                                type = decoratorTypes.FirstOrDefault(t => t.Description == LookSupplierDecoratingType.DECORATION_FOURCOLOR);
                                break;
                            case "USR_OFFSET":
                                type = decoratorTypes.FirstOrDefault(t => t.Description == LookSupplierDecoratingType.DECORATION_OFFSET);
                                break;
                            case "USR_DIE_STAMP":
                                type = decoratorTypes.FirstOrDefault(t => t.Description == LookSupplierDecoratingType.DECORATION_DIESTAMP);
                                break;
                            #endregion decorator types
                        }

                        if (type != null)
                        {
                            var existing = memberData.DecoratingTypes.FirstOrDefault(t => t.Id == type.Id);
                            if (element.Value == "1" && existing == null)
                                memberData.DecoratingTypes.Add(type);
                            else if (element.Value == "0" && existing != null)
                                memberData.DecoratingTypes.Remove(existing);
                        }
                    }

                    memberData.ProductionTime = productionTimeMin;
                    if (productionTimeMax != productionTimeMin)
                    {
                        memberData.ProductionTime += " - " + productionTimeMax;
                    }

                    var productLines = GetProductLines(storeService, data.Elements());
                    if (productLines != null && productLines.Any())
                    {  // get first line
                        memberData.LineNames = productLines[0].Description;
                    }
                }
            }

            return memberData;
        }

	    private static StoreDetailDistributorMembership GetDistributorDemograpic(StoreDetailDistributorMembership memberData, IStoreService storeService, string masterCustomerId)
        {
            if (memberData == null)
                memberData = new StoreDetailDistributorMembership();

            var response = ExecutePersonifySP(SP_GET_DISTRIBUTOR_MEMBER_QUESTRIONS, new List<string>() { masterCustomerId, "0" });
	        if (response != null && !string.IsNullOrEmpty(response.Data) &&
	            response.Data.Trim().ToUpper() != "NO DATA FOUND")
	        {
                var xml = XDocument.Parse(response.Data);

                var data = xml.Root.Elements("Table").FirstOrDefault();
	            if (data != null)
	            {
	                memberData.ProductLines = GetProductLines(storeService, data.Elements(), memberData.ProductLines);
	                memberData.AccountTypes = GetDistAccountTypes(storeService, data.Elements(), memberData.AccountTypes);
	                var businessTypes = storeService.GetAll<LookDistributorRevenueType>().ToList();
                    foreach (var element in data.Elements())
	                {
	                    int intValue = 0;
                        Int32.TryParse(element.Value, out intValue);
	                    switch (element.Name.ToString())
	                    {
	                        case "USR_YEAR_ESTABLISHED":
	                            if( intValue != 0 ) memberData.EstablishedDate = new DateTime(intValue, 1, 1);
	                            break;
	                        case "USR_FACILITIESINFORMATION":
	                            if( intValue > 0 )  memberData.NumberOfEmployee = intValue;
	                            break;
	                        case "USR_TOTAL_SALES_FORCE":
	                            if( intValue > 0 )  memberData.NumberOfSalesEmployee = intValue;
	                            break;
	                        case "USR_SPECIALTY_SALES_AMOUNT":
	                            memberData.AnnualSalesVolumeASP = element.Value;
	                            break;
	                        case "USR_COMPANY_SALES_AMOUNT":
	                            memberData.AnnualSalesVolume = element.Value;
	                            break;
	                        case "USR_PRI_BUS_REV_SRC":
	                            if (!string.IsNullOrEmpty(element.Value))
	                            {
	                                switch( element.Value.ToLower())
	                                {
	                                    case "signs":
	                                        memberData.PrimaryBusinessRevenue = businessTypes.FirstOrDefault(t => t.Id == 1);
	                                        break;
                                        case "trophy & awards":
                                            memberData.PrimaryBusinessRevenue = businessTypes.FirstOrDefault(t => t.Id == 2);
                                            break;
                                        case "printing":
                                            memberData.PrimaryBusinessRevenue = businessTypes.FirstOrDefault(t => t.Id == 3);
                                            break;
                                        case "screen printing":
                                            memberData.PrimaryBusinessRevenue = businessTypes.FirstOrDefault(t => t.Id == 4);
                                            break;
                                        case "promotional products":
                                            memberData.PrimaryBusinessRevenue = businessTypes.FirstOrDefault(t => t.Id == 5);
                                            break;
                                    }
	                            }
	                            break;
	                        case "USR_PRI_BUS_OTHER_DESC":
	                            memberData.OtherBusinessRevenue = element.Value;
	                            break;
	                    }
	                }
                }
	        }

            return memberData;
        }

        private static IList<LookProductLine> GetProductLines(IStoreService storeService, IEnumerable<XElement> elements, IList<LookProductLine> productLines = null)
        {
            if (elements != null && elements.Count() > 1)
            {
                productLines = productLines ?? new List<LookProductLine>();
                var prodLines = storeService.GetAll<LookProductLine>().ToList();
                foreach (var element in elements)
                {
                    LookProductLine type = null;
                    switch (element.Name.ToString())
                    {
                        case "Auto_x0020_accessories":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "A");
                            break;
                        case "Awards_x0020_trophies_x0020__x0026__x0020_plaques":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "B");
                            break;
                        case "Badges_x0020_buttons":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "C");
                            break;
                        case "Bags":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "1");
                            break;
                        case "Calendars_x0020__x0026__x0020_timepieces":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "D");
                            break;
                        case "Cards":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "O");
                            break;
                        case "Cups_x0020_and_x0020_mugs":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "Y");
                            break;
                        case "Decals_x0020_transfers_x0020_emblems":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "Z");
                            break;
                        case "Electronic_x002F_computer_x0020_products":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "L");
                            break;
                        case "Emblematic_x0020_jewelry":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "G");
                            break;
                        case "Food_x0020_Edibles":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "F");
                            break;
                        case "Glass_x0020_and_x0020_ceramics_x0020_products":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "I");
                            break;
                        case "Health_x0020_safety_x0020_and_x0020_environmental_x0020_products":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "V");
                            break;
                        case "Housewares_x0020_and_x0020_home_x0020_products":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "J");
                            break;
                        case "Industrial_x0020_and_x0020_safety_x0020_items":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "H");
                            break;
                        case "Inflatables":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "K");
                            break;
                        case "Key_x0020_tags":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "U");
                            break;
                        case "Magnetic_x0020_products":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "X");
                            break;
                        case "Office_x0020_and_x0020_desk_x0020_products":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "M");
                            break;
                        case "Other_x0020_Product_x0020_Line":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "");
                            break;
                        case "Paper_x0020_Products":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "N");
                            break;
                        case "Party_x0020_Products":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "3");
                            break;
                        case "Personal_x0020_Care":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "4");
                            break;
                        case "Phone_x0020_Calling_x0020_cards":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "5");
                            break;
                        case "Plastic":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "E");
                            break;
                        case "Sports_x0020_Accessories":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "P");
                            break;
                        case "Toys_x0020__x0026__x0020_Stuffed_x0020_Animal":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "Q");
                            break;
                        case "Travel_x0020_products":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "W");
                            break;
                        case "Umbrellas":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "2");
                            break;
                        case "Vinyls":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "R");
                            break;
                        case "Wearables":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "S");
                            break;
                        case "Writing_x0020_Instruments":
                            type = prodLines.FirstOrDefault(t => t.SubCode == "T");
                            break;
                    }
                    if (type != null)
                    {
                        var existing = productLines.FirstOrDefault(p => p.Id == type.Id);
                        if (element.Value == "1" && existing == null)
                            productLines.Add(type);
                        else if (element.Value == "0" && existing != null)
                            productLines.Remove(existing);
                    }
                }
            }

            return productLines;
        }

        private static IList<LookDistributorAccountType> GetDistAccountTypes(IStoreService storeService, IEnumerable<XElement> elements, IList<LookDistributorAccountType> accountTypes)
        {
            if (elements != null && elements.Count() > 1)
            {
                accountTypes = accountTypes ?? new List<LookDistributorAccountType>();
                var types = storeService.GetAll<LookDistributorAccountType>().ToList();
                foreach (var element in elements)
                {
                    LookDistributorAccountType type = null;
                    switch (element.Name.ToString())
                    {
                        case "Agriculture_x0020__x0026__x0020_Farming":
                            type = types.FirstOrDefault(t => t.SubCode == "A");
                            break;
                        case "Automotive_x0020_dealers_x0020__x0026__x0020_mfgs":
                            type = types.FirstOrDefault(t => t.SubCode == "V");
                            break;
                        case "Chemical_x0020_and_x0020_Pharmaceutical_x0020_companies":
                            type = types.FirstOrDefault(t => t.SubCode == "K");
                            break;
                        case "Clothing_x0020_Appliances_x0020_Soft_x0020_goods_x0020_Mfgs":
                            type = types.FirstOrDefault(t => t.SubCode == "I");
                            break;
                        case "Clubs_x0020_Associations_x0020_Civic_x0020_Groups_x0020_nonprofits":
                            type = types.FirstOrDefault(t => t.SubCode == "B");
                            break;
                        case "Construction_x0020_companies":
                            type = types.FirstOrDefault(t => t.SubCode == "S");
                            break;
                        case "Financial":
                            type = types.FirstOrDefault(t => t.SubCode == "C");
                            break;
                        case "Food_x0020_Tobacco_x0020_Sundries":
                            type = types.FirstOrDefault(t => t.SubCode == "J");
                            break;
                        case "Government_x0020_Agencies":
                            type = types.FirstOrDefault(t => t.SubCode == "D");
                            break;
                        case "Health_x0020_and_x0020_medical":
                            type = types.FirstOrDefault(t => t.SubCode == "T");
                            break;
                        case "Hospitality":
                            type = types.FirstOrDefault(t => t.SubCode == "W");
                            break;
                        case "Industrial_x0020_Products":
                            type = types.FirstOrDefault(t => t.SubCode == "H");
                            break;
                        case "Insurance_x0020_companies_x0020_and_x0020_agencies":
                            type = types.FirstOrDefault(t => t.SubCode == "F");
                            break;
                        case "Manufacturing":
                            type = types.FirstOrDefault(t => t.SubCode == "X");
                            break;
                        case "Marketing_x0020_Services":
                            type = types.FirstOrDefault(t => t.SubCode == "O");
                           break;
                        case "Media":
                            type = types.FirstOrDefault(t => t.SubCode == "Y");
                            break;
                        case "Political_x0020_parties_x0020_and_x0020_candidates":
                            type = types.FirstOrDefault(t => t.SubCode == "L");
                            break;
                        case "Professional_x0020_Offices":
                            type = types.FirstOrDefault(t => t.SubCode == "N");
                            break;
                        case "Recreation":
                            type = types.FirstOrDefault(t => t.SubCode == "U");
                            break;
                        case "Retail":
                            type = types.FirstOrDefault(t => t.SubCode == "M");
                            break;
                        case "Schools_x0020_colleges_x0020_universities":
                            type = types.FirstOrDefault(t => t.SubCode == "E");
                            break;
                        case "Service_x0020_Businesses":
                            type = types.FirstOrDefault(t => t.SubCode == "P");
                            break;
                        case "Sports-Related":
                            type = types.FirstOrDefault(t => t.SubCode == "Z");
                            break;
                        case "Technology":
                            type = types.FirstOrDefault(t => t.SubCode == "1");
                            break;
                        case "Transportation":
                            type = types.FirstOrDefault(t => t.SubCode == "Q");
                            break;
                        case "Utilities":
                            type = types.FirstOrDefault(t => t.SubCode == "G");
                            break;
                        case "Wholesalers":
                            type = types.FirstOrDefault(t => t.SubCode == "R");
                            break;
                    }

                    if (type != null)
                    {
                        var existing = accountTypes.FirstOrDefault(p => p.Id == type.Id);
                        if (element.Value == "1" && existing == null)
                            accountTypes.Add(type);
                        else if (element.Value == "0" && existing != null)
                            accountTypes.Remove(existing);
                    }
                }
            }

            return accountTypes;
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

                    try
                    {
                        response = SvcClient.Post<StoredProcedureOutput>("GetStoredProcedureDataXML", spRequest);
                    }
                    catch (Exception ex)
                    {
                        _log.Error(string.Format("ExecutePersonifySP Error - message: {0}, stack track: {1}", ex.Message, ex.StackTrace) );
                    }
                }
            }

            _log.Debug(string.Format("ExecutePersonifySP - StoreProcedure end: ({0})", DateTime.Now.Subtract(startTime).TotalMilliseconds));

            return response;
        }

        private static List<PersonifyCustomerInfo> GetCustomerInfoFromSP(string spName, List<string> parameters)
        {
            var companyInfoList = new List<PersonifyCustomerInfo>();

            var response = ExecutePersonifySP(spName, parameters);
            if (response != null && !string.IsNullOrEmpty(response.Data) && response.Data.Trim().ToUpper() != "NO DATA FOUND")
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
                            case "LAST_FIRST_NAME":
                                var names = value.Split(',');
                                customerInfo.FirstName = names.ElementAt(names.Length - 1).Trim();
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

                    if( customerInfo.RecordType == null || customerInfo.RecordType != RECORD_TYPE_CORPORATE ||
                        ((customerInfo.CustomerStatusCode == null || customerInfo.CustomerStatusCode != CUSTOMER_INFO_STATUS_DUPLICATE) && 
                         (customerInfo.MemberStatus == null || customerInfo.MemberStatus != StatusCode.MMS_LOAD.ToString())))
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
                                AddRelationship(customerInfo.MasterCustomerId, customerInfo.SubCustomerId, companyMasterId, companySubId, true);
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

        private static void AddRelationship(string contactMasterId, int contactSubId, string companyMasterId, int companySubId, bool newContact = false)
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
            cusRelationship.PrimaryEmployerFlag = newContact;

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

        public static CusCommunication AddCompanyEmail(StoreCompany company, CompanyInformation companyInfo)
        {
            if (company == null || string.IsNullOrEmpty(company.Email) || companyInfo == null ) 
                return null;

            // check if the email is the same as contact email
            if (company.Individuals != null && company.Individuals.Any() && 
                string.Compare(company.Email, company.Individuals[0].Email, StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                return null;
            }

            var cusCommRecords = SvcClient.Ctxt.CusCommunications.Where(c => c.CommTypeCodeString == COMMUNICATION_INPUT_EMAIL
                                                              && string.Compare(c.SearchPhoneAddress, company.Email, StringComparison.CurrentCultureIgnoreCase) == 0).ToList();

            CusCommunication cusCommRecord = null;
            if (!cusCommRecords.Any())
            {
                var existingEmails = GetCusCommunications(companyInfo.MasterCustomerId, companyInfo.SubCustomerId, COMMUNICATION_INPUT_EMAIL);
                var commType = COMMUNICATION_LOCATION_CODE_CORPORATE;
                if (existingEmails.Any())
                {
                    var curLocTypes = existingEmails.Select(c => c.CommLocationCodeString).ToList();
                    commType = new string[] {COMMUNICATION_LOCATION_CODE_BUSINESS, COMMUNICATION_LOCATION_CODE_CORPORATE}.FirstOrDefault(c => !curLocTypes.Contains(c));
                    // already having both BUSINESS and CORPORATE emails
                    if (string.IsNullOrEmpty(commType))
                    {
                        if (curLocTypes.Count(c => c.Contains(COMMUNICATION_LOCATION_CODE_CORPORATE)) < 10)
                        {
                            commType = COMMUNICATION_LOCATION_CODE_CORPORATE;
                        }
                        else if (curLocTypes.Count(c => c.Contains(COMMUNICATION_LOCATION_CODE_BUSINESS)) < 10)
                        {
                            commType = COMMUNICATION_LOCATION_CODE_BUSINESS;
                        }

                        if ( !string.IsNullOrEmpty(commType))
                        {
                            var emailCnt = 1;
                            while (curLocTypes.Contains(COMMUNICATION_LOCATION_CODE_CORPORATE + emailCnt))
                            {
                                emailCnt++;
                            }
                            commType += emailCnt;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(commType))
                {
                    try
                    {
                        var cusEmail = SvcClient.Create<CusCommunication>();
                        cusEmail.MasterCustomerId = companyInfo.MasterCustomerId;
                        cusEmail.SubCustomerId = companyInfo.SubCustomerId;
                        cusEmail.CommTypeCodeString = COMMUNICATION_INPUT_EMAIL;
                        cusEmail.PrimaryFlag = string.IsNullOrEmpty(companyInfo.Email) ? true : false;
                        cusEmail.CommLocationCodeString = commType;
                        cusEmail.FormattedPhoneAddress = company.Email;

                        cusCommRecord = SvcClient.Save<CusCommunication>(cusEmail);
                    }
                    catch (Exception ex)
                    {
                        _log.Error(string.Format("Faild to add email '{0}' to company '{1}' in Personify; error message: {2}",
                                    company.Email, companyInfo.Name, ex.Message));
                    }
                }                    
            }

            return cusCommRecord;
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

			if ( company.HasExternalReference())
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
            int subCustomerId,
            string payOrderLineNumbers = null)
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
            var orderLineNumbers = string.IsNullOrEmpty(payOrderLineNumbers) ? GetOrderLinesByOrderId(orderNumber, ref amount) : payOrderLineNumbers;
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

        public static string GetOrderLinesByOrderId(string orderId, ref decimal amount, List<PersonifyMapping> requestedProducts = null )
        {
            var orderLines = SvcClient.Ctxt.OrderDetailInfos.Where(c => c.OrderNumber == orderId).ToList();

            if (requestedProducts != null && requestedProducts.Any())
            {
                orderLines = orderLines.FindAll(o => requestedProducts.Find(p => p.PersonifyProduct.HasValue && p.PersonifyProduct == o.ProductId 
                                                                                   && o.ActualTotalAmount.HasValue && o.ActualTotalAmount.Value > 0) != null);
            }

            string result = null;
            if (orderLines.Any())
            {
                result = string.Join(",", orderLines.Select(o => o.OrderLineNumber));
                var payOrderSum = orderLines.Sum(o => o.ActualTotalAmount);
                if (payOrderSum != null)
                    amount = payOrderSum.Value;
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