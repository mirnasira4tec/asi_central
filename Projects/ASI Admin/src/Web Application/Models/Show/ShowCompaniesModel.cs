using asi.asicentral.model.show;
using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.models.show
{
    public class ShowCompaniesModel 
    {
        public ShowCompaniesModel()
        {
            ShowAttendees = new List<ShowAttendee>();
            ShowEmployees = new List<EmployeeAttendance>();
        }

        public ShowASI Show { get; set; }
        public IList<ShowAttendee> ShowAttendees { get; set; }
        public IList<EmployeeAttendance> ShowEmployees { get; set; }
    }

    public class EmployeeAttendance    
    {
        public ShowEmployee Employee { get; set; }
        public bool IsAttending { get; set; }
    }
}