using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.util;
using Moq;
using System.Web;
using System.Text.RegularExpressions;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class IPHelperTest
    {
        [TestMethod]
        public void CheckIPLookup()
        {
            var session = new Mock<HttpSessionStateBase>();
            session.SetupGet(x => x["IpCountry"]).Returns(string.Empty);
            string country = IPHelper.GetCountry(session.Object, "98.221.206.30");
            Assert.AreEqual("united states", country);
            Assert.IsFalse(IPHelper.IsFromAsia(session.Object, "98.221.206.30"));
            bool isFromAsia = IPHelper.IsFromAsia(session.Object, "1.186.255.255");
            Assert.IsTrue(isFromAsia);
            isFromAsia = false;
            isFromAsia = IPHelper.IsFromAsia(session.Object, "1.2.31.255");
            country = IPHelper.GetCountry(session.Object, "1.2.31.255");
            Assert.IsTrue(isFromAsia);
            Assert.AreEqual("china", country);
        }

        [TestMethod]
        public void LookUpIp_GeoIpNekudoTest()
        {
            var ipLookup = new LookUpIp_GeoIpNekudo();
            var result = ipLookup.GetCountry("42.104.255.255");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LookUpIp_ipstackTest()
        {
            var ipLookup = new LookupIp_ipstack();
            var result = ipLookup.GetCountry("42.104.255.255");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LookUpIpTest()
        {
            var session = new Mock<HttpSessionStateBase>();
            session.SetupGet(x => x["IpCountry"]).Returns(string.Empty);
            var result = IPHelper.GetCountry(session.Object, "42.104.255.255");
            Assert.IsNotNull(result);
        }
    }
}
