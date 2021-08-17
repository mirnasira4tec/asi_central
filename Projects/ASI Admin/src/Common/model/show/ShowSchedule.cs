using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public class ShowSchedule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public string Breaks { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public virtual IList<ShowScheduleDetail> ShowScheduleDetails { get; set; }
    }
}
