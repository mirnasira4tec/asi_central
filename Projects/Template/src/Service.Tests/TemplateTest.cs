using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.interfaces;
using asi.asicentral.services;
using System.Reflection;
using asi.asicentral.util;
using System.Collections.Generic;
using System.Web.Mvc;

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

        [TestMethod]
        public void CountryList()
        {
            List<SelectListItem> countries = asi.asicentral.util.HtmlHelper.GetCountries() as List<SelectListItem>;
            List<string> codes = new List<string>();
            foreach (SelectListItem country in countries)
            {
                codes.Add(country.Value);
                Console.WriteLine(country.Value + ":" + country.Text);
            }
            Console.WriteLine("========================");
            codes.Sort();
            foreach (string code in codes)
            {
                Console.WriteLine(code);
            }
            List<SelectListItem> countriesRestricted = asi.asicentral.util.HtmlHelper.GetCountries(includeAll: false) as List<SelectListItem>;
            Assert.IsTrue(countries.Count > 0);
            Assert.IsTrue(countriesRestricted.Count > 0);
            Assert.IsTrue(countries.Count > countriesRestricted.Count);
        }
    }
}
