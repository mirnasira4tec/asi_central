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
            bool result = ASIOAuthClient.IsValidEmail("kphani@macrosoftindia.com");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidEmail()
        {
            bool result = ASIOAuthClient.IsValidEmail("kphani1@macrosoftindia.com");
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

        [TestMethod]
        public void GetUserBySSO()
        {
            asi.asicentral.model.User result = ASIOAuthClient.GetUser(168793);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUserByToken()
        {
            asi.asicentral.model.User result = ASIOAuthClient.GetUser("endfOr-JCWaCHazhs25cMHS1N4ddMqjV7jqjgMi62_m4ifiU19TLfnOOUfzOXIvQUli25TFs3xAF8AVXp6sxSTikZaM1");
            Assert.IsNull(result);
        }
    }
}
