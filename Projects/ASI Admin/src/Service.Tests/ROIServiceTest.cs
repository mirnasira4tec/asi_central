using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.interfaces;
using asi.asicentral.services;
using System.Collections;
using asi.asicentral.model.ROI;
using System.Collections.Generic;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class ROIServiceTest
    {
        [TestMethod]
        public void GetImpressionsPerCategory()
        {
            IROIService roiService = new ROIService();
            int count = 0;
            IEnumerable<Category> categories = roiService.GetImpressionsPerCategory(76575);
            foreach (Category cat in categories) 
            {
                count++;
            }
            Assert.IsTrue(count > 0);
        }
    }
}
