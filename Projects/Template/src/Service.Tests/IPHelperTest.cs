using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.util;
using Moq;
using System.Web;

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
            Assert.AreEqual("UNITED STATES", country);
            Assert.IsFalse(IPHelper.IsFromAsia(session.Object, "98.221.206.30"));
        }
    }
}
