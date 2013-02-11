using asi.asicentral.database;
using asi.asicentral.model.counselor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class ASIPublicationTest
    {
        [TestMethod]
        public void LegacyDataTest()
        {
            using (var context = new ASIPublicationContext())
            {
                CounselorFeature feature = context.Features.Where(feat => feat.Id == 1).SingleOrDefault();
                Assert.IsNotNull(feature);
                CounselorContent content = feature.Content;
                Assert.IsNotNull(content);
                Assert.IsTrue(content.Categories.Count > 0);
                CounselorFeatureRotator featureRotator = context.FeatureRotators.FirstOrDefault();
                Assert.IsNotNull(featureRotator);
                content = feature.Content;
                Assert.IsNotNull(content);
                Assert.IsTrue(content.Categories.Count > 0);
            }
        }
    }
}
