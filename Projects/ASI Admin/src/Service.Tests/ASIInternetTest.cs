using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.database;
using asi.asicentral.model.sgr;
using asi.asicentral.model.store;
using System;
using System.Collections.Generic;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class ASIInternetTest
    {
        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void OrderRetreiveTest()
        {
            int orderIdentifier = 1;

            int count = 0;
            //using orders from the database dump, might need to improve that
            //Needed to make sure we can retrieve legacy orders and that all objects are populated properly
            using (var context = new ASIInternetContext())
            {
                count = context.Orders.Count();
                Assert.IsTrue(count > 0);
                Order order = context.Orders.Where(theOrder => theOrder.Id == orderIdentifier).SingleOrDefault();
                Assert.IsNotNull(order);
                Assert.IsNotNull(order.CreditCard);
                Assert.IsNotNull(order.OrderDetails);
                Assert.IsTrue(order.OrderDetails.Count() > 0);
                foreach (OrderDetail detail in order.OrderDetails)
                {
                    Assert.IsNotNull(detail.Product);
                }
            }
        }

        [TestMethod]
        public void ASPNetMembership()
        {
            int count = 0;
            using (var context = new ASIInternetContext())
            {
                count = context.ASPNetMemberships.Count();
                Assert.IsTrue(count > 0);
            }
        }

        [TestMethod]
        public void Order()
        {
            int count = 0;
            using (var context = new ASIInternetContext())
            {
                count = context.Orders.Count();
                Assert.IsTrue(count > 0);
                //checking current order details work
                Order order = context.Orders.Where(or => or.Id == 10335).SingleOrDefault();
                Assert.IsNotNull(order);
                OrderDetail detail = order.OrderDetails.Where(det => det.ProductId == 104).SingleOrDefault();
                Assert.IsNotNull(detail);
            }
        }

        [TestMethod]
        public void DistributorAppTest()
        {
            Guid appIdentifier;
            using (var context = new ASIInternetContext())
            {
                DistributorBusinessRevenue revenue = context.DistributorBusinessRevenues.FirstOrDefault();
                Assert.IsNotNull(revenue);
                //create a new app
                DistributorMembershipApplication application = new DistributorMembershipApplication()
                {
                    Company = "DistributorAppTest",
                    PrimaryBusinessRevenue = revenue,
                };
                context.DistributorMembershipApplications.Add(application);
                //add a few accont types
                IList<DistributorAccountType> accountTypes = context.DistributorAccountTypes.ToList();
                Assert.IsTrue(accountTypes.Count > 1);
                application.AccountTypes.Add(accountTypes[0]);
                application.AccountTypes.Add(accountTypes[1]);
                //add a few product lines
                IList<DistributorProductLine> productLines = context.DistributorProductLines.ToList();
                Assert.IsTrue(productLines.Count > 1);
                application.ProductLines.Add(productLines[0]);
                application.ProductLines.Add(productLines[1]);
                context.SaveChanges();
                appIdentifier = application.Id;
            }
            using (var context = new ASIInternetContext())
            {
                //try to retrieve it
                DistributorMembershipApplication application = context.DistributorMembershipApplications.Where(app => app.Id == appIdentifier).SingleOrDefault();
                Assert.IsNotNull(application);
                Assert.IsNotNull(application.PrimaryBusinessRevenueId);
                Assert.AreEqual(2, application.AccountTypes.Count);
                Assert.AreEqual(2, application.ProductLines.Count);
            }
        }
    }
}