using System;
using asi.asicentral.oauth;
using NUnit.Framework;

namespace Common.Tests.OAuth
{
    [TestFixture]
    public class SSOTests
    {
        [Test]
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
