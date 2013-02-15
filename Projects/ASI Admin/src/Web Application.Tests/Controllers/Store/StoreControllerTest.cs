using Moq;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.web.Controllers.Store;
using asi.asicentral.web.Models.Store;

namespace asi.asicentral.WebApplication.Tests.Controllers.Store
{
    [TestClass]
    public class StoreControllerTest
    {
        [TestMethod]
        public void GetApplication()
        {
            // prepare
            ViewOrders viewOrders = new ViewOrders();
            IList<Order> orders = new List<Order>();
            IList<OrderDetail> orderDetails = new List<OrderDetail>();

            // create some orders, details and products


            Mock<IStoreService> mockObjectService = new Mock<IStoreService>();
            mockObjectService.Setup(objectService => objectService.GetAll<Order>(true)).Returns(orders.AsQueryable());
            OrdersController controller = new OrdersController();
            controller.StoreObjectService = mockObjectService.Object;
            
            // act - pass ViewOrder object from controller to view

        }
    }
}
