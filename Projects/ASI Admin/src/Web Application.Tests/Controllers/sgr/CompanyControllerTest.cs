using System;
using System.Linq;
using System.Web.Mvc;

using Moq;
using asi.asicentral.model.sgr;
using asi.asicentral.interfaces;
using asi.asicentral.web.Models.sgr;
using asi.asicentral.web.Controllers.sgr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace asi.asicentral.WebApplication.Tests.Controllers.sgr
{
    [TestClass]
    public class CategoryControllerTest
    {
        [TestMethod]
        public void List()
        {
            // arrange
            IList<Company> companies = new List<Company>();
            Mock<IObjectService> mockObjectService = new Mock<IObjectService>();
            mockObjectService.Setup(objectService => objectService.GetAll<Company>(true)).Returns(companies.AsQueryable());
            CompanyController controller = new CompanyController();
            controller.ObjectService = mockObjectService.Object;

            // act
            ViewResult result = controller.List() as ViewResult;
            Assert.IsNotNull(result.Model);
            Assert.IsNotNull(result.ViewBag.Title);
        }

        [TestMethod]
        public void Edit()
        {
            ViewResult result;

            // arrange
            IList<Company> companies = new List<Company>();
            companies.Add(new Company { Id = 1, Name = "Test Company 1" });
            
            Mock<IObjectService> mockObjectService = new Mock<IObjectService>();
            mockObjectService.Setup(objectService => objectService.GetAll<Company>(false)).Returns(companies.AsQueryable());
            CompanyController controller = new CompanyController();
            controller.ObjectService = mockObjectService.Object;

            // returning a Company model to the view
            result = controller.Edit(1) as ViewResult;
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result.Model, typeof(Company));
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            // editing and saving a Company model to the database
            Company company = (Company)result.Model;
            company.Id = 2;
            company.Name = "New Company";
            company.Summary = "Summary";
            result = controller.Edit(company) as ViewResult;
        }
    }
}
