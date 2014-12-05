using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using asi.asicentral.interfaces;
using asi.asicentral.model;
using asi.asicentral.model.store;
using asi.asicentral.web.Models.forms;
using System.Text;

namespace asi.asicentral.web.Controllers.forms
{
	[Authorize]
	public class FormsController : Controller
    {
		public IStoreService StoreService { get; set; }
		public IEmailService EmailService { get; set; }
        public ITemplateService TemplateService { get; set; }

        public ActionResult Index(DateTime? dateStart, DateTime? dateEnd, String formTab, string creator)
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
                if (!string.IsNullOrEmpty(creator)) 
                    formInstanceQuery = formInstanceQuery.Where(form => form.Sender.Contains(creator));

            }
			if (dateStart.HasValue) viewModel.StartDate = dateStart.Value.ToString("MM/dd/yyyy");
			if (dateEnd.HasValue) viewModel.EndDate = dateEnd.Value.ToString("MM/dd/yyyy");
	        if (!string.IsNullOrEmpty(creator)) viewModel.Creator = creator;
			viewModel.FormTab = formTab;
            viewModel.Forms = formInstanceQuery.OrderByDescending(form => form.CreateDate).ToList();
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
				Greetings = "Here is the order we discussed together. Please review and let me know if you have any issues.\n\nThank you.",
            };
            var model = new FormModel
            {
                Form = form,
                Command = "Send",
            };
            return View("../Forms/SendForm", model);
        }

		public ActionResult SendForm(int id)
		{
			FormInstance instance = StoreService.GetAll<FormInstance>(true).SingleOrDefault(form => form.Id == id);
            var model = new FormModel
            {
                Command = "Send",
                Form = instance,
            };
			return View("../Forms/SendForm", model);
		}

		[ValidateAntiForgeryToken]
		public ActionResult PostSendForm(FormModel model)
		{
			if (ModelState.IsValid)
			{
                var form = model.Form;
				//save the form in the database
				if (string.IsNullOrEmpty(form.ExternalReference))
				{
					//new one
					form.ExternalReference = Guid.NewGuid().ToString();
					form.CreateDate = DateTime.UtcNow;
					form.UpdateDate = form.CreateDate;
					form.UpdateSource = "FormsController.PostSendForm";
					form.Sender = ((System.Security.Principal.WindowsIdentity) System.Web.HttpContext.Current.User.Identity).Name;
					form.Sender = form.Sender.Replace(@"ASINETWORK\", "");
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
                    //copying the fields values
                    oldForm.Comments = form.Comments;
                    oldForm.Email = form.Email;
                    oldForm.Greetings = form.Greetings;
                    oldForm.NotificationEmails = form.NotificationEmails;
                    oldForm.Salutation = form.Salutation;
                    oldForm.Total = form.Total;
                    oldForm.UpdateDate = DateTime.UtcNow;
                    oldForm.UpdateSource = "FormsController.PostSendForm";
                    //copying the form values
					oldForm.Values = oldForm.Values.OrderBy(value => value.Sequence).ToList();
					for (int i = 0; i < oldForm.Values.Count; i++)
					{
						oldForm.Values[i].Value = form.Values[i].Value;
						oldForm.Values[i].UpdateDate = DateTime.UtcNow;
						oldForm.Values[i].UpdateSource = oldForm.UpdateSource;
					}
				}
				StoreService.SaveChanges();
                if (model.Command == "Send")
                {
                    //email the customer
                    form.FormType = StoreService.GetAll<FormType>(true).SingleOrDefault(f => f.Id == form.FormTypeId);
                    string emailBody = TemplateService.Render("asi.asicentral.web.Views.Emails.FormSentEmail.cshtml", form);
                    MailMessage mail = new MailMessage();
                    string to = form.Email;
                    mail.To.Add(new MailAddress(to));
                    mail.Subject = "You have an order for " + form.FormType.Name + " waiting to be reviewed";
                    mail.Body = emailBody;
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.IsBodyHtml = true;
                    EmailService.SendMail(mail);
                }
				return new RedirectResult("/Forms/Index");
			}
			else
			{
				return View("../Forms/SendForm", model);
			}
		}
	}
}
