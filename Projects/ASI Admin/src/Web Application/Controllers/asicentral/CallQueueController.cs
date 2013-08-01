using asi.asicentral.interfaces;
using asi.asicentral.model.call;
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
            return List();
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
