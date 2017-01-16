using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using asi.asicentral.model.excit;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using asi.asicentral.services;

namespace asi.asicentral.web.Controllers.asicentralApi
{
    public class AsiCentralApiController : Controller
    {
        //
        // GET: /asicentralApi/
        public IObjectService ObjectService { get; set; }
        public ActionResult SupplierUpdateRequestList(string requestStatus)
        {
            IList<SupUpdateRequest> objList = ObjectService.GetAll<SupUpdateRequest>().OrderByDescending(item => item.CreateDate).ToList();
            if (!string.IsNullOrEmpty(requestStatus))
            {
                SupRequestStatus status = requestStatus == "0" ? SupRequestStatus.Pending : requestStatus == "1" ? SupRequestStatus.Approved : requestStatus == "2" ? SupRequestStatus.Rejected : requestStatus == "3" ? SupRequestStatus.Cancelled : SupRequestStatus.Pending;
                objList = ObjectService.GetAll<SupUpdateRequest>().Where(item => item.Status == status).OrderByDescending(item => item.CreateDate).ToList();
            }
            return View(objList);
        }
        [HttpGet]
        public ActionResult SupplierUpdateRequestDetail(int id)
        {
            SupUpdateRequest objSupUpdateRequest = ObjectService.GetAll<SupUpdateRequest>().FirstOrDefault(item => item.Id == id);
            return View(objSupUpdateRequest);
        }

