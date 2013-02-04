using System;
using System.Web.Mvc;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web_Application;
using Web_Application.Controllers;

namespace asi.asicentral.WebApplication.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            //arrange
            HomeController controller = new HomeController();
            
            //act
            ViewResult result = controller.Index() as ViewResult;

            //assert
            Assert.IsNotNull(result.ViewBag.Message);
        }
    }
}
