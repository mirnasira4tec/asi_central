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
    public class ESPAdvertisingApplicationTest
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

        public Category[] GetImpressionsPerCategoryTemplate()
        {
            Category[] categories = new Category[3];
            categories[0] = new Category() { Name = "BABY ITEMS - DEMO", SupplierCount = 217 };
            categories[0].Results = new Company[7];
            categories[0].Results[0] = new Company() { IsRequestor = false, Impressions = 1143, Rank = 1 };
            categories[0].Results[1] = new Company() { IsRequestor = false, Impressions = 1143, Rank = 1 };
            categories[0].Results[2] = new Company() { IsRequestor = false, Impressions = 1026, Rank = 3 };
            categories[0].Results[3] = new Company() { IsRequestor = true, Impressions = 846, Rank = 4 };
            categories[0].Results[4] = new Company() { IsRequestor = false, Impressions = 829, Rank = 5 };
            categories[0].Results[5] = new Company() { IsRequestor = false, Impressions = 700, Rank = 6 };
            categories[0].Results[6] = new Company() { IsRequestor = false, Impressions = 35, Rank = 81 };
            categories[0].Requestor = categories[0].Results[6];

            categories[1] = new Category() { Name = "BASEBALL CAPS - DEMO", SupplierCount = 357 };
            categories[1].Results = new Company[7];
            categories[1].Results[0] = new Company() { IsRequestor = false, Impressions = 8207, Rank = 1 };
            categories[1].Results[1] = new Company() { IsRequestor = false, Impressions = 7497, Rank = 2 };
            categories[1].Results[2] = new Company() { IsRequestor = false, Impressions = 6015, Rank = 3 };
            categories[1].Results[3] = new Company() { IsRequestor = false, Impressions = 5580, Rank = 4 };
            categories[1].Results[4] = new Company() { IsRequestor = false, Impressions = 5429, Rank = 5 };
            categories[1].Results[5] = new Company() { IsRequestor = false, Impressions = 4326, Rank = 6 };
            categories[1].Results[6] = new Company() { IsRequestor = true, Impressions = 272, Rank = 66 };
            categories[1].Requestor = categories[1].Results[6];

            categories[2] = new Category() { Name = "GOLF/POLO SHIRTS - DEMO", SupplierCount = 262 };
            categories[2].Results = new Company[7];
            categories[2].Results[0] = new Company() { IsRequestor = true, Impressions = 33218, Rank = 1 };
            categories[2].Results[1] = new Company() { IsRequestor = false, Impressions = 8981, Rank = 2 };
            categories[2].Results[2] = new Company() { IsRequestor = false, Impressions = 5127, Rank = 3 };
            categories[2].Results[3] = new Company() { IsRequestor = false, Impressions = 4109, Rank = 4 };
            categories[2].Results[4] = new Company() { IsRequestor = false, Impressions = 4055, Rank = 5 };
            categories[2].Results[5] = new Company() { IsRequestor = false, Impressions = 3943, Rank = 6 };
            categories[2].Results[6] = new Company() { IsRequestor = false, Impressions = 919, Rank = 31 };
            categories[2].Requestor = categories[2].Results[6];
            return categories;
        }

        [TestMethod]
        public void EditPayForPlacement()
        {
            StoreIndividual individual = new StoreIndividual() { LastName = "Last", FirstName = "First" };
            StoreOrder order = new StoreOrder() { Id = 0, BillingIndividual = individual };
            StoreOrderDetail detail = new StoreOrderDetail { Id = 5, Order = order, };
            List<StoreOrderDetail> details = new List<StoreOrderDetail>();
            List<StoreDetailPayForPlacement> advertisings = new List<StoreDetailPayForPlacement>();
            CENTUserProfilesPROF profile = new CENTUserProfilesPROF { PROF_UserID = new Guid("e6a9bb54-da25-102b-9a03-2db401e887ec"), PROF_ASINo = "12345"};
            List<CENTUserProfilesPROF> profiles = new List<CENTUserProfilesPROF>();
            profiles.Add(profile);
            details.Add(detail);
            detail.Product = CreateProduct(47);
            detail.Quantity = 1;
            StoreDetailPayForPlacement espPayForPlacement = null;

            StoreDetailPayForPlacement espPayForPlacementDetail = new StoreDetailPayForPlacement();
            espPayForPlacementDetail.OrderDetailId = 5;
            espPayForPlacementDetail.CategoryName = "BASEBALL CAPS - DEMO";
            espPayForPlacementDetail.PaymentType = "FB";
            advertisings.Add(espPayForPlacementDetail);

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
            mockStoreService.Setup(service => service.GetAll<StoreDetailPayForPlacement>(false)).Returns(advertisings.AsQueryable());
            mockStoreService.Setup(service => service.GetAll<CENTUserProfilesPROF>(false)).Returns(profiles.AsQueryable());
            mockStoreService.Setup(objectService => objectService.Add<StoreDetailPayForPlacement>(It.IsAny<StoreDetailPayForPlacement>()))
                            .Callback<StoreDetailPayForPlacement>((theESPAdvertising) => espPayForPlacement = theESPAdvertising);

            ApplicationController controller = new ApplicationController();
            controller.FulfilmentService = mockFulFilService.Object;
            controller.StoreService = mockStoreService.Object;

            order.UserId = "e6a9bb54-da25-102b-9a03-2db401e887ec";
            ESPPayForPlacementModel model = new ESPPayForPlacementModel(detail, controller.StoreService);
            Assert.IsNotNull(model.Categries);
            Assert.AreEqual(model.Categries.Count, 3);
            model.Categries.ElementAt(0).IsSelected = true;
            model.Categries.ElementAt(0).CategoryName = "BABY ITEMS - DEMO";
            model.Categries.ElementAt(0).CPMOption = 3;
            model.Categries.ElementAt(0).PaymentOption = "IPM";
            model.Categries.ElementAt(0).Impressions = "5000";
            model.Categries.ElementAt(1).IsSelected = false;
            model.Categries.ElementAt(2).IsSelected = false;

            model.OrderDetailId = 5;
            model.ActionName = ApplicationController.COMMAND_SAVE;
            model.ExternalReference = "102";
            // Pay for placement -- 47
            RedirectToRouteResult result = controller.EditPayForPlacement(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.IsNotNull(detail);
            Assert.AreEqual(detail.Quantity, 1);
            Assert.IsNotNull(espPayForPlacement);
            Assert.AreEqual(espPayForPlacement.CategoryName, "BABY ITEMS - DEMO");
            Assert.AreEqual(espPayForPlacement.CPMOption, 3);
            Assert.AreEqual(espPayForPlacement.PaymentType, "IPM");
            Assert.AreEqual(espPayForPlacement.Cost, 0.00M);
            Assert.AreEqual(espPayForPlacement.ImpressionsRequested, 5000);
            Assert.AreEqual(detail.Cost, 1200.00M);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(1));
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(0));
            espPayForPlacement = null;

            model.Categries.ElementAt(2).IsSelected = true;
            model.Categries.ElementAt(2).IsSelected = true;
            model.Categries.ElementAt(2).CategoryName = "GOLF/POLO SHIRTS - DEMO";
            model.Categries.ElementAt(2).CPMOption = 0;
            model.Categries.ElementAt(2).PaymentOption = "FB";
            model.Categries.ElementAt(2).PaymentAmount = "1000";


            result = controller.EditPayForPlacement(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.IsNotNull(detail);
            Assert.AreEqual(detail.Quantity, 1);
            Assert.IsNotNull(espPayForPlacement);
            Assert.AreEqual(espPayForPlacement.CategoryName, "GOLF/POLO SHIRTS - DEMO");
            Assert.AreEqual(espPayForPlacement.CPMOption, 0);
            Assert.AreEqual(espPayForPlacement.PaymentType, "FB");
            Assert.AreEqual(espPayForPlacement.Cost, 1000.00M);
            Assert.AreEqual(espPayForPlacement.ImpressionsRequested, 0);
            Assert.AreEqual(detail.Cost, 2200.00M);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(2));

            // user clicks reject - order should be updated to reject
            model.ActionName = ApplicationController.COMMAND_REJECT;
            RedirectToRouteResult result2 = controller.EditPayForPlacement(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(3));

            // user clicks approve with reference id
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            RedirectToRouteResult result3 = controller.EditPayForPlacement(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(4));
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(1));
        }

        [TestMethod]
        public void EditBannerTileTower()
        {
            StoreIndividual individual = new StoreIndividual() { LastName = "Last", FirstName = "First" };
            StoreOrder order = new StoreOrder() { Id = 0, BillingIndividual = individual };
            StoreOrderDetail detail = new StoreOrderDetail { Id = 5, Order = order, };
            List<StoreOrderDetail> details = new List<StoreOrderDetail>();
            List<StoreDetailESPAdvertising> advertisings = new List<StoreDetailESPAdvertising>();
            details.Add(detail);
            detail.Product = CreateProduct(48);
            detail.Quantity = 1;

            StoreDetailESPAdvertising espAdvertisingDetail = new StoreDetailESPAdvertising();
            espAdvertisingDetail.OrderDetailId = 5;
            advertisings.Add(espAdvertisingDetail);

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
            mockStoreService.Setup(service => service.GetAll<StoreDetailESPAdvertising>(false)).Returns(advertisings.AsQueryable());

            ApplicationController controller = new ApplicationController();
            controller.FulfilmentService = mockFulFilService.Object;
            controller.StoreService = mockStoreService.Object;

            ESPAdvertisingModel model = new ESPAdvertisingModel();
            model.OrderDetailId = 5;
            model.Products_OptionId_First = 1;
            model.ActionName = ApplicationController.COMMAND_SAVE;
            model.ExternalReference = "102";
            
            // Banner, Tile & Tower -- 48
            RedirectToRouteResult result = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.IsNotNull(detail);
            Assert.AreEqual(detail.Quantity, 1);
            Assert.IsNotNull(espAdvertisingDetail);
            Assert.AreEqual(espAdvertisingDetail.FirstOptionId, model.Products_OptionId_First);
            Assert.AreEqual(detail.Cost, Convert.ToDecimal(ESPAdvertisingHelper.ESPAdvertising_BANNER_TILE_TOWER_COST[0]));
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(1));

            // user clicks reject - order should be updated to reject
            model.ActionName = ApplicationController.COMMAND_REJECT;
            RedirectToRouteResult result2 = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(2));

            // user clicks approve with reference id
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            RedirectToRouteResult result3 = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(3));
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(1));
        }

        [TestMethod]
        public void EditEventPlanner()
        {
            StoreIndividual individual = new StoreIndividual() { LastName = "Last", FirstName = "First" };
            StoreOrder order = new StoreOrder() { Id = 0, BillingIndividual = individual };
            StoreOrderDetail detail = new StoreOrderDetail { Id = 5, Order = order, };
            List<StoreOrderDetail> details = new List<StoreOrderDetail>();
            List<StoreDetailESPAdvertising> advertisings = new List<StoreDetailESPAdvertising>();
            details.Add(detail);
            detail.Product = CreateProduct(49);
            detail.Quantity = 1;

            StoreDetailESPAdvertising espAdvertisingDetail = new StoreDetailESPAdvertising();
            espAdvertisingDetail.OrderDetailId = 5;
            advertisings.Add(espAdvertisingDetail);

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
            mockStoreService.Setup(service => service.GetAll<StoreDetailESPAdvertising>(false)).Returns(advertisings.AsQueryable());

            ApplicationController controller = new ApplicationController();
            controller.FulfilmentService = mockFulFilService.Object;
            controller.StoreService = mockStoreService.Object;

            ESPAdvertisingModel model = new ESPAdvertisingModel();
            model.OrderDetailId = 5;
            model.NumberOfItems_First = "123;456;789";
            model.ActionName = ApplicationController.COMMAND_SAVE;
            model.ExternalReference = "102";

            // EventPlanner -- 49
            RedirectToRouteResult result = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.IsNotNull(detail);
            Assert.AreEqual(detail.Quantity, 1);
            Assert.IsNotNull(espAdvertisingDetail);
            Assert.AreEqual(espAdvertisingDetail.FirstItemList, model.NumberOfItems_First);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(1));

            // user clicks reject - order should be updated to reject
            model.ActionName = ApplicationController.COMMAND_REJECT;
            RedirectToRouteResult result2 = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(2));

            // user clicks approve with reference id
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            RedirectToRouteResult result3 = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(3));
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(1));
        }

        [TestMethod]
        public void EditClearanceNewSpecials()
        {
            StoreIndividual individual = new StoreIndividual() { LastName = "Last", FirstName = "First" };
            StoreOrder order = new StoreOrder() { Id = 0, BillingIndividual = individual };
            StoreOrderDetail detail = new StoreOrderDetail { Id = 5, Order = order, };
            List<StoreOrderDetail> details = new List<StoreOrderDetail>();
            List<StoreDetailESPAdvertising> advertisings = new List<StoreDetailESPAdvertising>();
            details.Add(detail);
            detail.Product = CreateProduct(50);
            detail.Quantity = 1;

            StoreDetailESPAdvertising espAdvertisingDetail = new StoreDetailESPAdvertising();
            espAdvertisingDetail.OrderDetailId = 5;
            advertisings.Add(espAdvertisingDetail);

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
            mockStoreService.Setup(service => service.GetAll<StoreDetailESPAdvertising>(false)).Returns(advertisings.AsQueryable());

            ApplicationController controller = new ApplicationController();
            controller.FulfilmentService = mockFulFilService.Object;
            controller.StoreService = mockStoreService.Object;

            ESPAdvertisingModel model = new ESPAdvertisingModel();
            model.OrderDetailId = 5;
            model.NumberOfItems_First = "123;456;789";
            model.Products_OptionId_First = 0;
            model.NumberOfItems_Second = "345;456;789";
            model.Products_OptionId_Second = 1;
            model.NumberOfItems_Third = "678;456;789";
            model.Products_OptionId_Third = 2;
            model.ActionName = ApplicationController.COMMAND_SAVE;
            model.ExternalReference = "102";

            // ClearanceNewSpecials - 50
            RedirectToRouteResult result = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.IsNotNull(detail);
            Assert.AreEqual(detail.Quantity, 1);
            Assert.IsNotNull(espAdvertisingDetail);
            Assert.AreEqual(espAdvertisingDetail.FirstOptionId, model.Products_OptionId_First);
            Assert.AreEqual(espAdvertisingDetail.FirstItemList, model.NumberOfItems_First);
            Assert.AreEqual(espAdvertisingDetail.SecondOptionId, model.Products_OptionId_Second);
            Assert.AreEqual(espAdvertisingDetail.FirstItemList, model.NumberOfItems_First);
            Assert.AreEqual(espAdvertisingDetail.ThirdOptionId, model.Products_OptionId_Third);
            Assert.AreEqual(espAdvertisingDetail.FirstItemList, model.NumberOfItems_First);
            Assert.AreEqual(detail.Cost, 466.25M);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(1));

            // user clicks reject - order should be updated to reject
            model.ActionName = ApplicationController.COMMAND_REJECT;
            RedirectToRouteResult result2 = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(2));

            // user clicks approve with reference id
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            RedirectToRouteResult result3 = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(3));
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(1));
        }

        [TestMethod]
        public void EditLogoLineOrVideo()
        {
            StoreIndividual individual = new StoreIndividual() { LastName = "Last", FirstName = "First" };
            StoreOrder order = new StoreOrder() { Id = 0, BillingIndividual = individual };
            StoreOrderDetail detail = new StoreOrderDetail { Id = 5, Order = order, };
            List<StoreOrderDetail> details = new List<StoreOrderDetail>();
            List<StoreDetailESPAdvertising> advertisings = new List<StoreDetailESPAdvertising>();
            details.Add(detail);
            detail.Product = CreateProduct(51);
            detail.Quantity = 1;

            StoreDetailESPAdvertising espAdvertisingDetail = new StoreDetailESPAdvertising();
            espAdvertisingDetail.OrderDetailId = 5;
            advertisings.Add(espAdvertisingDetail);

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
            mockStoreService.Setup(service => service.GetAll<StoreDetailESPAdvertising>(false)).Returns(advertisings.AsQueryable());

            ApplicationController controller = new ApplicationController();
            controller.FulfilmentService = mockFulFilService.Object;
            controller.StoreService = mockStoreService.Object;

            ESPAdvertisingModel model = new ESPAdvertisingModel();
            model.OrderDetailId = 5;
            model.ActionName = ApplicationController.COMMAND_SAVE;
            model.ExternalReference = "102";

            // LogoLine - 51
            RedirectToRouteResult result = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.IsNotNull(detail);
            Assert.AreEqual(detail.Quantity, 1);
            Assert.IsNotNull(espAdvertisingDetail);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(1));

            // user clicks reject - order should be updated to reject
            model.ActionName = ApplicationController.COMMAND_REJECT;
            RedirectToRouteResult result2 = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(2));

            // user clicks approve with reference id
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            RedirectToRouteResult result3 = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(3));
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(1));

            detail.Product = CreateProduct(53);
            model = new ESPAdvertisingModel();
            model.OrderDetailId = 5;
            model.ActionName = ApplicationController.COMMAND_SAVE;
            model.ExternalReference = "102";

            // Video - 51
            result = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.IsNotNull(detail);
            Assert.AreEqual(detail.Quantity, 1);
            Assert.IsNotNull(espAdvertisingDetail);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(4));

            // user clicks reject - order should be updated to reject
            model.ActionName = ApplicationController.COMMAND_REJECT;
            result2 = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(5));

            // user clicks approve with reference id
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            result3 = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(6));
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(2));

        }

        [TestMethod]
        public void EditLoginScreen()
        {
            StoreIndividual individual = new StoreIndividual() { LastName = "Last", FirstName = "First" };
            StoreOrder order = new StoreOrder() { Id = 0, BillingIndividual = individual };
            StoreOrderDetail detail = new StoreOrderDetail { Id = 5, Order = order, };
            List<StoreOrderDetail> details = new List<StoreOrderDetail>();
            List<StoreDetailESPAdvertising> advertisings = new List<StoreDetailESPAdvertising>();
            details.Add(detail);
            detail.Product = CreateProduct(52);
            detail.Quantity = 1;

            StoreDetailESPAdvertising espAdvertisingDetail = new StoreDetailESPAdvertising();
            espAdvertisingDetail.OrderDetailId = 5;
            advertisings.Add(espAdvertisingDetail);

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
            mockStoreService.Setup(service => service.GetAll<StoreDetailESPAdvertising>(false)).Returns(advertisings.AsQueryable());

            ApplicationController controller = new ApplicationController();
            controller.FulfilmentService = mockFulFilService.Object;
            controller.StoreService = mockStoreService.Object;

            ESPAdvertisingModel model = new ESPAdvertisingModel();
            model.OrderDetailId = 5;
            model.AdSelectedDate = new DateTime(2013, 12, 18);
            model.ActionName = ApplicationController.COMMAND_SAVE;
            model.ExternalReference = "102";

            // LoginScreen - 52
            RedirectToRouteResult result = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.IsNotNull(detail);
            Assert.AreEqual(detail.Quantity, 1);
            Assert.IsNotNull(espAdvertisingDetail);
            Assert.AreEqual(espAdvertisingDetail.AdSelectedDate, model.AdSelectedDate);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(1));

            // user clicks reject - order should be updated to reject
            model.ActionName = ApplicationController.COMMAND_REJECT;
            RedirectToRouteResult result2 = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(2));

            // user clicks approve with reference id
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            RedirectToRouteResult result3 = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(3));
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(1));
        }

        [TestMethod]
        public void EditPromoCafe()
        {
            StoreIndividual individual = new StoreIndividual() { LastName = "Last", FirstName = "First" };
            StoreOrder order = new StoreOrder() { Id = 0, BillingIndividual = individual };
            StoreOrderDetail detail = new StoreOrderDetail { Id = 5, Order = order, };
            List<StoreOrderDetail> details = new List<StoreOrderDetail>();
            List<StoreDetailESPAdvertising> advertisings = new List<StoreDetailESPAdvertising>();
            details.Add(detail);
            detail.Product = CreateProduct(54);
            detail.Quantity = 1;

            StoreDetailESPAdvertising espAdvertisingDetail = new StoreDetailESPAdvertising();
            espAdvertisingDetail.OrderDetailId = 5;
            advertisings.Add(espAdvertisingDetail);

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
            mockStoreService.Setup(service => service.GetAll<StoreDetailESPAdvertising>(false)).Returns(advertisings.AsQueryable());

            ApplicationController controller = new ApplicationController();
            controller.FulfilmentService = mockFulFilService.Object;
            controller.StoreService = mockStoreService.Object;

            ESPAdvertisingModel model = new ESPAdvertisingModel();
            model.OrderDetailId = 5;
            model.Products_OptionId_First = 0;
            model.ActionName = ApplicationController.COMMAND_SAVE;
            model.ExternalReference = "102";

            // PromoCafe - 54
            RedirectToRouteResult result = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.IsNotNull(detail);
            Assert.AreEqual(detail.Quantity, 1);
            Assert.IsNotNull(espAdvertisingDetail);
            Assert.AreEqual(espAdvertisingDetail.FirstOptionId, (model.Products_OptionId_First + 1));
            Assert.AreEqual(detail.Cost, 750.00M);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(1));

            // user clicks reject - order should be updated to reject
            model.ActionName = ApplicationController.COMMAND_REJECT;
            RedirectToRouteResult result2 = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(2));

            // user clicks approve with reference id
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            RedirectToRouteResult result3 = controller.EditESPAdvertising(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(3));
            mockFulFilService.Verify(service => service.Process(It.IsAny<StoreOrder>(), It.IsAny<StoreDetailApplication>()), Times.Exactly(1));
        }
    }
}
