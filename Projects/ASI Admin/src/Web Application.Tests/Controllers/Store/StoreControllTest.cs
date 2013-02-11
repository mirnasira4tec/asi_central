using System;
using System.Linq;
using System.Web.Mvc;

using Moq;
using System.Collections.Generic;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.web.Controllers.Store;

namespace asi.asicentral.WebApplication.Tests.Controllers.Store
{
    [TestClass]
    public class StoreControllTest
    {
        [TestMethod]
        public void GetDistributorApplication()
        {
            //arrange
            IList<DistributorMembershipApplication> distributorApplications = new List<DistributorMembershipApplication>();
            distributorApplications.Add(new DistributorMembershipApplication { Id = new Guid("A3358D61-B20D-42D0-B536-5E036170487F"), Company = "TestCompany" });
            Mock<IStoreService> mockObjectService = new Mock<IStoreService>();
            mockObjectService.Setup(objectService => objectService.GetAll<DistributorMembershipApplication>(true)).Returns(distributorApplications.AsQueryable());
            OrdersController controller = new OrdersController();
            controller.StoreObjectService = mockObjectService.Object;

            //get application with user id = A3358D61-B20D-42D0-B536-5E036170487F
            ViewResult viewResult = controller.GetDistributorApplication(new Guid("A3358D61-B20D-42D0-B536-5E036170487F")) as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(((DistributorMembershipApplication)viewResult.Model).Company == "TestCompany");
        }

        [TestMethod]
        public void GetSupplierApplication()
        {
            //arrange
            IList<SupplierMembershipApplication> distributorApplications = new List<SupplierMembershipApplication>();
            distributorApplications.Add(new SupplierMembershipApplication { Id = new Guid("A3358D61-B20D-42D0-B536-5E036170487F"), Company = "TestCompany" });
            Mock<IStoreService> mockObjectService = new Mock<IStoreService>();
            mockObjectService.Setup(objectService => objectService.GetAll<SupplierMembershipApplication>(true)).Returns(distributorApplications.AsQueryable());
            OrdersController controller = new OrdersController();
            controller.StoreObjectService = mockObjectService.Object;

            //get application with user id = A3358D61-B20D-42D0-B536-5E036170487F
            ViewResult viewResult = controller.GetSupplierApplication(new Guid("A3358D61-B20D-42D0-B536-5E036170487F")) as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(((SupplierMembershipApplication)viewResult.Model).Company == "TestCompany");
        }
    }
}
