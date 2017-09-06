using Moq;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.Controllers.Store;
using asi.asicentral.web.model.store;
using NUnit.Framework;

namespace asi.asicentral.WebApplication.Tests.Controllers.Store
{
    [TestFixture]
    public class StoreControllerTest
    {
        [Test]
        public void OrderList()
        {
            // arrange            
            OrdersController controller = new OrdersController();
            IList<StoreOrderDetail> orderDetails = new List<StoreOrderDetail>();

            StoreIndividual billingIndividual = new StoreIndividual()
            {
                FirstName = "Billing Name",
                LastName = "Billing Last",
            };

            StoreCreditCard orderCreditCard = new StoreCreditCard()
            {
                ExpMonth = "May",
                ExpYear = "2015",
                ExternalReference = new Guid().ToString(),
                CardNumber = "5442254242555033",
                CardType = "Mastercard"
            };

            ContextProduct orderProduct = new ContextProduct()
            {
                Id = 102,
                Name = "Supplier Membership",
                IsSubscription = true,
            };

            StoreOrder order = new StoreOrder()
            {
                Id = 0,
                IsCompleted = true,
                ProcessStatus = OrderStatus.Pending,
                CreateDate = DateTime.Now.AddDays(-3),
                CreditCard = orderCreditCard,
            };

            StoreOrderDetail orderDetail = new StoreOrderDetail()
            {
                CreateDate = DateTime.Now.AddDays(-3),
                Order = order,
                Product = orderProduct,
                Quantity = 1,
                Cost = 500,
            };

            order.OrderDetails.Add(orderDetail);
            orderDetails.Add(orderDetail);

            Mock<IStoreService> mockObjectService = new Mock<IStoreService>();
            mockObjectService.Setup(objectService => objectService.GetAll<StoreOrderDetail>(true)).Returns(orderDetails.AsQueryable());
            controller.StoreService = mockObjectService.Object;
            Mock<IEncryptionService> mockEncryptionService = new Mock<IEncryptionService>();
            mockEncryptionService.Setup(encryptionSrvc => encryptionSrvc.LegacyDecrypt("mk8$3njkl", orderCreditCard.CardNumber)).Returns(orderCreditCard.CardNumber);
            controller.EncryptionService = mockEncryptionService.Object;

            // act - getting results from the last 6 days
           ViewResult result = (ViewResult)controller.List(DateTime.Now.AddDays(-6), DateTime.Now, "", null, "", "", "", "",true);
            OrderPageModel pageModel = (OrderPageModel)result.Model;
            Assert.IsNotNull(pageModel);

            OrderModel orderModel = pageModel.Orders.Where(theOrder => theOrder.OrderId == 0).SingleOrDefault();
            Assert.IsNotNull(orderModel);
        }
    }
}
