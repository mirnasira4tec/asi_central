using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.services;
using StructureMap.Query;
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
            IList<Coupon> couponList = StoreService.GetAll<Coupon>(true).OrderByDescending(x => x.CreateDate).ToList(); 

            return View("../Store/Coupon/CouponList", couponList);
        }

        public ActionResult Add()
        {
            CouponModel productToUpdate = new CouponModel();
            productToUpdate.Products = GetSelectedProductList();
            productToUpdate.Contexts = GetSelectedContextList();
            productToUpdate.ValidFrom = DateTime.UtcNow;
            productToUpdate.ValidUpto = DateTime.UtcNow;
            return View("../Store/Coupon/CouponDetails", productToUpdate);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            CouponModel productToUpdate = new CouponModel();
            productToUpdate.Products = GetSelectedProductList();
            productToUpdate.Contexts = GetSelectedContextList();
            if (id != 0)
            {
                Coupon couponModel = StoreService.GetAll<Coupon>().Where(item => item.Id == id).FirstOrDefault();
                if (productToUpdate != null)
                {
                    if (Convert.ToDecimal(couponModel.MonthlyCost) + Convert.ToDecimal(couponModel.AppFeeDiscount) + Convert.ToDecimal(couponModel.ProductDiscount) <= 0)
                    {
                        ModelState.AddModelError(" ", "Please fill any one of Application Fee Discount, Product Discount and Monthly Subscription Cost fields");
                        return View("../Store/Coupon/CouponDetails", couponModel);
                    }
                    productToUpdate.CouponCode = couponModel.CouponCode;
                    productToUpdate.Description = couponModel.Description;
                    productToUpdate.ValidFrom = couponModel.ValidFrom;
                    productToUpdate.ValidUpto = couponModel.ValidUpto;
                    productToUpdate.ProductId = couponModel.ProductId;
                    productToUpdate.ContextId = couponModel.ContextId;
                    productToUpdate.MonthlyCost = couponModel.MonthlyCost.ToString();
                    productToUpdate.AppFeeDiscount = couponModel.AppFeeDiscount.ToString();
                    productToUpdate.ProductDiscount = couponModel.ProductDiscount.ToString();
                }
            }
            else
            {
                productToUpdate.ValidFrom = DateTime.UtcNow;
                productToUpdate.ValidUpto = DateTime.UtcNow;
            }
            return View("../Store/Coupon/CouponDetails", productToUpdate);
        }

        private IList<SelectListItem> GetSelectedContextList()
        {
            IList<SelectListItem> contextList = null;
            IList<Context> contexts = StoreService.GetAll<Context>(true).ToList();
            if (contexts != null && contexts.Count > 0)
            {
                contextList = new List<SelectListItem>();
                string text = string.Empty;
                foreach (Context product in contexts)
                {
                    text = product.Id.ToString() + "-" + product.Name;
                    contextList.Add(new SelectListItem() { Text = text, Value = product.Id.ToString(), Selected = false });
                }
            }
            return contextList;
        }

        private IList<SelectListItem> GetSelectedProductList()
        {
            IList<SelectListItem> productList = null;
            IList<ContextProduct> products = StoreService.GetAll<ContextProduct>(true).ToList();
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
        public JsonResult GetProductName(int id)
        {
            if (id != 0)
            {
                ContextProduct product = StoreService.GetAll<ContextProduct>(true).Where(detail => detail.Id == id).FirstOrDefault();
                if(product.IsSubscription)
                 return Json(true);
            }
            return Json(false);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCouponDetails(CouponModel couponModel)
        {
            if (couponModel.ActionName == "Cancel") return List();
            if (ModelState.IsValid)
            {
                if ((Convert.ToDecimal(couponModel.MonthlyCost) + Convert.ToDecimal(couponModel.AppFeeDiscount) + Convert.ToDecimal(couponModel.ProductDiscount) <= 0))
                {
                    ModelState.AddModelError(" ", "Please fill any one of Application Fee Discount, Product Discount and Monthly Subscription Cost fields");
                    couponModel.Products = GetSelectedProductList();
                    couponModel.Contexts = GetSelectedContextList();
                    couponModel.ValidFrom = DateTime.UtcNow;
                    couponModel.ValidUpto = DateTime.UtcNow;
                    return View("../Store/Coupon/CouponDetails", couponModel);
                }
                else
                {
                    Coupon coupon = StoreService.GetAll<Coupon>().Where(item => item.Id == couponModel.Id).FirstOrDefault();
                    if (coupon == null)
                    {
                        coupon = new Coupon();
                        coupon.CreateDate = DateTime.UtcNow;
                        StoreService.Add<Coupon>(coupon);
                    }


                    coupon.CouponCode = couponModel.CouponCode;
                    coupon.Description = couponModel.Description;
                    coupon.ValidFrom = couponModel.ValidFrom;
                    coupon.ValidUpto = couponModel.ValidUpto;
                    coupon.ProductId = couponModel.ProductId;
                    coupon.ContextId = couponModel.ContextId;
                    coupon.MonthlyCost = Convert.ToDecimal(couponModel.MonthlyCost);
                    coupon.AppFeeDiscount = Convert.ToDecimal(couponModel.AppFeeDiscount);
                    coupon.ProductDiscount = Convert.ToDecimal(couponModel.ProductDiscount);
                    coupon.UpdateDate = DateTime.UtcNow;
                    coupon.UpdateSource = "CouponController - SaveCouponDetails";
                    StoreService.SaveChanges();
                    IList<Coupon> couponList = StoreService.GetAll<Coupon>(true).OrderByDescending(x => x.CreateDate).ToList();
                    return View("../Store/Coupon/CouponList", couponList);
                }
            }
            else
            {
                if (couponModel != null)
                {
                    couponModel.Products = GetSelectedProductList();
                    couponModel.Contexts = GetSelectedContextList();
                }
                return View("../Store/Coupon/CouponDetails", couponModel);
            }
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
