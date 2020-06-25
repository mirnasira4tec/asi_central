using asi.asicentral.model.asicentral;
using System.Collections.Generic;
using System.Web.Mvc;

namespace asi.asicentral.web.Models.forms.asicentral
{
    public class FormListModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        public string FormType { get; set; }
        public IList<AsicentralFormInstance> AsicentralForms { get; set; }
        public IList<AsicentralFormType> AsicentralFormTypes { get; set; }
        public IList<SelectListItem> StatusList { get; set; }
        public IList<SelectListItem> TypeList { get; set; }

    }
}