using System;
using System.Collections.Generic;

namespace asi.asicentral.model.show
{
    public class ShowFormInstance
    {
        public ShowFormInstance()
        {
            if (this.GetType() == typeof(ShowFormInstance))
            {
                PropertyValues = new List<ShowFormPropertyValue>();
            }
        }

        public int InstanceId { get; set; }
        
        public int TypeId { get; set; }

        public string Email { get; set; }

        public int? AttendeeId { get; set; }

        public int? EmployeeAttendeeId { get; set; }

        public string RequestReference { get; set; }

        public string Identity { get; set; }
        
        public string SenderIP { get; set; }        

        public bool SubmitSuccessful { get; set; }
        
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }


        public virtual ShowFormType FormType { get; set; }
        public virtual IList<ShowFormPropertyValue> PropertyValues { get; set; }
        public virtual ShowAttendee Attendee { get; set; }
        public virtual ShowEmployeeAttendee EmployeeAttendee { get; set; }
    }
}
