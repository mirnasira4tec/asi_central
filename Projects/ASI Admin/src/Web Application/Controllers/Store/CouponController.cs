using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers.Store
{
    public class CouponController : Controller
    {
        public IStoreService StoreService { get; set; }
        //
        // GET: /Coupon/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            IList<Coupon> couponList = StoreService.GetAll<Coupon>(true).ToList();
            return View("../Store/Coupon/CouponList",couponList);
        }

    }
}
