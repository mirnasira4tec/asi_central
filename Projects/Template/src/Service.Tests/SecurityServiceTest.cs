using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.services;
using asi.asicentral.interfaces;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class SecurityServiceTest
    {
        [TestMethod]
        public void EncryptDecryptTest()
        {
            ISecurityService securityService = new SecurityService();
            string key = "My Key";
            string test = "Test to encrypt";
            string encryptedTest = securityService.Encrypt(key, test);
            Assert.AreNotEqual(test, encryptedTest);
            Assert.AreEqual(test, securityService.Decrypt(key, encryptedTest));
            try
            {
                securityService.Decrypt(key + "1", encryptedTest);
                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                //expected
            }
            Assert.AreNotEqual(encryptedTest, securityService.Encrypt("other key", test));
        }
    }
}
