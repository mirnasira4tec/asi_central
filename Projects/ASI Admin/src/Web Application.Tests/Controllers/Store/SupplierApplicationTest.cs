using Moq;
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using asi.asicentral.model.store;
using asi.asicentral.interfaces;
using asi.asicentral.web.Controllers.Store;
using asi.asicentral.web.model.store;
using System.Web.Mvc;

namespace asi.asicentral.WebApplication.Tests.Controllers.Store
{
    [TestClass]
    public class SupplierApplicationTest
    {
        [TestMethod]
        public void EditSupplier()
        {
            //prepare for EditSupplier(SupplierApplicationModel application)
            List<LegacyOrder> orders = new List<LegacyOrder>();
            List<LegacySupplierMembershipApplication> applications = new List<LegacySupplierMembershipApplication>();
            List<LegacySupplierDecoratingType> decoratingtypes = new List<LegacySupplierDecoratingType>();
            decoratingtypes.Add(new LegacySupplierDecoratingType() { Name = LegacySupplierDecoratingType.DECORATION_ETCHING });

            Guid guid = Guid.NewGuid();
            LegacyOrder order = new LegacyOrder() { Id = 0, BillFirstName = "FirstName", UserId = guid };
            order.CreditCard = new LegacyOrderCreditCard() { ExternalReference = "111" };
            LegacyOrder orderRef = order;
            LegacySupplierMembershipApplication application = new LegacySupplierMembershipApplication() { Id = guid, UserId = guid };
            application.DecoratingTypes = new List<LegacySupplierDecoratingType>();
            application.Contacts = new List<LegacySupplierMembershipApplicationContact>();
            application.Contacts.Add(new LegacySupplierMembershipApplicationContact() { Id = 0 });
            application.Contacts.Add(new LegacySupplierMembershipApplicationContact() { Id = 1 });
            LegacySupplierMembershipApplication applicationRef = application;

            Mock<IStoreService> mockStoreService = new Mock<IStoreService>();
            Mock<IFulfilmentService> mockFulFilService = new Mock<IFulfilmentService>();
            mockStoreService.Setup(service => service.GetAll<LegacyOrder>(false)).Returns(orders.AsQueryable());
            mockStoreService.Setup(service => service.GetAll<LegacySupplierDecoratingType>(false)).Returns(decoratingtypes.AsQueryable());
            mockStoreService.Setup(service => service.GetAll<LegacySupplierMembershipApplication>(false)).Returns(applications.AsQueryable());

            ApplicationController controller = new ApplicationController();
            controller.FulfilmentService = mockFulFilService.Object;
            controller.StoreService = mockStoreService.Object;

            orders.Add(order);
            applications.Add(application);

            SupplierApplicationModel model = new SupplierApplicationModel(application, orderRef);
            model.ExternalReference = "102";
            model.ActionName = ApplicationController.COMMAND_SAVE;
            model.Etching = true;

            // user selects imprinting methods and clicks save - order should be updated wih externalreference, imprinting methods should be saved.
            RedirectToRouteResult result = controller.EditSupplier(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.AreEqual(applicationRef.DecoratingTypes.ElementAt(0).Name, LegacySupplierDecoratingType.DECORATION_ETCHING);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(1));

            // user clicks reject - order should be updated to reject
            model.DecoratingTypes.Clear();
            model.ActionName = ApplicationController.COMMAND_REJECT;
            RedirectToRouteResult result2 = controller.EditSupplier(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(2));

            // user clicks approve with reference id
            model.DecoratingTypes.Clear();
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            RedirectToRouteResult result3 = controller.EditSupplier(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(3));
            mockFulFilService.Verify(service => service.Process(It.IsAny<LegacyOrder>(), It.IsAny<LegacyOrderDetailApplication>()), Times.Exactly(1));
        }
    }
}
