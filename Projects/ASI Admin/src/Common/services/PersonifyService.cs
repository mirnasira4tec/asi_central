using asi.asicentral.interfaces;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public virtual CompanyInformation PlaceOrder(StoreOrder order, IEmailService emailService, string url)
        {
            CompanyInformation companyInfo = null;
            log.Debug(string.Format("Place order Start : {0}", order));
            IList<LookSendMyAdCountryCode> countryCodes = storeService.GetAll<LookSendMyAdCountryCode>(true).ToList();
            if (order == null || order.Company == null || order.Company.Individuals == null || countryCodes == null)
                throw new ArgumentException("You must pass a valid order and the country codes");
            try
            {
                IEnumerable<StoreAddressInfo> storeAddress = null;
                var memberType = string.IsNullOrEmpty(order.Company.MemberType) ? "UNKNOWN" : order.Company.MemberType;
                companyInfo = PersonifyClient.ReconcileCompany(order.Company, memberType, countryCodes, ref storeAddress, true);

                // do not place order for terminated company
                if ( companyInfo.IsTerminated())
                {
                    return companyInfo;
                }

                log.Debug(string.Format("Reconciled company '{1}' to order '{0}'.", order, companyInfo.MasterCustomerId + ";" + companyInfo.SubCustomerId));

                var individualInfos = PersonifyClient.AddIndividualInfos(order.Company, countryCodes, companyInfo.MasterCustomerId, companyInfo.SubCustomerId).ToList();
                if (!individualInfos.Any()) throw new Exception("Failed in creating individual in Personify.");
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
                var coupon = orderDetail.Coupon;

                // update demographic questions
                PersonifyClient.UpdateDemographicData(storeService, orderDetail);

                // processing coupon
                if (coupon != null && !string.IsNullOrEmpty(coupon.CouponCode) )
                {
                    coupon.CouponCode = coupon.CouponCode.Trim();
                    if (coupon.CouponCode == "(Unknown)")
                        coupon.CouponCode = string.Empty;
                }

                #region mapping items from mapping table
                var allMappings = storeService.GetAll<PersonifyMapping>(true)
                                              .Where(map => (map.StoreContext == null || map.StoreContext == order.ContextId) && map.StoreProduct == orderDetail.Product.Id)
                                              .OrderByDescending(m => m.StoreContext).ToList();

                if (orderDetail.Product.Id == PersonifyClient.EMAIL_MARKETING_PRODUCT_ID && orderDetail.OptionId.HasValue)  // email marketing
                {
                    allMappings = allMappings.FindAll(m => m.StoreOption == orderDetail.OptionId.Value.ToString());
                }
                else if (orderDetail.Product.Id != 61 && orderDetail.Product.Id != 77 )
                {
                    if (coupon != null && !string.IsNullOrEmpty(coupon.CouponCode))
                    {
                        allMappings = allMappings.FindAll(m => m.StoreOption != null && m.StoreOption.Trim() == coupon.CouponCode);
                    }
                    else
                    {
                        allMappings = allMappings.FindAll(m => m.StoreOption == null);
                    }
                }

                if (!allMappings.Any())
                {
                    var orderContext = order.Context != null ? string.Format(", context - {0}", order.Context.Id): string.Empty;
                    var orderOption = coupon != null && !string.IsNullOrEmpty(coupon.CouponCode) ? string.Format(", coupon - '{0}'", coupon.CouponCode) : string.Empty;
                    throw new Exception(string.Format("No record found in mapping table for: product - {0}{1}{2}", orderDetail.Product.Id, orderContext, orderOption));
                }
                #endregion mapping items from mapping table

                #region ******* need to revisit and remove after packages ******
                // handle bundles first if any
                var mappedBundles = allMappings.FindAll(m => m.PersonifyBundle != null );
                if (mappedBundles.Any())
                {
                    PersonifyClient.CreateBundleOrder(order, mappedBundles[0], companyInfo, contactMasterId, contactSubId, billToAddr, shipToAddr);
                }
                #endregion ******* need to revisit and remove after packages *******

                // get all non-bundle products
                var mappedProducts = allMappings.FindAll(map => map.PersonifyProduct != null && map.PersonifyBundle == null);

                if (mappedProducts.Count > 1 && orderDetail.Product.Id != 61 && orderDetail.Product.Id != 77)
                {
                    // remove potential duplicated product Ids if any, 
                    // always get the products in the leading place, which match both context and product id
                    var productIds = new List<int>();
                    for (var i = 0; i < mappedProducts.Count; i++)
                    {
                        var product = mappedProducts[i];
                        if ( productIds.Contains(product.PersonifyProduct.Value))
                        {
                            mappedProducts.Remove(product);
                            i--;
                        }
                        else
                        {
                            productIds.Add(product.PersonifyProduct.Value);
                        }
                    }
                }

                #region scheduled products
                var scheduledProducts = mappedProducts.FindAll(m => m.PaySchedule.HasValue && m.PaySchedule.Value);
                if (scheduledProducts.Any() )
                {
                    // create an order if there isn't one existing
                    if (string.IsNullOrEmpty(order.BackendReference))
                    {
                        CreateProductOrder(order, companyInfo, scheduledProducts, contactMasterId, contactSubId, billToAddr, shipToAddr);
                    }
                    else
                    { // add scheduled products to the existing order
                        foreach (var item in scheduledProducts)
                        {
                            PersonifyClient.AddLineItemToOrder(order, item.PersonifyProduct.Value, item.PersonifyRateStructure, item.PersonifyRateCode);
                        }
                    }
                }

                // get ASI# and schedule payment for all the lineItems added to the order already so far
                if (!string.IsNullOrEmpty(order.BackendReference))
                {
                    var asiNumber = PersonifyClient.GetCompanyAsiNumber(companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
                    if (!string.IsNullOrWhiteSpace(asiNumber) && asiNumber.Trim().Length < 7)
                    {
                        order.Company.ASINumber = asiNumber;
                    }
                    PersonifyClient.ScheduleOrderPayment(order);
                }
                #endregion scheduled products

                #region non-scheduled products
                var nonScheduledOrderLineNumbers = string.Empty;
                var nonScheduledProducts = mappedProducts.FindAll(m => !m.PaySchedule.HasValue || !m.PaySchedule.Value);
                decimal payAmount = 0;
                if (nonScheduledProducts.Any())
                {
                    if (string.IsNullOrEmpty(order.BackendReference))
                    {
                        CreateProductOrder(order, companyInfo, nonScheduledProducts, contactMasterId, contactSubId, billToAddr, shipToAddr);
                    }
                    else
                    {   // add scheduled products to the existing order
                        foreach (var item in nonScheduledProducts)
                        {
                            PersonifyClient.AddLineItemToOrder(order, item.PersonifyProduct.Value, item.PersonifyRateStructure, item.PersonifyRateCode);
                        }
                    }
                
                    nonScheduledOrderLineNumbers = PersonifyClient.GetOrderLinesByOrderId(order.BackendReference, ref payAmount, nonScheduledProducts);
                }

                // validate order total before any payment
                ValidateOrderTotal(order, emailService, url);

                if (!string.IsNullOrEmpty(nonScheduledOrderLineNumbers) && payAmount > 0)
                {
                    try
                    {
                        PersonifyClient.PayOrderWithCreditCard(order.BackendReference, payAmount, order.CreditCard.ExternalReference, billToAddr, 
                                                               companyInfo.MasterCustomerId, companyInfo.SubCustomerId, nonScheduledOrderLineNumbers);
                        log.Debug(string.Format("Payed the order '{0}'.", order));
                        log.Debug(string.Format("Place order End: {0}", order));
                    }
                    catch (Exception e)
                    {
                        string s = string.Format("Failed to pay the order '{0} {3}'. Error is {2}{1}", order, e.StackTrace, e.Message, order.BackendReference);
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
                #endregion non-scheduled products
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

            return companyInfo;
        }

        private CreateOrderOutput CreateProductOrder(StoreOrder order, CompanyInformation companyInfo, List<PersonifyMapping> orderProducts, 
                                        string contactMasterId, int contactSubId, AddressInfo billToAddr, AddressInfo shipToAddr)
        {
            if (order == null || order.Company == null || order.Company.Individuals == null || order.OrderDetails == null || !order.OrderDetails.Any())
                return null;

            var orderDetail = order.OrderDetails[0];
            var lineItems = new List<CreateOrderLineInput>();

            switch (orderDetail.Product.Id)
            {
                case 77: //supplier specials
                    var startDate =
                        (orderDetail.DateOption.HasValue ? orderDetail.DateOption.Value : DateTime.Now).ToString(
                            "MM/dd/yyyy");
                    var endDate =
                        (orderDetail.DateOption.HasValue ? orderDetail.DateOption.Value : DateTime.Now).AddMonths(1)
                            .ToString("MM/dd/yyyy");
                    var option = orderDetail.OptionId.ToString();
                    var mapping = orderProducts.Single(map => map.StoreOption == option);
                    mapping.Quantity = orderDetail.Quantity;
                    var lineItem = new CreateOrderLineInput
                    {
                        ProductId = mapping.PersonifyProduct,
                        RateCode = mapping.PersonifyRateCode,
                        RateStructure = mapping.PersonifyRateStructure,
                        ShipAddressID = Convert.ToInt32(shipToAddr.CustomerAddressId),
                        Quantity = Convert.ToInt16(orderDetail.Quantity),
                        BeginDate = startDate,
                        EndDate = endDate,
                        DiscountCode = "0"
                    };
                    lineItems.Add(lineItem);
                    break;
                case 61: //email express
                    var emailexpressdetails =
                        storeService.GetAll<StoreDetailEmailExpress>(true)
                            .Single(details => details.OrderDetailId == orderDetail.Id);
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
                    mapping = orderProducts.Single(map => map.StoreOption == option);

                    //need to create a new line item for each one rather than one for all quantity
                    for (int i = 0; i < orderDetail.Quantity; i++)
                    {
                        lineItem = new CreateOrderLineInput
                        {
                            ProductId = mapping.PersonifyProduct,
                            RateCode = mapping.PersonifyRateCode,
                            RateStructure = mapping.PersonifyRateStructure,
                            ShipAddressID = Convert.ToInt32(shipToAddr.CustomerAddressId),
                            Quantity = 1,
                            DiscountCode = "0"
                        };

                        lineItems.Add(lineItem);
                    }
                    mapping.ItemCount = orderDetail.Quantity;
                    mapping.Quantity = 1;
                    break;
                default:
                    foreach (var m in orderProducts)
                    {
                        var item = new CreateOrderLineInput
                        {
                            ProductId = m.PersonifyProduct,
                            RateCode = m.PersonifyRateCode,
                            RateStructure = m.PersonifyRateStructure,
                            ShipAddressID = Convert.ToInt32(shipToAddr.CustomerAddressId),
                            Quantity = Convert.ToInt16(orderDetail.Quantity),
                            DiscountCode = "0"
                        };
                        lineItems.Add(item);
                        
                        // membership purchase
                        if ( !string.IsNullOrEmpty(m.ProductCode) && !string.IsNullOrEmpty(m.PersonifyRateCode) && 
                             !string.IsNullOrEmpty(m.ClassCode) && orderDetail.Product.IsMembership())
                        {
                            PersonifyClient.UpdatePersonifyCompany(companyInfo, m);
                        }
                    }
                    break;
            }

            var output = PersonifyClient.CreateOrder(
                order,
                companyInfo.MasterCustomerId,
                companyInfo.SubCustomerId,
                contactMasterId,
                contactSubId,
                billToAddr.CustomerAddressId,
                shipToAddr.CustomerAddressId,
                lineItems);

            log.Debug(string.Format("The order '{0}' has been created in Personify.", order));

            if( output != null )
                order.BackendReference = output.OrderNumber;

            return output;
        }
 
        private decimal ValidateOrderTotal(StoreOrder order, IEmailService emailService, string url)
        {
            if (order == null)
            {
                throw new Exception("Order is null");
            }

            if (string.IsNullOrEmpty(order.BackendReference))
            {
                throw new Exception("No Personify Order has been generated");
            }

            decimal backEndTotal = PersonifyClient.GetOrderBalanceTotal(order.BackendReference);
            log.Debug(string.Format("Got the order total {0} of for the order '{1}'.", backEndTotal, order));

            if (backEndTotal != order.Total)
            {
                var data = new EmailData()
                {
                    Subject = "here is a price discrepancy for an order from the store to Personify",
                    EmailBody = string.Format("A new order created in the store ({0}) has been transferred to a Personify "
                    + "order ({1}). The prices do not match, the order needs to be looked at. The store price is {2:C} and "
                    + "the Personify price is {3:C}.{4}", order.Id.ToString(), order.BackendReference, order.Total, backEndTotal, EmailData.GetMessageSuffix(url))
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

        public virtual CompanyInformation UpdateCompanyStatus(StoreCompany storeCompany, StatusCode status)
        {
            log.Debug(string.Format("Start UpdateCompanyStatus {0}, {1} ).", storeCompany.Name, status.ToString()));
            var companyInfo = PersonifyClient.UpdateCompanyStatus(storeCompany, status);
            log.Debug(string.Format("End UpdateCompanyStatus {0}, {1}).", storeCompany.Name, status.ToString()));
            return companyInfo;
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
                //check if the matchList have lead company already created
                var matchIdList = order.Company.MatchingCompanyIds;
                if (!string.IsNullOrEmpty(matchIdList))
                {
                    var matches = Regex.Matches(matchIdList, @"(\d+),(LEAD|ASICENTRAL),\w+\|?");

                    if (matches.Count > 0)
                    {
                        var personifyCompany = PersonifyClient.GetPersonifyCompanyInfo(matches[0].Groups[1].Value, 0);
                        companyInfo = PersonifyClient.GetCompanyInfo(personifyCompany);

                        order.Company.MatchingCompanyIds = matchIdList.Replace(matches[0].Value, "");
                    }
                }
                else if( !order.IsGuestLogin ) //login user
                {
                    var matchCompany = new StoreCompany()
                    {
                        Name = company.Name,
                        Phone = company.Phone,
                        Individuals = company.Individuals,
                        MemberType = newMemberType
                    };

                    List<string> matchList = null;
                    var personifyCompany = PersonifyClient.FindCustomerInfo(matchCompany, ref matchList);
                    if (personifyCompany != null)
                    {
                        if (string.Compare(personifyCompany.MemberStatus, "LEAD", StringComparison.CurrentCulture) == 0 ||
                            string.Compare(personifyCompany.MemberStatus, "ASICENTRAL", StringComparison.CurrentCulture) == 0 )
                        {
                            companyInfo = personifyCompany;
                        }
                        else if ( matchList != null && matchList.Count > 0 )
                        {
                            foreach (var m in matchList)
                            {
                                var match = Regex.Match(m, @"(\d+),(LEAD|ASICENTRAL),\w+");
                                if (match.Success)
                                {
                                    var leadCompany = PersonifyClient.GetPersonifyCompanyInfo(match.Groups[1].Value, 0);
                                    if (leadCompany != null)
                                    {
                                        companyInfo = PersonifyClient.GetCompanyInfo(leadCompany);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (companyInfo == null)
                {
                    //create a new Personify company for new membership type
                    companyInfo = PersonifyClient.CreateCompany(order.Company, newMemberType, countryCodes);
                }

                if (company.HasExternalReference())
                { // add current matched company to matchIds
                    var newMatches = string.Format("{0},{1},{2}", company.ExternalReference.Split(';')[0],company.MemberStatus, company.MemberType);
                    if (string.IsNullOrEmpty(company.MatchingCompanyIds))
                        company.MatchingCompanyIds = newMatches;
                    else
                    {
                        company.MatchingCompanyIds = newMatches + "|" + company.MatchingCompanyIds;
                    }
                }

                company.ExternalReference = string.Join(";", companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
                company.MemberType = newMemberType;
                company.MemberStatus = companyInfo.MemberStatus;
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

        public bool ValidateRateCode(string groupName, string rateStructure, string rateCode, ref int persProductId)
        {
            return PersonifyClient.ValidateRateCode(groupName, rateStructure, rateCode, ref persProductId);
        }

        public CompanyInformation AddEEXSubscription(User user, bool isBusinessAddress)
        {
            CompanyInformation companyInfo = null;
            if (user != null && !string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.CompanyName) && !string.IsNullOrEmpty(user.MemberType_CD))
            {
                //create equivalent store objects
                var company = new StoreCompany { Name = user.CompanyName, Phone = user.Phone, ASINumber = user.AsiNumber };
                var address = new StoreAddress { Street1 = user.Street1,  Street2 = user.Street2, City = user.City, State = user.State,  Country = user.Country, Zip = user.Zip };
                company.Addresses.Add(new StoreCompanyAddress { Address = address });

                company.Individuals.Add(new StoreIndividual()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.PhoneAreaCode + user.Phone,
                    Address = address
                });

                var countryCodes = storeService != null ? storeService.GetAll<LookSendMyAdCountryCode>(true).ToList() : null;
                companyInfo = PersonifyClient.AddEEXSubscription(company, user, countryCodes, isBusinessAddress);
            }

            return companyInfo;
        }

        public virtual PersonifyStatus OptOutEmailSubscription(string email, List<string> usageCodes)
        {
            return PersonifyClient.OptOutEmailSubscription(email, usageCodes);
        }

        public virtual StoreDetailApplication GetDemographicData(StoreOrderDetail orderDetail)
        {
            var storeDetailApp = PersonifyClient.GetDemographicData(storeService, orderDetail);
            storeService.SaveChanges();

            return storeDetailApp;
        }

        public virtual List<XElement> GetASICOMPData(string masterId)
        {
            return PersonifyClient.GetASICOMPData(masterId);
        }

        public virtual void UpdateASICompData(List<string> parameters)
        {
            PersonifyClient.UpdateASICompData(parameters);
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