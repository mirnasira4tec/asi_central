using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.model.asicentral;
using asi.asicentral.web.Models.forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using asi.asicentral.model;
using asi.asicentral.web.Models.forms.asicentral;
using asi.asicentral.util;

namespace asi.asicentral.web.Controllers.forms
{
    [Authorize]
    public class FormsController : Controller
    {
        public IStoreService StoreService { get; set; }
        public IEmailService EmailService { get; set; }
        public ITemplateService TemplateService { get; set; }
        public IBackendService PersonifyService { get; set; }

        public ActionResult Index(DateTime? dateStart, DateTime? dateEnd, String formTab, string creator, string companyName, string showPendingOnly = null)
        {
            var viewModel = new FormPageModel();
            IQueryable<FormInstance> formInstanceQuery = StoreService.GetAll<FormInstance>(true);
            if (string.IsNullOrEmpty(formTab)) formTab = FormPageModel.TAB_DATE;
            if (formTab == FormPageModel.TAB_DATE)
            {
                //assume first time if no dates specified
                viewModel.ShowPendingOnly = (dateStart == null && dateEnd == null ? true : !string.IsNullOrEmpty(showPendingOnly));
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
            else if (formTab == FormPageModel.TAB_COMPANY && !string.IsNullOrEmpty(companyName))
            {
                if (dateStart == null) dateStart = DateTime.Now.AddDays(-7);
                if (dateEnd == null) dateEnd = DateTime.Now;
                else dateEnd = dateEnd.Value.Date + new TimeSpan(23, 59, 59);
                viewModel.ShowPendingOnly = false;
                formInstanceQuery = formInstanceQuery.Where(form => form.OrderDetail != null && form.OrderDetail.Order != null && form.OrderDetail.Order.Company != null
                 && form.OrderDetail.Order.Company.Name.Contains(companyName));
            }
            if (dateStart.HasValue) viewModel.StartDate = dateStart.Value.ToString("MM/dd/yyyy");
            if (dateEnd.HasValue) viewModel.EndDate = dateEnd.Value.ToString("MM/dd/yyyy");
            if (!string.IsNullOrEmpty(creator)) viewModel.Creator = creator;
            if (!string.IsNullOrEmpty(companyName)) viewModel.CompanyName = companyName;
            viewModel.FormTab = formTab;
            viewModel.Forms = formInstanceQuery.OrderByDescending(form => form.CreateDate).ToList();
            viewModel.FormTypes = StoreService.GetAll<FormType>(true).Where(ft => !ft.IsObsolete && !(ft.Implementation == string.Empty || ft.Implementation == null)).ToList();
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
                    form.Sender = ((System.Security.Principal.WindowsIdentity)System.Web.HttpContext.Current.User.Identity).Name;
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
                    oldForm.Copy(form, "FormsController.PostSendForm");
                    form = oldForm;
                }
                if (model.Command == "Cancel")
                {
                    StoreOrder order = form.CreateOrder(StoreService);
                    if (order.Id == 0) StoreService.Add(order);
                    order.ProcessStatus = OrderStatus.Rejected;
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
                FormType formType = StoreService.GetAll<FormType>(true).SingleOrDefault(fType => fType.Id == model.Form.FormTypeId);
                model.Form.FormType = formType;
                return View("../Forms/SendForm", model);
            }
        }

        public ActionResult FormsDetails(int? id)
        {
            FormInstance instance = null;
            if (id.HasValue)
            {
                instance = StoreService.GetAll<FormInstance>().Where(f => f.Id == id.Value).FirstOrDefault();
            }
            return View("../Forms/FormsDetails", instance);
        }

