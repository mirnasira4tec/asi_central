using System;
using System.Collections.Generic;
using System.Linq;
using asi.asicentral.interfaces;
using asi.asicentral.model;
using asi.asicentral.model.store;
using asi.asicentral.PersonifyDataASI;
using asi.asicentral.services;
using asi.asicentral.services.PersonifyProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using asi.asicentral.model.timss;
using asi.asicentral.util.store;
using PersonifySvcClient;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class PersonifyTest
    {
        [TestMethod]
        public void PlaceOrderNewCompanyTest()
        {
            IStoreService storeService = MockupStoreService();
            var supplierSpecials = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 77);
            var emailExpress = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 61);
            PlaceOrderTest(string.Empty, storeService, new ContextProduct[] { supplierSpecials, emailExpress });
        }

        [TestMethod]
        public void PlaceOrderExistingCompanyTest()
        {
            IStoreService storeService = MockupStoreService();
            var supplierSpecials = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 77);
            var emailExpress = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 61);
            PlaceOrderTest("30279", storeService, new ContextProduct[] { supplierSpecials, emailExpress });
        }

        [TestMethod]
        public void AddCompany()
        {
            var tag = DateTime.Now.Ticks;

            var companyInfo = new CompanyInformation
            {
                Name = "New Company1 " + tag,
                Street1 = "4800 Street Rd",
                City = "Trevose",
                State = "PA",
                Zip = "19053",
                Country = "USA",
				MemberTypeNumber = 6,
            };
            IBackendService personify = new PersonifyService();
            companyInfo = personify.AddCompany(companyInfo);
            Assert.IsTrue(companyInfo.CompanyId > 0);
			Assert.AreEqual("DISTRIBUTOR", companyInfo.MemberType);
			Assert.AreEqual("ASICENTRAL", companyInfo.MemberStatus);
		}

	    [TestMethod]
	    public void LookupCompany()
	    {
			IBackendService personify = new PersonifyService();
		    var companyInformation = personify.GetCompanyInfoByAsiNumber("30279");
			Assert.IsNotNull(companyInformation.CompanyId);
            Assert.IsNotNull(companyInformation.City);
            Assert.IsNotNull(companyInformation.MemberType);
			Assert.IsNotNull(companyInformation.MemberStatus);
			companyInformation = personify.GetCompanyInfoByIdentifier(5806901);
            Assert.IsNotNull(companyInformation.Name);
            Assert.IsNotNull(companyInformation.City);
            Assert.IsNotNull(companyInformation.MemberType);
        }

        [TestMethod]
        public void EquipmentASINumberTest()
        {
            IBackendService personify = new PersonifyService();
            string[] asiNumbers = { "18200","12555" };

            //Equipment ASI Numbers
            //"12310", "12550", "12553", "12555", "12600", 
            //                            "14703", "18200", "14970", "14971", "14972", 
            //                            "14973", "14974", "14976", "14977", "14980", 
            //                            "14981", "16000", "16001", "16002", "16004",
            //                            "16008", "16010", "18200", "18201", "18202",
            //                            "18203", "18205"

            foreach (string number in asiNumbers)
            {
                var companyInformation = personify.GetCompanyInfoByAsiNumber(number);
                Assert.IsNotNull(companyInformation.CompanyId);
                Assert.IsNotNull(companyInformation.City);
                Assert.IsNotNull(companyInformation.MemberType, "EQUIPMENT");
                Assert.IsNotNull(companyInformation.MemberStatus);
            }
        }

        private void PlaceOrderTest(string asiNumber, IStoreService storeService, ContextProduct[] products)
        {
            IBackendService personify = new PersonifyService(storeService);
            StoreOrder order = CreateOrder(asiNumber, products);

            //simulate the store process by first processing the credit card
            ICreditCardService cardService = new CreditCardService(new PersonifyService(storeService));
            var cc = new CreditCard
            {
                Address = "",
                CardHolderName = order.CreditCard.CardHolderName,
                Type = order.CreditCard.CardType,
                Number = order.CreditCard.CardNumber,
                ExpirationDate = new DateTime(int.Parse(order.CreditCard.ExpYear), int.Parse(order.CreditCard.ExpMonth), 1),
            };
            Assert.IsTrue(cardService.Validate(cc));
            var profileIdentifier = cardService.Store(order, cc, true);
            Assert.IsNotNull(profileIdentifier);
            Assert.IsNotNull(order.Company.ExternalReference);
            order.CreditCard.ExternalReference = profileIdentifier;
            personify.PlaceOrder(order, new Mock<IEmailService>().Object, null);
        }

        [TestMethod]
        public void AddPhoneNumberTest()
        {
            CustomerInfo companyInfo = PersonifyClient.GetCustomerInfoByASINumber("33020");
	        if (companyInfo != null)
	        {
		        PersonifyClient.AddPhoneNumber("2222222222", "USA", companyInfo);
	        }
	        else
	        {
		        Assert.IsTrue(false, "Could not find the Company forASI# 33020");
	        }
        }

		[TestMethod]
		public void GetCompanyCreditCards()
		{
			StoreCompany company = new StoreCompany()
			{
				ASINumber = "125724",
				Name = "ASI"
			};
			var creditCards = PersonifyClient.GetCompanyCreditCards(company, "ASI");
			if (creditCards.Any())
			{
				foreach (var creditCard in creditCards)
				{
					Assert.IsTrue(!string.IsNullOrEmpty(creditCard.CardNumber));
				}
			}
			else
			{
				Assert.IsTrue(false, "Could not find any credit cards for ASI# 33020");
			}
		}

        [TestMethod]
        public void CreatCompany()
        {
            var companyName = "Createcompany" + DateTime.Now.Ticks;
            StoreCompany company = GetStoreCompany(companyName, "2122223333", "createCompany@unittest.com", "SUPPLIER");
            IBackendService personify = new PersonifyService(MockupStoreService());
            var companyInfo = personify.CreateCompany(company, "SUPPLIER");
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual("SUPPLIER", companyInfo.MemberType);
            Assert.AreEqual(companyName, companyInfo.Name);
            Assert.IsTrue(!string.IsNullOrEmpty(company.ExternalReference));
        }

        #region FindCompanyInfo performance tests
        // Companys and Employees: 
        //      Reconcile Company Supplier 1, 2135555551, LEAD, 000010252975
        //          Reconcile Individual1, 2135555551, 2135555552, individual1@reconcile.com, : 000010252976
        //
        //      Reconcile Company Supplier 2, 2135555561, LEAD, 000010252977
        //          Reconcile Individual2, 2135555561, 2135555552, individual2@reconcile.com, : 000010252978
        //
        //      Reconcile Company Supplier 3, 2135555541, ASICENTRAL : 000010252985
        //          Reconcile Individual5, 2135555541, 2135555552, individual5@reconcile.com, : 000010252986
        //
        //      Reconcile Company Distributor 1, 2135555571,      : 000010252979
        //          Reconcile Individual3, 2135555571, 2135555552, individual3@reconcile.com, : 000010252980
        //
        //      Reconcile Company Distributor 2, 2135555501, LEAD : 000010252994
        //          Reconcile Individual6, 2135555502, 2135555552, individual6@reconcile.com, : 000010252995
        //
        //      Reconcile Company Distributor 3, 2135555591, LEAD : 000010252996
        //          Reconcile Individual7, 2135555592, 2135555552, individual7@reconcile.com, : 
        //
        //      Reconcile Company Decorator 1, 2135555581, LEAD  : 000010252982
        //          Reconcile Individual4, 2135555581, 2135555552, individual4@reconcile.com, : 000010252984
        //
        //      Reconcile Company Decorator 2, 2135555511,      : 000010252998
        //          Reconcile Individual8, 2135555512, 2135555553, individual8@reconcile.com, : 000010252999
        //
        //      Reconcile Company Decorator 3, 2135555531,      : 000010253000
        //          Reconcile Individual9, 2135555532, 2135555553, individual9@reconcile.com, : 000010253001
        //
        [TestMethod]
        public void ReconcileCompanyPerformance()
        {
            ReconcileCompanyMatchNameOnly();
            ReconcileCompanyMatchNameOnly();
            ReconcileCompanySupplierWithPhoneEmail();
            ReconcileCompanyNonSupplierPhoneEmail();
            ReconcileCompanyMultipleMatches();
            ReconcileCompanySupplierNoMatch();
        }

        [TestMethod]
        public void ReconcileCompanyMatchNameOnly()
        {
            var company = GetStoreCompany("Reconcile Company Distributor 1", 
                                          "1110001111", 
                                          "noMatch@reconcile.com", 
                                          "DISTRIBUTOR");

            IBackendService personify = new PersonifyService();
            List<string> masterIdList = null;
            bool dnsFlag = false;
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList, ref dnsFlag);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual("DISTRIBUTOR", companyInfo.MemberType);
            Assert.AreEqual(companyInfo.Name, "Reconcile Company Distributor 1");

            // company name with extra special characters
            company.Name = ".Reconcile $@#?Company $@#?$!&,Distributor 1.";
            companyInfo = personify.FindCompanyInfo(company, ref masterIdList, ref dnsFlag);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual(companyInfo.Name, "Reconcile Company Distributor 1");
            Assert.AreEqual("DISTRIBUTOR", companyInfo.MemberType);
        }

        [TestMethod]
        public void ReconcileCompanySupplierWithPhoneEmail()
        {
            // match email only
            var company = GetStoreCompany("Failed aaa Match",
                                          "1110011100",
                                          "individual1@reconcile.com",
                                          "SUPPLIER");

            IBackendService personify = new PersonifyService();
            List<string> masterIdList = null;
            bool dnsFlag = false;
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList, ref dnsFlag);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual(companyInfo.Name, "Reconcile Company Supplier 1");

            // match phone or email with LEAD matches
            company.Phone = "2135555552";
            companyInfo = personify.FindCompanyInfo(company, ref masterIdList, ref dnsFlag);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.IsTrue(companyInfo.MemberStatus.ToUpper() == "ASICENTRAL" ||
                          companyInfo.MemberStatus.ToUpper() == "LEAD");

            company.Phone = "2135555553";
            companyInfo = personify.FindCompanyInfo(company, ref masterIdList, ref dnsFlag);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.IsTrue(companyInfo.MemberStatus.ToUpper() == "ASICENTRAL" ||
                          companyInfo.MemberStatus.ToUpper() == "LEAD");

            // phone or email without LEAD matches
            company = GetStoreCompany("Failed aaa Match",
                                      "2135555553",
                                      "nommmmatch@reconcile.com",
                                      "SUPPLIER"); ;
            companyInfo = personify.FindCompanyInfo(company, ref masterIdList, ref dnsFlag);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.IsTrue(companyInfo.MemberStatus.ToUpper() != "ASICENTRAL" &&
                          companyInfo.MemberStatus.ToUpper() != "LEAD");
        }

        [TestMethod]
        public void ReconcileCompanyNonSupplierPhoneEmail()
        {
            //Distributor
            var company = GetStoreCompany("No matchhhh",
                                          "2135555571",
                                          "individual3@reconcile.com",
                                          "DISTRIBUTOR");

            IBackendService personify = new PersonifyService();
            List<string> masterIdList = null;
            bool dnsFlag = false;
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList, ref dnsFlag);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual(companyInfo.Name, "Reconcile Company Distributor 1");

            //Decorator 
            company = GetStoreCompany("No matchhhh",
                                      "2135555553",
                                      "individual8@reconcile.com",
                                      "DECORATOR");

            companyInfo = personify.FindCompanyInfo(company, ref masterIdList, ref dnsFlag);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual(companyInfo.Name, "Reconcile Company Decorator 2");
        }

        [TestMethod]
        public void ReconcileCompanyMultipleMatches()
        {
            //Distributor
            var company = GetStoreCompany("Reconcile Company Distributor 1",
                                          "2135555552",
                                          "individual3@reconcile.com",
                                          "DISTRIBUTOR");

            IBackendService personify = new PersonifyService();
            List<string> masterIdList = null;
            bool dnsFlag = false;
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList, ref dnsFlag);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual("DISTRIBUTOR", companyInfo.MemberType);

            //Decorator with LEAD
            company = GetStoreCompany("Reconcile Company Decorator 1",
                                      "2135555553",
                                      "individual8@reconcile.com",
                                      "DECORATOR");

            companyInfo = personify.FindCompanyInfo(company, ref masterIdList, ref dnsFlag);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual(companyInfo.Name, "Reconcile Company Decorator 1");
            Assert.IsTrue(companyInfo.MemberStatus.ToUpper() == "ASICENTRAL" ||
                          companyInfo.MemberStatus.ToUpper() == "LEAD");

            //Decorator without LEAD
            company = GetStoreCompany("Reconcile Company Decorator 2",
                                      "2135555553",
                                      "individual8@reconcile.com",
                                      "DECORATOR");

            companyInfo = personify.FindCompanyInfo(company, ref masterIdList, ref dnsFlag);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.IsTrue(companyInfo.MemberStatus.ToUpper() != "ASICENTRAL" &&
                          companyInfo.MemberStatus.ToUpper() != "LEAD");
        }

        [TestMethod]
        public void ReconcileCompanySupplierNoMatch()
        {
            var company = GetStoreCompany("Invalid XXX Name",
                                          "1110011100",
                                          "nomatch@reconcile.com",
                                          "SUPPLIER");

            IBackendService personify = new PersonifyService();
            List<string> masterIdList = null;
            bool dnsFlag = false;
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList, ref dnsFlag);
            Assert.AreEqual(companyInfo, null);
        }

        private StoreCompany GetStoreCompany(string name, string phone, string email, string memberType)
        {
            var company = new StoreCompany
            {
                Name = name,
                Phone = phone,
                MemberType = memberType
            };
            var address = new StoreAddress
            {
                Street1 = "4805 Street Rd",
                City = "Trevose",
                State = "PA",
                Country = "USA",
                Zip = "19053"
            };
            company.Addresses.Add(new StoreCompanyAddress
            {
                Address = address,
                IsBilling = true,
                IsShipping = true,
            });

            if (!string.IsNullOrEmpty(email))
            {
                company.Individuals = new List<StoreIndividual>();
                company.Individuals.Add(new StoreIndividual() { Email = email, IsPrimary = true });
            }
            return company;
        }

        #endregion end FindCompanyInfo performance tests

        [TestMethod]
        public void AddActivity()
        {
            var company = GetStoreCompany("Reconcile Company Supplier 1",
                                          "2135555552",
                                          "individual4@reconcile.com",
                                          "SUPPLIER");

            IBackendService personify = new PersonifyService();
            List<string> masterIdList = null;
            bool dnsFlag = false;
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList, ref dnsFlag);            
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.IsTrue(masterIdList.Count > 0);
            Assert.IsTrue(dnsFlag);

            company.ExternalReference = string.Join(";", companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
            company.MatchingCompanyIds = string.Join("|", masterIdList);

            personify.AddActivity(company, 
                                  "This company tried to purchase supplier membership from the store but was rejected because the DNS flag is set to true. ", 
                                  Activity.Exception);
        }

        [TestMethod]
        public void PersonifyAddActivity()
        {
            var activity = SvcClient.Create<CusActivity>();
            activity.MasterCustomerId = "000010252985";
            activity.SubCustomerId = 0;
            activity.ActivityCode = "CONTACTTRACKING";
            activity.CallTopicCode = "EXCEPTION";
            activity.CallTopicSubcode = "VALIDATION";
            activity.CallTypeCode = "STORE"; 
            activity.ActivityDate = DateTime.Now;
            activity.Subsystem = "MRM";
            activity.ActivityText = "Test personify activity Type and Subject.";

            var result = SvcClient.Save<CusActivity>(activity);
            Assert.AreEqual(result.CallTopicCode, "EXCEPTION");
            Assert.AreEqual(result.CallTopicSubcode, "VALIDATION");
            Assert.AreEqual(result.CallTypeCode, "STORE");
        }

        private IStoreService MockupStoreService()
        {
            var products = new List<ContextProduct>();
            products.Add(new ContextProduct { Id = 77, HasBackEndIntegration = true });
            products.Add(new ContextProduct { Id = 61, HasBackEndIntegration = true });
            var emailExpresses = new List<StoreDetailEmailExpress>();
            emailExpresses.Add(new StoreDetailEmailExpress { OrderDetailId = 1, ItemTypeId = 1 });
            var mappings = new List<PersonifyMapping>();
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 77, StoreOption = "0", PersonifyProduct = 14471, PersonifyRateCode = "STD", PersonifyRateStructure = "MEMBER" });
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 61, StoreOption = "1;1X", PersonifyProduct = 1587, PersonifyRateCode = "1X", PersonifyRateStructure = "MEMBER" });
            var codes = new List<LookSendMyAdCountryCode>();
            codes.Add(new LookSendMyAdCountryCode { Alpha2 = "USA", Alpha3 = "USA", CountryName = "United States" });

            var mockObjectService = new Mock<IStoreService>();
            mockObjectService.Setup(objectService => objectService.GetAll<ContextProduct>(true)).Returns(products.AsQueryable());
            mockObjectService.Setup(objectService => objectService.GetAll<StoreDetailEmailExpress>(true)).Returns(emailExpresses.AsQueryable());
            mockObjectService.Setup(objectService => objectService.GetAll<PersonifyMapping>(true)).Returns(mappings.AsQueryable());
            mockObjectService.Setup(objectService => objectService.GetAll<LookSendMyAdCountryCode>(true)).Returns(codes.AsQueryable());
            return mockObjectService.Object;
        }

        private static StoreOrder CreateOrder(string asiNumber, ContextProduct[] products)
        {
            var tag = DateTime.Now.Ticks;
            var address1 = new StoreAddress()
            {
                City = "Trevose",
                Country = "USA",
                State = "PA",
                Street1 = "Street1",
                Zip = "19053"
            };
            var address2 = new StoreAddress()
            {
                City = "Feasterville Trevose",
                Country = "USA",
                State = "PA",
                Street1 = "4800 Street Road",
                Zip = "19053",
            };
            var address3 = new StoreAddress()
            {
                City = "Feasterville Trevose",
                Country = "USA",
                State = "PA",
                Street1 = "Street2",
                Zip = "19053",
            };
            var companyAddresses = new List<StoreCompanyAddress>()
			{
				new StoreCompanyAddress() { Address = address1, IsBilling = false, IsShipping = false },
				new StoreCompanyAddress() { Address =  address2, IsShipping = true, IsBilling = false },
                new StoreCompanyAddress() { Address =  address3, IsShipping = false, IsBilling = true },
			};
            var person = new StoreIndividual()
            {
                Address = address1,
                IsPrimary = true,
                FirstName = "Yann",
                LastName = "Perrin",
                Title = "Accountant",
                Email = asiNumber == string.Empty ? "test" + tag + "@gmail.com" : "perrin.yann@gmail.com",
            };
            var contacts = new List<StoreIndividual>() { person };
            var company = new StoreCompany()
            {
                Name = "ORDER Test9 " + tag,
                Addresses = companyAddresses,
                Individuals = contacts,
                ASINumber = asiNumber,
                Phone = "2153233242",
                WebURL = "http://asicentral.com/default.aspx"
            };
            var creditCard = new StoreCreditCard()
            {
                CardType = "Visa",
                CardNumber = "4111111111111111",
                ExpMonth = "11",
                ExpYear = "2015",
                CardHolderName = "ASI Store",
            };
            var orderDetails = new List<StoreOrderDetail>();
            var order = new StoreOrder()
            {
                Company = company,
                AnnualizedTotal = 10,
                Total = 10,
                BillingIndividual = person,
                OrderDetails = orderDetails,
                OrderRequestType = "Supplier",
                CreditCard = creditCard,
            };
            foreach (var product in products)
            {
                var orderDetail = new StoreOrderDetail()
                {
                    Id = 1,
                    ApplicationCost = 0,
                    Cost = 0,
                    OptionId = 0,
                    Quantity = 2,
                    IsSubscription = true,
                    Product = product,
                    Order = order,
                    DateOption = DateTime.Now.AddDays(2),
                };
                orderDetails.Add(orderDetail);
            }
            return order;
        }
    }
}
