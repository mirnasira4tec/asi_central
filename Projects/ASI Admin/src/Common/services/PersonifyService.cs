﻿using System.Linq;
using System.Net.Mail;
using System.Text;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.PersonifyDataASI;
using asi.asicentral.model;
using System;
using System.Collections.Generic;
using asi.asicentral.services.PersonifyProxy;
using asi.asicentral.model.timss;
using ASI.EntityModel;

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

        public virtual void PlaceOrder(StoreOrder order, IEmailService emailService)
        {
            log.Debug(string.Format("Place order Start : {0}", order));
            IList<LookSendMyAdCountryCode> countryCodes = storeService.GetAll<LookSendMyAdCountryCode>(true).ToList();
            if (order == null || order.Company == null || countryCodes == null)
                throw new ArgumentException("You must pass a valid order and the country codes");
            try
            {
                var companyInfo = PersonifyClient.ReconcileCompany(order.Company, "UNKNOWN", countryCodes);
                log.Debug(string.Format("Reconciled company '{1}' to order '{0}'.", order, companyInfo.MasterCustomerId + ";" + companyInfo.SubCustomerId));

                IList<CustomerInfo> individualInfos = PersonifyClient.AddIndividualInfos(order, countryCodes, companyInfo).ToList();
                log.Debug(string.Format("Added individuals to company '{1}' to order '{0}'.", order, companyInfo.MasterCustomerId + ";" + companyInfo.SubCustomerId));

                IList<StoreAddressInfo> addresses2 = PersonifyClient.AddIndividualAddresses(order.Company, individualInfos, countryCodes).ToList();
                log.Debug(string.Format("Address added to individuals to the order '{0}'.", order));

                StoreIndividual primaryContact = order.GetContact();
                CustomerInfo primaryContactInfo = individualInfos.FirstOrDefault(c =>
                    string.Equals(c.FirstName, primaryContact.FirstName, StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(c.LastName, primaryContact.LastName, StringComparison.InvariantCultureIgnoreCase));

                var shipToAddr = GetAddressInfo(addresses2, AddressType.Shipping, order);
                var billToAddr = GetAddressInfo(addresses2, AddressType.Billing, order);
                var lineItems = GetPersonifyLineInputs(order, shipToAddr.PersonifyAddr.CustomerAddressId);
                log.Debug(string.Format("Retrieved the line items to the order '{0}'.", order.ToString()));
                var orderOutput = PersonifyClient.CreateOrder(
                    order,
                    companyInfo,
                    primaryContactInfo,
                    billToAddr.PersonifyAddr.CustomerAddressId,
                    shipToAddr.PersonifyAddr.CustomerAddressId,
                    lineItems);
                log.Debug(string.Format("The order '{0}' has been created in Personify.", order));


                order.BackendReference = orderOutput.OrderNumber;
                decimal orderTotal = PersonifyClient.GetOrderTotal(orderOutput.OrderNumber);
                log.Debug(string.Format("Got the order total for the order '{0}'.", order));
                if (orderTotal != order.Total)
                {
                    var data = new EmailData()
                    {
                        Subject = "here is a price discrepancy for an order from the store to Personify",
                        EmailBody = string.Format("A new order created in the store ({0}) has been transferred to a Personify "
                        + "order ({1}). The prices do not match, the order needs to be looked at. The store price is {2:C} and "
                        + "the Personify price is {3:C}.<br /><br />Thanks,<br /><br />ASICentral team", order.Id.ToString(), orderOutput.OrderNumber, order.Total, orderTotal)
                    };
                    data.SendEmail(emailService);
                }
                try
                {
                    PersonifyClient.PayOrderWithCreditCard(orderOutput.OrderNumber, orderTotal, order.CreditCard.ExternalReference, billToAddr.PersonifyAddr, companyInfo);
                    log.Debug(string.Format("Payed the order '{0}'.", order));
                    log.Debug(string.Format("Place order End: {0}", order));
                }
                catch (Exception e)
                {
                    string s = string.Format("Failed to pay the order '{0}'. Error is {2}{1}", order, e.StackTrace, e.Message);
                    log.Error(s);
                    var data = new EmailData()
                    {
                        Subject = "order failed to be charged",
                        EmailBody = s + "<br /><br />Thanks,<br /><br />ASICentral team"
                    };
                    data.SendEmail(emailService);
                    log.Debug(string.Format("Place order End: {0}", order));
                }
            }
            catch (Exception ex)
            {
                string error1 = string.Format("Unknown Error while adding order to personify: {0}{1}", ex.Message, ex.StackTrace);
                string error2 = string.Format("Place order End: {0}", order);
                log.Error(error1);
                log.Debug(error2);
                var data = new EmailData()
                {
                    Subject = error2,
                    EmailBody = error1 + "<br /><br />" + error2 + "<br /><br />Thanks,<br /><br />ASICentral team"
                };
                data.SendEmail(emailService);
                throw ex;
            }
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

        public virtual string SaveCreditCard(StoreCompany company, CreditCard creditCard)
        {
            log.Debug(string.Format("Save credit of {0} ({1})", creditCard.MaskedPAN, company.Name));
            //assuming credit card is valid already
            if (company == null || creditCard == null) throw new ArgumentException("Invalid parameters");
            IList<LookSendMyAdCountryCode> countryCodes = storeService.GetAll<LookSendMyAdCountryCode>(true).ToList();
            //create company if not already there
            var companyInfo = PersonifyClient.ReconcileCompany(company, "UNKNOWN", countryCodes);
            //Add credit card to the company
            string profile = PersonifyClient.GetCreditCardProfileId(companyInfo, creditCard);
            if (profile == string.Empty) profile = PersonifyClient.SaveCreditCard(companyInfo, creditCard);
            log.Debug(string.IsNullOrWhiteSpace(profile) ?
                "Fail to save the credit." : string.Format("Saved credit profile id : {0}", profile));
            return profile;
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
                        var mapping = storeService.GetAll<PersonifyMapping>(true).Single(map => Equals(map.StoreContext, orderDetail.Order.ContextId) &&
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
                        mapping = storeService.GetAll<PersonifyMapping>(true).Single(map => Equals(map.StoreContext, orderDetail.Order.ContextId) &&
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
                            };
                            lineItems.Add(lineItem);
                        }
                        mapping.ItemCount = orderDetail.Quantity;
                        mapping.Quantity = 1;
                        break;
                    default:
                        mapping = storeService.GetAll<PersonifyMapping>(true).Single(map => Equals(map.StoreContext, orderDetail.Order.ContextId) &&
                            map.StoreProduct == orderDetail.Product.Id);

                        lineItem = new CreateOrderLineInput
                        {
                            ProductId = mapping.PersonifyProduct,
                            RateCode = mapping.PersonifyRateCode,
                            RateStructure = mapping.PersonifyRateStructure,
                            ShipAddressID = Convert.ToInt32(shipAddressId),
                            Quantity = Convert.ToInt16(orderDetail.Quantity),
                        };
                        lineItems.Add(lineItem);
                        break;
                }
            }
            return lineItems;
        }

        public virtual CompanyInformation AddCompany(CompanyInformation companyInformation)
        {
            //create equivalent store objects
            var company = new StoreCompany
            {
                Name = companyInformation.Name,
            };
            var address = new StoreAddress
            {
                Street1 = companyInformation.Street1,
                Street2 = companyInformation.Street2,
                City = companyInformation.City,
                State = companyInformation.State,
                Country = companyInformation.Country,
                Zip = companyInformation.Zip,
            };
            company.Addresses.Add(new StoreCompanyAddress
            {
                Address = address,
                IsBilling = true,
                IsShipping = true,
            });
			UpdateMemberType(companyInformation);
			if (companyInformation.MemberStatus == "ACTIVE") throw new Exception("We should not be creating an active company");
            //create company if not already there
            var companyInfo = PersonifyClient.ReconcileCompany(company, companyInformation.MemberType, null);
            PersonifyClient.AddCustomerAddresses(company, companyInfo, null);
	        companyInformation = PersonifyClient.GetCompanyInfo(companyInfo);
            return companyInformation;
        }

        public virtual CompanyInformation GetCompanyInfoByAsiNumber(string asiNumber)
        {
            var companyInfo = PersonifyClient.GetCompanyInfoByASINumber(asiNumber);
			UpdateMemberType(companyInfo);
			return companyInfo;
        }

        public virtual CompanyInformation GetCompanyInfoByIdentifier(int companyIdentifier)
        {
            var companyInfo = PersonifyClient.GetCompanyInfoByIdentifier(companyIdentifier);
			UpdateMemberType(companyInfo);
            return companyInfo;
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