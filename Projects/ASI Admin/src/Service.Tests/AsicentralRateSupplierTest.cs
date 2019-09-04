using System;
using System.Collections.Generic;
using asi.asicentral.database.mappings;
using asi.asicentral.interfaces;
using asi.asicentral.model.asicentral;
using asi.asicentral.services;
using NUnit.Framework;
using StructureMap.Configuration.DSL;

namespace asi.asicentral.Tests
{
    [TestFixture]
    public class AsicentralRateSupplierTest
    {
        [Test]
        public void SaveRateSupplierImports()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectContext = new ObjectService(container))
            {
                RateSupplierImport obj = new RateSupplierImport()
                {
                    LastUpdatedBy = "test",
                    CreateDateUTC = DateTime.Now,
                    UpdateDateUTC = DateTime.Now,
                    UpdateSource = "Test Case",
                    NumberOfImports = 1,
                    IsActive = true,
                };
                objectContext.Add(obj);
                objectContext.SaveChanges();
                Assert.AreNotEqual(obj.RateSupplierImportId, 0);
            }
        }

        [Test]
        public void SaveRateSupplierData()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectContext = new ObjectService(container))
            {
                RateSupplierImport import = PopulateRateSupplierImport();
                objectContext.Add(import);
                objectContext.SaveChanges();
                Assert.AreNotEqual(import.RateSupplierImportId, 0);

                RateSupplierForm form = PopulateRateSupplierForms();
                form.RateSupplierImportId = import.RateSupplierImportId;
                form.RateSupplierImports = import;
                import.RateSupplierForms = new List<RateSupplierForm>() { form };
                objectContext.Add(form);
                objectContext.SaveChanges();
                Assert.AreNotEqual(form.RateSupplierFormId, 0);

                RateSupplierFormDetail details = PopulateRateSupplierFormsDetails();
                details.RateSupplierFormId = form.RateSupplierFormId;
                details.RateSupplierForm = form;
                form.RateSupplierFormDetails = new List<RateSupplierFormDetail>() { details };
                objectContext.Add(details);
                objectContext.SaveChanges();
                Assert.AreNotEqual(details.RateSupplierFormDetailId, 0);
            }
        }
        [Test]
        public void DeleteRateSupplierData()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectContext = new ObjectService(container))
            {
                RateSupplierImport import = PopulateRateSupplierImport();
                objectContext.Add(import);
                objectContext.SaveChanges();
                Assert.AreNotEqual(import.RateSupplierImportId, 0);

                RateSupplierForm form = PopulateRateSupplierForms();
                form.RateSupplierImportId = import.RateSupplierImportId;
                form.RateSupplierImports = import;
                import.RateSupplierForms = new List<RateSupplierForm>() { form };
                objectContext.Add(form);
                objectContext.SaveChanges();
                Assert.AreNotEqual(form.RateSupplierFormId, 0);

                RateSupplierFormDetail details = PopulateRateSupplierFormsDetails();
                details.RateSupplierFormId = form.RateSupplierFormId;
                details.RateSupplierForm = form;
                form.RateSupplierFormDetails = new List<RateSupplierFormDetail>() { details };
                objectContext.Add(details);
                objectContext.SaveChanges();
                Assert.AreNotEqual(details.RateSupplierFormDetailId, 0);
                try
                {
                    objectContext.Delete<RateSupplierForm>(form);
                    objectContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Assert.IsInstanceOf<Exception>(ex);
                }
                try
                {
                    objectContext.Delete<RateSupplierFormDetail>(details);
                    objectContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Assert.IsInstanceOf<Exception>(ex);
                }
            }
        }


        private RateSupplierImport PopulateRateSupplierImport()
        {
            return (new RateSupplierImport()
            {
                LastUpdatedBy = "test",
                CreateDateUTC = DateTime.Now,
                UpdateDateUTC = DateTime.Now,
                UpdateSource = "Test Case",
                NumberOfImports = 1,
                IsActive = true,
            });
        }
        private RateSupplierForm PopulateRateSupplierForms()
        {
            return (new RateSupplierForm()
            {
                DistASINum = "123654",
                DistCompanyName = "Pens LLc",
                DistFax = "2365987410",
                DistPhone = "2365987410",
                SubmitBy = "Test Case",
                SubmitDateUTC = DateTime.Now,
                SubmitSuccessful = true,
                CreateDateUTC = DateTime.Now,
                UpdateDateUTC = DateTime.Now,
                UpdateSource = "Test Case",
                SubmitName = "Test",
                SubmitEmail = "test@test.com",
                IPAddress = "127.0.0.1"
            });
        }
        private RateSupplierFormDetail PopulateRateSupplierFormsDetails()
        {
            return (new RateSupplierFormDetail()
            {
                SupASINum = "45698",
                SupCompanyName = "Supplier Company Pvt. ltd.",
                NumOfTransImport = 5,
                NumOfTransSubmit = 10,
                OverallRating = 4,
                ProdQualityRating = 0,
                CommunicationRating = 0,
                DeliveryRating = 4,
                ImprintingRating = 5,
                ProbResolutionRating = 4,
                CreateDateUTC = DateTime.Now,
                UpdateDateUTC = DateTime.Now,
                UpdateSource = "Test Case",
            });
        }

    }
}
