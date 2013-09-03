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
    public class CatalogsApplicationTest
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
        public void EditCatalogs()
        {
            Guid guid = Guid.NewGuid();
            StoreIndividual individual = new StoreIndividual() { LastName = "Last", FirstName = "First" };
            StoreOrder order = new StoreOrder() { Id = 0, BillingIndividual = individual };
            StoreOrderDetail detail = new StoreOrderDetail { Id = 5, Order = order, };
            List<StoreOrderDetail> details = new List<StoreOrderDetail>();
            List<StoreDetailCatalog> catalogs = new List<StoreDetailCatalog>();
            List<LookCatalogOption> options = new List<LookCatalogOption>();
            details.Add(detail);
            detail.Order = order;
            detail.Quantity = 30;
            detail.Product = CreateProduct(39);
            detail.ShippingMethod = "UPS2DAY";

            order.Company = new StoreCompany();
            order.Company.Email = "asi@asi.com";
            order.Company.WebURL = "http://asicentral.com";
            
            StoreDetailCatalog catalogDetail = new StoreDetailCatalog();
            catalogDetail.OrderDetailId = 5;
            catalogDetail.AreaId = 25;
            catalogDetail.CoverId = 1;
            catalogDetail.SupplementId = 24;
            catalogDetail.ImprintId = 21;
            catalogDetail.ColorId = 26;
            catalogDetail.IsArtworkToProof = true;
            catalogDetail.IsUploadImageTobeUsed = true;
            catalogDetail.ArtworkOption = "PRINT";
            catalogDetail.Line1 = "Line1";
            catalogDetail.Line2 = "Line2";
            catalogDetail.Line3 = "Line3";
            catalogDetail.Line4 = "Line4";
            catalogDetail.Line5 = "Line5";
            catalogDetail.Line6 = "Line6";
            catalogDetail.BackLine1 = "Back line1";
            catalogDetail.BackLine2 = "Back line2";
            catalogDetail.BackLine3 = "Back line3";
            catalogDetail.BackLine4 = "Back line4";
            catalogs.Add(catalogDetail);
            
            order.CreditCard = new StoreCreditCard() { ExternalReference = "111" };
            StoreOrder orderRef = order;
            order.Company.Name = "Company";
            order.Company.Individuals.Add(new StoreIndividual() { FirstName = "First1", LastName = "Last1", Id = 0 });
            order.Company.Individuals.Add(new StoreIndividual() { FirstName = "First2", LastName = "Last2", Id = 1 });

            order.Company.Addresses = new List<StoreCompanyAddress>();
            order.Company.Addresses.Add(CreateCompanyWithAddress());

            Mock<IStoreService> mockStoreService = new Mock<IStoreService>();
            Mock<IFulfilmentService> mockFulFilService = new Mock<IFulfilmentService>();
            mockStoreService.Setup(service => service.GetAll<StoreOrderDetail>(false)).Returns(details.AsQueryable());
            mockStoreService.Setup(service => service.GetAll<StoreDetailCatalog>(false)).Returns(catalogs.AsQueryable());
            mockStoreService.Setup(service => service.GetAll<LookCatalogOption>(false)).Returns(options.AsQueryable());

            ApplicationController controller = new ApplicationController();
            controller.FulfilmentService = mockFulFilService.Object;
            controller.StoreService = mockStoreService.Object;

            CatalogsApplicationModel model = new CatalogsApplicationModel(detail, catalogDetail, controller.StoreService);
            catalogDetail.AreaId = 25;
            model.ShippingMethod = "UPSGround";
            model.Cover = "7";
            model.Supplement = "24";
            model.Imprint = "21";
            model.Color = "11";
            model.IsArtworkToProof = false;
            model.IsUploadImageTobeUsed = false;
            model.Line1 = "Line11";
            model.Line2 = "Line21";
            model.Line3 = "Line31";
            model.Line4 = "Line41";
            model.Line5 = "Line51";
            model.Line6 = "Line61";
            model.BackLine1 = "Back line11";
            model.BackLine2 = "Back line21";
            model.BackLine3 = "Back line31";
            model.BackLine4 = "Back line41";
            model.Quantity = "40";
            model.ExternalReference = "102";
            model.ActionName = ApplicationController.COMMAND_SAVE;

            // user selects imprinting methods and clicks save - order should be updated wih externalreference, imprinting methods should be saved.
            RedirectToRouteResult result = controller.EditCatalogs(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.IsNotNull(detail);
            Assert.AreEqual(detail.Quantity.ToString(), model.Quantity);
            Assert.AreEqual(detail.ShippingMethod, model.ShippingMethod);
            Assert.IsNotNull(catalogDetail);
            Assert.AreEqual(catalogDetail.Line1, model.Line1);
            Assert.AreEqual(catalogDetail.Line2, model.Line2);
            Assert.AreEqual(catalogDetail.Line3, model.Line3);
            Assert.AreEqual(catalogDetail.Line4, model.Line4);
            Assert.AreEqual(catalogDetail.Line5, model.Line5);
            Assert.AreEqual(catalogDetail.Line6, model.Line6);
            Assert.AreEqual(catalogDetail.BackLine1, model.BackLine1);
            Assert.AreEqual(catalogDetail.BackLine2, model.BackLine2);
            Assert.AreEqual(catalogDetail.BackLine3, model.BackLine3);
            Assert.AreEqual(catalogDetail.BackLine4, model.BackLine4);
            Assert.AreEqual(catalogDetail.AreaId.ToString(), model.Area);
            Assert.AreEqual(catalogDetail.ColorId.ToString(), model.Color);
            Assert.AreEqual(catalogDetail.CoverId.ToString(), model.Cover);
            Assert.AreEqual(catalogDetail.SupplementId.ToString(), model.Supplement);
            Assert.AreEqual(catalogDetail.ImprintId.ToString(), model.Imprint);
            Assert.AreEqual(catalogDetail.IsArtworkToProof, model.IsArtworkToProof);
            Assert.AreEqual(catalogDetail.IsUploadImageTobeUsed, model.IsUploadImageTobeUsed);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(1));

            // user clicks reject - order should be updated to reject
            model.ActionName = ApplicationController.COMMAND_REJECT;
            RedirectToRouteResult result2 = controller.EditCatalogs(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(2));

            // user clicks approve with reference id
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            RedirectToRouteResult result3 = controller.EditCatalogs(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(3));
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(1));
        }
    }
}
