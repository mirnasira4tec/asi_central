using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public class AttendeeSchedule
    {
        public int Id { get; set; }
        public int SupplierAttendeeId { get; set; }
        public int DistributorAttendeeId { get; set; }
        public int ShowScheduleDetailId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public int Team { get; set; } = 1;
        public virtual ShowAttendee SupplierAttendee { get; set; }
        public virtual ShowAttendee DistributorAttendee { get; set; }
        public virtual ShowScheduleDetail ShowScheduleDetail { get; set; }
    }
}
