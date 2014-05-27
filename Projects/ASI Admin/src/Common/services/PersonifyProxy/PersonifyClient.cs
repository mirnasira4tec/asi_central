using System.Configuration;
using System.Net.Sockets;
using System.Web.Mvc;
using asi.asicentral.model.ROI;
using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using asi.asicentral.util.store.companystore;
using System.Threading.Tasks;
using asi.asicentral.model;
using asi.asicentral.PersonifyDataASI;
using System.Diagnostics;
using ASI.EntityModel;
using DotLiquid.Tags;
using PersonifySvcClient;
using asi.asicentral.interfaces;

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

		public static CreateOrderOutput CreateOrder(StoreOrder storeOrder, CustomerInfo companyInfo, IList<CreateOrderLineInput> lineItems)
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
					ShipMasterCustomerID = companyInfo.MasterCustomerId,
					ShipSubCustomerID = Convert.ToInt16(companyInfo.SubCustomerId),
					OrderLines = orderLineInputs,
					AddedOrModifiedBy = ADDED_OR_MODIFIED_BY,
				};
			var resp = SvcClient.Post<CreateOrderOutput>("CreateOrder", createOrderInput);
			return resp;
		}

		public static bool ComparePriceFromStoreAndPersonify(StoreOrder storeOrder, string masterCustomerId)
		{
			IList<WebOrderBalanceView> oOrdBalInfo = SvcClient.Ctxt.WebOrderBalanceViews
				.Where(o => o.OrderNumber == masterCustomerId).ToList();
			decimal personifyTotal = 0;
			if (oOrdBalInfo.Count > 0)
			{
				personifyTotal = oOrdBalInfo.Sum(o => Convert.ToDecimal(o.ActualBalanceAmount));
			}
			return personifyTotal == storeOrder.Total;
		}

		public static IEnumerable<AddressInfo> GetBillingAndShippingAddresses(StoreOrder storeOrder)
		{
			StoreCompany storeCompany = storeOrder.Company;
			CustomerInfo companyInfo = GetCompanyInfoByName(storeCompany.Name);
			if (companyInfo == null)
			{
				throw new Exception("Company information is not available in Personify.");
			}
			List<AddressInfo> companyAddressInfos = SvcClient.Ctxt.AddressInfos.Where(
			   a => a.MasterCustomerId == companyInfo.MasterCustomerId).ToList();
			AddressInfo companyAddressInfo = companyAddressInfos.SingleOrDefault(a => a.PrioritySeq == 0);
			if (companyAddressInfos == null || companyAddressInfos.Count <= 0 || companyAddressInfo == null)
			{
				throw new Exception("Company address is not available in Personify.");
			}
			AddressInfo billingAddressInfo = companyAddressInfos.FirstOrDefault(a => a.BillToFlag ?? false);
			AddressInfo shippingAddressInfo = companyAddressInfos.FirstOrDefault(a => a.ShipToFlag ?? false);

			billingAddressInfo = (billingAddressInfo ?? companyAddressInfo);
			billingAddressInfo.BillToFlag = true;
			shippingAddressInfo = shippingAddressInfo ?? companyAddressInfo;
			shippingAddressInfo.BillToFlag = true;
			return new List<AddressInfo>() { billingAddressInfo, shippingAddressInfo };
		}

		public static bool ValidateCreditCard(CreditCard info)
		{
			var companyinfo = GetPersonifyCreditCardCompany();
			if (companyinfo == null) return true;

			var asiValidateCreditCardInput = new ASIValidateCreditCardInput()
			{
				ReceiptType = CreditCardType[info.Type.ToUpper()],
				CreditCardNumber = info.Number
			};
			var resp = SvcClient.Post<ASIValidateCreditCardOutput>("ASIValidateCreditCard", asiValidateCreditCardInput);
			return resp.IsValid ?? false;
		}

		public static string SaveCreditCard(CreditCard info)
		{
			Dictionary<string, string> companyInfo = GetPersonifyCreditCardCompany();
			if (companyInfo == null) return "545235";

			var customerCreditCardInput = new CustomerCreditCardInput()
				{
					MasterCustomerId = companyInfo["MasterCustomerId"],
					SubCustomerId = Convert.ToInt32(companyInfo["SubCustomerId"]),
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
					CompanyNumber = companyInfo["CompanyNumber"],
					AddedOrModifiedBy = ADDED_OR_MODIFIED_BY
				};
			var resp = SvcClient.Post<CustomerCreditCardOutput>("AddCustomerCreditCard", customerCreditCardInput);
			return resp.Success ?? false ? resp.CreditCardProfileId : null;
		}

		public static string GetCreditCardProfileId(CreditCard creditCard)
		{
			Dictionary<string, string> companyInfo = GetPersonifyCreditCardCompany();
			if (companyInfo == null) return "545235";

			IEnumerable<ASICustomerCreditCard> oCreditCards = SvcClient.Ctxt.ASICustomerCreditCards
				.Where(c => c.MasterCustomerId == companyInfo["MasterCustomerId"]
						 && c.SubCustomerId == Convert.ToInt32(companyInfo["SubCustomerId"])
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

		public static CustomerInfo AddCompanyInfo(StoreOrder order, IList<LookSendMyAdCountryCode> countryCodes)
		{
			CustomerInfo companyInfo = null;
			StoreCompany storeCompany = order.Company;

			if (storeCompany == null || string.IsNullOrWhiteSpace(storeCompany.Name))
			{
				throw new Exception("Store company is not valid.");
			}
			//look company by ASI#
			if (!string.IsNullOrEmpty(storeCompany.ASINumber))
			{
				companyInfo = GetCompanyInfoByAsiNumber(order.Company.ASINumber);
			}
			else
			{
				companyInfo = GetCompanyInfoByName(storeCompany.Name);
			}
			if (companyInfo != null)
			{
                List<AddressInfo> addressInfos = SvcClient.Ctxt.AddressInfos.Where(
                                        a => a.MasterCustomerId == companyInfo.MasterCustomerId).ToList();
			    IList<StoreCompanyAddress> addresses =
                    storeCompany.Addresses.Where(a => addressInfos.All(a2 => a2.Address1 != a.Address.Street1
                                                                             && a2.PostalCode != a.Address.Zip)).ToList();
                if (addresses.Count > 0)
                {
                    StoreCompanyAddress billTo = addresses.FirstOrDefault(a => a.IsBilling);
                    if (billTo != null)
                    {
                        var countryCode = countryCodes.Alpha3Code(billTo.Address.Country);
                        AddCompanyAddress(billTo, companyInfo, storeCompany.Name, countryCode);
                    }
                    StoreCompanyAddress shipTo = addresses.FirstOrDefault(a => a.IsShipping);
                    if (shipTo != null)
                    {
                        var countryCode = countryCodes.Alpha3Code(shipTo.Address.Country);
                        AddCompanyAddress(shipTo, companyInfo, storeCompany.Name, countryCode);
                    }
                    StoreCompanyAddress companyAddress = addresses.FirstOrDefault(a => !a.IsShipping && !a.IsBilling);
                    if (companyAddress != null)
                    {
                        var countryCode = countryCodes.Alpha3Code(companyAddress.Address.Country);
                        AddCompanyAddress(companyAddress, companyInfo, storeCompany.Name, countryCode);
                    }
                }
			    //@todo need to reconcile the addresses
			}
			else {
				//company not already there, create a new one
				StoreAddress companyAddress = storeCompany.GetCompanyAddress();
				bool isUsaAddress = countryCodes.IsUSAAddress(companyAddress.Country);
				string countryCode = countryCodes.Alpha3Code(companyAddress.Country);
				var saveCustomerInput = new SaveCustomerInput { LastName = storeCompany.Name, CustomerClassCode = "UNKNOWN"};
				AddCompanyAddresses(saveCustomerInput, storeCompany, countryCodes);
				AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_PHONE, storeCompany.Phone, COMMUNICATION_LOCATION_CODE_CORPORATE, countryCode, isUsaAddress);
				AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_FAX, storeCompany.Fax, COMMUNICATION_LOCATION_CODE_CORPORATE, countryCode, isUsaAddress);
				AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_EMAIL, storeCompany.Email, COMMUNICATION_LOCATION_CODE_CORPORATE);
				AddCusCommunicationInput(saveCustomerInput, COMMUNICATION_INPUT_WEB, storeCompany.WebURL, COMMUNICATION_LOCATION_CODE_CORPORATE);
				var result = SvcClient.Post<SaveCustomerOutput>("CreateCompany", saveCustomerInput);
				if (result != null && string.IsNullOrWhiteSpace(result.WarningMessage))
				{
					var subCustomerId = result.SubCustomerId.HasValue ? result.SubCustomerId.Value : 0;
					//try update status - not caring so much whether it works or not
					var q = SvcClient.Ctxt.ASICustomers.Where(p => p.MasterCustomerId == result.MasterCustomerId && p.SubCustomerId == subCustomerId).Select(o => o);
					var customers = new DataServiceCollection<ASICustomer>(q, TrackingMode.None);
					if (customers.Count > 0)
					{
						ASICustomer customer = customers[0];
						customer.UserDefinedAsiStatus = "ACTIVE";
						SvcClient.Save<ASICustomer>(customer);
					}
					companyInfo = GetCompanyInfo(result.MasterCustomerId, subCustomerId);
				}
			}
			return companyInfo;
		}

        private static SaveAddressOutput AddCompanyAddress(StoreCompanyAddress address, CustomerInfo companyInfo, string companyName, string countryCode)
        {
            SaveAddressOutput result = null;
	        AddressInfo primaryAddress = companyInfo.Addresses.FirstOrDefault(a => a.PrioritySeq == 0);
	        if (primaryAddress != null)
	        {
	            var newCustomerAddress = new SaveAddressInput()
	            {
                    OwnerAddressId = Convert.ToInt32(primaryAddress.CustomerAddressId),
                    MasterCustomerId = primaryAddress.MasterCustomerId,
                    SubCustomerId = primaryAddress.SubCustomerId,
                    AddressTypeCode = COMMUNICATION_LOCATION_CODE_CORPORATE,
                    Address1 = address.Address.Street1,
                    Address2 = address.Address.Street2,
                    City = address.Address.City,
                    State = address.Address.State,
                    PostalCode = address.Address.Zip,
                    CountryCode = countryCode,
                    BillToFlag = address.IsBilling,
                    ShipToFlag = address.IsShipping,
                    DirectoryFlag = true,
                    CompanyName = companyName,
                    WebMobileDirectory = false,
                    CreateNewAddressIfOrdersExist = true,
                    OverrideAddressValidation = true,
                    AddedOrModifiedBy = ADDED_OR_MODIFIED_BY
	            };
	            result = PersonifySvcClient.SvcClient.Post<SaveAddressOutput>("CreateOrUpdateAddress", newCustomerAddress);
	        }
	        return result;
	    }

		public static SaveCustomerOutput AddCompanyByNameAndMemberTypeId(string companyName, int memberTypeId)
		{
			MemberData memberData = MemberTypeIDToCD.Data[memberTypeId];
			if (string.IsNullOrWhiteSpace(companyName) || memberData == null)
			{
				throw new Exception("Company name or member type id is not valid.");
			}
			var saveCustomerInput = CreateCompanyCustomerInfoInput(companyName, memberData);
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

		public static CustomerInfo GetCompanyInfoByName(string companyName)
		{
			CustomerInfo customerInfo = null;
			if (companyName != null)
			{
				var oCusInfo = SvcClient.Ctxt.CustomerInfos.Where(
				  a => a.LabelName == companyName && a.RecordType == RECORD_TYPE_CORPORATE).ToList();
				customerInfo = oCusInfo.Count == 0 ? null : oCusInfo.FirstOrDefault();
			}
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
						var customerInfo = CreateIndividualCustomerInfoInput(storeIndividual);
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
			var result2 = Task.WhenAll(saveCustomerOutputs);
			return result2.Result;
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

		public static Dictionary<string, string> GetPersonifyCreditCardCompany()
		{
			Dictionary<string, string> result = null;
			string personifyCreditCardCompany = ConfigurationManager.AppSettings["PersonifyCreditCardCompany"];
			if (personifyCreditCardCompany != null)
			{
				string[] companyInfos = personifyCreditCardCompany.Split(new char[] { ';' });
				result = companyInfos.ToDictionary(item =>
					item.Substring(0, item.IndexOf('=')).Trim(),
					item => item.Substring(item.IndexOf('=') + 1).Trim(),
					StringComparer.InvariantCultureIgnoreCase);
			}
			return result;
		}

		private static SaveCustomerInput CreateIndividualCustomerInfoInput(StoreIndividual storeIndividual)
		{
			var customerInfo = new SaveCustomerInput
			{
				FirstName = storeIndividual.FirstName,
				LastName = storeIndividual.LastName,
				CustomerClassCode = CUSTOMER_CLASS_INDIV
			};
			return customerInfo;
		}

		private static SaveCustomerInput AddIndividualAddress(SaveCustomerInput customerInfo, StoreIndividual storeIndividual, StoreCompany storeCompany)
		{
			CustomerInfo companyInfo = GetCompanyInfoByName(storeCompany.Name);
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

		private static SaveCustomerInput CreateCompanyCustomerInfoInput(string companyName, MemberData memberData)
		{
			var customerInfo = new SaveCustomerInput
			{
				LastName = companyName,
				CustomerClassCode = memberData.MemberTypeCD,
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
					CompanyName = storeCompany.Name,
					AddedOrModifiedBy = ADDED_OR_MODIFIED_BY,
					PrioritySeq = prioritySeq
				};
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
			var asiValidateCreditCardInput = new ASIValidateCreditCardInput()
			{
				ReceiptType = receiptType,
				CreditCardNumber = creditCardNum
			};
			var resp = SvcClient.Post<ASIValidateCreditCardOutput>("ASIValidateCreditCard", asiValidateCreditCardInput);
			return resp.IsValid ?? false;
		}
	}
}