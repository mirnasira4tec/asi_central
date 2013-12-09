using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers.Store
{
    public class CouponController : Controller
    {
        //
        // GET: /Coupon/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View("../Store/Admin/Coupon");
        }

    }
}
