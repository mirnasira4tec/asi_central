using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.database;
using asi.asicentral.model;
using System.Collections.Generic;
using asi.asicentral.interfaces;
using asi.asicentral.model.sgr;
using asi.asicentral.model.store;

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
    }
}