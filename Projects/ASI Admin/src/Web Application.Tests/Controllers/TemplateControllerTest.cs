using System;
using System.Web.Mvc;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web_Application;
using asi.asicentral.web.Controllers;

namespace asi.asicentral.WebApplication.Tests.Controllers
{
    [TestClass]
    public class TemplateControllerTest
    {
        [TestMethod]
        public void Form()
        {
            // arrange
            TemplateController controller = new TemplateController();
            
            // act
            ViewResult form = controller.Form() as ViewResult;
        }

        [TestMethod]
        public void Dialog()
        {
            // arrange
            TemplateController controller = new TemplateController();
            
            // act
            ViewResult dialog = controller.Dialog() as ViewResult;
        }
    }
}
