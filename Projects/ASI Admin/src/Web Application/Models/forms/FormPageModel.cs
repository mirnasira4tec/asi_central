using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.model.store;

namespace asi.asicentral.web.Models.forms
{
	public class FormPageModel
	{
		public const String TAB_DATE = "date";
		public const String TAB_COMPANY = "company";
		public String FormTab { get; set; }
		public string StartDate { get; set; }
		public string EndDate { get; set; }
        public string Creator { get; set; }
		public string CompanyName { get; set; }
		public bool ShowPendingOnly { get; set; }
		public IList<FormInstance> Forms { get; set; }
		public IList<FormType> FormTypes { get; set; } 
	}
}