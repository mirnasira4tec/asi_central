using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model;
using asi.asicentral.model.store;
using asi.asicentral.web.Models.forms;

namespace asi.asicentral.web.Controllers.forms
{
	[Authorize]
	public class FormsController : Controller
    {
		public IStoreService StoreService { get; set; }
		public IEmailService EmailService { get; set; }

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

        public ActionResult AddForm(int id)
        {
            FormType formType = StoreService.GetAll<FormType>(true).SingleOrDefault(fType => fType.Id == id);
            if (formType == null) throw new Exception("Invalid FormType identifier: " + id);
            var form = new FormInstance
            {
                FormType = formType,
            };
            return View("../Forms/SendForm", form);
        }

		public ActionResult SendForm(int id)
		{
			FormInstance instance = StoreService.GetAll<FormInstance>(true).SingleOrDefault(form => form.Id == id);
			return View("../Forms/SendForm", instance);
		}

		[ValidateAntiForgeryToken]
		public ActionResult PostSendForm(FormInstance form)
		{
			if (ModelState.IsValid)
			{
				//save the form in the database
				if (string.IsNullOrEmpty(form.ExternalReference))
				{
					//new one
					form.ExternalReference = Guid.NewGuid().ToString();
					form.CreateDate = DateTime.UtcNow;
					form.UpdateDate = form.CreateDate;
					form.UpdateSource = "FormsController.PostSendForm";
					form.Sender = ((System.Security.Principal.WindowsIdentity) System.Web.HttpContext.Current.User.Identity).Name;
					int i = 0;
					foreach (var value in form.Values)
					{
						value.CreateDate = form.CreateDate;
						value.UpdateDate = form.CreateDate;
						value.UpdateSource = form.UpdateSource;
						value.Sequence = i;
						i++;
					}
					StoreService.Add(form);
				}
				else
				{
					//edit an existing one
					var oldForm = StoreService.GetAll<FormInstance>().SingleOrDefault(f => f.ExternalReference == form.ExternalReference);
					if (oldForm == null) throw new Exception("invalid form reference");
					oldForm.Values = oldForm.Values.OrderBy(value => value.Sequence).ToList();
					oldForm.UpdateDate = DateTime.UtcNow;
					oldForm.UpdateSource = "FormsController.PostSendForm";
					for (int i = 0; i < oldForm.Values.Count; i++)
					{
						oldForm.Values[i].Value = form.Values[i].Value;
						oldForm.Values[i].UpdateDate = DateTime.UtcNow;
						oldForm.Values[i].UpdateSource = oldForm.UpdateSource;
					}
				}
				StoreService.SaveChanges();
				//email the customer
				var mail = new Mail
				{
					Body = "You have an order to review, <a href=\"\">click here to continue</a>",
					Subject = "You have an order waiting to be reviewed",
					To = form.Email,
				};
				EmailService.SendMail(mail);
				return new RedirectResult("/Forms/Index");
			}
			else
			{
				return View("../Forms/SendForm", form);
			}
		}
	}
}
