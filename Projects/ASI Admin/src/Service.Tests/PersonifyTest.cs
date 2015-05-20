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
				ASINumber = "33020",
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
