﻿using asi.asicentral.interfaces;
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
            if (companyInfo == null || contactInfo == null)
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

        public static CustomerInfo ReconcileCompany(StoreCompany company, string customerClassCode, IList<LookSendMyAdCountryCode> countryCodes)
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
            StoreAddress companyAddress = company.GetCompanyAddress();
            string countryCode = countryCodes != null ? countryCodes.Alpha3Code(companyAddress.Country) : companyAddress.Country;
            if (companyInfo == null)
            {
                //company not already there, create a new one
                var saveCustomerInput = new SaveCustomerInput { LastName = company.Name, CustomerClassCode = customerClassCode };
                AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_PHONE, company.Phone,
                    COMMUNICATION_LOCATION_CODE_CORPORATE, countryCode);
                AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_FAX, company.Fax,
                    COMMUNICATION_LOCATION_CODE_CORPORATE, countryCode);
                AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_EMAIL, company.Email,
                    COMMUNICATION_LOCATION_CODE_CORPORATE);
                AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_WEB, company.WebURL,
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
                    companyInfo = GetCompanyInfo(result.MasterCustomerId, subCustomerId);
                }
            }
            else
            {
                AddPhoneNumber(company.Phone, countryCode, companyInfo);
            }
            if (companyInfo != null) company.ExternalReference = companyInfo.MasterCustomerId + ";" + companyInfo.SubCustomerId;
            PersonifyClient.AddCustomerAddresses(company, companyInfo, countryCodes);
            return companyInfo;
        }

        public static IEnumerable<StoreAddressInfo> AddCustomerAddresses(
            StoreCompany storeCompany,
            CustomerInfo customerInfo,
            IEnumerable<LookSendMyAdCountryCode> countryCodes)
        {
            if (storeCompany == null || storeCompany.Addresses == null || customerInfo == null)
            {
                throw new Exception("Store company and addresses, customer personify information and country codes are required");
            }
            IList<AddressInfo> existingAddressInfos = SvcClient.Ctxt.AddressInfos.Where(
                a => a.MasterCustomerId == customerInfo.MasterCustomerId && a.SubCustomerId == customerInfo.SubCustomerId).ToList();
            IEnumerable<StoreAddressInfo> storeCompanyAddresses = ProcessStoreAddresses(storeCompany, countryCodes);
            storeCompanyAddresses = ProcessPersonifyAddresses(storeCompanyAddresses, customerInfo, existingAddressInfos);
            if (storeCompanyAddresses.Any(a => a.PersonifyAddr == null))
            {
                AddressInfo existingPrimaryAddress = existingAddressInfos.FirstOrDefault(a => a.PrioritySeq == 0);
                if (existingPrimaryAddress == null)
                {
                    storeCompanyAddresses = storeCompanyAddresses.Select(addr =>
                    {
                        if (addr.PrioritySeq.HasValue && addr.PrioritySeq.Value == 0)
                        {
                            AddCustomerAddress(addr, null, customerInfo, storeCompany.Name);
                            existingPrimaryAddress = addr.PersonifyAddr;
                        }
                        return addr;
                    }).ToList();
                }
                storeCompanyAddresses = storeCompanyAddresses.Select(addr =>
                {
                    if (addr.PersonifyAddr == null && !addr.IsAdded)
                        AddCustomerAddress(addr, existingPrimaryAddress, customerInfo, storeCompany.Name);
                    return addr;
                }).ToList();
            }
            return storeCompanyAddresses;
        }

        private static IList<StoreAddressInfo> ProcessStoreAddresses(StoreCompany storeCompany, IEnumerable<LookSendMyAdCountryCode> countryCodes)
        {
            IList<StoreAddressInfo> storeCompanyAddresses = new List<StoreAddressInfo>(3);
            StoreAddress primaryAddress = storeCompany.GetCompanyAddress();
            StoreAddress billToAddress = storeCompany.GetCompanyBillingAddress();
            StoreAddress shipToAdress = storeCompany.GetCompanyShippingAddress();
            var addr = new StoreAddressInfo()
            {
                StoreAddr = primaryAddress,
                StoreIsPrimary = true
            };
            if (primaryAddress.Equals(billToAddress))
            {
                addr.PersonifyIsBilling = true;
                addr.StoreIsBilling = true;
            }
            if (primaryAddress.Equals(shipToAdress))
            {
                addr.PersonifyIsShipping = true;
                addr.StoreIsShipping = true;
            }
            storeCompanyAddresses.Add(addr);
            if (!billToAddress.Equals(primaryAddress))
            {
                addr = new StoreAddressInfo()
                {
                    StoreAddr = billToAddress,
                    PersonifyIsBilling = true,
                    StoreIsBilling = true
                };
                if (billToAddress.Equals(shipToAdress))
                {
                    addr.PersonifyIsShipping = true;
                    addr.StoreIsShipping = true;
                }
                storeCompanyAddresses.Add(addr);
            }
            if (!shipToAdress.Equals(primaryAddress) && !shipToAdress.Equals(billToAddress))
            {
                storeCompanyAddresses.Add(new StoreAddressInfo()
                {
                    StoreAddr = shipToAdress,
                    PersonifyIsShipping = true,
                    StoreIsShipping = true
                });
            }
            storeCompanyAddresses = storeCompanyAddresses.Select(a =>
            {
                a.CountryCode = countryCodes != null ? countryCodes.Alpha3Code(a.StoreAddr.Country) : a.StoreAddr.Country;
                return a;
            }).ToList();
            return storeCompanyAddresses;
        }

        private static IEnumerable<StoreAddressInfo> ProcessPersonifyAddresses(
            IEnumerable<StoreAddressInfo> customerAddresses, CustomerInfo customerInfo, IList<AddressInfo> existingAddressInfos)
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
                addr.CustuInfo = customerInfo;
                return addr;
            });
            return customerAddresses;
        }

        private static SaveAddressOutput AddCustomerAddress(
            StoreAddressInfo storeAddressInfo,
            AddressInfo existingPrimaryAddress,
            CustomerInfo customerInfo,
            string companyName)
        {
            var newCustomerAddress = new SaveAddressInput()
            {
                MasterCustomerId = customerInfo.MasterCustomerId,
                SubCustomerId = customerInfo.SubCustomerId,
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
                   a => a.MasterCustomerId == customerInfo.MasterCustomerId
                     && a.SubCustomerId == customerInfo.SubCustomerId
                     && a.CustomerAddressId == result.CusAddressId).ToList().FirstOrDefault();
            return result;
        }

        #region Getting company information

        public static CompanyInformation GetCompanyInfo(CustomerInfo customerInfo)
        {
            var customers = SvcClient.Ctxt.ASICustomerInfos.Where(p => p.MasterCustomerId == customerInfo.MasterCustomerId && p.SubCustomerId == customerInfo.SubCustomerId).ToList();
            if (customers.Count == 0) return null;
            return GetCompanyInfo(customers[0]);
        }

        public static CompanyInformation GetCompanyInfoByASINumber(string asiNumber)
        {
            var customers = SvcClient.Ctxt.ASICustomerInfos.Where(p => p.UserDefinedAsiNumber == asiNumber).ToList();
            if (customers.Count == 0) return null;
            return GetCompanyInfo(customers[0]);
        }

        public static CompanyInformation GetCompanyInfoByIdentifier(int companyIdentifier)
        {
            var customers = SvcClient.Ctxt.ASICustomerInfos.Where(p => p.UserDefinedCustomerNumber == companyIdentifier).ToList();
            if (customers.Count == 0) return null;
            return GetCompanyInfo(customers[0]);
        }

        private static CompanyInformation GetCompanyInfo(ASICustomerInfo customerInfo)
        {
            var company = new CompanyInformation
            {
                ASINumber = customerInfo.UserDefinedAsiNumber,
                Name = customerInfo.LabelName,
                MasterCustomerId = customerInfo.MasterCustomerId,
                SubCustomerId = customerInfo.SubCustomerId,
                MemberType = customerInfo.CustomerClassCodeString,
                MemberStatus = customerInfo.UserDefinedMemberStatusString,
            };
            if (customerInfo.UserDefinedCustomerNumber.HasValue)
            {
                company.CompanyId = Convert.ToInt32(customerInfo.UserDefinedCustomerNumber.Value);
            }
            //get the company primary address
            List<AddressInfo> companyAddressInfos = SvcClient.Ctxt.AddressInfos.Where(
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
            return company;
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

        #endregion Getting company information

        public static IEnumerable<CustomerInfo> AddIndividualInfos(StoreOrder storeOrder,
            IList<LookSendMyAdCountryCode> countryCodes,
            CustomerInfo companyInfo)
        {
            if (storeOrder == null || storeOrder.Company == null)
            {
                throw new Exception("Order or company can't be null.");
            }
            if(companyInfo == null) throw new Exception("Company information is needed.");
            StoreCompany storeCompany = storeOrder.Company;
            StoreAddress companyAddress = storeCompany.GetCompanyAddress();
            string countryCode = countryCodes.Alpha3Code(companyAddress.Country);
            var allCustomers = new List<CustomerInfo>();

            foreach (var storeIndividual in storeCompany.Individuals)
            {
                CustomerInfo customerInfo = null;
                //check if individual already exist based on email
                List<CusCommunication> communications = SvcClient.Ctxt.CusCommunications.
                    Where(comm => comm.SearchPhoneAddress == storeIndividual.Email).ToList();
                if (communications.Count > 0)
                {
                    //we already have a contact with that email
                    customerInfo = GetIndividualInfo(communications[0].MasterCustomerId);
                    //check if contact belong to company
                    try
                    {
                        List<CusRelationship> relationships = SvcClient.Ctxt.CusRelationships
                            .Where(rel => rel.MasterCustomerId == customerInfo.MasterCustomerId
                                        && rel.RelatedMasterCustomerId == companyInfo.MasterCustomerId
                                        && rel.RelatedSubCustomerId == companyInfo.SubCustomerId).ToList();
                        if (relationships.Count == 0)
                        {
                            //also link this user to the company
                            AddRelationship(customerInfo, companyInfo);
                        }
                    }
                    catch (Exception ex)
                    {
                        string s = string.Format("customerInfo.MasterCustomerId = {0}", customerInfo.MasterCustomerId)
                                   + string.Format("\ncompanyInfo.MasterCustomerId = {0}", companyInfo.MasterCustomerId)
                                   + string.Format("\ncompanyInfo.SubCustomerId = {0}\n", companyInfo.SubCustomerId)
                                   + ex.Message
                                   + ex.StackTrace;
                        throw new Exception(s, ex);
                    }
                }
                else
                {
                    //check if there is a contact with same name for same company already
                    customerInfo = GetIndividualInfo(storeIndividual.FirstName, storeIndividual.LastName, companyInfo);
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
                        customerInfo = GetIndividualInfo(customerInfoOutput.MasterCustomerId);
                        AddRelationship(customerInfo, companyInfo);
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

        private static void AddRelationship(CustomerInfo customerInfo, CustomerInfo companyInfo)
        {
            var cusRelationship = SvcClient.Create<CusRelationship>();
            cusRelationship.AddedBy = ADDED_OR_MODIFIED_BY;
            //Provide values and Save
            cusRelationship.MasterCustomerId = customerInfo.MasterCustomerId;
            cusRelationship.SubCustomerId = customerInfo.SubCustomerId;

            cusRelationship.RelationshipType = "EMPLOYMENT";
            cusRelationship.RelationshipCode = "Employee";

            cusRelationship.RelatedMasterCustomerId = companyInfo.MasterCustomerId;
            cusRelationship.RelatedSubCustomerId = companyInfo.SubCustomerId;
            cusRelationship.ReciprocalCode = "Employer";
            cusRelationship.BeginDate = DateTime.Now.AddDays(-1);
            SvcClient.Save<CusRelationship>(cusRelationship);
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

        public static CustomerInfo GetIndividualInfoByEmail(string emailAddress)
        {
            CustomerInfo customerInfo = null;
            List<CusCommunication> comms = SvcClient.Ctxt.CusCommunications.
                 Where(comm => comm.SearchPhoneAddress == emailAddress).ToList();
            if (comms.Any())
            {
                customerInfo = GetIndividualInfo(comms[0].MasterCustomerId);
            }
            return customerInfo;
        }

        public static IEnumerable<StoreAddressInfo> AddIndividualAddresses(
           StoreCompany storeCompany,
           IEnumerable<CustomerInfo> individualInfos,
           IEnumerable<LookSendMyAdCountryCode> countryCodes)
        {
            var storeIndividualInfos = individualInfos.SelectMany(individual =>
            {
                return AddCustomerAddresses(storeCompany, individual, countryCodes);
            });
            return storeIndividualInfos;
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
            string countryCode = null)
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                if ((key == COMMUNICATION_INPUT_PHONE || key == COMMUNICATION_INPUT_FAX) && !string.IsNullOrWhiteSpace(key))
                {
                    value = new string(value.Where(Char.IsDigit).ToArray());
                    value = value.Substring(0, Math.Min(value.Substring(0).Length, PHONE_NUMBER_LENGTH));
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
            if (string.IsNullOrEmpty(phoneNumber)) return respSave;
            phoneNumber = new string(phoneNumber.Where(Char.IsDigit).ToArray());
            IList<CusCommunication> oCusComms = GetCusCommunications(companyInfo, COMMUNICATION_INPUT_PHONE);
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
                    respCreate.MasterCustomerId = companyInfo.MasterCustomerId;
                    respCreate.SubCustomerId = companyInfo.SubCustomerId;
                    respCreate.CommLocationCodeString = commTypes2.First();
                    respCreate.CommTypeCodeString = COMMUNICATION_INPUT_PHONE;
                    respCreate.PrimaryFlag = false;
                    if (!string.IsNullOrWhiteSpace(countryCode)
                        && string.Equals(countryCode, "USA", StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(countryCode, "CAN", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (phoneNumber.Length == PHONE_NUMBER_LENGTH && !IsPhoneExist(phoneNumber, companyInfo))
                        {
                            respCreate.CountryCode = countryCode;
                            respCreate.PhoneAreaCode = phoneNumber.Substring(0, 3);
                            respCreate.PhoneNumber = phoneNumber.Substring(3, 7);
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
                catch (Exception ex)
                {
                    respSave = null;
                }
            }
            return respSave;
        }

        private static bool IsPhoneExist(string phoneNumber, CustomerInfo companyInfo)
        {
            return GetCusCommunications(companyInfo, COMMUNICATION_INPUT_PHONE).Any(c => c.SearchPhoneAddress == phoneNumber);
        }

        private static IList<CusCommunication> GetCusCommunications(CustomerInfo companyInfo, string cusCommType)
        {
            IEnumerable<CusCommunication> cc = SvcClient.Ctxt.CusCommunications
                .Where(c => c.MasterCustomerId == companyInfo.MasterCustomerId
                         && c.SubCustomerId == companyInfo.SubCustomerId);
            return cc.Where(c => string.Equals(c.CommTypeCodeString, cusCommType, StringComparison.InvariantCultureIgnoreCase)).ToList();
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
                string ccReference = GetCreditCardReference(creditCard.Number);
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
                throw new ArgumentException(
                    string.Format("Billto address and company information are required for order {0}, created by Company {1}", 
                    orderNumber, companyInfo.MasterCustomerId));
            }
            if (string.IsNullOrWhiteSpace(ccProfileid))
            {
                throw new Exception(string.Format("Creadit card profile id is null for order {0}, created by Company {1}", 
                    orderNumber, companyInfo.MasterCustomerId));
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

    public enum AddressType
    {
        Primary,
        Shipping,
        Billing
    }
}