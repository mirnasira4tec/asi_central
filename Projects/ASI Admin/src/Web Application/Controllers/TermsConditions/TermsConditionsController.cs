using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.services;
using asi.asicentral.web.Models.TermsConditions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers.TermsConditions
{
    public class TermsConditionsController : Controller
    {
        public IStoreService StoreService { get; set; }
        public IEmailService EmailService { get; set; }
        public ITemplateService TemplateService { get; set; }

        #region Actions for Terms and Conditions Instances 
        public ActionResult Index()
        {
            var viewModelList = new List<TermsConditionsInstanceVM>();
            try
            {
                // List all terms and conditions instances for last 7 days
                var startDate = DateTime.Now.AddDays(-7);
                var modelList = StoreService.GetAll<TermsConditionsInstance>(true)
                                            .Where(t => t.CreateDate >= startDate && t.DateAgreedOn == null)
                                            .ToList(); //ToList is necessary here to get TermsConditionsType object
                foreach (var model in modelList)
                {
                    viewModelList.Add(model.ToViewModel());
                }
            }
            catch (Exception ex) 
            { 
                TempData["Message"] = "Error: " + ex.Message; 
            }

            return View("../TermsConditions/Index", viewModelList);
        }

        public ActionResult Add()
        {
            return Edit(string.Empty);
        }

        public ActionResult Edit(string guid)
        {
            TermsConditionsInstanceVM viewModel = null;
            try
            {
                if (!string.IsNullOrEmpty(guid))
                {
                    var termsModel = StoreService.GetAll<TermsConditionsInstance>(true)
                                                 .Where(t => t.GUID == guid)
                                                 .ToList();
                    if (termsModel != null && termsModel.Count > 0)
                    {
                        viewModel = termsModel[0].ToViewModel();

                        //add all TypeIds
                        for (int i = 1; i < termsModel.Count; i++)
                        {
                            if( termsModel[i].TermsAndConditions != null)
                                viewModel.TypesWithSameGuid.Add(termsModel[i].TermsAndConditions.Name);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid Terms and Conditions GUID.");
                    }
                }
                else
                    viewModel = new TermsConditionsInstanceVM();

                viewModel.TermList = StoreService.GetAll<TermsConditionsType>(true)
                                                 .Where(t => t.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
            }
            return View("../TermsConditions/Edit", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string btnSubmit, TermsConditionsInstanceVM model, int[] termsConditionsList)
        {
            try
            {
                if (ModelState.IsValid && termsConditionsList != null && termsConditionsList.Length > 0 )
                {
                    int newTermsStartIndex = 0;
                    var emailTermsList = new List<TermsConditionsInstance>();
                    var user = ControllerContext.HttpContext.User.Identity.Name.Replace(@"ASINETWORK\", "");
                    model.LastUpdatedBy = user;
                    model.UpdateSource = "ASI Admin TermsConditionsController - Edit";

                    if (model.Id == 0)
                    {
                        model.CreatedBy = user;
                        model.GUID = System.Guid.NewGuid().ToString();
                    }
                    else
                    { 
                        var origInstList = StoreService.GetAll<TermsConditionsInstance>(true)
                                                       .Where( t => t.GUID == model.GUID)
                                                       .ToList();
                        int numOfUpdates = Math.Min(termsConditionsList.Length, origInstList.Count);

                        // update existing instances with selected terms and conditions type
                        for (int i = 0; i < numOfUpdates; i++)
                        {
                            var curTermsInst = model.ToDataModel();
                            // use instance Id in database
                            curTermsInst.Id = origInstList[i].Id;
                            curTermsInst.TypeId = termsConditionsList[i];
                            StoreService.Update<TermsConditionsInstance>(curTermsInst);

                            emailTermsList.Add(curTermsInst);
                            newTermsStartIndex++;
                        }

                        // delete existing instances if fewer types selected
                        if (origInstList.Count > termsConditionsList.Length)
                        {
                            for (int j = numOfUpdates; j < origInstList.Count; j++)
                            {
                                StoreService.Delete<TermsConditionsInstance>(origInstList[j]);
                            }
                        }
                    }

                    // create instances for all selected types if new,
                    // otherwise add additional instances if more types selected
                    for (int i = newTermsStartIndex; i < termsConditionsList.Length; i++)
                    {
                        var newTerms = model.ToDataModel();
                        newTerms.TypeId = termsConditionsList[i];
                        StoreService.Add<TermsConditionsInstance>(newTerms);
                        emailTermsList.Add(newTerms);
                    }

                    StoreService.SaveChanges();  
   
                    // send email to customer
                    if (btnSubmit == "Send" && emailTermsList.Count > 0)
                    {
                        string emailBody = TemplateService.Render("asi.asicentral.web.Views.Emails.TermsConditionsEmail.cshtml", model);
                        var mail = new MailMessage();
                        mail.Subject = "You have Terms and Conditions to be accepted";
                        mail.Body = emailBody;
                        mail.BodyEncoding = Encoding.UTF8;
                        mail.IsBodyHtml = true;
                        mail.To.Add(new MailAddress(model.CustomerEmail));

                        EmailService.SendMail(mail);
                    }
                }
                else
                {
                    model.TermList = StoreService.GetAll<asi.asicentral.model.store.TermsConditionsType>(true)
                                                                       .Where(t => t.IsActive == true).ToList();

                    string errorMessage = string.Empty;

                    if (termsConditionsList == null || termsConditionsList.Length < 1)
                    {
                        errorMessage += "Please select at least one terms and conditions.";
                    }

                    if (!ModelState.IsValid)
                    {
                        var modelStateErrors = ModelState.Values.SelectMany(m => m.Errors);
                        errorMessage += string.Join(". ", modelStateErrors.Select(m => m.ErrorMessage));
                    }

                    TempData["Message"] = "Error: " + errorMessage;

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error: " + ex.Message;
                LogService log = LogService.GetLog(this.GetType());
                log.Error(ex.Message + " " + ex.StackTrace);
            }

            return RedirectToAction("Index");
        }

        public JsonResult Search(string startDate, string endDate, string creator, bool? showOnlyPending,
                             string customerName, string customerEmail, string company)
        {
            var termInstList = new List<TermsConditionsInstanceVM>();
            try
            {
                var modelList = StoreService.GetAll<TermsConditionsInstance>(true);
                if (!string.IsNullOrEmpty(startDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(startDate, out date))
                        modelList = modelList.Where(t => t.CreateDate >= date);
                }

                if (!string.IsNullOrEmpty(endDate))
                {
                    DateTime date;
                    if (DateTime.TryParse(endDate, out date))
                        modelList = modelList.Where(t => t.CreateDate <= date);
                }

                if (!string.IsNullOrEmpty(creator))
                {
                    modelList = modelList.Where(t => t.CreatedBy == creator);
                }

                if (showOnlyPending != null && showOnlyPending.Value)
                {
                    modelList = modelList.Where(t => t.DateAgreedOn == null);
                }

                if (!string.IsNullOrEmpty(customerName))
                {
                    modelList = modelList.Where(t => t.CustomerName == customerName);
                }

                if (!string.IsNullOrEmpty(customerEmail))
                {
                    modelList = modelList.Where(t => t.CustomerEmail == customerEmail);
                }

                if (!string.IsNullOrEmpty(company))
                {
                    modelList = modelList.Where(t => t.CompanyName == company);
                }

                foreach (var model in modelList.ToList())
                {
                    termInstList.Add(model.ToViewModel());
                }
            }
            catch (Exception) { }

            return Json(termInstList.OrderByDescending(t => t.CreateDate));
        }

        public ActionResult OrderDetail(int id)
        {
            if (id != 0)
            {
                try
                {
                    var order = StoreService.GetAll<StoreOrder>(true)
                                            .Where(o => o.Id == id).FirstOrDefault();

                    if (order != null && order.OrderDetails != null && order.OrderDetails.Count > 0)
                    {
                        int orderDetailId = order.OrderDetails[0].Id;
                        return Redirect("/Store/Application/Edit/" + orderDetailId);
                    }

                    TempData["Message"] = string.Format("The order {1} has no details provided. ", id.ToString());
                }
                catch (Exception ex) 
                {
                    TempData["Message"] = "Error: " + ex.Message;
                }
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Terms and Conditions Types tab
        // show specified terms and conditions in modal dialog
        public ActionResult Type(int id)
        {
            TermsConditionsType term = null;
            try
            {
                term = StoreService.GetAll<TermsConditionsType>()
                                       .Where(t => t.Id == id)
                                       .FirstOrDefault();
            }
            catch (Exception) { }

            return PartialView("../TermsConditions/Terms&Conditions", term);
        }

        // list types for all Terms and Conditions
        public ActionResult Types()
        {
            var viewModelList = new List<TermsConditionsTypeVM>();
            try
            {
                var modelList = StoreService.GetAll<TermsConditionsType>(true);
                foreach (var model in modelList)
                {
                    viewModelList.Add(model.ToViewModel());
                }
            }
            catch (Exception) { }

            return View("../TermsConditions/Types", viewModelList);
        }
        #endregion
    }
}
