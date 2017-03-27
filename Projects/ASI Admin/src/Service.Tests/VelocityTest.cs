using asi.asicentral.web.database;
using asi.asicentral.web.Models.velocity;
using asi.asicentral.web.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class VelocityTest 
    {
        [TestMethod]
        public void SupplierUpdateColourMappingTest()
        {
            ColorMapping colorMapping = new ColorMapping();
            colorMapping.CompayId = 9207; //Company Name - ASI 30232
            colorMapping.MappingColor = Guid.NewGuid().ToString();
            colorMapping.BaseColor = Guid.NewGuid().ToString();

            var velocityService = new VelocityContext();
            var velocityColorMapping = new VelocityService(velocityService);

            var isColorMapped = velocityColorMapping.MapColor(colorMapping);

            Assert.IsTrue(isColorMapped);
        }
    }
}
