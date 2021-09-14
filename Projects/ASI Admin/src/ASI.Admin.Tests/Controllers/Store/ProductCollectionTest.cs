using System;
using asi.asicentral.model.store;
using Moq;
using System.Web.Mvc;
using asi.asicentral.web.Controllers.Store;
using System.Collections.Generic;
using asi.asicentral.interfaces;
using asi.asicentral.web.model.store;
using System.Linq;
using NUnit.Framework;

namespace asi.asicentral.WebApplication.Tests.Controllers.Store
{
    [TestFixture]
    public class ProductCollectionTest
    {
        public ContextProduct CreateProduct(int id = 1)
        {
            ContextProduct product = new ContextProduct()
            {
                Name = "test product" + id.ToString(),
                Cost = 100,
                Id = id,
                Origin = "ASI"
            };
            return product;
        }

        public StoreAddress CreateAddress()
        {
            StoreAddress address = new StoreAddress()
            {
                Street1 = "address1",
                Street2 = "address2",
                City = "city",
                State = "Pennsylvania",
                Country = "USA",
                Zip = "1234",
                Phone = "23445"
            };
            return address;
        }

        public StoreCompanyAddress CreateCompanyWithAddress()
        {
            StoreCompanyAddress companywithAddress = new StoreCompanyAddress()
            {
                Address = CreateAddress(),
                IsBilling = false,
                IsShipping = false,
            };
            return companywithAddress;
        }

        [Test]
        [Ignore("Ignore a test")]
        public void EditProductCollections()
        {
            StoreIndividual individual = new StoreIndividual() { LastName = "Last", FirstName = "First" };
            StoreOrder order = new StoreOrder() { Id = 0, BillingIndividual = individual };
            StoreOrderDetail detail = new StoreOrderDetail { Id = 5, Order = order, };
            StoreOrderDetail detail1 = new StoreOrderDetail { Id = 6, Order = order, };
            List<StoreOrderDetail> details = new List<StoreOrderDetail>();
            List<StoreDetailProductCollection> productCollections = new List<StoreDetailProductCollection>();
            List<StoreDetailProductCollectionItem> productCollectionItems = new List<StoreDetailProductCollectionItem>();
            details.Add(detail);
            details.Add(detail1);
            detail.Product = CreateProduct(64);
            detail.Quantity = 1;

            StoreDetailProductCollection productCollection = new StoreDetailProductCollection();
            productCollection.OrderDetailId = 5;
            productCollection.ItemMonthId = 1;
            productCollection.Month = 2;
            productCollection.Year = 2013;

            StoreDetailProductCollectionItem item3 = new StoreDetailProductCollectionItem();
            item3.ItemId = 1;
            item3.ItemNumbers = "123";
            item3.Collection = "abc";
            productCollectionItems.Add(item3);
            productCollection.ProductCollectionItems.Add(item3);
            productCollections.Add(productCollection);
            
            StoreDetailProductCollection productCollection1 = new StoreDetailProductCollection();
            productCollection1.OrderDetailId = 5;
            productCollection1.ItemMonthId = 2;
            productCollection1.Month = 3;
            productCollection1.Year = 2014;
            productCollection1.ProductCollectionItems = new List<StoreDetailProductCollectionItem>();
            StoreDetailProductCollectionItem item1 = new StoreDetailProductCollectionItem();
            item1.ItemId = 2;
            item1.ItemNumbers = "123";
            item1.Collection = "abc";
            productCollectionItems.Add(item1);
            productCollection1.ProductCollectionItems.Add(item1);

            StoreDetailProductCollectionItem item2 = new StoreDetailProductCollectionItem();
            item2.ItemId = 3;
            item2.ItemNumbers = "234";
            item2.Collection = "def";
            productCollectionItems.Add(item2);
            productCollection1.ProductCollectionItems.Add(item2);

            productCollections.Add(productCollection1);

            order.CreditCard = new StoreCreditCard() { ExternalReference = "111" };
            StoreOrder orderRef = order;
            order.Company = new StoreCompany();
            order.Company.Name = "Company";
            order.Company.Individuals.Add(new StoreIndividual() { FirstName = "First1", LastName = "Last1", Id = 0 });
            order.Company.Individuals.Add(new StoreIndividual() { FirstName = "First2", LastName = "Last2", Id = 1 });

            order.Company.Addresses = new List<StoreCompanyAddress>();
            order.Company.Addresses.Add(CreateCompanyWithAddress());

            Mock<IStoreService> mockStoreService = new Mock<IStoreService>();
            mockStoreService.Setup(service => service.GetAll<StoreOrderDetail>(false)).Returns(details.AsQueryable());
            mockStoreService.Setup(service => service.GetAll<StoreDetailProductCollection>(false)).Returns(productCollections.AsQueryable());
            mockStoreService.Setup(service => service.GetAll<StoreDetailProductCollectionItem>(false)).Returns(productCollectionItems.AsQueryable());

            ApplicationController controller = new ApplicationController();
            controller.StoreService = mockStoreService.Object;

            ProductCollectionsModel model = new ProductCollectionsModel();
            model.productCollections = new List<StoreDetailProductCollection>();
            StoreDetailProductCollection productCollection2 = new StoreDetailProductCollection();
            productCollection2.OrderDetailId = 5;
            productCollection2.ItemMonthId = 2;
            productCollection2.Month = 3;
            productCollection2.Year = 2014;
            productCollection2.ProductCollectionItems = new List<StoreDetailProductCollectionItem>();
            StoreDetailProductCollectionItem item4 = new StoreDetailProductCollectionItem();
            item4.ItemId = 2;
            item4.ItemNumbers = "4";
            item4.Collection = "bcd";

            model.productCollections.Add(productCollection2);
            model.productCollections.Add(productCollection1);
            productCollection.ProductCollectionItems.ElementAt(0).Collection = "bcd";
            productCollection.ProductCollectionItems.ElementAt(0).ItemNumbers = "4";
            model.OrderDetailId = 5;
            model.ActionName = ApplicationController.COMMAND_SAVE;
            model.ExternalReference = "102";

            // Product Collections -- 64
            RedirectToRouteResult result = controller.EditProductCollections(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.IsNotNull(detail);
            Assert.AreEqual(detail.Quantity, 1);
            Assert.AreEqual(productCollection.ProductCollectionItems.ElementAt(0).Collection, item4.Collection);
            Assert.AreEqual(productCollection.ProductCollectionItems.ElementAt(0).ItemNumbers, item4.ItemNumbers);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(1));

            // user clicks reject - order should be updated to reject
            model.ActionName = ApplicationController.COMMAND_REJECT;
            RedirectToRouteResult result2 = controller.EditProductCollections(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(2));

            // user clicks approve with reference id
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            RedirectToRouteResult result3 = controller.EditProductCollections(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(3));
        }
    }
}
