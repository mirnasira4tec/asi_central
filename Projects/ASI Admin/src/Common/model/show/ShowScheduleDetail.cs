using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public class ShowScheduleDetail
    {
        public int Id { get; set; }
        public int ShowScheduleId { get; set; }
        public int Day { get; set; }
        public string TimeSchedule { get; set; }
        public bool IsBreak { get; set; }
        public int Sequence { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public virtual ShowSchedule ShowSchedule { get; set; }
        public virtual IList<AttendeeSchedule> AttendeeSchedules { get; set; }
    }
}
