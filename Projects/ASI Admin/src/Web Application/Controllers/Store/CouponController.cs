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
            CouponModel productToUpdate = new CouponModel();
            IList<ContextProduct> productList = StoreService.GetAll<ContextProduct>(true).ToList();
            productToUpdate.Products =  GetSelectedProductList(productList);
            IList<Context> contextList = StoreService.GetAll<Context>(true).ToList();
            productToUpdate.Contexts = GetSelectedContextList(contextList);
            if (id != 0)
            {
                Coupon couponModel = StoreService.GetAll<Coupon>().Where(item => item.Id == id).FirstOrDefault();
                if (productToUpdate != null)
                {
                    productToUpdate.CouponCode = couponModel.CouponCode;
                    productToUpdate.IsSubscription = couponModel.IsSubscription;
                    productToUpdate.ValidFrom = couponModel.ValidFrom;
                    productToUpdate.ValidUpto = couponModel.ValidUpto;
                    productToUpdate.ProductId = couponModel.ProductId;
                    productToUpdate.ContextId = couponModel.ContextId;
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
                    if (couponModel.ProductId != null)
                        productToUpdate.IsProduct = true;
                    else
                        productToUpdate.IsProduct = false;
                }
            }
            return View("../Store/Coupon/CouponDetails", productToUpdate);
        }

        private IList<SelectListItem>  GetSelectedContextList(IList<Context> contexts)
        {
            IList<SelectListItem> productList = null;
            if (contexts != null && contexts.Count > 0)
            {
                productList = new List<SelectListItem>();
                string text = string.Empty;
                foreach (Context product in contexts)
                {
                    text = product.Id.ToString() + "-" + product.Name;
                    productList.Add(new SelectListItem() { Text = text, Value = product.Id.ToString(), Selected = false });
                }
            }
            return productList;
        }

        private IList<SelectListItem> GetSelectedProductList(IList<ContextProduct> products)
        {
            IList<SelectListItem> productList = null;
            if (products != null && products.Count > 0)
            {
                productList = new List<SelectListItem>();
                string text = string.Empty;
                foreach (ContextProduct product in products)
                {
                    text = product.Id.ToString() + "-" + product.Name;
                    productList.Add(new SelectListItem() { Text = text, Value = product.Id.ToString(), Selected = false });
                }
            }
            return productList;
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
                if (couponModel.IsProduct)
                    coupon.ProductId = couponModel.ProductId;
                else
                    coupon.ContextId = couponModel.ContextId;
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
