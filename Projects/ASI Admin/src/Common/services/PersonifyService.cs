﻿using asi.asicentral.interfaces;
using asi.asicentral.model;
using asi.asicentral.model.personify;
using asi.asicentral.model.store;
using asi.asicentral.model.timss;
using asi.asicentral.oauth;
using asi.asicentral.PersonifyDataASI;
using asi.asicentral.services.PersonifyProxy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace asi.asicentral.services
{
    public class PersonifyService : IBackendService, IDisposable
    {
        private ILogService log = null;
        private readonly IStoreService storeService;
        private bool disposed = false;

        public PersonifyService()
        {
            log = LogService.GetLog(this.GetType());
        }

        public PersonifyService(IStoreService storeService)
        {
            log = LogService.GetLog(this.GetType());
            this.storeService = storeService;
        }

        public virtual void PlaceOrder(StoreOrder order, IEmailService emailService, string url)
        {
            log.Debug(string.Format("Place order Start : {0}", order));
            IList<LookSendMyAdCountryCode> countryCodes = storeService.GetAll<LookSendMyAdCountryCode>(true).ToList();
            if (order == null || order.Company == null || order.Company.Individuals == null || countryCodes == null)
                throw new ArgumentException("You must pass a valid order and the country codes");
            try
            {
                IEnumerable<StoreAddressInfo> storeAddress = null;
                var memberType = string.IsNullOrEmpty(order.Company.MemberType) ? "UNKNOWN" : order.Company.MemberType;
                var companyInfo = PersonifyClient.ReconcileCompany(order.Company, memberType, countryCodes, ref storeAddress, true);

                log.Debug(string.Format("Reconciled company '{1}' to order '{0}'.", order, companyInfo.MasterCustomerId + ";" + companyInfo.SubCustomerId));

                var individualInfos = PersonifyClient.AddIndividualInfos(order.Company, countryCodes, companyInfo.MasterCustomerId, companyInfo.SubCustomerId).ToList();
                if (!individualInfos.Any()) throw new Exception("Failed in creating individuald in Personify.");
                log.Debug(string.Format("Added individuals to company '{1}' to order '{0}'.", order, companyInfo.MasterCustomerId + ";" + companyInfo.SubCustomerId));

                var contactAddresses = individualInfos.SelectMany(i =>
                                        {
                                            return PersonifyClient.AddCustomerAddresses(order.Company, i.MasterCustomerId, i.SubCustomerId, countryCodes);
                                        }).ToList();

                log.Debug(string.Format("Address added to individuals to the order '{0}'.", order));

                StoreIndividual primaryContact = order.GetContact();
                var primaryContactInfo = individualInfos.FirstOrDefault(c =>
                    string.Equals(c.FirstName, primaryContact.FirstName, StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(c.LastName, primaryContact.LastName, StringComparison.InvariantCultureIgnoreCase));

                if (primaryContactInfo == null && !string.IsNullOrEmpty(primaryContact.Email))
                {
                    primaryContactInfo = PersonifyClient.GetIndividualInfoByEmail(primaryContact.Email);
                }

                var contactMasterId = primaryContactInfo != null ? primaryContactInfo.MasterCustomerId : individualInfos[0].MasterCustomerId;
                var contactSubId = primaryContactInfo != null ? primaryContactInfo.SubCustomerId : individualInfos[0].SubCustomerId;

                var shipToAddr = GetAddressInfo(contactAddresses, AddressType.Shipping, order).PersonifyAddr;
                var billToAddr = storeAddress.FirstOrDefault(a => a.StoreIsBilling == true).PersonifyAddr;

                var orderDetail = order.OrderDetails[0];
                var mappings = storeService.GetAll<PersonifyMapping>(true)
                                           .Where(map => (order.ContextId == null || map.StoreContext == order.ContextId ) &&
                                                         map.StoreProduct == orderDetail.Product.Id &&
                                                         map.PersonifyRateStructure == "BUNDLE").ToList();
                if( mappings.Count > 0 )
                {
                    PersonifyMapping mapping = null;
                    bool waiveAppFee = false;
                    bool firstmonthFree = false;
                    var coupon = orderDetail.Coupon;
                    bool couponError = false;
                    coupon.CouponCode = coupon.CouponCode.Trim();

                    if( coupon != null && !string.IsNullOrEmpty(coupon.CouponCode) && coupon.CouponCode != "(Unknown)")
                    {
                        mapping = mappings.FirstOrDefault(m => !string.IsNullOrEmpty(m.StoreOption) && m.StoreOption.Trim() == coupon.CouponCode);

                        var context = storeService.GetAll<Context>(true).FirstOrDefault(p => p.Id == order.ContextId);
                        var contextProduct = context != null ? context.Products.FirstOrDefault(p => p.Product.Id == orderDetail.Product.Id) : null;

                        if (contextProduct != null && coupon.IsFixedAmount)
                        {
                            if (coupon.DiscountAmount == contextProduct.Cost)
                            {
                                firstmonthFree = true;
                            }
                            else
                            {
                                waiveAppFee = coupon.DiscountAmount >= contextProduct.ApplicationCost;
                                firstmonthFree = !waiveAppFee && coupon.DiscountAmount >= contextProduct.Cost ||
                                                  waiveAppFee && coupon.DiscountAmount >= contextProduct.ApplicationCost + contextProduct.Cost;
                            }

                            couponError = coupon.DiscountAmount != contextProduct.ApplicationCost &&
                                          coupon.DiscountAmount != contextProduct.Cost &&
                                          coupon.DiscountAmount != contextProduct.ApplicationCost + contextProduct.Cost;
                        }
                        else
                            couponError = true; 
                    }

                    if (mapping == null)
                    {
                        mapping = mappings.FirstOrDefault(m => string.IsNullOrEmpty(m.StoreOption));
                    }

                    PersonifyClient.CreateBundleOrder(order, mapping, companyInfo, contactMasterId, contactSubId, 
                                                      billToAddr, shipToAddr, waiveAppFee, firstmonthFree);

                    ValidateOrderTotal(order, emailService, url, true, firstmonthFree);

                    // update company ASI#, to be included in the internal email
                    var asiNumber = PersonifyClient.GetCompanyAsiNumber(companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
                    if ( !string.IsNullOrWhiteSpace(asiNumber) && asiNumber.Trim().Length < 7 )
                    {
                        order.Company.ASINumber = asiNumber;
                    }

                    if( couponError)
                    {   // send internal email 
                        var data = new EmailData()
                        {
                            Subject = "There is a problem with coupon discount amount for an order from the store to Personify",
                            EmailBody = string.Format("A new order created in the store ({0}) has been transferred to a Personify "
                            + "order ({1}). There is problem with coupon discount {2}, the order needs to be looked at. {3}",
                            order.Id.ToString(), order.BackendReference, coupon.IsFixedAmount ? coupon.DiscountAmount : coupon.DiscountPercentage, EmailData.GetMessageSuffix(url))
                        };

                        data.SendEmail(emailService);
                    }
                }
                else
                {
                    var lineItems = GetPersonifyLineInputs(order, shipToAddr.CustomerAddressId);
                    log.Debug(string.Format("Retrieved the line items to the order '{0}'.", order.ToString()));
                    var orderOutput = PersonifyClient.CreateOrder(
                        order,
                        companyInfo.MasterCustomerId,
                        companyInfo.SubCustomerId,
                        contactMasterId, 
                        contactSubId,
                        billToAddr.CustomerAddressId,
                        shipToAddr.CustomerAddressId,
                        lineItems);
                    log.Debug(string.Format("The order '{0}' has been created in Personify.", order));

                    order.BackendReference = orderOutput.OrderNumber;
                    decimal orderTotal = ValidateOrderTotal(order, emailService, url);

                    try
                    {
                        PersonifyClient.PayOrderWithCreditCard(orderOutput.OrderNumber, orderTotal, order.CreditCard.ExternalReference, 
                                                               billToAddr, companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
                        log.Debug(string.Format("Payed the order '{0}'.", order));
                        log.Debug(string.Format("Place order End: {0}", order));
                    }
                    catch (Exception e)
                    {
                        string s = string.Format("Failed to pay the order '{0} {3}'. Error is {2}{1}", order, e.StackTrace, e.Message, orderOutput.OrderNumber);
                        log.Error(s);
                        var data = new EmailData()
                        {
                            Subject = "order failed to be charged",
                            EmailBody = s + EmailData.GetMessageSuffix(url)
                        };
                        data.SendEmail(emailService);
                        log.Debug(string.Format("Place order End: {0}", order));
                    }
                }
            }
            catch (Exception ex)
            {
                string error1 = string.Format("Unknown Error while adding order to personify: {0}{1}", ex.Message, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    error1 = string.Format("Unknown Error while adding order to personify: {0}{1}\n{2}",
                        ex.InnerException.Message, ex.InnerException.StackTrace, error1);
                }
                string error2 = string.Format("Place order End: {0}", order);
                log.Error(error1);
                log.Debug(error2);
                var data = new EmailData()
                {
                    Subject = error2,
                    EmailBody = error1 + "<br /><br />" + error2 + EmailData.GetMessageSuffix(url)
                };
                data.SendEmail(emailService);
                throw ex;
            }
        }

        private decimal ValidateOrderTotal(StoreOrder order, IEmailService emailService, string url, 
                                           bool bundleOrder = false, bool firstMonthFree = false)
        {
            if (order == null)
            {
                throw new Exception("Order is null");
            }

            decimal backEndTotal = PersonifyClient.GetOrderBalanceTotal(order.BackendReference);
            log.Debug(string.Format("Got the order total {0} of for the order '{1}'.", backEndTotal, order));

            decimal orderTotal = 0;
            if (!firstMonthFree)
            {
                if (!bundleOrder)
                {
                    orderTotal = order.Total;
                }
                else if (order.OrderDetails != null && order.OrderDetails.Count > 0)
                    orderTotal = order.OrderDetails[0].Cost + order.OrderDetails[0].TaxCost;
            }
            else
                orderTotal = order.AnnualizedTotal;

            if (backEndTotal != orderTotal)
            {
                var data = new EmailData()
                {
                    Subject = "here is a price discrepancy for an order from the store to Personify",
                    EmailBody = string.Format("A new order created in the store ({0}) has been transferred to a Personify "
                    + "order ({1}). The prices do not match, the order needs to be looked at. The store price is {2:C} and "
                    + "the Personify price is {3:C}.{4}", order.Id.ToString(), order.BackendReference, orderTotal, backEndTotal, EmailData.GetMessageSuffix(url))
                };
                data.SendEmail(emailService);
            }

            return backEndTotal;
        }

        private StoreAddressInfo GetAddressInfo(IList<StoreAddressInfo> addresses, AddressType type, StoreOrder order)
        {
            var addr = addresses.FirstOrDefault(a =>
            {
                if (type == AddressType.Shipping) return a.StoreIsShipping && !a.StoreIsPrimary;
                if (type == AddressType.Billing) return a.StoreIsBilling && !a.StoreIsPrimary;
                return false;
            });
            if (addr == null)
            {
                addr = addresses.FirstOrDefault(a =>
                {
                    if (type == AddressType.Shipping) return a.StoreIsShipping;
                    if (type == AddressType.Billing) return a.StoreIsBilling;
                    return false;
                });
            };
            if (addr == null || addr.PersonifyAddr == null)
            {
                string s = string.Format("Shipping and billing personify customer addresses are required for order {0}.", order.ToString());
                log.Debug(s);
                throw new Exception(s);
            }
            return addr;
        }

        public virtual bool IsProcessUsingBackend(StoreOrderDetail orderDetail)
        {
            log.Debug(string.Format("Check if {0} is processed using backend.", orderDetail.ToString()));
            bool processUsingBackend = false;
            if (orderDetail != null && orderDetail.Product != null)
            {
                processUsingBackend = orderDetail.Product.HasBackEndIntegration;
                if (processUsingBackend && orderDetail.Product.Id == 61)
                {
                    //not all email express products are to be integrated
                    StoreDetailEmailExpress emailexpressdetails = storeService.GetAll<StoreDetailEmailExpress>(true).SingleOrDefault(details => details.OrderDetailId == orderDetail.Id);
                    processUsingBackend = (emailexpressdetails != null && (emailexpressdetails.ItemTypeId == 1 || emailexpressdetails.ItemTypeId == 2));
                }
            }
            log.Debug(string.Format("Processing {0} is using the backend: {1}", orderDetail.ToString(), processUsingBackend));
            return processUsingBackend;
        }

        public virtual bool ValidateCreditCard(CreditCard creditCard)
        {

            log.Debug(string.Format("Validate credit card {0} ({1}).", creditCard.MaskedPAN, creditCard.Type));
            var result = PersonifyClient.ValidateCreditCard(creditCard);
            log.Debug(string.Format("Credit card {0} ({1}) is {2}.", creditCard.MaskedPAN, creditCard.Type, result ? "valid" : "invalid"));
            return result;
        }

		public virtual string SaveCreditCard(StoreOrder order, CreditCard creditCard)
		{
			StoreCompany company = order.Company;
            log.Debug(string.Format("Save credit of {0} ({1})", creditCard.MaskedPAN, company.Name));
            //assuming credit card is valid already
            if (company == null || creditCard == null) throw new ArgumentException("Invalid parameters");
            IList<LookSendMyAdCountryCode> countryCodes = storeService.GetAll<LookSendMyAdCountryCode>(true).ToList();
            
            //create company if not already there
            CompanyInformation companyInfo = null;
            string newMemberType = string.Empty;
            if (order.IsNewMemberShip(ref newMemberType))
            {
                //todo?? reconcile company based on the new member type
                //create a new Personify company for new membership type
                companyInfo = PersonifyClient.CreateCompany(order.Company, newMemberType, countryCodes);
                order.Company.ExternalReference = string.Join(";", companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
                order.Company.MemberType = newMemberType;
            }
            else
            {
                var memberType = string.IsNullOrEmpty(company.MemberType) ? "UNKNOWN" : company.MemberType;
                IEnumerable<StoreAddressInfo> storeAddress = null;
                companyInfo = PersonifyClient.ReconcileCompany(company, memberType, countryCodes, ref storeAddress);
            }

			//field used to map an order to a company before approval for non backoffice orders
			order.ExternalReference = companyInfo.MasterCustomerId;
            //Add credit card to the company
			//@todo CC Personify temporary measure, always add the credit card. No longer checking if (profile == string.Empty) from string profile = PersonifyClient.GetCreditCardProfileId(order.GetASICompany(), companyInfo, creditCard);
			var profile = PersonifyClient.SaveCreditCard(order.GetASICompany(), companyInfo.MasterCustomerId, companyInfo.SubCustomerId, creditCard);
            log.Debug(string.IsNullOrWhiteSpace(profile) ?
                "Fail to save the credit." : string.Format("Saved credit profile id : {0}", profile));
            if (string.IsNullOrEmpty(profile)) throw new Exception("Credit card can't be saved to Personify.");
            return profile;
        }

	    public virtual IEnumerable<StoreCreditCard> GetCompanyCreditCards(StoreCompany company, string asiCompany)
	    {
		    return PersonifyClient.GetCompanyCreditCards(company, asiCompany);
	    }

        private IList<CreateOrderLineInput> GetPersonifyLineInputs(StoreOrder order, long shipAddressId)
        {
            log.Debug(string.Format("Create personify order line input for order {0} with shipping address id {1}", order.ToString(), shipAddressId));
            var lineItems = new List<CreateOrderLineInput>();
            foreach (var orderDetail in order.OrderDetails)
            {
                switch (orderDetail.Product.Id)
                {
                    case 77: //supplier specials
                        var startDate = (orderDetail.DateOption.HasValue ? orderDetail.DateOption.Value : DateTime.Now).ToString("MM/dd/yyyy");
                        var endDate = (orderDetail.DateOption.HasValue ? orderDetail.DateOption.Value : DateTime.Now).AddMonths(1).ToString("MM/dd/yyyy");
                        var option = orderDetail.OptionId.ToString();
                        var mapping = storeService.GetAll<PersonifyMapping>(true).Single(map => (map.StoreContext ?? -1) == (orderDetail.Order.ContextId ?? -1) &&
                            map.StoreProduct == orderDetail.Product.Id &&
                            map.StoreOption == option);
                        mapping.Quantity = orderDetail.Quantity;
                        var lineItem = new CreateOrderLineInput
                        {
                            ProductId = mapping.PersonifyProduct,
                            RateCode = mapping.PersonifyRateCode,
                            RateStructure = mapping.PersonifyRateStructure,
                            ShipAddressID = Convert.ToInt32(shipAddressId),
                            Quantity = Convert.ToInt16(orderDetail.Quantity),
                            BeginDate = startDate,
                            EndDate = endDate,
                            DiscountCode = "0"
                        };
                        lineItems.Add(lineItem);
                        break;
                    case 61: //email express
                        var emailexpressdetails = storeService.GetAll<StoreDetailEmailExpress>(true).Single(details => details.OrderDetailId == orderDetail.Id);
                        option = emailexpressdetails.ItemTypeId.ToString();
                        if (option == "1" || option == "2")
                        {
                            option += ";";
                            if (orderDetail.Quantity >= 120) option += "120X";
                            else if (orderDetail.Quantity >= 52) option += "52X";
                            else if (orderDetail.Quantity >= 26) option += "26X";
                            else if (orderDetail.Quantity >= 12) option += "12X";
                            else if (orderDetail.Quantity >= 6) option += "6X";
                            else if (orderDetail.Quantity >= 3) option += "3X";
                            else option += "1X";
                        }
                        mapping = storeService.GetAll<PersonifyMapping>(true).Single(map => (map.StoreContext ?? -1) == (orderDetail.Order.ContextId ?? -1) &&
                            map.StoreProduct == orderDetail.Product.Id &&
                            map.StoreOption == option);
                        //need to create a new line item for each one rather than one for all quantity
                        for (int i = 0; i < orderDetail.Quantity; i++)
                        {
                            lineItem = new CreateOrderLineInput
                            {
                                ProductId = mapping.PersonifyProduct,
                                RateCode = mapping.PersonifyRateCode,
                                RateStructure = mapping.PersonifyRateStructure,
                                ShipAddressID = Convert.ToInt32(shipAddressId),
                                Quantity = 1,
                                DiscountCode = "0"
                            };
                            lineItems.Add(lineItem);
                        }
                        mapping.ItemCount = orderDetail.Quantity;
                        mapping.Quantity = 1;
                        break;
                    default:
                        mapping = storeService.GetAll<PersonifyMapping>(true).Single(map => (map.StoreContext ?? -1) == (orderDetail.Order.ContextId ?? -1) &&
                            map.StoreProduct == orderDetail.Product.Id);

                        lineItem = new CreateOrderLineInput
                        {
                            ProductId = mapping.PersonifyProduct,
                            RateCode = mapping.PersonifyRateCode,
                            RateStructure = mapping.PersonifyRateStructure,
                            ShipAddressID = Convert.ToInt32(shipAddressId),
                            Quantity = Convert.ToInt16(orderDetail.Quantity),
                            DiscountCode = "0"
                        };
                        lineItems.Add(lineItem);
                        break;
                }
            }
            return lineItems;
        }

        public virtual CompanyInformation AddCompany(User user)
        {
            // create Store CompanyInformation
            var companyInformation = new CompanyInformation
            {
                CompanyId = user.CompanyId,
                Name = user.CompanyName,
                Phone = user.PhoneAreaCode + user.Phone,
                Street1 = user.Street1,
                Street2 = user.Street2,
                City = user.City,
                Zip = user.Zip,
                State = user.State,
                Country = user.CountryCode,
                MemberTypeNumber = user.MemberTypeId,
                ASINumber = user.AsiNumber
            };

            //create equivalent store objects
            var company = new StoreCompany
            {
                Name = companyInformation.Name,
                Phone = companyInformation.Phone,
                ASINumber = user.AsiNumber
            };
            var address = new StoreAddress
            {
                Street1 = companyInformation.Street1,
                Street2 = companyInformation.Street2,
                City = companyInformation.City,
                State = companyInformation.State,
                Country = companyInformation.Country,
                Zip = companyInformation.Zip
            };
            company.Addresses.Add(new StoreCompanyAddress
            {
                Address = address,
                IsBilling = true,
                IsShipping = true,
            });

            company.Individuals.Add(new StoreIndividual()
            { 
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneAreaCode + user.Phone,
                Address = address,
                IsPrimary = true 
            });

			UpdateMemberType(companyInformation);
            company.MemberType = companyInformation.MemberType;

			if (companyInformation.MemberStatus == "ACTIVE") throw new Exception("We should not be creating an active company");
            //create company if not already there
            IEnumerable<StoreAddressInfo> storeAddress = null;
            var companyInfo = PersonifyClient.ReconcileCompany(company, companyInformation.MemberType, null, ref storeAddress, true);

            if (storeAddress == null) 
            {
                PersonifyClient.AddCustomerAddresses(company, companyInfo.MasterCustomerId, companyInfo.SubCustomerId, null);
                PersonifyClient.AddPhoneNumber(companyInformation.Phone, GetCountryCode(companyInformation.Country), companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
            }
            // Add contact to personify
            PersonifyClient.AddIndividualInfos(company, null, companyInfo.MasterCustomerId, companyInfo.SubCustomerId);

            user.CompanyId = companyInfo.CompanyId;

            return companyInfo;
        }

        public virtual CompanyInformation CreateCompany(StoreCompany storeCompany, string storeType)
        {
            var countryCodes = storeService.GetAll<LookSendMyAdCountryCode>(true).ToList();
            var companyInfo = PersonifyClient.CreateCompany(storeCompany, storeType, countryCodes);
            if (companyInfo != null)
            {
                PersonifyClient.AddIndividualInfos(storeCompany, countryCodes, companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
            }

            return companyInfo;
        }

        public virtual CompanyInformation FindCompanyInfo(StoreCompany company, ref List<string> matchList)
        {
            var startTime = DateTime.Now;
            log.Debug(string.Format("FindCompanyInfo - start: Company {0} , phone {1}", company.Name, company.Phone));
            var companyInfo = PersonifyClient.FindCustomerInfo(company, ref matchList);

            log.Debug(string.Format("FindCompanyInfo - end: Company {0}, total matches: {1}; time: {2}",
                                    company.Name,
                                    matchList != null ? matchList.Count : 0,
                                    DateTime.Now.Subtract(startTime).TotalMilliseconds));
            return companyInfo;
        }

        public virtual CompanyInformation GetCompanyInfoByAsiNumber(string asiNumber)
        {
            CompanyInformation companyInfo = null;
            var customerInfo = PersonifyClient.GetCompanyInfoByASINumber(asiNumber);
            if (customerInfo != null)
            {
                companyInfo = PersonifyClient.GetCompanyInfo(customerInfo);
                UpdateMemberType(companyInfo);
            }
			return companyInfo;
        }

        public virtual CompanyInformation GetCompanyInfoByIdentifier(int companyIdentifier)
        {
            var companyInfo = PersonifyClient.GetCompanyInfoByIdentifier(companyIdentifier);
			UpdateMemberType(companyInfo);
            return companyInfo;
        }

        public virtual void AddActivity(StoreCompany company, string activityText, Activity activityType)
        {
            PersonifyClient.AddActivity(company, activityText, activityType);
        }

        public string GetCompanyStatus(string masterCustomerId, int subCustomerId)
        {
            return PersonifyClient.GetCompanyStatus(masterCustomerId, subCustomerId);
        }

        public string GetCompanyAsiNumber(string masterCustomerId, int subCustomerId)
        {
            return PersonifyClient.GetCompanyAsiNumber(masterCustomerId, subCustomerId);
        }

 		private static string GetCountryCode(string country)
		{
			string result = null;
			if (!string.IsNullOrWhiteSpace(country))
			{
				country = country.ToLower();
				if (country.Contains("united states") || country == "usa") result = "USA";
				if (country == "canada" || country == "can") result = "CAN";
			}
			return result;
		}

		private static void UpdateMemberType(CompanyInformation companyInformation)
	    {
		    if (companyInformation != null)
		    {
			    if (companyInformation.MemberTypeNumber == 0 &&
			        !string.IsNullOrEmpty(companyInformation.MemberType) &&
			        !string.IsNullOrEmpty(companyInformation.MemberStatus))
			    {
				    if (companyInformation.MemberStatus == "ACTIVE")
				    {
					    switch (companyInformation.MemberType)
					    {
						    case "DISTRIBUTOR":
							    companyInformation.MemberTypeNumber = 1;
							    break;
						    case "DECORATOR":
							    companyInformation.MemberTypeNumber = 3;
							    break;
						    case "SUPPLIER":
                            case "EQUIPMENT":
							    companyInformation.MemberTypeNumber = 2;
							    break;
					    }
				    }
				    else
				    {
					    switch (companyInformation.MemberType)
					    {
						    case "DISTRIBUTOR":
							    companyInformation.MemberTypeNumber = 6;
							    break;
						    case "DECORATOR":
							    companyInformation.MemberTypeNumber = 12;
							    break;
						    case "SUPPLIER":
							    companyInformation.MemberTypeNumber = 7;
							    break;
						    case "AFFILIATE":
							    companyInformation.MemberTypeNumber = 13;
							    break;
						    case "END_BUYER":
							    companyInformation.MemberTypeNumber = 9;
							    break;
						    default:
							    companyInformation.MemberTypeNumber = 15;
							    break;
					    }
				    }
			    }
			    else if (companyInformation.MemberTypeNumber > 0)
			    {
				    companyInformation.MemberStatus = "ASICENTRAL";
				    switch (companyInformation.MemberTypeNumber)
				    {
					    case 1:
					    case 16:
						    companyInformation.MemberStatus = "ACTIVE";
						    companyInformation.MemberType = "DISTRIBUTOR";
						    break;
					    case 3:
						    companyInformation.MemberStatus = "ACTIVE";
						    companyInformation.MemberType = "DECORATOR";
						    break;
					    case 2:
						    companyInformation.MemberStatus = "ACTIVE";
						    companyInformation.MemberType = "SUPPLIER";
						    break;
					    case 6:
						    companyInformation.MemberType = "DISTRIBUTOR";
						    break;
					    case 12:
						    companyInformation.MemberType = "DECORATOR";
						    break;
					    case 7:
						    companyInformation.MemberType = "SUPPLIER";
						    break;
					    case 13:
						    companyInformation.MemberType = "AFFILIATE";
						    break;
					    case 9:
						    companyInformation.MemberType = "END_BUYER";
						    break;
					    default:
						    companyInformation.MemberType = "UNKNOWN";
						    break;
				    }
			    }
		    }
	    }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
            }
            disposed = true;
        }

        ~PersonifyService()
        {
            Dispose(false);
        }
    }
}