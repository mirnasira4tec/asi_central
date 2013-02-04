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
            // arrange
            IList<Company> companies = new List<Company>();
            companies.Add(new Company { Id = 1, Name = "Test Company 1", Summary = "Summary" });
            
            Mock<IObjectService> mockObjectService = new Mock<IObjectService>();
            mockObjectService.Setup(objectService => objectService.GetAll<Company>(false)).Returns(companies.AsQueryable());
            CompanyController controller = new CompanyController();
            controller.ObjectService = mockObjectService.Object;

            // returning a Company model to the view
            ViewResult result = controller.Edit(1) as ViewResult;
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result.Model, typeof(Company));
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            // editing and saving a Company model to the database
            Company company = new Company();
            company.Name = "New Company";
            company.Summary = "Summary";
            ActionResult actionResult = controller.Edit(company);
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            mockObjectService.Verify(objectService => objectService.Update<Company>(company), Times.Exactly(1));
            mockObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(1));
        }

        [TestMethod]
        public void Add()
        {
            // arrange
            IList<Company> companies = new List<Company>();
            IList<Category> categories = new List<Category>();
            categories.Add(new Category { Id = 32, Name = "All" });

            Mock<IObjectService> mockObjectService = new Mock<IObjectService>();
            mockObjectService.Setup(objectService => objectService.GetAll<Company>(false)).Returns(companies.AsQueryable());
            mockObjectService.Setup(objectService => objectService.GetAll<Category>(false)).Returns(categories.AsQueryable());
            CompanyController controller = new CompanyController();
            controller.ObjectService = mockObjectService.Object;

            // create new model and pass it to the view
            ViewResult viewResult = controller.Add() as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            Assert.IsNotNull(viewResult.ViewBag.Title);

            // add new company to the data
            Company company = new Company { Id = 1, Name = "New Company", Summary = "Summary" };
            ActionResult actionResult = controller.Add(company);
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            mockObjectService.Verify(objectService => objectService.Add<Company>(company), Times.Exactly(1));
            mockObjectService.Verify(objectService => objectService.SaveChanges());
        }

        [TestMethod]
        public void Delete()
        {

        }
    }
}
