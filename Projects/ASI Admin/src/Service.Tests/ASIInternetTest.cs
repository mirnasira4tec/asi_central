﻿using System.Linq;
using asi.asicentral.database;
using asi.asicentral.model.sgr;
using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using asi.asicentral.model.excit;
using NUnit.Framework;

namespace asi.asicentral.Tests
{
    [TestFixture]
    public class ASIInternetTest
    {
        [Test]
        public void CompanyTest()
        {
            int count = 0;
            //basic crud operations for Companies
            using (var context = new ASIInternetContext())
            {
                count = context.Companies.Count();
                //make sure we have some
                Assert.IsTrue(count > 0);
                Company company = context.Companies.FirstOrDefault();
                Assert.IsNotNull(company);
                Assert.IsTrue(company.Products.Count > 0);
                Assert.IsTrue(company.Categories.Count > 0);
            }
        }

        [Test]
        public void ProductTest()
        {
            int count = 0;
            //basic crud operations for Companies
            using (var context = new ASIInternetContext())
            {
                count = context.Products.Count();
                //make sure we have some
                Assert.IsTrue(count > 0);
                Product product = context.Products.FirstOrDefault();
                Assert.IsNotNull(product);
                Assert.IsNotNull(product.Company);
                Assert.IsTrue(product.Categories.Count > 0);
            }
        }

        [Test]
        public void CategoryTest()
        {
            int count = 0;
            //basic crud operations for Companies
            using (var context = new ASIInternetContext())
            {
                count = context.Categories.Count();
                //make sure we have some
                Assert.IsTrue(count > 0);
                Category category = context.Categories.FirstOrDefault();
                Assert.IsNotNull(category);
                Assert.IsTrue(category.Products.Count > 0);
                Assert.IsTrue(category.Companies.Count > 0);
            }
        }

        [Test]
        public void OrderRetreiveTest()
        {
            int count = 0;
            using (var context = new ASIInternetContext())
            {
                count = context.Orders.Count();
                Assert.IsTrue(count > 0);
                LegacyOrder order = context.Orders.Where(theOrder => theOrder.CreditCard != null && theOrder.OrderDetails.Count > 0).FirstOrDefault();
                Assert.IsNotNull(order);
                Assert.IsNotNull(order.CreditCard);
                Assert.IsNotNull(order.OrderDetails);
                Assert.IsTrue(order.OrderDetails.Count() > 0);
                foreach (LegacyOrderDetail detail in order.OrderDetails)
                {
                    Assert.IsNotNull(detail.Product);
                }
            }
        }

        [Test]
        public void ASPNetMembership()
        {
            int count = 0;
            using (var context = new ASIInternetContext())
            {
                count = context.ASPNetMemberships.Count();
                Assert.IsTrue(count > 0);
            }
        }

        [Test]
        public void CENTUserProfilesPROF()
        {
            int count = 0;
            using (var context = new ASIInternetContext())
            {
                count = context.UserProfileDetails.Count();
                Assert.IsTrue(count > 0);
            }
        }

        [Test]
        public void Order()
        {
            int count = 0;
            using (var context = new ASIInternetContext())
            {
                count = context.Orders.Count();
                Assert.IsTrue(count > 0);
                //checking current order details work
                LegacyOrder order = context.Orders.Where(or => or.Id == 10335).SingleOrDefault();
                Assert.IsNotNull(order);
                LegacyOrderDetail detail = order.OrderDetails.Where(det => det.ProductId == 104).SingleOrDefault();
                Assert.IsNotNull(detail);
            }
        }

