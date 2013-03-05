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
            //const string EncryptKey = "mk8$3njkl";
            
            OrdersController controller = new OrdersController();
            IList<OrderDetail> orderDetails = new List<OrderDetail>();

            Guid userid = Guid.NewGuid();

            ASPNetMembership membership = new ASPNetMembership()
            {
                ApplicationId = new Guid("71792474-3BB3-4108-9C88-E26179DB443A"),
                UserId = userid,
                CreateDate = DateTime.Now.AddDays(-6),
            };

            Order order = new Order()
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

            OrderProduct orderProduct = new OrderProduct()
            {
                Id = 102,
                Description = "Supplier Membership",
                Summary = "Supplier"
            };

            OrderDetail orderDetail = new OrderDetail()
            {
                Added = DateTime.Now.AddDays(-3),
                Order = order,
                OrderId = order.Id,
                Product = orderProduct,
                ProductId = orderProduct.Id,
                Quantity = 1,
                Subtotal = 500,
            };

            OrderCreditCard orderCreditCard = new OrderCreditCard()
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
            mockObjectService.Setup(objectService => objectService.GetAll<OrderDetail>("Order;Order.Membership;Order.CreditCard", true)).Returns(orderDetails.AsQueryable());
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
