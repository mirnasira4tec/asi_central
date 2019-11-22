using System;
using System.Web.Mvc;
using Web_Application;
using asi.asicentral.web.Controllers;
using NUnit.Framework;

namespace asi.asicentral.WebApplication.Tests.Controllers
{
     [TestFixture]
    public class TemplateControllerTest
    {
        [Test]
        public void Form()
        {
            // arrange
            TemplateController controller = new TemplateController();
            
            // act
            ViewResult form = controller.Form() as ViewResult;
        }

        [Test]
        public void Dialog()
        {
            // arrange
            TemplateController controller = new TemplateController();
            
            // act
            ViewResult dialog = controller.Dialog() as ViewResult;
        }
    }
}
