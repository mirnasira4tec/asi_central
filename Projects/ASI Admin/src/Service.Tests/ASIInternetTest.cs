﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.database;
using asi.asicentral.model;
using System.Collections.Generic;
using asi.asicentral.services.interfaces;
using asi.asicentral.model.sgr;

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
            }
        }
    }
}