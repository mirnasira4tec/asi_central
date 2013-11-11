using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers.Store
{
    [Authorize]
    public class ContextController : Controller
    {
        public IStoreService StoreObjectService { get; set; }

        public ActionResult Index()
        {
            return View("List");
        }
        
        [HttpGet]
        public ActionResult List()
        {
            IList<Context> contextList = StoreObjectService.GetAll<Context>(true).ToList();
            return View("../Store/Context/List", contextList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult List(IList<Context> contexts)
        {
            IList<Context> contextList = StoreObjectService.GetAll<Context>().ToList();
            foreach (Context context in contexts)
            {
                Context contextToUpdate = contextList.Where(ctx => ctx.Id == context.Id).FirstOrDefault();
                if (contextToUpdate != null)
                {
                    contextToUpdate.HeaderImage = context.HeaderImage;
                    contextToUpdate.NotificationEmails = context.NotificationEmails;
                    contextToUpdate.Active = context.Active;
                }
            }
            StoreObjectService.SaveChanges();
            return new RedirectResult("/Store/Context/List");
        }

        public ActionResult References()
        {
            return View("../Store/Context/References");
        }

        public ActionResult ProductComparison(int id)
        {
            Context context = StoreObjectService.GetAll<Context>(true).Where(ctx => ctx.Id == id).SingleOrDefault();
            if (context == null) throw new Exception("Invalid identifier for a context");
            return View("../Store/Context/ProductComparison", context);
        }
    }
}
