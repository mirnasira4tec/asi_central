using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public class ShowASI
    {
        public ShowASI()
        {
            if (this.GetType() == typeof(ShowASI))
            {
                Attendees = new List<ShowAttendee>();
            }
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End Date is required")]
        public DateTime EndDate { get; set; }
        public int? ShowTypeId { get; set; }
        public virtual ShowType ShowType { get; set; }
        public string Address { get; set; } 
        public virtual IList<ShowAttendee> Attendees { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
