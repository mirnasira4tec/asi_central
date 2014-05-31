using System;
using System.Collections.Generic;
using System.Linq;
using asi.asicentral.database.mappings;
using asi.asicentral.interfaces;
using asi.asicentral.model;
using asi.asicentral.model.sgr;
using asi.asicentral.model.store;
using asi.asicentral.PersonifyDataASI;
using asi.asicentral.services;
using asi.asicentral.services.PersonifyProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace asi.asicentral.Tests
{
	[TestClass]
	public class PersonifyTest
	{
		[TestMethod]
		public void StoreAndAssignCCTest()
		{
			CreditCard cc = new CreditCard()
			{
				Type = "Visa",
				Number = "4111111111111111",
				ExpirationDate = new DateTime(2014, 11, 15),
				CardHolderName = "ASI Store",
				Address = "4800 Street Road",
				City = "Trevose",
				State = "PA",
				Country = "USA",
				PostalCode = "19053",
			};
			//first store the credit card
			string identifier = PersonifyClient.GetCreditCardProfileId(cc);
			if (string.IsNullOrEmpty(identifier))
			{
				Assert.IsTrue(PersonifyClient.ValidateCreditCard(cc));
				identifier = PersonifyClient.SaveCreditCard(cc);
			}
			long profileIdentifier = long.Parse(identifier);
			//lookup company to assign CC to
			CustomerInfo companyInfo = PersonifyClient.GetCompanyInfoByAsiNumber("33020");
			Assert.IsNotNull(companyInfo);
			PersonifyClient.AssignCreditCard(profileIdentifier, companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
		}

		[TestMethod]
		public void PlaceOrderNewCompanyTest()
		{
			using (IStoreService storeService = new StoreService(new Container(new EFRegistry())))
			{
				IBackendService personify = new PersonifyService(storeService);
				var supplierSpecials = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 77);
                StoreOrder order = CreateOrder("", new ContextProduct[] { supplierSpecials });
				personify.PlaceOrder(order);
			}
		}

		[TestMethod]
		public void PlaceOrderExistingCompanyTest()
		{
			using (IStoreService storeService = new StoreService(new Container(new EFRegistry())))
			{
				IBackendService personify = new PersonifyService(storeService);
				var supplierSpecials = storeService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == 77);
                StoreOrder order = CreateOrder("33020", new ContextProduct[] { supplierSpecials });
				personify.PlaceOrder(order);
			}
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
			var companyAddresses = new List<StoreCompanyAddress>()
			{
				new StoreCompanyAddress() { Address = address1, IsBilling = false, IsShipping = false },
				new StoreCompanyAddress() { Address =  address2, IsShipping = true, IsBilling = true },
			};
			var person = new StoreIndividual()
			{
				Address = address1,
				IsPrimary = true,
				FirstName = "Store Test " + tag,
				LastName = "Store Test " + tag,
				Title = "Server",
			};
			var contacts = new List<StoreIndividual>() { person };
			var company = new StoreCompany()
			{
				Name = "ORDER Test " + tag,
				Addresses = companyAddresses,
				Individuals = contacts,
				ASINumber = asiNumber,				
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
            };
            foreach (var product in products)
            {
                var orderDetail = new StoreOrderDetail()
                {
                    ApplicationCost = 0,
                    Cost = 0,
                    OptionId = 0,
                    IsSubscription = true,
                    Product = product,
                    Order = order,
                };
                orderDetails.Add(orderDetail);
            }
			return order;
		}
	}
}
