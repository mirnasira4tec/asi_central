﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public class EmployeeAttendee
    {
        public int Id { get; set; }
        public int AttendeesId { get; set; }
        public int EmployeeId { get; set; }
        public Attendee Attendee { get; set; }
        public Employee Employee { get; set; } 
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
