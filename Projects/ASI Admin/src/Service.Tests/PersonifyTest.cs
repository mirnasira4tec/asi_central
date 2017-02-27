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
using asi.asicentral.model.personify;

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
            var order = CreateStoreOrder(string.Empty, new ContextProduct[] { supplierSpecials, emailExpress }, "PlaceOrderNewCompanyTest");
            var companyInfo = CreatePersonifyOrder(order, storeService);
            Assert.IsNotNull(companyInfo);
            Assert.IsNotNull(order.BackendReference);
        }

        [TestMethod]
        public void PlaceOrderExistingCompanyTest()
        {  // supplierSpecials and emailExpress don't have personify integrations any more
            IStoreService storeService = MockupStoreService();
            var supplierSpecials = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 77);
            var emailExpress = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 61);
            var order = CreateStoreOrder("30279", new ContextProduct[] { supplierSpecials, emailExpress }, "PlaceOrderExistingCompanyTest");
            var companyInfo = CreatePersonifyOrder(order, storeService);
            Assert.IsNotNull(companyInfo);
            Assert.IsNotNull(order.BackendReference);
        }

        [TestMethod]
        public void AddEEXSubscription()
        {
            var tag = DateTime.Now.Ticks.ToString();
            tag = tag.Substring(tag.Length - 5);

            var storeService = MockupStoreService();
            var personifyService = new PersonifyService(storeService);
            var user = new User () 
                { AsiNumber = "", FirstName = "EEX" + tag, LastName = "EEXSubscription", Email = string.Format("DistEEX{0}@gmail.com", tag),
                  PhoneAreaCode = "201", Phone = "34" + tag, CompanyName = "Distributor EEX Subscription " + tag,
                  MemberType_CD = "Distributor", 
                   Street1 = "1 Street1", City = "Trevose", State = "PA", Country = "USA" };

            var companyInfo = personifyService.AddEEXSubscription(user, true);
            Assert.IsNotNull(companyInfo);

            user = new User()
            {
                AsiNumber = "",
                FirstName = "EEX-supplier" + tag,
                LastName = "EEXSubscription",
                Email = string.Format("SupEEX{0}@gmail.com", tag),
                PhoneAreaCode = "202",
                Phone = "34" + tag,
                CompanyName = "Supplier EEX Subscription",
                MemberType_CD = "Supplier",
                Street1 = "1 Street1",
                City = "Trevose",
                State = "PA",
                Country = "USA"
            };

            companyInfo = personifyService.AddEEXSubscription(user, false);
            Assert.IsNotNull(companyInfo);

            user = new User()
            {
                AsiNumber = "",
                FirstName = "EEX-Deco" + tag,
                LastName = "EEXSubscription",
                Email = string.Format("DecoEEX{0}@gmail.com", tag),
                PhoneAreaCode = "203",
                Phone = "34" + tag,
                CompanyName = "Decorator EEX Subscription",
                MemberType_CD = "Decorator",
                Street1 = "1 Street1",
                City = "Trevose",
                State = "PA",
                Country = "USA"
            };

            companyInfo = personifyService.AddEEXSubscription(user, false);
            Assert.IsNotNull(companyInfo);
        }

        [TestMethod]
        public void PackageOrderTest()
        {
            IStoreService storeService = MockupStoreService();
            var distributor = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 7);
            var order = CreateStoreOrder("12345", new ContextProduct[] { distributor }, "PlaceBundleOrderTest");
            var companyInfo = CreatePersonifyOrder(order, storeService);
            Assert.IsNotNull(companyInfo);
            Assert.IsNotNull(order.BackendReference);
        }

        [TestMethod]
        public void EmailMrktOrderTest()
        {
            IStoreService storeService = MockupStoreService();
            var distributor = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 126);
            var order = CreateStoreOrder("181255", new ContextProduct[] { distributor }, "EmailMrktOrderTest");
            order.OrderDetails[0].OptionId = 1;
            var companyInfo = CreatePersonifyOrder(order, storeService);
            Assert.IsNotNull(companyInfo);
            Assert.IsNotNull(order.BackendReference);
        }

        // rate-code is not set properly on staging
        [Ignore] 
        [TestMethod]
        public void BundleOrderDistributorFirstMonthFree()
        {
            var storeService = MockupStoreService("DISTFIRSTMONTH");
            var distributor = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 7);
            var context = storeService.GetAll<Context>(true).FirstOrDefault(c => c.Id == 8);
            var order = CreateStoreOrder("12345", new ContextProduct[] { distributor }, "PlaceBundleOrderTest", "DISTFIRSTMONTH", 199, context);
            var companyInfo = CreatePersonifyOrder(order, storeService);
            Assert.IsNotNull(companyInfo);
            Assert.IsNotNull(order.BackendReference);
        }

        [TestMethod]
        public void MemberOrderSupplierWaveFirstMonth()
        {
            IStoreService storeService = MockupStoreService();
            var distributor = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 2);
            var order = CreateStoreOrder("12345", new ContextProduct[] { distributor }, "MemberOrderSupplierWaveFirstMonth");
            var companyInfo = CreatePersonifyOrder(order, storeService);
            Assert.IsNotNull(companyInfo);
            Assert.IsNotNull(order.BackendReference);
        }

        [TestMethod]
        public void NoOrderCreatedForTerminatedCompany()
        {
            IStoreService storeService = MockupStoreService();
            var distributor = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 2);
            var order = CreateStoreOrder("10275773", new ContextProduct[] { distributor }, "BundleOrderSupplierWaveFirstMonth");
            order.Company.Name = "Terminateddist001@gmail.Com";
            order.Company.Phone = "9999990001";
            var companyInfo = CreatePersonifyOrder(order, storeService);
            Assert.IsNotNull(companyInfo);
            Assert.IsTrue(companyInfo.IsTerminated());
            Assert.IsNull(order.BackendReference);
        }

        [TestMethod]
        public void AddPersonifyCompanyForDualDecorator()
        {
            StoreCompany company = GetStoreCompany("NewMembership", 
                                                   "151111222222", 
                                                   "newMembershipPurchase@unittest.com", 
                                                   "Distributor");

            IEnumerable<StoreAddressInfo> storeAddress = null; 
            var companyInfo = PersonifyClient.ReconcileCompany(company, "DISTRIBUTOR", null, ref storeAddress, true);
            var storeService = MockupStoreService();
            IBackendService personify = new PersonifyService(storeService);

            var order = new StoreOrder()
            {
                Company = company,
                OrderRequestType = "Distributor",
            };

            var product = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 70);
            order.OrderDetails.Add(new StoreOrderDetail() 
                         {
                             Id = 1,
                             ApplicationCost = 0,
                             Cost = 0,
                             OptionId = 0,
                             Quantity = 1,
                             Product = product,
                         });

            var creditCard = new CreditCard()
            {
                Type = "Visa",
                Number = "4111111111111111",
                ExpirationDate = new DateTime(2018, 11, 15),
                CardHolderName = "ASI Store",
                Address = "Street",
                City = "City",
                State = "NJ",
                Country = "USA",
                PostalCode = "98123",
            };

            var profile = personify.SaveCreditCard(order, creditCard);
            Assert.IsNotNull(profile);
            Assert.AreNotEqual(string.Join(";", companyInfo.MasterCustomerId, companyInfo.SubCustomerId), 
                               order.Company.ExternalReference);
            Assert.AreNotEqual("distributor", order.Company.MemberType.ToLower());
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
            var accountInfo = new User
            {
                FirstName = "New First Name",
                LastName =  "New Last Name",
                Email = String.Format("user{0}@addCompany.com", tag),
                CompanyName = "New Company1 " + tag,
                Street1 = "4800 Street Rd",
                City = "Trevose",
                State = "PA",
                Zip = "19053",
                Country = "USA",
                MemberTypeId  = 6,
                CountryCode = "USA"
            };
            IBackendService personify = new PersonifyService(MockupStoreService());
            companyInfo = personify.AddCompany(accountInfo);
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

        private CompanyInformation CreatePersonifyOrder(StoreOrder order, IStoreService storeService)
        {
            IBackendService personify = new PersonifyService(storeService);

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
            return personify.PlaceOrder(order, new Mock<IEmailService>().Object, null);
        }
        [TestMethod]
        public void AddPhoneNumberTest()
        {
            var companyInfo = PersonifyClient.GetCompanyInfoByASINumber("33020");
	        if (companyInfo != null)
	        {
		        PersonifyClient.AddPhoneNumber("2222222222", "USA", companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
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
        public void TestIsNewMembership()
        {

            IStoreService storeService = MockupStoreService();

            // test active distributor is buying supplier membership
            var distributor = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 5);
            var order = CreateStoreOrder("10275773", new ContextProduct[] { distributor }, "TestIsNewMembership");
            //order.OrderRequestType = "Distributor";

            order.Company = GetStoreCompany("Create Company Dist",
                              "2152220001",
                              "newDist001@gmail.com",
                              "DISTRIBUTOR");

            IBackendService personify = new PersonifyService(storeService);
            List<string> masterIdList = null;
            var companyInfo = personify.FindCompanyInfo(order.Company, ref masterIdList);
            //if (companyInfo == null)
            //{
            //    companyInfo = personify.CreateCompany(order.Company, "DISTRIBUTOR");
            //    companyInfo = personify.CreateCompany(order.Company, "DISTRIBUTOR");
            //}

            //companyInfo = personify.FindCompanyInfo(order.Company, ref masterIdList);
            Assert.IsNotNull(companyInfo);
            //Assert.AreEqual(companyInfo.MemberStatus, "ACTIVE");

            order.Company.MemberStatus = companyInfo.MemberStatus;
            order.Company.MemberType = companyInfo.MemberType;

            order.Company.MatchingCompanyIds = string.Join("|", masterIdList);
            var newMemberType = string.Empty;
            var isNewMembership = order.IsNewMemberShip(ref newMemberType);
            Assert.IsTrue(isNewMembership);

            var creditCard = new CreditCard()
            {
                Type = "Visa",
                Number = "4111111111111111",
                ExpirationDate = new DateTime(2018, 11, 15),
                CardHolderName = "ASI Store",
                Address = "Street",
                City = "City",
                State = "NJ",
                Country = "USA",
                PostalCode = "98123",
            };
            var profile = personify.SaveCreditCard(order, creditCard);

            Assert.IsNotNull(profile);

            // Test Supplier is buying supplier membership
        }

        [TestMethod]
        public void ReconcileCompanyMatchNameOnly()
        {
            IBackendService personify = new PersonifyService();
            List<string> masterIdList = null;

            //match name only  
            var company = GetStoreCompany("Reconcile Company Distributor 1",
                                          "11000000",
                                          "noMatch@reconcile.com", 
                                          "DISTRIBUTOR");

            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsNull(companyInfo);

            // match name with extra special characters and phone/email
            company = GetStoreCompany(".Reconcile $@#?Company $@#?$!&,Distributor 1.",
                                      "2135555571",
                                      "individual3@reconcile.com",
                                      "DISTRIBUTOR"); 

            companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual("DISTRIBUTOR", companyInfo.MemberType);
            Assert.AreEqual(companyInfo.Name, "Reconcile Company Distributor 1");
            Assert.IsTrue(companyInfo.DNSFlag);
        }

        [TestMethod]
        public void MatchDistributorChange()
        {
            //Distributor should not match to a delisted supplier
            //match name only  
            IBackendService personify = new PersonifyService();
            var company = GetStoreCompany("10 Again Inc",
                                          "818501800",
                                          "wholesale@10againclothing.com",
                                          "SUPPLIER");
            List<string> masterIdList = null;
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsNotNull(companyInfo);
            Assert.AreEqual("000000090868", companyInfo.MasterCustomerId);
            company.MemberType = "DISTRIBUTOR";
            if (masterIdList != null) masterIdList = null;
            var companyInfo1 = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo1 == null || companyInfo1.MasterCustomerId != companyInfo.MasterCustomerId);
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
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual(companyInfo.Name, "Reconcile Company Supplier 1");

            // match phone or email with LEAD matches
            company.Phone = "2135555552";
            companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.IsTrue(companyInfo.MemberStatus.ToUpper() == "ASICENTRAL" ||
                          companyInfo.MemberStatus.ToUpper() == "LEAD");

            company.Phone = "2135555553";
            companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.IsTrue(companyInfo.MemberStatus.ToUpper() == "ASICENTRAL" ||
                          companyInfo.MemberStatus.ToUpper() == "LEAD");

            // phone or email without LEAD matches
            company = GetStoreCompany("Failed aaa Match",
                                      "2135555531",
                                      "nommmmatch@reconcile.com",
                                      "SUPPLIER"); ;
            companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.IsTrue(string.IsNullOrEmpty(companyInfo.MemberStatus) || 
                          ( companyInfo.MemberStatus.ToUpper() != "ASICENTRAL" && companyInfo.MemberStatus.ToUpper() != "LEAD"));
        }

        [TestMethod]
        public void ReconcileCompanyWithPhoneEmailForMMSLoad()
        {
            // match email only
            var company = GetStoreCompany("11x17 Inc",
                                          "9035410100",
                                          "jrw@11x17.com",
                                          "MMS_LOAD");

            IBackendService personify = new PersonifyService();
            List<string> masterIdList = null;
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsNull(companyInfo);
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
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual(companyInfo.Name, "Reconcile Company Distributor 1");

            //Decorator 
            company = GetStoreCompany("No matchhhh",
                                      "2135555511",
                                      "individual8@reconcile.com",
                                      "DECORATOR");

            companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual(companyInfo.Name, "Reconcile Company Decorator 2");
        }

        [TestMethod]
        public void ReconcileCompanyMultipleMatches()
        {
            //Distributor matching both name and phone/email, one LEAD
            var company = GetStoreCompany("Reconcile Company Distributor 1",
                                          "2135555552",
                                          "individual3@reconcile.com",
                                          "DISTRIBUTOR");

            IBackendService personify = new PersonifyService();
            List<string> masterIdList = null;
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual("DISTRIBUTOR", companyInfo.MemberType);
            Assert.IsTrue(companyInfo.MemberStatus.ToUpper() == "ASICENTRAL" ||
                          companyInfo.MemberStatus.ToUpper() == "LEAD");

            //Supplier match both name and phone/email, multiple LEAD with the latest note
            company = GetStoreCompany("Reconcile Company Supplier 1",
                                      "2135555551",
                                      "individual10@reconcile.com",
                                      "Supplier");

            companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.AreEqual(companyInfo.Name, "Reconcile Company Supplier 1");
            Assert.IsTrue(companyInfo.MemberStatus.ToUpper() == "ASICENTRAL" ||
                          companyInfo.MemberStatus.ToUpper() == "LEAD");

            //Decorator matching name and email only
            company = GetStoreCompany("Reconcile Company Decorator 2",
                                      "2135555553",
                                      "individual8@reconcile.com",
                                      "DECORATOR");

            companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
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
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
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
                company.Individuals = new List<StoreIndividual>()
                {
                    new StoreIndividual() 
                      {   Email = email, 
                          IsPrimary = true, 
                          FirstName = "TestFirstName", 
                          LastName = "TestLastName",
                          Address = address
                      }
                };
            }
            return company;
        }

        #endregion end FindCompanyInfo performance tests

        [TestMethod]
        [Ignore]
        public void AddActivity()
        {
            var company = GetStoreCompany("Reconcile Company Supplier 1",
                                          "2135555552",
                                          "individual4@reconcile.com",
                                          "SUPPLIER");

            IBackendService personify = new PersonifyService();
            List<string> masterIdList = null;
            var companyInfo = personify.FindCompanyInfo(company, ref masterIdList);
            Assert.IsTrue(companyInfo.CompanyId > 0);
            Assert.IsTrue(masterIdList.Count > 0);
            Assert.IsTrue(companyInfo.DNSFlag);

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

        private IStoreService MockupStoreService(string couponCode = null)
        {
            var products = new List<ContextProduct>();
            products.Add(new ContextProduct { Id = 126, HasBackEndIntegration = true, Cost = 25 });
            products.Add(new ContextProduct { Id = 77, HasBackEndIntegration = true });
            products.Add(new ContextProduct { Id = 61, HasBackEndIntegration = true });
            products.Add(new ContextProduct { Id = 5, HasBackEndIntegration = true, Type = "Distributor Membership" });
            products.Add(new ContextProduct { Id = 2, HasBackEndIntegration = true, Type = "Supplier Membership", ApplicationCost = 250, Cost = 99 });
            products.Add(new ContextProduct { Id = 70, HasBackEndIntegration = true, Type = "Dual Distributor Decorator Membership" });
            products.Add(new ContextProduct { Id = 7, HasBackEndIntegration = true, Type = "Distributor Membership", ApplicationCost = 150, Cost = 199 });

            var emailExpresses = new List<StoreDetailEmailExpress>();
            emailExpresses.Add(new StoreDetailEmailExpress { OrderDetailId = 1, ItemTypeId = 1 });
            var mappings = new List<PersonifyMapping>();
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 126, StoreOption = "1", PersonifyProduct = 458162300, PersonifyRateCode = "500_credits", PersonifyRateStructure = "MEMBER", ProductCode="Email_Marketing" });
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 77, StoreOption = "0", PersonifyProduct = 14471, PersonifyRateCode = "STD", PersonifyRateStructure = "MEMBER" });
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 61, StoreOption = "1;1X", PersonifyProduct = 1587, PersonifyRateCode = "1X", PersonifyRateStructure = "MEMBER" });
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 5, PersonifyProduct = 1003113722, PersonifyRateCode = "FY_DISTMEM", PersonifyRateStructure = "MEMBER", ClassCode = "DISTRIBUTOR", PaySchedule = true});
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 70, PersonifyProduct = 1003113722, PersonifyRateCode = "", PersonifyRateStructure = "MEMBER", ClassCode = "DISTRIBUTOR" });
            //mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 2, PersonifyProduct = 1003113955, StoreOption = "SUPWAVEAPPFEE", PersonifyRateCode = "FP_SUPMEMSTDCON", PersonifyRateStructure = "BUNDLE", PersonifyBundle = "SUPPLIERMEM_STD_CON" });
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 2, PersonifyProduct = 439255209, StoreOption = string.Empty, PersonifyRateCode = "FP_SUPMEMSTDCON", PersonifyRateStructure = "MEMBER", ClassCode = "SUPPLIER", PaySchedule = true });
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 2, PersonifyProduct = 439255209, StoreOption = string.Empty, PersonifyRateCode = "STD_SUPMEMSTD", PersonifyRateStructure = "MEMBER", ClassCode = "SUPPLIER", PaySchedule = true });
            mappings.Add(new PersonifyMapping { StoreContext = null, StoreProduct = 7, PersonifyProduct = 438951661, PersonifyRateCode = "FP_ESPPMDLMORD14", PersonifyRateStructure = "MEMBER", ClassCode = "DISTRIBUTOR", PaySchedule = true });
            mappings.Add(new PersonifyMapping { StoreContext = 8, StoreProduct = 7, StoreOption = "DISTFIRSTMONTH", PersonifyRateCode = "TRIAL1000_ESPPMDLMORD15", PersonifyRateStructure = "MEMBER", ClassCode = "DISTRIBUTOR", PaySchedule = true });
            var codes = new List<LookSendMyAdCountryCode>();
            codes.Add(new LookSendMyAdCountryCode { Alpha2 = "USA", Alpha3 = "USA", CountryName = "United States" });

            var mockObjectService = new Mock<IStoreService>();
            mockObjectService.Setup(objectService => objectService.GetAll<ContextProduct>(true)).Returns(products.AsQueryable());
            mockObjectService.Setup(objectService => objectService.GetAll<StoreDetailEmailExpress>(true)).Returns(emailExpresses.AsQueryable());
            mockObjectService.Setup(objectService => objectService.GetAll<PersonifyMapping>(true)).Returns(mappings.AsQueryable());
            mockObjectService.Setup(objectService => objectService.GetAll<LookSendMyAdCountryCode>(true)).Returns(codes.AsQueryable());

            if (!string.IsNullOrEmpty(couponCode))
            {
                var contexts = new List<Context>()
                {
                    new Context() { Id = 8, Products = new List<ContextProductSequence>(){ new ContextProductSequence () { Product = products.FirstOrDefault(p => p.Cost > 0), Cost = 199, ApplicationCost = 150 } } }
                };
                mockObjectService.Setup(objectService => objectService.GetAll<Context>(true)).Returns(contexts.AsQueryable());
            }

            return mockObjectService.Object;
        }

        private static StoreOrder CreateStoreOrder(string asiNumber, ContextProduct[] products, string testName, 
                                              string couponCode=null, decimal discountAmount = 0, Context context = null)
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
                FirstName = string.IsNullOrEmpty(testName) ? "UnitTest" : testName,
                LastName = "UnitTest" + tag,
                Title = "Accountant",
                Email = "unitTest" + tag + "@gmail.com"
            };
            var contacts = new List<StoreIndividual>() { person };
            var company = new StoreCompany()
            {
                Name = string.Format("{0} {1}", string.IsNullOrEmpty(testName) ? "ORDER Test" : testName , tag),
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
                ExpYear = "2017",
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
                CreditCard = creditCard
            };
            foreach (var product in products)
            {
                Coupon coupon = null;
                if( !string.IsNullOrEmpty(couponCode) && discountAmount > 0 && context != null)
                {
                    coupon = new Coupon() { ProductId = product.Id, CouponCode = couponCode, AppFeeDiscount = discountAmount };
                    order.ContextId = context.Id;
                }

                var orderDetail = new StoreOrderDetail()
                {
                    Id = 1,
                    ApplicationCost = product.ApplicationCost,
                    Cost = product.Cost,
                    OptionId = 0,
                    Quantity = 1,
                    IsSubscription = true,
                    Product = product,
                    Order = order,
                    DateOption = DateTime.Now.AddDays(2),
                    Coupon = coupon
                };
                orderDetails.Add(orderDetail);
            }
            return order;
        }
    }
}
