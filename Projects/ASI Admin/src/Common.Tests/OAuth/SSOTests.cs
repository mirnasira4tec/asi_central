using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.oauth;

namespace Common.Tests.OAuth
{
    [TestClass]
    public class SSOTests
    {
        [TestMethod]
        public void GetRoleNameTest()
        {
            var result = SSO.GetRoleName("ABCD", "ACTIVE");
            Assert.AreEqual(result, "Guest");

            result = SSO.GetRoleName("MLRP", "ACTIVE");
            Assert.AreEqual(result, "MultiLineRep");

            result = SSO.GetRoleName("MLRP", "DELS");
            Assert.AreEqual(result, "Guest");
        }
    }
}