        #region ASICentral form
        public ActionResult Asicentral(DateTime? dateStart, DateTime? dateEnd, string status = "", string formType = "", string command = "Search")
        {
            var viewModel = _initAsicentralForm();

            var formInstanceQuery = StoreService.GetAll<AsicentralFormInstance>(true).Where(f => !f.FormType.IsDynamic);
            if (!string.IsNullOrEmpty(status))
            {
                status = status.ToLower();
                formInstanceQuery = formInstanceQuery.Where(i => i.Status != null && i.Status.ToLower() == status);
            }
            if (!string.IsNullOrEmpty(formType))
            {
                int id;
                if (Int32.TryParse(formType, out id))
                {
                    formInstanceQuery = formInstanceQuery.Where(i => i.TypeId == id);
                }
            }
            //assume first time if no dates specified
            if (dateStart == null) dateStart = DateTime.Now.AddDays(-1);
            if (dateEnd == null) dateEnd = DateTime.Now;
            else dateEnd = dateEnd.Value.Date + new TimeSpan(23, 59, 59);
            DateTime dateStartParam = dateStart.Value.ToUniversalTime();
            DateTime dateEndParam = dateEnd.Value.ToUniversalTime();
            formInstanceQuery = formInstanceQuery.Where(form => form.CreateDate >= dateStartParam && form.CreateDate <= dateEndParam);
            if (command == "download")
            {
                return _downloadFormCSV(formInstanceQuery);
            }
            else
            {
                if (dateStart.HasValue) viewModel.StartDate = dateStart.Value.ToString("MM/dd/yyyy");
                if (dateEnd.HasValue) viewModel.EndDate = dateEnd.Value.ToString("MM/dd/yyyy");
                viewModel.AsicentralForms = formInstanceQuery.OrderByDescending(form => form.CreateDate).ToList();
                viewModel.AsicentralFormTypes = StoreService.GetAll<AsicentralFormType>(true).Where(ft => !ft.IsObsolete).ToList();
                return View("../Forms/Asicentral", viewModel);
            }
        }
        public ActionResult AsicentralFormsDetails(int? id)
        {
            var form = new FormInstanceModel();
            if (id.HasValue)
            {
                form.AsicentralForm = StoreService.GetAll<AsicentralFormInstance>("FormType;DataValues.Question").Where(f => f.Id == id.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(form.AsicentralForm.CompanyConstituentId))
                {
                    form.Company = PersonifyService.GetPersonifyCompanyInfo(form.AsicentralForm.CompanyConstituentId, 0);
                    //if( form.Company == null)
                    //{
                    //    form.Company = new CompanyInformation() { MasterCustomerId = "112121" };
                    //}
                }
            }
            return View("../Forms/asicentral/FormDetails", form);
        }
        public ActionResult DistributorMembershipApplication(int? id)
        {
            var form = new FormInstanceModel();
            var formQuestions = new FormQuestions();

            if (id.HasValue)
            {
                form.AsicentralForm = StoreService.GetAll<AsicentralFormInstance>("FormType;DataValues.Question;DataValues.Question.QuestionOptions").Where(f => f.Id == id.Value).FirstOrDefault();
                form.FormQuestions= StoreService.GetAll<AsicentralFormQuestion>("QuestionOptions").Where(f => f.FormTypeId == form.AsicentralForm.TypeId).ToList();
                var hasCCSubmit = form != null && form.AsicentralForm.Values != null && form.AsicentralForm.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_HOLDER_NAME) != null;
                form.FormQuestions.Where(q => q.InputType != "None" && q.InputType != "Section").OrderBy(q => q.Sequence);
                form.FormQuestions= form.FormQuestions.OrderBy(q => q.Sequence)
                             .ToList();
                foreach (var question in form.FormQuestions)
                {
                    var data = form.AsicentralForm.DataValues.FirstOrDefault(q => q.QuestionId == question.Id);
                    if (data==null)
                    {
                        form.AsicentralForm.DataValues.Add(new FormDataValue()
                        {
                            InstanceId = Convert.ToInt32(id),
                            Value = "",
                            QuestionId = question.Id,
                            Question = question

                        });
                         
                    }
                }
              //  form.AsicentralForm.DataValues.OrderBy(o => o.Question.Sequence);

            }

