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

namespace asi.asicentral.web.Controllers
{
    public class asicentralApiController : Controller
    {
        //
        // GET: /asicentralApi/
        public IObjectService ObjectService { get; set; }
        public ActionResult SupplierUpdateRequestList(string requestStatus)
        {
            IList<SupUpdateRequest> objList = new List<SupUpdateRequest>();
            objList = ObjectService.GetAll<SupUpdateRequest>().OrderByDescending(item => item.CreateDate).ToList();
            if (requestStatus != string.Empty)
            {
                SupRequestStatus status = requestStatus == "0" ? SupRequestStatus.Pending : requestStatus == "1" ? SupRequestStatus.Approved : requestStatus == "2" ? SupRequestStatus.Rejected : requestStatus == "3" ? SupRequestStatus.Cancelled : SupRequestStatus.Pending;
                objList = ObjectService.GetAll<SupUpdateRequest>().Where(item => item.Status == status).OrderByDescending(item => item.CreateDate).ToList();
            }
            return View(objList);
        }
        public ActionResult SupplierUpdateRequestDetail(int id)
        {
            IList<SupUpdateRequestDetail> objList = new List<SupUpdateRequestDetail>();
            objList = ObjectService.GetAll<SupUpdateRequestDetail>().Where(item => item.SupUpdateRequestId == id).OrderByDescending(item => item.CreateDate).ToList();
            return View(objList);
        }
       
        [HttpPost]
        public ActionResult EditServicesData(string command, int supUpdateRequestId)
        {
            SupUpdateRequest supUpdateRequest = null;
            var excitUrl = ConfigurationManager.AppSettings["ExcitUrl"];
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
                            var url = excitUrl + "/v1/Suppliers/" + supUpdateRequest.CompanyId + "/config?client=QA";
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
                                    stageConfig.Username = item.UpdateValue;
                                }
                                else if (item.UpdateField.Name == "PasswordTest")
                                {
                                    stageConfig.Password = item.UpdateValue;
                                }
                                else if (item.UpdateField.Name == "InventoryUrlTest")
                                {
                                    stageConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.Inventory].Available = true;
                                    stageConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.Inventory].Url = item.UpdateValue;
                                }
                                else if (item.UpdateField.Name == "LoginValidateUrlTest")
                                {
                                    stageConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.LoginValidate].Available = true;
                                    stageConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.LoginValidate].Url = item.UpdateValue;
                                }
                                else if (item.UpdateField.Name == "OrderCreateUrlTest")
                                {
                                    stageConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.OrderCreation].Available = true;
                                    stageConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.OrderCreation].Url = item.UpdateValue;
                                }
                                else if (item.UpdateField.Name == "OrderStatusUrlTest")
                                {
                                    stageConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.OrderStatus].Available = true;
                                    stageConfig.Services[ASI.Contracts.Excit.Supplier.Version1.Configuration.API.OrderStatus].Url = item.UpdateValue;
                                }
                            }
                            await asi.asicentral.util.HtmlHelper.SubmitWebRequestAsync(url, headerParams, Newtonsoft.Json.JsonConvert.SerializeObject(stageConfig), true, true);
                        }
                    }).Wait();
                }

            }
            catch (Exception ex)
            {
                LogService log = LogService.GetLog(this.GetType());
                log.Error(ex.Message);
            }
            if (supUpdateRequest != null)
            {
                supUpdateRequest.Status = command == "Accept" ? SupRequestStatus.Approved : SupRequestStatus.Rejected;
                supUpdateRequest.ApprovedBy = ((System.Security.Principal.WindowsIdentity)System.Web.HttpContext.Current.User.Identity).Name;
                supUpdateRequest.UpdateDate = DateTime.UtcNow;
                ObjectService.SaveChanges();
            }
            return RedirectToAction("SupplierUpdateRequestList");
        }
    }
}
