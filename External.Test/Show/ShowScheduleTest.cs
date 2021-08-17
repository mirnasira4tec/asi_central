using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.database.mappings;
using NUnit.Framework;
using StructureMap.Configuration.DSL;
using asi.asicentral.interfaces;
using asi.asicentral.services;
using asi.asicentral.model.show;
using asi.asicentral.util.show;
using asi.asicentral.model.store;

namespace External.Test.Show
{
    [TestFixture]
    public class ShowScheduleTest
    {
        Random rand = new Random();
        private int _SHOWID = 207;
        private ObjectService InitializeObjectService()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            return new ObjectService(container);
        }

        [Test]
        public void AddAttendeeSchedule()
        {
            using (var objectService = InitializeObjectService())
            {
                var show = objectService.GetAll<ShowASI>().Where(s => s.Id == _SHOWID).FirstOrDefault();
                if (show != null && show.ShowScheduleId.HasValue)
                {
                    var attendees = objectService.GetAll<ShowAttendee>(true).Where(attendee => attendee.ShowId == _SHOWID).ToList();
                    var scheduleDetail = objectService.GetAll<ShowScheduleDetail>().Where(q => q.ShowScheduleId == show.ShowScheduleId && !q.IsBreak).OrderBy(q => q.Sequence).FirstOrDefault();
                    if (scheduleDetail != null)
                    {                        
                        var supplier = attendees.Where(q => q.Company.MemberType.ToLower() == "supplier").FirstOrDefault();
                        var distributor = attendees.Where(q => q.Company.MemberType.ToLower() == "distributor").FirstOrDefault();
                        if (supplier != null && distributor != null)
                        {
                            var attendeeSchedule = new AttendeeSchedule()
                            {
                                SupplierAttendeeId = supplier.Id,
                                DistributorAttendeeId = distributor.Id,
                                ShowScheduleDetailId = scheduleDetail.Id,
                                CreateDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                                UpdateSource = "TestCase"
                            };
                            objectService.Add<AttendeeSchedule>(attendeeSchedule);
                            objectService.SaveChanges();
                            Assert.AreNotEqual(attendeeSchedule.Id, 0);

                            #region cleanUp

                            objectService.Delete<AttendeeSchedule>(attendeeSchedule);
                            objectService.SaveChanges();
                            #endregion
                        }
                    }
                }
            }

        }
    }
}
