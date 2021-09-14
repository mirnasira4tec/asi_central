using asi.asicentral.web.database;
using asi.asicentral.web.Models.velocity;
using asi.asicentral.web.Service;
using NUnit.Framework;
using System;

namespace asi.asicentral.Tests
{
   [TestFixture]
    public class VelocityTest 
    {
        [Test]
        [Ignore("Ignore a test")]
        public void SupplierUpdateColourMappingTest()
        {
            ColorMapping colorMapping = new ColorMapping();
            colorMapping.CompayId = 9207; //Company Name - ASI 30232
            colorMapping.ColorGroup = Guid.NewGuid().ToString();
            colorMapping.SupplierColor = Guid.NewGuid().ToString();

            var velocityService = new VelocityContext();
            var velocityColorMapping = new VelocityService(velocityService);

            var isColorMapped = velocityColorMapping.MapColor(colorMapping);

            Assert.IsTrue(isColorMapped);
        }
    }
}
