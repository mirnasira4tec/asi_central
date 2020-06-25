using asi.asicentral.model;
using asi.asicentral.model.asicentral;
using System.Collections.Generic;

namespace asi.asicentral.web.Models.forms.asicentral
{
    public class FormInstanceModel
    {
        public CompanyInformation Company { get; set; }
        public AsicentralFormInstance AsicentralForm { get; set; }
        public bool IsNewCompany { get; set; } = false;
    }
}