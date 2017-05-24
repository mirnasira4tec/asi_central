using System;
using asi.asicentral.interfaces;
using asi.asicentral.services;
using System.Collections;
using asi.asicentral.model.ROI;
using System.Collections.Generic;
using NUnit.Framework;

namespace asi.asicentral.Tests
{
    [TestFixture]
    public class ROIServiceTest
    {
        [Test]
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
