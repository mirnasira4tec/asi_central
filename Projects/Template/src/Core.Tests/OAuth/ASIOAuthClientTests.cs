using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.oauth;

namespace Core.Tests.OAuth
{
    [TestClass]
    public class ASIOAuthClientTests
    {
        [TestMethod]
        public void IsValidUser()
        {
            bool result = ASIOAuthClient.IsValidUser("kphani@macrosoftindia.com");
            Assert.IsTrue(result);
        }
    }
}
