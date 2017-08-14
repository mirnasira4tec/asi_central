using asi.asicentral.database;
using asi.asicentral.model.counselor;
using asi.asicentral.model.news;
using NUnit.Framework;
using System;
using System.Linq;

namespace asi.asicentral.Tests
{
    [TestFixture]
    [Ignore("Ignore a fixture")]
    public class InternetTest
    {
        [Test]
        [Ignore("Ignore a test")]
        public void LegacyDataTest()
        {
            using (var context = new InternetContext())
            {
                Assert.IsTrue(context.News.Count() > 0);
                News aNews = context.News.FirstOrDefault();
                Assert.IsNotNull(aNews);
                Assert.IsNotNull(aNews.Source);
            }
        }
    }
}
