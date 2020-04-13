using System;
using System.Collections.Generic;
using System.Linq;
using asi.asicentral.database.mappings;
using asi.asicentral.interfaces;
using asi.asicentral.model.asicentral;
using asi.asicentral.services;
using asi.asicentral.web.Controllers.asicentral;
using NUnit.Framework;
using StructureMap.Configuration.DSL;
using System.Web.Mvc;
using System.Web;
using System.IO;
using System.Security.Principal;
using Moq;
using asi.asicentral.model;

namespace External.Test.Admin
{
    [TestFixture]
    public class CatalogControllerTest
    {
        Random rand = new Random();


        [Test]
        public void ApproveContactRemaingCatalogMoreThanRequestTest()
        {
            //Initializing the objectService Object for db access
            var objectService = _initializeObjectService();

            //Creating data for CatalogContactImport
            var import = _createImport(objectService, "test industry", "Test Catalog");
            Assert.NotNull(import);

            //Creating data for CatalogContacts
            int catalogQty1 = 500;
            var catalog1 = _createCatalogContact(objectService, import.CatalogContactImportId, "AK", "Aleutians East", catalogQty1);
            Assert.NotNull(catalog1);

            int catalogQty2 = 700;
            var catalog2 = _createCatalogContact(objectService, import.CatalogContactImportId, "AL", "Lee", catalogQty2);
            Assert.NotNull(catalog2);
            import.CatalogContacts = new List<CatalogContact>() { catalog1, catalog2 };

            //Creating data for CatalogContactSale
            var sale = _createCatalogSale(objectService, import);
            sale.CatalogContactSaleDetails = new List<CatalogContactSaleDetail>();
            var reqCatalog = 40;
            foreach (var contact in import.CatalogContacts)
            {
                //Creating data for CatalogContactSaleDetails
                var details = _createCatalogSalesDetails(objectService, sale.CatalogContactSaleId, contact.CatalogContactId, reqCatalog);
                reqCatalog += 10;
            }
            Assert.NotNull(sale);

            //Mockup controller object
            CatalogController controller = _mockCatalogController();
            controller.ObjectService = objectService;
           
            //Method to test
            controller.ApproveContact(sale.CatalogContactSaleDetails.ToList(), sale.ASINumber, 0, false, string.Empty, false);

            Assert.IsTrue(sale.IsApproved);
            foreach (var catalog in import.CatalogContacts)
            {
                var details = sale.CatalogContactSaleDetails.Where(c => c.CatalogContactId == catalog.CatalogContactId).FirstOrDefault();
                Assert.AreEqual(catalog.OriginalContacts- details.ContactsRequested, details.CatalogContacts.RemainingContacts);
            }
            #region cleanUp
            for (int i = sale.CatalogContactSaleDetails.Count; i > 0; i--)
            {
                objectService.Delete<CatalogContactSaleDetail>(sale.CatalogContactSaleDetails.ElementAt(i - 1));
            }

            objectService.Delete<CatalogContactSale>(sale);
            for (int i = import.CatalogContacts.Count; i > 0; i--)
            {
                objectService.Delete<CatalogContact>(import.CatalogContacts.ElementAt(i - 1));
            }
            objectService.Delete<CatalogContactImport>(import);
            objectService.SaveChanges();
            #endregion
        }

        [Test]
        public void ApproveContactRemaingCatalogLessThanRequestTest()
        {
            
            //Initializing the objectService Object for db access
            var objectService = _initializeObjectService();

            //Creating data for CatalogContactImport
            var import = _createImport(objectService, "test industry", "Test Catalog");
            Assert.NotNull(import);

            //Creating data for CatalogContacts
            int catalogQty1 = 50;
            var catalog1 = _createCatalogContact(objectService, import.CatalogContactImportId, "AK", "Aleutians East", catalogQty1);
            Assert.NotNull(catalog1);

            int catalogQty2 = 70;
            var catalog2 = _createCatalogContact(objectService, import.CatalogContactImportId, "AL", "Lee", catalogQty2);
            Assert.NotNull(catalog2);
            import.CatalogContacts = new List<CatalogContact>() { catalog1, catalog2 };

            //Creating data for CatalogContactSale
            var sale = _createCatalogSale(objectService, import);
            sale.CatalogContactSaleDetails = new List<CatalogContactSaleDetail>();
            var reqCatalog = 100;
            foreach (var contact in import.CatalogContacts)
            {
                //Creating data for CatalogContactSaleDetails
                var details = _createCatalogSalesDetails(objectService, sale.CatalogContactSaleId, contact.CatalogContactId, reqCatalog);
                reqCatalog += 10;
            }
            Assert.NotNull(sale);

            //Mockup controller object
            CatalogController controller = _mockCatalogController();
            controller.ObjectService = objectService;
            
            //Method to test
            controller.ApproveContact(sale.CatalogContactSaleDetails.ToList(), sale.ASINumber, 0, false, string.Empty, false);
            Assert.IsFalse(sale.IsApproved);
            foreach (var catalog in import.CatalogContacts)
            {
                var details = sale.CatalogContactSaleDetails.Where(c => c.CatalogContactId == catalog.CatalogContactId).FirstOrDefault();
                Assert.AreEqual(catalog.OriginalContacts, details.CatalogContacts.RemainingContacts);
            }

            #region cleanUp
            for (int i = sale.CatalogContactSaleDetails.Count; i > 0; i--)
            {
                objectService.Delete<CatalogContactSaleDetail>(sale.CatalogContactSaleDetails.ElementAt(i - 1));
            }

            objectService.Delete<CatalogContactSale>(sale);
            for (int i = import.CatalogContacts.Count; i > 0; i--)
            {
                objectService.Delete<CatalogContact>(import.CatalogContacts.ElementAt(i - 1));
            }
            objectService.Delete<CatalogContactImport>(import);
            objectService.SaveChanges();
            #endregion
        }


