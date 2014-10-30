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
                asi.asicentral.model.DM_memberDemogr.CompanyASIRep rep = context.CompanyASIReps.FirstOrDefault();
                Assert.IsNotNull(rep);
            }
        }
    }
}
