using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.Models.forms
{
    public class FormModel
    {
        public FormInstance Form { get; set; }
        public string Command { get; set; }
    }
}