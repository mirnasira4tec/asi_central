using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.interfaces;
using asi.asicentral.services;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class ROIServiceTest
    {
        [TestMethod]
        public void GetImpressionsPerCategory()
        {
            IROIService roiService = new ROIService();
            Assert.IsTrue(roiService.GetImpressionsPerCategory(12345).Length > 0);
        }
    }
}