            return View("../Forms/asicentral/DistributorMembershipApplication", form);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostAsicentralForm(FormInstanceModel formDetails, string submitBtn)
        {
            var form = formDetails?.AsicentralForm;
            form.FormType = StoreService.GetAll<AsicentralFormType>().FirstOrDefault(t => t.Id == form.TypeId);

            if (form != null && submitBtn.Contains("Create Company"))
            {
                formDetails.Company = _createCompanyInPersonify(form);
                //if (formDetails.Company == null)
                //{
                //    formDetails.Company = new CompanyInformation()
                //    {
                //        Name = "Test COmpany",
                //        Email = "LocalEmail@gmail.com",
                //        Phone = "2122223333",
                //        MasterCustomerId = "123456"
                //    };
                //}
                if (formDetails.Company != null)
                {
                    var dbForm = StoreService.GetAll<AsicentralFormInstance>().FirstOrDefault(i => i.Id == form.Id);
                    dbForm.CompanyConstituentId = formDetails.Company.MasterCustomerId;
                    dbForm.UpdateDate = DateTime.Now;
                    StoreService.SaveChanges();
                    ModelState.Clear();
                    form.CompanyConstituentId = formDetails.Company.MasterCustomerId;
                    formDetails.IsNewCompany = true;
                }
            }
            else if (form != null && !string.IsNullOrEmpty(form.CompanyConstituentId))
            {
                if (form.CompanyConstituentId.Length < 12)
                {
                    form.CompanyConstituentId = form.CompanyConstituentId.PadLeft(12, '0');
                }
                try
                {
                    formDetails.Company = PersonifyService.GetPersonifyCompanyInfo(form.CompanyConstituentId, 0);
                }
                catch (Exception) { }

                if (formDetails.Company == null)
                {
                    TempData["IDErrors"] = $"Constituent Id {form.CompanyConstituentId} doesn't exist in Personify";
                    return View("../Forms/asicentral/FormDetails", formDetails);
                };

                if (submitBtn.Contains("Attach"))
                {
                    try
                    {
                        var creditcard = new CreditCard()
                        {
                            CardHolderName = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_HOLDER_NAME)?.Value,
                            Type = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_TYPE)?.Value,
                            Number = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_NUMBER)?.Value,
                            Address = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_ADDRESS)?.Value,
                            City = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_CITY)?.Value,
                            State = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_STATE)?.Value,
                            PostalCode = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_POSTALCODE)?.Value,
                            Country = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_COUNTRY)?.Value,
                            TokenId = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_TOKEN_ID)?.Value,
                            AuthReference = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_AUTH_REFERENCE)?.Value,
                            ExpirationDate = new DateTime(Int32.Parse(form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_EXP_YEAR)?.Value),
                                                          Int32.Parse(form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_EXP_MONTH)?.Value),
                                                          01),
                            FirstName = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_FIRST_NAME)?.Value,
                            LastName = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_LAST_NAME)?.Value,
                            CompanyName = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_COMPANY)?.Value,
                            RequestToken = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_REQUEST_TOKEN)?.Value,
                            ResponseCode = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_RESPONSE_CODE)?.Value,
                            ResponseMessage = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_RESPONSE_MESSAGE)?.Value,
                            AVS_Result = form.Values.FirstOrDefault(v => v.Name == AsicentralFormValue.CC_AVS_RESULT)?.Value
                        };

                        // attach credit card to the specified company
                        var canadianField = form.Values.FirstOrDefault(v => v.Name == "Is Canadian Membership")?.Value;
                        var isCanada = !string.IsNullOrEmpty(canadianField) && canadianField == "Yes";
                        var ccProfileId = PersonifyService.SaveCreditCard(isCanada ? "ASI Canada" : "ASI",
                                                                          form.CompanyConstituentId,
                                                                          0,
                                                                          creditcard,
                                                                          form.IPAddress,
                                                                          isCanada ? "CAD" : "USD");

                        if (!string.IsNullOrEmpty(ccProfileId))
                        {
                            int id = 0;
                            if (Int32.TryParse(ccProfileId, out id))
                            {
                                var dbForm = StoreService.GetAll<AsicentralFormInstance>().FirstOrDefault(i => i.Id == form.Id);
                                dbForm.CompanyConstituentId = form.CompanyConstituentId;
                                dbForm.CCProfileId = id;
                                dbForm.ApprovedBy = User?.Identity?.ToString();
                                dbForm.UpdateDate = DateTime.Now;
                                dbForm.Status = "Complete";
                                StoreService.SaveChanges();

                                form.CCProfileId = id;
                                form.ApprovedBy = dbForm.ApprovedBy;
                                form.Status = "Complete";
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = $"Could not attach CC to Personify company {form.CompanyConstituentId}";
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = $"Unexpected happened while adding CC info to Personify company, please try it again. \r\n" +
                                              $"Error message: {ex.Message} \r\n Stack Trace: {ex.StackTrace}";
                    }
                }
            }
            else
            {
                TempData["IDErrors"] = "Please provide company constituent Id.";
            }

            return View("../Forms/asicentral/FormDetails", formDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendCCRequest(string toEmail, string mailMessage, string fromEmail, string subject, string formReference)
        {
            if (!string.IsNullOrWhiteSpace(toEmail) && !string.IsNullOrEmpty(mailMessage) && !string.IsNullOrEmpty(fromEmail))
            {
                var message = System.Web.HttpUtility.HtmlEncode(mailMessage);
                var mail = new MailMessage()
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = message.Replace("\r\n", "<br>"),
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true
                };

                var to = toEmail.Split(';');
                foreach (var email in to)
                {
                    if (!string.IsNullOrEmpty(email))
                        mail.To.Add(new MailAddress(email));
                }
                try
                {
                    EmailService.SendMail(mail);
                    TempData["SuccessMessage"] = "Email sent to customer.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Exception while sending customer email.";

                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid from or to email address.";
            }

            if (!string.IsNullOrEmpty(formReference))
            {
                try
                {
                    var formInstance = StoreService.GetAll<AsicentralFormInstance>().FirstOrDefault(t => t.Reference == formReference);
                    formInstance.IsCCRequestSent = true;
                    StoreService.SaveChanges();

                    return new RedirectResult("/Forms/AsicentralFormsDetails/" + formInstance.Id);
                }
                catch
                { }
            }

            return new RedirectResult("/Forms/Asicentral");
        }

        private FormListModel _initAsicentralForm(FormListModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new FormListModel();
            }

            // initialize status list
            viewModel.StatusList = new List<SelectListItem>();
            var allStatus = StoreService.GetAll<AsicentralFormInstance>(true).Where(i => i.Status != null && i.Status != "").Select(i => i.Status).Distinct().ToList();
            foreach (var status in allStatus)
            {
                viewModel.StatusList.Add(new SelectListItem() { Selected = false, Text = status, Value = status });
            }

            // initialize type list
            viewModel.TypeList = new List<SelectListItem>();
            var allTypes = StoreService.GetAll<AsicentralFormType>(true).Where(f => !f.IsDynamic)?.ToList();
            if (allTypes.Any())
            {
                foreach (var type in allTypes)
                {
                    viewModel.TypeList.Add(new SelectListItem() { Selected = false, Text = type.Name, Value = type.Id.ToString() });
                }
            }
            return viewModel;
        }

        private CompanyInformation _createCompanyInPersonify(AsicentralFormInstance form)
        {
            CompanyInformation companyInfo = null;

            var company = new StoreCompany()
            {
                Name = form.Values.FirstOrDefault(v => v.Name == "Company Name")?.Value,
                Email = form.Values.FirstOrDefault(v => v.Name == "Email")?.Value,
                Phone = form.Values.FirstOrDefault(v => v.Name == "Phone")?.Value,
                MemberType = "DISTRIBUTOR"
            };

            var hasShippingAddress = !string.IsNullOrEmpty(form.Values.FirstOrDefault(v => v.Name == "Shipping Address")?.Value);
            company.Addresses = new List<StoreCompanyAddress>()
                {
                    new StoreCompanyAddress()
                    {
                        IsBilling = true,
                        IsShipping = !hasShippingAddress,
                        Address = new StoreAddress()
                        {
                            Street1 = form.Values.FirstOrDefault(v => v.Name == "Billing Address")?.Value,
                            City = form.Values.FirstOrDefault(v => v.Name == "Billing City")?.Value,
                            State = form.Values.FirstOrDefault(v => v.Name == "Billing State")?.Value,
                            Zip = form.Values.FirstOrDefault(v => v.Name == "Billing Zip")?.Value,
                            Country = "USA"
                        }
                    }
                };

            if (hasShippingAddress)
            {
                company.Addresses.Add(new StoreCompanyAddress()
                {
                    IsBilling = false,
                    IsShipping = true,
                    Address = new StoreAddress()
                    {
                        Street1 = form.Values.FirstOrDefault(v => v.Name == "Shipping Address")?.Value,
                        City = form.Values.FirstOrDefault(v => v.Name == "Shipping City")?.Value,
                        State = form.Values.FirstOrDefault(v => v.Name == "Shipping State")?.Value,
                        Zip = form.Values.FirstOrDefault(v => v.Name == "Shipping Zip")?.Value,
                        Country = "USA"
                    }
                });
            }

            company.Individuals = new List<StoreIndividual>()
                {
                    new StoreIndividual()
                    {
                        FirstName = form.Values.FirstOrDefault(v => v.Name == "Name")?.Value,
                        LastName = form.Values.FirstOrDefault(v => v.Name == "Name")?.Value,
                        Phone = form.Values.FirstOrDefault(v => v.Name == "Phone")?.Value,
                        Email = form.Values.FirstOrDefault(v => v.Name == "Email")?.Value,
                        Address = company.Addresses.FirstOrDefault()?.Address,
                        Company = company
                    }
                };

            try
            {
                companyInfo = PersonifyService.CreateCompany(company, "DISTRIBUTOR");
            }
            catch { }

            if (companyInfo == null)
            {
                TempData["ErrorMessage"] = "Failed to create a new company in Personify";
            }

            return companyInfo;
        }
        private ActionResult _downloadFormCSV(IQueryable<AsicentralFormInstance> formInstanceQuery)
        {
            var columnNames = string.Empty;
            var formTypeName = string.Empty;
            var csvString = new StringBuilder();

            if (formInstanceQuery != null && formInstanceQuery.Count() > 0)
            {
                var firstInstance = formInstanceQuery.FirstOrDefault();
                if (firstInstance != null)
                {
                    formTypeName = formInstanceQuery.FirstOrDefault()?.FormType.Name;
                    var isAddImpressionForm = formTypeName == "ASI AD Impression Study";
                    var isNewForm = firstInstance.DataValues != null && firstInstance.DataValues.Count() > 0;
                    if (isNewForm && firstInstance.FormType != null && firstInstance.FormType.FormQuestions != null)
                    {
                        columnNames = string.Join(",", firstInstance.FormType.FormQuestions.OrderBy(q => q.Sequence).Select(q => q.Description));
                    }
                    else
                    {
                        columnNames = string.Join(",", formInstanceQuery.FirstOrDefault().Values.Where(n => n.Name != "IPAddress").Select(n => n.Name));
                        if (isAddImpressionForm)
                        {
                            columnNames = "Date," + columnNames;
                        }
                    }
                    csvString.AppendLine(columnNames);

                    foreach (var instance in formInstanceQuery.ToList())
                    {
                        if (instance.Values != null && instance.Values.Any())
                        {
                            var line = string.Empty;
                            var email = instance.Values?.Where(v => v.Name == "Email").FirstOrDefault()?.Value;
                            if (!string.IsNullOrWhiteSpace(email))
                            {
                                foreach (var value in instance.Values)
                                {
                                    if (value.Name != "IPAddress")
                                    {
                                        var data = Utility.ParseCSVValue(value.Value);
                                        line += data + ",";
                                    }
                                }
                                line = line.Remove(line.Length - 1, 1);
                                if (isAddImpressionForm)
                                {
                                    line = $"{ instance.CreateDate.ToString("MM/dd/yyyy: hh:mm tt")},{line}";
                                }
                                csvString.AppendLine(line);
                            }
                        }
                        else if (instance.DataValues != null && instance.DataValues.Any())
                        {
                            var line = string.Empty;
                            var email = instance.Email;
                            if (!string.IsNullOrWhiteSpace(email))
                            {
                                foreach (var value in instance.DataValues)
                                {
                                    var data = Utility.ParseCSVValue(value.Value);
                                    line += data + ",";
                                }
                                line = line.Remove(line.Length - 1, 1);
                                csvString.AppendLine(line);
                            }
                        }
                    }
                }
            }
            var filename = formTypeName + "_" + DateTime.Now.ToString("MM/dd/yyyy: hh:mm tt") + ".csv";
            return File(new UTF8Encoding().GetBytes(csvString.ToString()), "text/csv", filename);
        }
        #endregion ASICentral form
    }
}
