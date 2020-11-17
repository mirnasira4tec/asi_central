using System;
using System.Collections.Generic;
using System.Linq;
using asi.asicentral.database.mappings;
using asi.asicentral.interfaces;
using asi.asicentral.model.asicentral;
using asi.asicentral.services;
using asi.asicentral.web.Controllers.asicentral;
using NUnit.Framework;
using System.Web.Mvc;
using System.Web;
using System.IO;
using System.Security.Principal;
using Moq;
using ClosedXML.Excel;
using asi.asicentral.model;

namespace Internal.Test.Admin
{
    [TestFixture]
    public class CatalogControllerTest
    {

        Random rand = new Random();
        [Test]
        public void ApproveContactRemaingCatalogMoreThanRequestTest()
        {
            var mockObjectService = new Mock<IObjectService>();

            //Setup object for CatalogContactImport
            var import = _createImport("test industry", "Test Catalog");

            //Setup object for CatalogContactSale
            var sale = _createCatalogSale();

            //Setup object for CatalogContacts
            int catalogQty1 = 500;
            var catalog1 = _createCatalogContact(2123, import.CatalogContactImportId, "AK", "Aleutians East", catalogQty1);

            int catalogQty2 = 700;
            var catalog2 = _createCatalogContact(5643, import.CatalogContactImportId, "AL", "Lee", catalogQty2);
            var catalogList = new List<CatalogContact>() { catalog1, catalog2 };

            //Setup object for CatalogContactSaleDetails
            var reqCatalog1 = 40;
            var details1 = _createCatalogSalesDetails(sale, catalog1.CatalogContactId, reqCatalog1);
            details1.CatalogContacts = catalog1;

            var reqCatalog2 = 50;
            var details2 = _createCatalogSalesDetails(sale, catalog2.CatalogContactId, reqCatalog2);
            details2.CatalogContacts = catalog2;
            sale.CatalogContactSaleDetails = new List<CatalogContactSaleDetail>() { details1, details2 };

            //Mocking up the controller object
            CatalogController controller = _mockCatalogController();

            mockObjectService.Setup(m => m.GetAll<CatalogContactSaleDetail>("CatalogContactSale", false)).Returns(sale.CatalogContactSaleDetails.AsQueryable());
            controller.ObjectService = mockObjectService.Object;

            //Main function to test
            controller.ApproveContact(sale.CatalogContactSaleDetails.ToList(), sale.ASINumber, 0, false, string.Empty, false);

            #region assertion and verification
            Assert.IsTrue(sale.IsApproved);
            foreach (var catalog in catalogList)
            {
                var details = sale.CatalogContactSaleDetails.Where(c => c.CatalogContactId == catalog.CatalogContactId).FirstOrDefault();
                Assert.AreEqual(catalog.OriginalContacts - details.ContactsRequested, details.CatalogContacts.RemainingContacts);
            }
            mockObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(1));
            #endregion
        }

