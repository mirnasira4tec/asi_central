using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.interfaces;
using asi.asicentral.services;
using asi.asicentral.database.mappings;

namespace asi.asicentral.WebApplication.Tests.Controllers.Store
{
    /// <summary>
    /// Summary description for MagazineAdvertisingApplicationTest
    /// </summary>
    [TestClass]
    public class MagazineAdvertisingApplicationTest
    {
        public MagazineAdvertisingApplicationTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //
        }

        [TestMethod]
        public void StoreDetailMagazineAdvertisingTest()
        {
           // using (IStoreService storeService = new StoreService(new Container(new EFRegistry())))
            {
                //var closedDate1 = storeService.ClosedCampaignDates;
                //var closedDate2 = storeService.ClosedCampaignDates.FirstOrDefault();
                //var closedDate3 = storeService.ClosedCampaignDates
                //    .Where(date1 => date1.Reactivated == true || date1.Reactivated == false)
                //    .Select(date2 => date2);

                //Assert.IsTrue(storeService.ClosedCampaignDates.Count() > 0);
                //Assert.IsTrue(closedDate1.Count() == closedDate3.Count());
                //Assert.IsNotNull(closedDate2);
                //Assert.IsNotNull(closedDate2.Reactivated);
            }
        }
    }
}
