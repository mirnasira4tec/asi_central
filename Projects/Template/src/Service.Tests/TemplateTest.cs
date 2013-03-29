using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.interfaces;
using asi.asicentral.services;
using System.Reflection;

namespace asi.asicentral.Tests
{
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [TestClass]
    public class TemplateTest
    {

        [TestMethod]
        public void TemplateRenderTest()
        {
            IFileSystemService fileService = new AssemblyFileService(Assembly.GetAssembly(this.GetType()));
            ITemplateService templateService = new RazorTemplateEngine(fileService);
            Contact contact = new Contact { FirstName = "First", LastName = "Last" };
            string result = templateService.Render<Contact>("asi.asicentral.Tests.Template.TemplateTest.cshtml", contact);
            Assert.AreEqual("Hello First Last, how are you?", result);
            result = templateService.Render("asi.asicentral.Tests.Template.TemplateTest.cshtml", new { FirstName = "First", LastName = "Last" });
            Assert.AreEqual("Hello First Last, how are you?", result);
        }
    }
}
