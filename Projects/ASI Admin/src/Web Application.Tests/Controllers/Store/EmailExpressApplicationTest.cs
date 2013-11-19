using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.model.store;
using System.Collections.Generic;
using asi.asicentral.interfaces;
using Moq;
using System.Linq;
using asi.asicentral.util.store;
using asi.asicentral.web.Controllers.Store;
using asi.asicentral.web.model.store;
using System.Web.Mvc;
using asi.asicentral.model.ROI;

namespace asi.asicentral.WebApplication.Tests.Controllers.Store
{
    [TestClass]
    public class EmailExpressApplicationTest
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

        [TestMethod]
        public void EditEmailExpress()
        {
            StoreIndividual individual = new StoreIndividual() { LastName = "Last", FirstName = "First" };
            StoreOrder order = new StoreOrder() { Id = 0, BillingIndividual = individual };
            StoreOrderDetail detail = new StoreOrderDetail { Id = 5, Order = order, };
            StoreDetailEmailExpressItem dateItem = null;
            List<StoreOrderDetail> details = new List<StoreOrderDetail>();
            List<StoreDetailEmailExpress> advertisings = new List<StoreDetailEmailExpress>();
            details.Add(detail);
            detail.Product = CreateProduct(61);
            detail.Quantity = 1;

            StoreDetailEmailExpress emailexpressDetail = new StoreDetailEmailExpress();
            emailexpressDetail.OrderDetailId = 5;
            advertisings.Add(emailexpressDetail);

            order.CreditCard = new StoreCreditCard() { ExternalReference = "111" };
            StoreOrder orderRef = order;
            order.Company = new StoreCompany();
            order.Company.Name = "Company";
            order.Company.Individuals.Add(new StoreIndividual() { FirstName = "First1", LastName = "Last1", Id = 0 });
            order.Company.Individuals.Add(new StoreIndividual() { FirstName = "First2", LastName = "Last2", Id = 1 });

            order.Company.Addresses = new List<StoreCompanyAddress>();
            order.Company.Addresses.Add(CreateCompanyWithAddress());

            Mock<IStoreService> mockStoreService = new Mock<IStoreService>();
            Mock<IFulfilmentService> mockFulFilService = new Mock<IFulfilmentService>();
            mockStoreService.Setup(service => service.GetAll<StoreOrderDetail>(false)).Returns(details.AsQueryable());
            mockStoreService.Setup(service => service.GetAll<StoreDetailEmailExpress>(false)).Returns(advertisings.AsQueryable());
            mockStoreService.Setup(objectService => objectService.Add<StoreDetailEmailExpressItem>(It.IsAny<StoreDetailEmailExpressItem>()))
                            .Callback<StoreDetailEmailExpressItem>((theLoginDate) => dateItem = theLoginDate);

            ApplicationController controller = new ApplicationController();
            controller.FulfilmentService = mockFulFilService.Object;
            controller.StoreService = mockStoreService.Object;

            EmailExpressModel model = new EmailExpressModel();
            model.OrderDetailId = 5;
            model.ActionName = ApplicationController.COMMAND_SAVE;
            model.ExternalReference = "102";
            model.Dates = "Wed, 23 Oct 13\r\nThu, 24 Oct 13\r\nFri, 25 Oct 13\r\n";
            model.ItemTypeId = 1;
            // EmailExpress - 61
            RedirectToRouteResult result = controller.EditEmailExpress(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.IsNotNull(detail);
            Assert.AreEqual(detail.Quantity, 3);
            Assert.IsNotNull(emailexpressDetail);
            Assert.IsNotNull(dateItem);
            Assert.AreEqual(dateItem.AdSelectedDate, new DateTime(2013,10,25));
            Assert.AreEqual(emailexpressDetail.ItemTypeId, model.ItemTypeId);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(1));

            // user clicks reject - order should be updated to reject
            model.ActionName = ApplicationController.COMMAND_REJECT;
            RedirectToRouteResult result2 = controller.EditEmailExpress(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(2));

            // user clicks approve with reference id
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            RedirectToRouteResult result3 = controller.EditEmailExpress(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(3));
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(1));

          
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(1));
        }

    }
}
