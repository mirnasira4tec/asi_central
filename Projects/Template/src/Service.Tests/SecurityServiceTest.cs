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
            IEncryptionService securityService = new EncryptionService();
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

        [TestMethod]
        public void LegacyDecrypt()
        {
            string key = "mk8$3njkl";
            IEncryptionService securityService = new EncryptionService();
            string encrypted = "AYpfTHTjNgVpB7LOD6r4rebWBNYDxhvYrp3A4PeJlPU=";
            string text = securityService.LegacyDecrypt(key, encrypted);
            Assert.AreNotEqual(encrypted, text);
            encrypted = "UEvdTgcTlJfkk+1TW6Ilco1fA+I2I+pN/+hk1IkEYjk=";
            Assert.AreNotEqual(encrypted, text);
            text = securityService.LegacyDecrypt("crap", encrypted);
            Assert.AreEqual(encrypted, text);
            text = securityService.LegacyDecrypt(key, "crap");
            Assert.AreEqual("crap", text);
        }
    }
}
