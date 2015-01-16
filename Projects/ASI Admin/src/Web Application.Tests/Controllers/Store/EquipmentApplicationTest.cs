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
    public class EquipmentApplicationTest
    {
        [TestMethod]
        public void EditEquipment()
        {
            List<StoreDetailEquipmentMembership> applications = new List<StoreDetailEquipmentMembership>();
            List<LookEquipmentType> equipmenttypes = new List<LookEquipmentType>();
            equipmenttypes.Add(new LookEquipmentType() { Description = LookEquipmentType.EMBROIDERY });

            Guid guid = Guid.NewGuid();
            StoreIndividual individual = new StoreIndividual() { LastName = "Last", FirstName = "First" };
            StoreOrder order = new StoreOrder() { Id = 0, BillingIndividual = individual };
            StoreOrderDetail detail = new StoreOrderDetail { Id = 0, Order = order, };
            List<StoreOrderDetail> details = new List<StoreOrderDetail>();
            details.Add(detail);
            detail.Order = order;
            order.CreditCard = new StoreCreditCard() { ExternalReference = "111" };
            StoreOrder orderRef = order;
            StoreCompany company = new StoreCompany()
            {
                Name = "Company",
            };
            order.Company = company;
            company.Individuals.Add(new StoreIndividual() { FirstName = "First1", LastName = "Last1", Id = 0 });
            company.Individuals.Add(new StoreIndividual() { FirstName = "First2", LastName = "Last2", Id = 1 });
            StoreDetailEquipmentMembership application = new StoreDetailEquipmentMembership();
            application.EquipmentTypes = new List<LookEquipmentType>();
            StoreDetailEquipmentMembership applicationRef = application;

            Mock<IStoreService> mockStoreService = new Mock<IStoreService>();
            Mock<IFulfilmentService> mockFulFilService = new Mock<IFulfilmentService>();
            mockStoreService.Setup(service => service.GetAll<StoreOrderDetail>(false)).Returns(details.AsQueryable());
            mockStoreService.Setup(service => service.GetAll<LookEquipmentType>(false)).Returns(equipmenttypes.AsQueryable());
            mockStoreService.Setup(service => service.GetAll<StoreDetailEquipmentMembership>(false)).Returns(applications.AsQueryable());

            ApplicationController controller = new ApplicationController();
            controller.FulfilmentService = mockFulFilService.Object;
            controller.StoreService = mockStoreService.Object;

            applications.Add(application);

            EquipmentApplicationModel model = new EquipmentApplicationModel(application, detail, controller.StoreService);
            model.ExternalReference = "102";
            model.ActionName = ApplicationController.COMMAND_SAVE;
            model.Embroidery = true;

            // user selects imprinting methods and clicks save - order should be updated wih externalreference, imprinting methods should be saved.
            RedirectToRouteResult result = controller.EditEquipment(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.AreEqual(applicationRef.EquipmentTypes.ElementAt(0).Description, LookEquipmentType.EMBROIDERY);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(1));

            // user clicks reject - order should be updated to reject
            model.EquipmentTypes.Clear();
            model.ActionName = ApplicationController.COMMAND_REJECT;
            RedirectToRouteResult result2 = controller.EditEquipment(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(2));

            // user clicks approve with reference id
            model.EquipmentTypes.Clear();
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            RedirectToRouteResult result3 = controller.EditEquipment(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(3));
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(1));
        }
    }
}
