using asi.asicentral.services.interfaces;
using asi.asicentral.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using asi.asicentral.web.Controllers;
using System.Web.Mvc;

namespace asi.asicentral.WebApplication.Tests.Controllers
{
    [TestClass]
    public class PublicationControllerTest
    {
        [TestMethod]
        public void List()
        {
            IList<Publication> publications = new List<Publication>();

            Mock<IObjectService> mockjObjectService = new Mock<IObjectService>();
            mockjObjectService.Setup(objectService => objectService.GetAll<Publication>(true)).Returns(publications.AsQueryable());
            PublicationController controller = new PublicationController(mockjObjectService.Object);
            ViewResult result = controller.List() as ViewResult;
            //verify the view returns the list
            Assert.IsNotNull(result.Model);
            Assert.IsInstanceOfType(result.Model, typeof(IList<Publication>));
            //verify the GetAll method was called
            mockjObjectService.Verify(objectService => objectService.GetAll<Publication>(true), Times.Exactly(1));
        }

        [TestMethod]
        public void Add()
        {
            Publication publication = new Publication() { Name = "test" };
            Mock<IObjectService> mockjObjectService = new Mock<IObjectService>();
            PublicationController controller = new PublicationController(mockjObjectService.Object);

            ActionResult result = controller.Add(publication);
            //make sure we redirect after adding
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            mockjObjectService.Verify(objectService => objectService.Add<Publication>(publication), Times.Exactly(1));
            mockjObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(1));
        }

        [TestMethod]
        public void Delete()
        {
            IList<Publication> publications = new List<Publication>();
            Publication publication = new Publication() { PublicationId = 1, Name = "test" };
            publications.Add(publication);

            Mock<IObjectService> mockjObjectService = new Mock<IObjectService>();
            mockjObjectService.Setup(objectService => objectService.GetAll<Publication>(false)).Returns(publications.AsQueryable());
            PublicationController controller = new PublicationController(mockjObjectService.Object);
            controller.Delete(publication.PublicationId);
            mockjObjectService.Verify(objectService => objectService.Delete<Publication>(publication), Times.Exactly(1));
            mockjObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(1));
        }
    }
}
