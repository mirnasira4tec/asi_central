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
        }

        public ShowASI Show { get; set; }
        public IList<ShowAttendee> ShowAttendees { get; set; }
    }
}