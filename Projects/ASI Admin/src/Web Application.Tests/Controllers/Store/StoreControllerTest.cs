using Moq;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.web.Controllers.Store;
using asi.asicentral.web.model.store;

namespace asi.asicentral.WebApplication.Tests.Controllers.Store
{
    [TestClass]
    public class StoreControllerTest
    {
        [TestMethod]
        public void OrderList()
        {
            // arrange            
            OrdersController controller = new OrdersController();
            IList<LegacyOrderDetail> orderDetails = new List<LegacyOrderDetail>();

            Guid userid = Guid.NewGuid();

            ASPNetMembership membership = new ASPNetMembership()
            {
                ApplicationId = new Guid("71792474-3BB3-4108-9C88-E26179DB443A"),
                UserId = userid,
                CreateDate = DateTime.Now.AddDays(-6),
            };

            LegacyOrder order = new LegacyOrder()
            {
                Id = 0,
                BillFirstName = "Billing Name",
                BillLastName = "Billing Last",
                Status = true,
                ProcessStatus = OrderStatus.Pending,
                DateCreated = DateTime.Now.AddDays(-3),
                UserId = userid,
                TransId = Guid.NewGuid(),
                Membership = membership
            };

            LegacyOrderProduct orderProduct = new LegacyOrderProduct()
            {
                Id = 102,
                Description = "Supplier Membership",
                Summary = "Supplier"
            };

            LegacyOrderDetail orderDetail = new LegacyOrderDetail()
            {
                Added = DateTime.Now.AddDays(-3),
                Order = order,
                OrderId = order.Id,
                Product = orderProduct,
                ProductId = orderProduct.Id,
                Quantity = 1,
                Subtotal = 500,
            };

            LegacyOrderCreditCard orderCreditCard = new LegacyOrderCreditCard()
            {
                ExpMonth = "May",
                ExpYear = "2015",
                ExternalReference = new Guid().ToString(),
                Name = "First Last",
                Number = "5442254242555033",
                Order = order,
                TotalAmount = orderDetail.Subtotal,
                Type = "Mastercard"
            };

            orderDetails.Add(orderDetail);

            Mock<IStoreService> mockObjectService = new Mock<IStoreService>();
            mockObjectService.Setup(objectService => objectService.GetAll<LegacyOrderDetail>("Order;Order.Membership;Order.CreditCard", true)).Returns(orderDetails.AsQueryable());
            controller.StoreService = mockObjectService.Object;

            // act - getting results from the last 6 days
            ViewResult result = (ViewResult)controller.List(DateTime.Now.AddDays(-6), DateTime.Now, "", null, "", "", "");
            OrderPageModel pageModel = (OrderPageModel)result.Model;
            Assert.IsNotNull(pageModel);

            OrderModel orderModel = pageModel.Orders.Where(theOrder => theOrder.OrderId == 0).SingleOrDefault();
            Assert.IsNotNull(orderModel);
        }
    }
}
