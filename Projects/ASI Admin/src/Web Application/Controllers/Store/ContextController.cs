using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers.Store
{
    public class ContextController : Controller
    {
        public IStoreService StoreObjectService { get; set; }

        public ActionResult Index()
        {
            return View("List");
        }
        
        public ActionResult List()
        {
            IList<Context> contextList = StoreObjectService.GetAll<Context>(true).ToList();
            return View("../Store/Context/List", contextList);
        }

        public ActionResult ProductComparison(int id)
        {
            Context context = StoreObjectService.GetAll<Context>(true).Where(ctx => ctx.ContextId == id).SingleOrDefault();
            if (context == null) throw new Exception("Invalid identifier for a context");
            return View("../Store/Context/ProductComparison", context);
        }
    }
}
