using System;
using System.Collections.Generic;
using System.Linq;
using asi.asicentral.database.mappings;
using asi.asicentral.interfaces;
using asi.asicentral.model.asicentral;
using asi.asicentral.services;
using NUnit.Framework;
using StructureMap.Configuration.DSL;

namespace asi.asicentral.Tests
{
    public class CatalogTest
    {

        private CatalogContactImport PopulateCatalogContactImport()
        {
            var import = new CatalogContactImport()
            {
                ImportedBy = "Test Case",
                IsActive = true,
                IndustryName = "Education",
                CatalogName = "TestCatalog",
                CreateDateUTC = DateTime.UtcNow,
                UpdateDateUTC = DateTime.UtcNow,
                UpdateSource = "TestCase",
            };


            return (import);

        }

        private CatalogContact PopulateCatalogContact(int importId)
        {
            return (new CatalogContact()
            {
                CatalogContactImportId = importId,
                State = "NY",
                County = "Test County",
                Percentage = 30.5M,
                OriginalContacts = 100,
                RemainingContacts = 60,
                CreateDateUTC = DateTime.UtcNow,
                UpdateDateUTC = DateTime.UtcNow,
                UpdateSource = "TestCase"
            });
        }

        private CatalogContactSale PopulateCatalogContactSale()
        {
            var contanctSale = new CatalogContactSale();

         //   contanctSale.CatalogContactImportId = importId;
            contanctSale.ASINumber = "12345";
            contanctSale.CompanyName = "AsiTest";
            contanctSale.FirstName = "FirstName";
            contanctSale.LastName = "LastName";
            contanctSale.Phone = "1234567890";
            contanctSale.Email = "email@testcase.com";
            contanctSale.IPAddress = "127.0.0.1";
            contanctSale.ASIRep = "RepTest";
            contanctSale.IsApproved = true;
            contanctSale.ApprovedBy = "Test Case";
            contanctSale.ApprovedDate = DateTime.Now;
            contanctSale.CreateDateUTC = DateTime.UtcNow;
            contanctSale.UpdateDateUTC = DateTime.UtcNow;
            contanctSale.UpdateSource = "Test Case";
            return contanctSale;
        }

        private CatalogContactSaleDetail PopulateSaleDetails(int salesId, int contactId)
        {
            var salesDetails = new CatalogContactSaleDetail();
            salesDetails.CatalogContactSaleId = salesId;
            salesDetails.CatalogContactId = contactId;
            salesDetails.ContactsRequested = 45;
            salesDetails.ContactsApproved = 40;
            salesDetails.CreateDateUTC = DateTime.UtcNow;
            salesDetails.UpdateDateUTC = DateTime.UtcNow;
            salesDetails.UpdateSource = "Test- Case";
            return salesDetails;
        }

        [Test]
        public void SaveCatalogContactImport()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectContext = new ObjectService(container))
            {
                CatalogContactImport obj = this.PopulateCatalogContactImport();
                var cataLogContacts = new List<CatalogContact>();
                cataLogContacts.Add(PopulateCatalogContact(obj.CatalogContactImportId));
                cataLogContacts.Add(PopulateCatalogContact(obj.CatalogContactImportId));
                obj.CatalogContacts = cataLogContacts;
                objectContext.Add(obj);
                objectContext.SaveChanges();
                Assert.AreNotEqual(obj.CatalogContactImportId, 0);
            }
        }

        [Test]
        public void CatalogSale()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectContext = new ObjectService(container))
            {
                CatalogContactImport obj = this.PopulateCatalogContactImport();
                var contact1 = PopulateCatalogContact(obj.CatalogContactImportId);
                var contact2 = PopulateCatalogContact(obj.CatalogContactImportId);
                obj.CatalogContacts = new List<CatalogContact>();
                obj.CatalogContacts.Add(contact1);
                obj.CatalogContacts.Add(contact2);
                objectContext.Add(obj);
                objectContext.SaveChanges();
                List<CatalogContactSale> catalogContactSales = new List<CatalogContactSale>();
                var sale = PopulateCatalogContactSale();
                var saleDetails1 = PopulateSaleDetails(sale.CatalogContactSaleId, contact1.CatalogContactId);
                var saleDetails2 = PopulateSaleDetails(sale.CatalogContactSaleId, contact2.CatalogContactId);
                sale.CatalogContactSaleDetails = new List<CatalogContactSaleDetail>();
                sale.CatalogContactSaleDetails.Add(saleDetails1);
                sale.CatalogContactSaleDetails.Add(saleDetails2);
                catalogContactSales.Add(sale);
                objectContext.SaveChanges();
                Assert.AreNotEqual(obj.CatalogContactImportId, 0);
            }
        }


    }
}