        [Test]
        public void SupplierAppTest()
        {
            Guid appIdentifier;
            using (var context = new ASIInternetContext())
            {
                LegacySupplierMembershipApplication application = new LegacySupplierMembershipApplication()
                {
                    Company = "SupplierTest",
                };
                IList<LegacySupplierDecoratingType> decoratingTypes = context.SupplierDecoratingTypes.ToList();
                Assert.IsTrue(decoratingTypes.Count > 1);
                application.DecoratingTypes.Add(decoratingTypes[0]);
                application.DecoratingTypes.Add(decoratingTypes[1]);
                context.SupplierMembershipApplications.Add(application);
                context.SaveChanges();
                appIdentifier = application.Id;
            }
            using (var context = new ASIInternetContext())
            {
                //try to retrieve it
                LegacySupplierMembershipApplication application = context.SupplierMembershipApplications.Where(app => app.Id == appIdentifier).SingleOrDefault();
                Assert.IsNotNull(application);
                Assert.AreEqual(2, application.DecoratingTypes.Count);
            }
        }

        [Test]
        public void DistributorAppTest()
        {
            Guid appIdentifier;
            using (var context = new ASIInternetContext())
            {
                LegacyDistributorBusinessRevenue revenue = context.DistributorBusinessRevenues.FirstOrDefault();
                Assert.IsNotNull(revenue);
                //create a new app
                LegacyDistributorMembershipApplication application = new LegacyDistributorMembershipApplication()
                {
                    Company = "DistributorAppTest",
                    PrimaryBusinessRevenue = revenue,
                };
                context.DistributorMembershipApplications.Add(application);
                //add a few accont types
                IList<LegacyDistributorAccountType> accountTypes = context.DistributorAccountTypes.ToList();
                Assert.IsTrue(accountTypes.Count > 1);
                application.AccountTypes.Add(accountTypes[0]);
                application.AccountTypes.Add(accountTypes[1]);
                //add a few product lines
                IList<LegacyDistributorProductLine> productLines = context.DistributorProductLines.ToList();
                Assert.IsTrue(productLines.Count > 1);
                application.ProductLines.Add(productLines[0]);
                application.ProductLines.Add(productLines[1]);
                context.SaveChanges();
                appIdentifier = application.Id;
            }
            using (var context = new ASIInternetContext())
            {
                //try to retrieve it
                LegacyDistributorMembershipApplication application = context.DistributorMembershipApplications.Where(app => app.Id == appIdentifier).SingleOrDefault();
                Assert.IsNotNull(application);
                Assert.IsNotNull(application.PrimaryBusinessRevenueId);
                Assert.AreEqual(2, application.AccountTypes.Count);
                Assert.AreEqual(2, application.ProductLines.Count);
            }
        }

        [Test]
        public void SupplierUpdateRequestTest()
        {
            using (var context = new ASIInternetContext())
            {
                //retrieve update field
                var fields = context.SupplierUpdateFields.Where(s => s.IsObsolete.HasValue).ToList();
                Assert.IsNotNull(fields);

                var updateRequest = new SupUpdateRequest()
                {
                    CompanyId = 1234,
                    CompanyName = "Supplier Config Update Request",
                    RequestedBy = "mzhang_unit",
                    Status = SupRequestStatus.Pending,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    UpdateSource = "Initial Unit Tests"
                };

                context.SupplierUpdateRequests.Add(updateRequest);
                context.SaveChanges();
                updateRequest = context.SupplierUpdateRequests.Where(r => r.CompanyId == 1234).OrderByDescending(r => r.Id).FirstOrDefault();
                Assert.IsNotNull(updateRequest);

                var requestDetails = new SupUpdateRequestDetail()
                {
                    SupUpdateRequestId = updateRequest.Id,
                    SupUpdateFieldId = 1,
                    UpdateValue = "http://testInventory.com/svc",
                    OrigValue = "origiValue",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    UpdateSource = "Unit Test"
                };

                context.SupplierUpdateRequestDetails.Add(requestDetails);
                context.SaveChanges();

                var detail = context.SupplierUpdateRequestDetails.FirstOrDefault(d => d.UpdateValue.Equals("http://testInventory.com/svc"));
                Assert.IsNotNull(detail);
            }
        }
    }
}