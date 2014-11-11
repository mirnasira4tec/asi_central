using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.Models.forms;
using DotLiquid.Tags;

namespace asi.asicentral.web.Controllers.forms
{
	[Authorize]
	public class FormsController : Controller
    {
		public IStoreService StoreService { get; set; }

		public ActionResult Index(DateTime? dateStart, DateTime? dateEnd, String formTab)
        {
			var viewModel = new FormPageModel();
			IQueryable<FormInstance> formInstanceQuery = StoreService.GetAll<FormInstance>(true);
			if (string.IsNullOrEmpty(formTab)) formTab = FormPageModel.TAB_DATE;
            if (formTab == FormPageModel.TAB_DATE)
            {
                //form uses date filter
                if (dateStart == null) dateStart = DateTime.Now.AddDays(-7);
                if (dateEnd == null) dateEnd = DateTime.Now;
                else dateEnd = dateEnd.Value.Date + new TimeSpan(23, 59, 59);
				DateTime dateStartParam = dateStart.Value.ToUniversalTime();
				DateTime dateEndParam = dateEnd.Value.ToUniversalTime();
	            formInstanceQuery =
		            formInstanceQuery.Where(form => form.CreateDate >= dateStartParam && form.CreateDate <= dateEndParam);
            }
			if (dateStart.HasValue) viewModel.StartDate = dateStart.Value.ToString("MM/dd/yyyy");
			if (dateEnd.HasValue) viewModel.EndDate = dateEnd.Value.ToString("MM/dd/yyyy");
			viewModel.FormTab = formTab;
			viewModel.Forms = formInstanceQuery.ToList();
			viewModel.FormTypes = StoreService.GetAll<FormType>(true).ToList();
			return View("../Forms/Index", viewModel);
        }
    }
}
