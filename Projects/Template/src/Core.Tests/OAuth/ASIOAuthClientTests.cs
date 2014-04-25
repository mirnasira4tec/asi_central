using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.oauth;

namespace Core.Tests.OAuth
{
    [TestClass]
    public class ASIOAuthClientTests
    {
        [TestMethod]
        public void IsValidUserByEmail()
        {
            bool result = ASIOAuthClient.IsValidUser("kphani@macrosoftindia.com");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidUserByFalseEmail()
        {
            bool result = ASIOAuthClient.IsValidUser("kphani1@macrosoftindia.com");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidUserByCredentials()
        {
            bool result = ASIOAuthClient.IsValidUser("125724pk", "password1");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidUserByFalseCredentials()
        {
            bool result = ASIOAuthClient.IsValidUser("125724pk1", "password1");
            Assert.IsFalse(result);
        }
    }
}
