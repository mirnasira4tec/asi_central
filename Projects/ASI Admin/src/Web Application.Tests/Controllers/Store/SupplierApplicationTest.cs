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
    public class SupplierApplicationTest
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
        public void EditSupplier()
        {
            //prepare for EditSupplier(SupplierApplicationModel application)
            List<StoreDetailSupplierMembership> applications = new List<StoreDetailSupplierMembership>();
            List<LookSupplierDecoratingType> decoratingtypes = new List<LookSupplierDecoratingType>();
            decoratingtypes.Add(new LookSupplierDecoratingType() { Description = LegacySupplierDecoratingType.DECORATION_ETCHING });

            Guid guid = Guid.NewGuid();
            StoreIndividual individual = new StoreIndividual() { LastName = "Last", FirstName = "First" };
            StoreOrder order = new StoreOrder() { Id = 0, BillingIndividual = individual };
            StoreOrderDetail detail = new StoreOrderDetail { Id = 0, Order = order, };
            List<StoreOrderDetail> details = new List<StoreOrderDetail>();
            detail.Order = order;
            detail.Product = CreateProduct(39);
            details.Add(detail);
            order.CreditCard = new StoreCreditCard() { ExternalReference = "111" };
            StoreOrder orderRef = order;
            StoreCompany company = new StoreCompany()
            {
                Name = "Company",
            };
            order.Company = company;
            order.OrderDetails = details;
            company.Individuals.Add(new StoreIndividual() { FirstName = "First1", LastName = "Last1", Id = 0 });
            company.Individuals.Add(new StoreIndividual() { FirstName = "First2", LastName = "Last2", Id = 1 });
            StoreDetailSupplierMembership application = new StoreDetailSupplierMembership();
            application.DecoratingTypes = new List<LookSupplierDecoratingType>();
            StoreDetailSupplierMembership applicationRef = application;

            Mock<IStoreService> mockStoreService = new Mock<IStoreService>();
            Mock<IFulfilmentService> mockFulFilService = new Mock<IFulfilmentService>();
            mockStoreService.Setup(service => service.GetAll<StoreOrderDetail>(false)).Returns(details.AsQueryable());
            mockStoreService.Setup(service => service.GetAll<LookSupplierDecoratingType>(false)).Returns(decoratingtypes.AsQueryable());
            mockStoreService.Setup(service => service.GetAll<StoreDetailSupplierMembership>(false)).Returns(applications.AsQueryable());

            ApplicationController controller = new ApplicationController();
            controller.FulfilmentService = mockFulFilService.Object;
            controller.StoreService = mockStoreService.Object;

            applications.Add(application);

            SupplierApplicationModel model = new SupplierApplicationModel(application, detail);
            model.ExternalReference = "102";
            model.ActionName = ApplicationController.COMMAND_SAVE;
            model.Etching = true;

            // user selects imprinting methods and clicks save - order should be updated wih externalreference, imprinting methods should be saved.
            RedirectToRouteResult result = controller.EditSupplier(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.AreEqual(applicationRef.DecoratingTypes.ElementAt(0).Description, LegacySupplierDecoratingType.DECORATION_ETCHING);
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
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(1));
        }
    }
}
