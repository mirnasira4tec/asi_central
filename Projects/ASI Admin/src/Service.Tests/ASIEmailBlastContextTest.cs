﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.database;
using asi.asicentral.model.store;
using System.Linq;

namespace asi.asicentral.Tests
{

    [TestClass]
    public class ASIEmailBlastContextTest
    {

        [TestMethod]
        public void ClosedCampaignDateTest()
        {
            using (var context = new ASIEmailBlastContext())
            {
                var closedDate1 = context.ClosedCampaignDates;
                var closedDate2 = context.ClosedCampaignDates.FirstOrDefault();
                var closedDate3 = context.ClosedCampaignDates
                    .Where(date1 => date1.Reactivated == true || date1.Reactivated == false)
                    .Select(date2 => date2);

                Assert.IsTrue(context.ClosedCampaignDates.Count() > 0);
                Assert.IsTrue(closedDate1.Count() == closedDate3.Count());
                Assert.IsNotNull(closedDate2);
                Assert.IsNotNull(closedDate2.Reactivated);
            }
        }
    }
}