        [HttpPost]
        public ActionResult EditServicesData(string command, int supUpdateRequestId, SupUpdateRequest model)
        {
            SupUpdateRequest supUpdateRequest = null;
            var isProduction = ConfigurationManager.AppSettings["IsProduction"];
            if (supUpdateRequestId != 0)
            {
                supUpdateRequest = ObjectService.GetAll<SupUpdateRequest>().FirstOrDefault(detail => detail.Id == supUpdateRequestId);
            }
            try
            {
                if (command == "Accept")
                {
                    Task.Run(async () =>
                    {
                        if (supUpdateRequest != null)
                        {
                            await UpdateStageConfigValue(supUpdateRequest);
                            if (string.IsNullOrEmpty(isProduction) &&  isProduction == "true")
                            {
                                await UpdateProdConfigValue(supUpdateRequest);
                            }
                        }
                    }).Wait();
                }
                else if (command == "Save")
                {
                    if (model != null)
                    {
                        IList<SupUpdateRequestDetail> requestFields = ObjectService.GetAll<SupUpdateRequestDetail>().ToList();
                        foreach (var item in model.RequestDetails)
                        {
                            var requestField = requestFields.FirstOrDefault(m => m.Id == item.Id);
                            if (requestField != null)
                            {
                                requestField.UpdateValue = item.UpdateValue;
                            }
                        }
                        ObjectService.SaveChanges();
                    }
                    return RedirectToAction("SupplierUpdateRequestDetail", new
                    {
                        id = supUpdateRequestId
                    });
                }

                if (supUpdateRequest != null && command != "Save")
                {
                    supUpdateRequest.Status = command == "Accept" ? SupRequestStatus.Approved : SupRequestStatus.Rejected;
                    supUpdateRequest.ApprovedBy = ((System.Security.Principal.WindowsIdentity)System.Web.HttpContext.Current.User.Identity).Name;
                    supUpdateRequest.UpdateDate = DateTime.UtcNow;
                    ObjectService.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogService log = LogService.GetLog(this.GetType());
                log.Error(ex.Message);
            }

            return RedirectToAction("SupplierUpdateRequestList");
        }

        private async Task UpdateStageConfigValue(SupUpdateRequest supUpdateRequest)
        {
            var excitUrlTest = ConfigurationManager.AppSettings["ExcitUrlTest"];
            var url = string.Format("{0}v1/Suppliers/{1}/config?client=QA", excitUrlTest, supUpdateRequest.CompanyId);
            var headerParams = new Dictionary<string, string>();
            headerParams.Add("contenttype", "application/json");

            var stageConfig = JsonConvert.DeserializeObject<ASI.Contracts.Excit.Supplier.Version1.Configuration>(await asi.asicentral.util.HtmlHelper.SubmitWebRequestAsync(url, headerParams, null, false, true));
            IList<SupUpdateRequestDetail> supUpdateRequestDetails = ObjectService.GetAll<SupUpdateRequestDetail>().Where(item => item.SupUpdateRequestId == supUpdateRequest.Id).ToList();
            foreach (var item in supUpdateRequestDetails)
            {
                if (item.UpdateField.Name == "AccountNoTest")
                {
                    stageConfig.AccountNumber = item.UpdateValue ?? item.OrigValue;
                }
                else if (item.UpdateField.Name == "UserNameTest")
                {
                    stageConfig.Username = item.UpdateValue ?? item.OrigValue;
                }
                else if (item.UpdateField.Name == "PasswordTest")
                {
                    stageConfig.Password = item.UpdateValue ?? item.OrigValue;
                }
                else if (item.UpdateField.Name == "InventoryUrlTest")
                {
                    stageConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.Inventory].Url = item.UpdateValue;
                }
                else if (item.UpdateField.Name == "LoginValidateUrlTest")
                {
                    stageConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.LoginValidate].Url = item.UpdateValue;
                }
                else if (item.UpdateField.Name == "OrderCreateUrlTest")
                {
                    stageConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.OrderCreation].Url = item.UpdateValue;
                }
                else if (item.UpdateField.Name == "OrderStatusUrlTest")
                {
                    stageConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.OrderStatus].Url = item.UpdateValue;
                }
                else if (item.UpdateField.Name == "LoginInstructionTest")
                {
                    stageConfig.LoginInstruction = item.UpdateValue ?? item.OrigValue;
                }
            }
            asi.asicentral.util.HtmlHelper.SubmitWebRequest(url, headerParams, Newtonsoft.Json.JsonConvert.SerializeObject(stageConfig), true, true);
        }

        private async Task UpdateProdConfigValue(SupUpdateRequest supUpdateRequest)
        {
            var excitUrlProd = ConfigurationManager.AppSettings["ExcitUrlProd"];
            var urlProd = string.Format("{0}v1/Suppliers/{1}/config?client=QA", excitUrlProd, supUpdateRequest.CompanyId);
            var headerParams = new Dictionary<string, string>();
            headerParams.Add("contenttype", "application/json");

            var prodConfig = JsonConvert.DeserializeObject<ASI.Contracts.Excit.Supplier.Version1.Configuration>(await asi.asicentral.util.HtmlHelper.SubmitWebRequestAsync(urlProd, headerParams, null, false, true));
            IList<SupUpdateRequestDetail> supUpdateRequestDetails = ObjectService.GetAll<SupUpdateRequestDetail>().Where(item => item.SupUpdateRequestId == supUpdateRequest.Id).ToList();
            foreach (var item in supUpdateRequestDetails)
            {
                if (item.UpdateField.Name == "AccountNoProd")
                {
                    prodConfig.AccountNumber = item.UpdateValue ?? item.OrigValue;
                }
                else if (item.UpdateField.Name == "UserNameProd")
                {
                    prodConfig.Username = item.UpdateValue ?? item.OrigValue;
                }
                else if (item.UpdateField.Name == "PasswordProd")
                {
                    prodConfig.Password = item.UpdateValue ?? item.OrigValue;
                }
                else if (item.UpdateField.Name == "InventoryUrlProd")
                {
                    prodConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.Inventory].Url = item.UpdateValue;
                }
                else if (item.UpdateField.Name == "LoginValidateUrlProd")
                {
                    prodConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.LoginValidate].Url = item.UpdateValue;
                }
                else if (item.UpdateField.Name == "OrderCreateUrlProd")
                {
                    prodConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.OrderCreation].Url = item.UpdateValue;
                }
                else if (item.UpdateField.Name == "OrderStatusUrlProd")
                {
                    prodConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.OrderStatus].Url = item.UpdateValue;
                }
                else if (item.UpdateField.Name == "LoginInstructionProd")
                {
                    prodConfig.LoginInstruction = item.UpdateValue ?? item.OrigValue;
                }
            }
            asi.asicentral.util.HtmlHelper.SubmitWebRequestAsync(urlProd, headerParams, Newtonsoft.Json.JsonConvert.SerializeObject(prodConfig), true, true);
        }
    }
}