        [Test]
        public void ApproveContactRemaingCatalogMoreLessRequestTest()
        {

            var mockObjectService = new Mock<IObjectService>();

            //Setup object for CatalogContactImport
            var import = _createImport("test industry", "Test Catalog");

            //Setup object for CatalogContactSale
            var sale = _createCatalogSale();

            //Setup object for CatalogContacts
            int catalogQty1 = 500;
            var catalog1 = _createCatalogContact(2123, import.CatalogContactImportId, "AK", "Aleutians East", catalogQty1);
            int catalogQty2 = 700;
            var catalog2 = _createCatalogContact(5643, import.CatalogContactImportId, "AL", "Lee", catalogQty2);
            var catalogList = new List<CatalogContact>() { catalog1, catalog2 };

            //Setup object for CatalogContactSaleDetails
            var reqCatalog1 = 540;
            var details1 = _createCatalogSalesDetails(sale, catalog1.CatalogContactId, reqCatalog1);
            details1.CatalogContacts = catalog1;
            var reqCatalog2 = 850;
            var details2 = _createCatalogSalesDetails(sale, catalog2.CatalogContactId, reqCatalog2);
            details2.CatalogContacts = catalog2;
            sale.CatalogContactSaleDetails = new List<CatalogContactSaleDetail>() { details1, details2 };

            //Mocking up the controller object
            CatalogController controller = _mockCatalogController();

            mockObjectService.Setup(m => m.GetAll<CatalogContactSaleDetail>("CatalogContactSale", false)).Returns(sale.CatalogContactSaleDetails.AsQueryable());
            controller.ObjectService = mockObjectService.Object;

            //Main function to test
            controller.ApproveContact(sale.CatalogContactSaleDetails.ToList(), sale.ASINumber, 0, false, string.Empty, false);

            #region assertion and verification
            Assert.IsFalse(sale.IsApproved);
            foreach (var catalog in catalogList)
            {
                var details = sale.CatalogContactSaleDetails.Where(c => c.CatalogContactId == catalog.CatalogContactId).FirstOrDefault();
                Assert.AreEqual(catalog.OriginalContacts, details.CatalogContacts.RemainingContacts);
            }
            mockObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(0));
            #endregion
        }


        [Test]
        public void CatalogContactUpdateTestForSameIndustry()
        {
            var mockObjectService = new Mock<IObjectService>();
            var industryName = "Healthcare";
            var import = _createImport(industryName, "Test Catalog");
            int catalogQty1 = 500;
            var catalog1 = _createCatalogContact(1232, import.CatalogContactImportId, "AK", "Aleutians East", catalogQty1);

            int catalogQty2 = 700;
            var catalog2 = _createCatalogContact(2345, import.CatalogContactImportId, "AL", "Lee", catalogQty2);
            import.CatalogContacts = new List<CatalogContact>() { catalog1, catalog2 };

            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Health_Catalogs");
            var xlCatalogQty1 = 200;
            var xlCatalogQty2 = 120;

            ws.Cell(1, 1).Value = "Industry";
            ws.Cell(1, 2).Value = "State";
            ws.Cell(1, 3).Value = "County";
            ws.Cell(1, 4).Value = "Leads";

            ws.Cell(2, 1).Value = industryName;
            ws.Cell(2, 2).Value = "AK";
            ws.Cell(2, 3).Value = "Aleutians East";
            ws.Cell(2, 4).Value = xlCatalogQty1;

            ws.Cell(3, 1).Value = industryName;
            ws.Cell(3, 2).Value = "WY";
            ws.Cell(3, 3).Value = "Goshen";
            ws.Cell(3, 4).Value = xlCatalogQty2;

            var controller = _mockCatalogController();

            mockObjectService.Setup(objectService => objectService.Delete(It.IsAny<CatalogContact>())).Callback<CatalogContact>((contact) => import.CatalogContacts.Remove(contact));
            controller.ObjectService = mockObjectService.Object;
            var result = controller.CatalogContactUpdate(import, ws) as RedirectToRouteResult;

            Assert.AreEqual(result.RouteValues["action"].ToString(), "CatalogContactImport");
            Assert.AreEqual(result.RouteValues["controller"].ToString(), "Catalog");
            Assert.NotNull(controller.TempData["SuccessMessage"]);
            Assert.AreEqual(controller.TempData["SuccessMessage"].ToString(), $"Data updated successfully");
            Assert.AreEqual(import.CatalogContacts.ElementAt(0).OriginalContacts, xlCatalogQty1);
            Assert.AreEqual(import.CatalogContacts.ElementAt(1).OriginalContacts, xlCatalogQty2);
            mockObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(1));
        }

        [Test]
        public void CatalogContactUpdateTestForDifferentIndustry()
        {
            var mockObjectService = new Mock<IObjectService>();
            var industryName = "Test Industry";
            var import = _createImport("Test Industry", "Test Catalog");
            int catalogQty1 = 500;
            var catalog1 = _createCatalogContact(1232, import.CatalogContactImportId, "AK", "Aleutians East", catalogQty1);

            int catalogQty2 = 700;
            var catalog2 = _createCatalogContact(2345, import.CatalogContactImportId, "AL", "Lee", catalogQty2);
            import.CatalogContacts = new List<CatalogContact>() { catalog1, catalog2 };

            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Health_Catalogs");
            var xlCatalogQty1 = 200;
            var xlCatalogQty2 = 120;

            ws.Cell(1, 1).Value = "Industry";
            ws.Cell(1, 2).Value = "State";
            ws.Cell(1, 3).Value = "County";
            ws.Cell(1, 4).Value = "Leads";

            ws.Cell(2, 1).Value = "Healthcare";
            ws.Cell(2, 2).Value = "AK";
            ws.Cell(2, 3).Value = "Aleutians East";
            ws.Cell(2, 4).Value = xlCatalogQty1;

            ws.Cell(3, 1).Value = "Healthcare";
            ws.Cell(3, 2).Value = "WY";
            ws.Cell(3, 3).Value = "Goshen";
            ws.Cell(3, 4).Value = xlCatalogQty2;

            var controller = _mockCatalogController();

            mockObjectService.Setup(objectService => objectService.Delete(It.IsAny<CatalogContact>())).Callback<CatalogContact>((contact) => import.CatalogContacts.Remove(contact));
            controller.ObjectService = mockObjectService.Object;
            var result = controller.CatalogContactUpdate(import, ws) as RedirectToRouteResult;

            Assert.AreEqual(result.RouteValues["action"].ToString(), "CatalogContactImport");
            Assert.AreEqual(result.RouteValues["controller"].ToString(), "Catalog");
            Assert.NotNull(controller.TempData["SuccessMessage"]);
            Assert.AreEqual(controller.TempData["SuccessMessage"].ToString(), $"Data imported partialy, this excel contains records other than {industryName} industry, those records are skipped.");
            mockObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(1));
        }

        private CatalogContactImport _createImport(string industryName, string catalogName)
        {
            var import = new CatalogContactImport();
            import.CatalogContactImportId = 1234;
            import.IndustryName = industryName;
            import.ImportedBy = "Test Case";
            import.IsActive = true;
            import.CatalogName = catalogName;
            import.CreateDateUTC = DateTime.Now;
            import.UpdateDateUTC = DateTime.Now;
            import.UpdateSource = "CatalogSalesTest.cs - CatalogContactImport";
            return import;

        }

        private CatalogContact _createCatalogContact(int contactId, int importId, string state, string county, int leads)
        {
            var contact = new CatalogContact();
            contact.CatalogContactId = contactId;
            contact.CatalogContactImportId = importId;
            contact.State = state;
            contact.County = county;
            contact.Percentage = 5;
            contact.OriginalContacts = leads;
            contact.RemainingContacts = leads;
            contact.CreateDateUTC = DateTime.Now;
            contact.UpdateDateUTC = DateTime.Now;
            contact.UpdateSource = "CatalogSalesTest.cs - CreateCatalogContact";
            contact.Note = string.Empty;
            return contact;
        }

        private CatalogContactSaleDetail _createCatalogSalesDetails(CatalogContactSale sale, int catalogContactId, int catalogRequested)
        {
            var salesDetails = new CatalogContactSaleDetail();
            salesDetails.CatalogContactSaleDetailId = rand.Next();
            salesDetails.CatalogContactSaleId = sale.CatalogContactSaleId;
            salesDetails.CatalogContactId = catalogContactId;
            salesDetails.ContactsRequested = catalogRequested;
            salesDetails.ContactsApproved = 0;
            salesDetails.CreateDateUTC = DateTime.Now;
            salesDetails.UpdateDateUTC = DateTime.Now;
            salesDetails.CatalogContactSale = sale;
            salesDetails.UpdateSource = "CatalogSalesTest.cs - CreateCatalogSalesDetails";
            return salesDetails;

        }

        private CatalogContactSale _createCatalogSale()
        {
            var sale = new CatalogContactSale();
            sale.CatalogContactSaleId = rand.Next();
            sale.ASINumber = rand.Next(0, 999999).ToString();
            sale.CompanyName = "Distributor Sales" + rand.Next();
            sale.FirstName = "Test Name" + rand.Next();
            sale.LastName = "Last Name";
            sale.Phone = "23232345";
            sale.Email = "test" + rand.Next() + "@gmail.com";
            sale.ASIRep = "rep1@gmail.com;rep2@gmail.com";
            sale.IsApproved = false;
            sale.ApprovedBy = string.Empty;
            sale.ApprovedDate = null;
            sale.CreateDateUTC = DateTime.Now;
            sale.UpdateDateUTC = DateTime.Now;
            sale.UpdateSource = "CatalogControllerTest.cs - _createCatalogSale";
            sale.OtherOptions = string.Empty;
            sale.ArtworkInFile = false;
            sale.IsCancelled = false;
            sale.CancelledBy = "test Case";
            sale.CancelledUTCDate = null;
            sale.ArtworkOption = string.Empty;
            sale.ArtworkRepeatNotes = string.Empty;
            sale.RequestMoreInfo = false;
            return sale;
        }
        private CatalogController _mockCatalogController()
        {
            var controller = new CatalogController();
            var controllerContext = new Mock<ControllerContext>();

            var mockTemplateService = new Mock<ITemplateService>();
            mockTemplateService.Setup(t => t.Render(It.IsAny<string>(), It.IsAny<object>())).Returns("Email Template");
            var mockEmailService = new Mock<IEmailService>();
            mockEmailService.Setup(m => m.SendMail(It.IsAny<Mail>()));

            controller.TemplateService = mockTemplateService.Object;
            controller.EmailService = mockEmailService.Object;

            var principal = new Mock<IPrincipal>();
            principal.Setup(p => p.IsInRole("Administrator")).Returns(true);
            principal.SetupGet(x => x.Identity.Name).Returns("Test User");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            controller.ControllerContext = controllerContext.Object;
            return controller;
        }
    }
}
