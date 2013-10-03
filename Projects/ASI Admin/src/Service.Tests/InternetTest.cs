using asi.asicentral.database;
using asi.asicentral.model.counselor;
using asi.asicentral.model.news;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace asi.asicentral.Tests
{
    [Ignore]
    public class InternetTest
    {
        [Ignore]
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
