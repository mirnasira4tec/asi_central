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
            return View("../Store/Coupon/CouponList", couponList);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (id != 0)
            {
                CouponModel productToUpdate = new CouponModel();
                Coupon couponModel = StoreService.GetAll<Coupon>().Where(item => item.Id == id).FirstOrDefault();
                if (productToUpdate != null)
                {
                    productToUpdate.CouponCode = couponModel.CouponCode;
                    productToUpdate.IsSubscription = couponModel.IsSubscription;
                    productToUpdate.ValidFrom = couponModel.ValidFrom;
                    productToUpdate.ValidUpto = couponModel.ValidUpto;
                    productToUpdate.IsFixedAmount = couponModel.IsFixedAmount;
                    if (productToUpdate.IsFixedAmount)
                    {
                        productToUpdate.DiscountAmount = couponModel.DiscountAmount;
                        productToUpdate.DiscountPercentage = 0;
                    }
                    else
                    {
                        productToUpdate.DiscountPercentage = couponModel.DiscountPercentage;
                        productToUpdate.DiscountAmount = 0;
                    }



                }

                return View("../Store/Coupon/CouponDetails", productToUpdate);
            }
            else
            {
                return View("../Store/Coupon/CouponDetails");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCouponDetails(CouponModel couponModel)
        {
            Coupon coupon = StoreService.GetAll<Coupon>().Where(item => item.Id == couponModel.Id).FirstOrDefault();
            if (coupon == null)
            {
                coupon = new Coupon();
                coupon.CreateDate = DateTime.UtcNow;
                StoreService.Add<Coupon>(coupon);
            }
           
                
                coupon.CouponCode = couponModel.CouponCode;
                coupon.IsSubscription = couponModel.IsSubscription;
                coupon.ValidFrom = couponModel.ValidFrom;
                coupon.ValidUpto = couponModel.ValidUpto;
                coupon.IsFixedAmount = couponModel.IsFixedAmount;
                if (coupon.IsFixedAmount)
                {
                    coupon.DiscountAmount =couponModel.DiscountAmount;
                    coupon.DiscountPercentage = 0;
                }
                else
                {
                    coupon.DiscountPercentage = couponModel.DiscountPercentage;
                    coupon.DiscountAmount = 0;
                }
                coupon.UpdateDate = DateTime.UtcNow;
                coupon.UpdateSource = "CouponController - SaveCouponDetails";
               
   
            
            StoreService.SaveChanges();
            IList<Coupon> couponList = StoreService.GetAll<Coupon>(true).ToList();
            return View("../Store/Coupon/CouponList", couponList);
        }

        public ActionResult Delete(int id)
        {
            Coupon coupon = StoreService.GetAll<Coupon>().Where(item => item.Id == id).FirstOrDefault();
            if (coupon != null)
            {
                StoreService.Delete<Coupon>(coupon);
                StoreService.SaveChanges();
            }
                
            IList<Coupon> couponList = StoreService.GetAll<Coupon>(true).ToList();
            return View("../Store/Coupon/CouponList", couponList);
            
        }


    }
}
