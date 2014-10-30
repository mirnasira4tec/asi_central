using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.database;
using System.Linq;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class DM_MemberDemogrContextTest
    {
        [TestMethod]
        public void GetAllRepsTest()
        {
            int count = 0;
            //basic crud operations for Companies
            using (var context = new DM_MemberDemogrContext())
            {
                count = context.CompanyASIReps.Count();
                //make sure we have some
                Assert.IsTrue(count > 0);
                asi.asicentral.model.DM_memberDemogr.CompanyASIRep asirep = context.CompanyASIReps.FirstOrDefault();
                Assert.IsNotNull(asirep);
            }
        }

        [TestMethod]
        public void GetAllRepsByCompanyIdTest()
        {
            int count = 0;
            //basic crud operations for Companies
            using (var context = new DM_MemberDemogrContext())
            {
                count = context.CompanyASIReps.Where(rep => rep.CompanyID == 114).Count();
                //make sure we have some
                Assert.AreEqual(count, 5);
                asi.asicentral.model.DM_memberDemogr.CompanyASIRep asirep = context.CompanyASIReps.FirstOrDefault();
                Assert.IsNotNull(asirep);
            }
        }
    }
}