        private ObjectService _initializeObjectService()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            return new ObjectService(container);
        }
        private CatalogContactImport _createImport(ObjectService objectService, string industryName, string catalogName)
        {
            var import = new CatalogContactImport();
            import.IndustryName = industryName + rand.Next();
            import.ImportedBy = "Test Case";
            import.IsActive = true;
            import.CatalogName = catalogName + rand.Next();
            import.CreateDateUTC = DateTime.Now;
            import.UpdateDateUTC = DateTime.Now;
            import.UpdateSource = "CatalogSalesTest.cs - CatalogContactImport";
            objectService.Add<CatalogContactImport>(import);
            objectService.SaveChanges();

            // import.CatalogContacts = new List<CatalogContact>() { catalog1, catalog2 };
            return import.CatalogContactImportId == 0 ? null : import;
        }

        private CatalogContact _createCatalogContact(ObjectService objectService, int importId, string state, string county, int leads)
        {
            var contact = new CatalogContact();
            //contact.    CatalogContactId 
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
            objectService.Add<CatalogContact>(contact);
            objectService.SaveChanges();
            return contact.CatalogContactId == 0 ? null : contact;
            // contact. CatalogContactSaleDetails 
        }

        private CatalogContactSaleDetail _createCatalogSalesDetails(ObjectService objectService, int salesId, int catalogContactId, int catalogRequested)
        {
            var salesDetails = new CatalogContactSaleDetail();
            //  CatalogContactSaleDetailId =
            salesDetails.CatalogContactSaleId = salesId;
            salesDetails.CatalogContactId = catalogContactId;
            salesDetails.ContactsRequested = catalogRequested;
            salesDetails.ContactsApproved = 0;
            salesDetails.CreateDateUTC = DateTime.Now;
            salesDetails.UpdateDateUTC = DateTime.Now;
            salesDetails.UpdateSource = "CatalogSalesTest.cs - CreateCatalogSalesDetails";
            objectService.Add<CatalogContactSaleDetail>(salesDetails);
            objectService.SaveChanges();
            return salesDetails.CatalogContactSaleDetailId == 0 ? null : salesDetails;

        }

        private CatalogContactSale _createCatalogSale(ObjectService objectService, CatalogContactImport import)
        {
            var sale = new CatalogContactSale();
            sale.ASINumber = "125724";
            sale.CompanyName = "Distributor Sales";
            sale.FirstName = "Test Name";
            sale.LastName = "Last Name";
            sale.Phone = "23232345";
            sale.Email = "test@gmail.com";
            sale.ASIRep = "rep1@gmail.com;rep2@gmail.com";
            sale.IsApproved = false;
            sale.ApprovedBy = string.Empty;
            sale.ApprovedDate = null;
            sale.CreateDateUTC = DateTime.Now;
            sale.UpdateDateUTC = DateTime.Now;
            sale.UpdateSource = "CatalogSalesTest.cs - CreateCatalogSale";
            sale.OtherOptions = string.Empty;
            sale.ArtworkInFile = false;
            sale.IsCancelled = false;
            sale.CancelledBy = "test Case";
            sale.CancelledUTCDate = null;
            sale.ArtworkOption = string.Empty;
            sale.ArtworkRepeatNotes = string.Empty;
            sale.RequestMoreInfo = false;
            objectService.Add<CatalogContactSale>(sale);
            objectService.SaveChanges();

            return sale.CatalogContactSaleId == 0 ? null : sale;
        }
        private CatalogController _mockCatalogController()
        {
            var controller = new CatalogController();
            var controllerContext = new Mock<ControllerContext>();

            var mockTemplateService = new Mock<ITemplateService>();
            mockTemplateService.Setup(t => t.Render(It.IsAny<string>(), It.IsAny<object>())).Returns("Email Template");
            var mockEmailService = new Mock<IEmailService>();
            mockEmailService.Setup(m => m.SendMail(It.IsAny<Mail>()));

            var principal = new Mock<IPrincipal>();
            principal.Setup(p => p.IsInRole("Administrator")).Returns(true);
            principal.SetupGet(x => x.Identity.Name).Returns("Test User");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            controller.ControllerContext = controllerContext.Object;
            controller.EmailService = mockEmailService.Object;
            controller.TemplateService = mockTemplateService.Object;
            return controller;
        }
    }
}
