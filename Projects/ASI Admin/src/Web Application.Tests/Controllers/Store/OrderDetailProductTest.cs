using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using asi.asicentral.model.store;
using asi.asicentral.interfaces;
using asi.asicentral.web.Controllers.Store;
using asi.asicentral.web.model.store;
using System.Web.Mvc;
using NUnit.Framework;

namespace asi.asicentral.WebApplication.Tests.Controllers.Store
{
    [TestFixture]
    public class OrderDetailProductTest
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

        [Test]
        public void EditOrderDetailProduct()
        {
            Guid guid = Guid.NewGuid();
            StoreIndividual individual = new StoreIndividual() { LastName = "Last", FirstName = "First" };
            StoreOrder order = new StoreOrder() { Id = 0, BillingIndividual = individual };
            StoreOrderDetail detail = new StoreOrderDetail { Id = 5, Order = order, };
            ContextProduct product = CreateProduct(1);
            detail.Product = product;
            List<StoreOrderDetail> details = new List<StoreOrderDetail>();
            details.Add(detail);
            detail.Order = order;
            detail.Quantity = 30;

            order.CreditCard = new StoreCreditCard() { ExternalReference = "111" };
            StoreOrder orderRef = order;
            StoreCompany company = new StoreCompany()
            {
                Name = "Company",
            };
            order.Company = company;
            company.Individuals.Add(new StoreIndividual() { FirstName = "First1", LastName = "Last1", Id = 0 });
            company.Individuals.Add(new StoreIndividual() { FirstName = "First2", LastName = "Last2", Id = 1 });

            Mock<IStoreService> mockStoreService = new Mock<IStoreService>();
            Mock<IFulfilmentService> mockFulFilService = new Mock<IFulfilmentService>();
            mockStoreService.Setup(service => service.GetAll<StoreOrderDetail>(false)).Returns(details.AsQueryable());

            ApplicationController controller = new ApplicationController();
            controller.FulfilmentService = mockFulFilService.Object;
            controller.StoreService = mockStoreService.Object;

            OrderDetailApplicationModel model = new OrderDetailApplicationModel(detail);
            model.Quantity = 40;
            model.ExternalReference = "102";
            model.ActionName = ApplicationController.COMMAND_SAVE;

            //Verification for Quantity is saving as intended
            RedirectToRouteResult result = controller.EditOrderDetailProduct(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.IsNotNull(detail);
            Assert.AreEqual(detail.Quantity, model.Quantity);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(1));

            // user clicks reject - order should be updated to reject
            model.ActionName = ApplicationController.COMMAND_REJECT;
            RedirectToRouteResult result2 = controller.EditOrderDetailProduct(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(2));

            // user clicks approve with reference id
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            RedirectToRouteResult result3 = controller.EditOrderDetailProduct(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(3));
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(1));

            model = new OrderDetailApplicationModel(detail);
            model.AcceptedByName = "Test User";
            product.Id = 62;

            //Verification for AcceptedByName is saving as intended
            result = controller.EditOrderDetailProduct(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.IsNotNull(detail);
            Assert.AreEqual(detail.Quantity, model.Quantity);
            Assert.AreEqual(detail.AcceptedByName, model.AcceptedByName);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(4));
            
            // user clicks reject - order should be updated to reject
            model.ActionName = ApplicationController.COMMAND_REJECT;
            result2 = controller.EditOrderDetailProduct(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(5));

            // user clicks approve with reference id
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            result3 = controller.EditOrderDetailProduct(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(6));
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(2));
        }
    }
}
