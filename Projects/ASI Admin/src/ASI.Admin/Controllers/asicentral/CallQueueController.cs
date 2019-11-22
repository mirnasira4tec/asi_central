using asi.asicentral.interfaces;
using asi.asicentral.model.call;
using asi.asicentral.web.Models.asicentral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers.asicentral
{
    [Authorize]
    public class CallQueueController : Controller
    {
        public IObjectService ObjectService { get; set; }

        public ActionResult Index()
        {
            return Volume(null);
        }

        public virtual ActionResult List()
        {
            List<CallQueue> queues = ObjectService.GetAll<CallQueue>(true).OrderBy(queue => queue.Id).ToList();
            return View("../asicentral/QueueList", queues);
        }

        public virtual ActionResult Enable(int id)
        {
            CallQueue queue = ObjectService.GetAll<CallQueue>(false).Where(q => q.Id == id).FirstOrDefault();
            if (queue != null)
            {
                queue.IsForcedClosed = queue.Enabled ? (byte)1 : (byte)0;
                ObjectService.SaveChanges();
            }
            return new RedirectResult("../../CallQueue/List");
        }

        public virtual ActionResult Volume(CallVolume callVolume)
        {
            try
            {
                if (callVolume == null || callVolume.StartDate == DateTime.MinValue)
                {
                    DateTime now = DateTime.Now;
                    callVolume = new CallVolume();
                    callVolume.StartDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                    callVolume.EndDate = now;
                }
                else
                {
                    callVolume.StartDate = new DateTime(callVolume.StartDate.Year, callVolume.StartDate.Month, callVolume.StartDate.Day, 0, 0, 0);
                    callVolume.EndDate = new DateTime(callVolume.EndDate.Year, callVolume.EndDate.Month, callVolume.EndDate.Day, 23, 59, 59);
                }
                IList<Volume> volumes = ObjectService.GetAll<CallRequest>(true)
                    .Where(req => req.CreateDate >= callVolume.StartDate && req.CreateDate <= callVolume.EndDate)
                    .GroupBy(req => new { req.Req_Queue } )
                    .Select( grouped => new Volume() {
                        QueueIdentifier = grouped.Key.Req_Queue,
                        Amount = grouped.Count() })
                    .ToList();
                if (volumes.Count > 0)
                {
                    //translate the queue ids
                    IList<CallQueue> queues = ObjectService.GetAll<CallQueue>(true).ToList();
                    foreach (Volume vol in volumes)
                    {
                        CallQueue queue = queues.Where(q => q.Id == vol.QueueIdentifier).FirstOrDefault();
                        if (queue != null) vol.QueueName = queue.Name;
                    }
                }
                callVolume.Data = volumes;
            }
            catch(Exception ex)
            {
                services.LogService log = services.LogService.GetLog(this.GetType());
                log.Error("CallQueue Controller exception message: " + ex.Message);
            }
            return View("../asicentral/Volume", callVolume);
        }

        public virtual ActionResult EnableAll()
        {
            return EnableAll(true);
        }

        public virtual ActionResult DisableAll()
        {
            return EnableAll(false);
        }

        private ActionResult EnableAll(bool enable)
        {
            List<CallQueue> queues = ObjectService.GetAll<CallQueue>(false).ToList();
            foreach (CallQueue queue in queues) queue.IsForcedClosed = enable ? (byte)0 : (byte)1;
            ObjectService.SaveChanges();
            return new RedirectResult("../CallQueue/List");
        }
    }
}
