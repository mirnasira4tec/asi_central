using asi.asicentral.interfaces;
using asi.asicentral.model.personify;
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
            IList<Coupon> couponList = StoreService.GetAll<Coupon>(true).OrderByDescending(x => x.UpdateDate).ToList();

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
                    productToUpdate.CouponCode = couponModel.CouponCode;
                    productToUpdate.Description = couponModel.Description;
                    productToUpdate.ValidFrom = couponModel.ValidFrom;
                    productToUpdate.ValidUpto = couponModel.ValidUpto;
                    productToUpdate.ProductId = couponModel.ProductId;
                    productToUpdate.ContextId = couponModel.ContextId;
                    productToUpdate.MonthlyCost = couponModel.MonthlyCost;
                    productToUpdate.AppFeeDiscount = couponModel.AppFeeDiscount;
                    productToUpdate.ProductDiscount = couponModel.ProductDiscount;
                    productToUpdate.RateStructure = couponModel.RateStructure;
                    productToUpdate.GroupName = couponModel.GroupName;
                    productToUpdate.RateCode = couponModel.RateCode;
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
                    contextList.Add(new SelectListItem() {Text = text, Value = product.Id.ToString(), Selected = false});
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
                    productList.Add(new SelectListItem() {Text = text, Value = product.Id.ToString(), Selected = false});
                }
            }
            return productList;
        }

        [HttpPost]
        public JsonResult GetProductName(int productId, int contextId = 0)
        {
            JsonResult result = null;
            if (productId != 0)
            {
                var product = StoreService.GetAll<ContextProduct>(true).FirstOrDefault(detail => detail.Id == productId);
                if (product != null)
                {
                    result = new JsonResult
                    {
                        Data = new
                        {
                            IsSubscription = product.IsSubscription,
                            HasBackEndIntegration = product.HasBackEndIntegration,
                            ApplicationCost = product.ApplicationCost,
                            Cost = product.Cost
                        }
                    };
                }
            }

            return result;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCouponDetails(CouponModel couponModel)
        {
            if (couponModel.ActionName == "Cancel") return List();

            ContextProduct product = null;

            if (ModelState.IsValid)
            {
                if (!couponModel.MonthlyCost.HasValue && couponModel.AppFeeDiscount + couponModel.ProductDiscount <= 0)
                {
                    ModelState.AddModelError("Error",
                        "Please fill out at least one of Application Fee Discount, Product Discount and Monthly Subscription Cost");
                }
                else if (couponModel.ContextId == null && couponModel.ProductId == null)
                {
                    ModelState.AddModelError("Error", "Please select at least one out of Product and Context");
                }
                else if (couponModel.ProductId.HasValue && couponModel.ProductId.Value != 0)
                {
                    product =
                        StoreService.GetAll<ContextProduct>(true)
                            .FirstOrDefault(detail => detail.Id == couponModel.ProductId.Value);
                    if (product != null && product.HasBackEndIntegration)
                    {
                        if (
                            !ValidatePersonifyRateCode(couponModel.RateStructure, couponModel.GroupName,
                                couponModel.RateCode))
                        {
                            ModelState.AddModelError("Error", "Invalid Rate Structure, Group Name or Rate Code");
                        }
                    }
                }
            }

            if (!ModelState.IsValid || ModelState.Values.SelectMany(m => m.Errors).ToList().Any())
            {
                couponModel.Products = GetSelectedProductList();
                couponModel.Contexts = GetSelectedContextList();
            }
            else
            {
                var coupon = StoreService.GetAll<Coupon>().FirstOrDefault(item => item.Id == couponModel.Id);
                if (coupon == null)
                {
                    coupon = new Coupon();
                    coupon.CreateDate = DateTime.UtcNow;
                    StoreService.Add(coupon);
                }

                coupon.CouponCode = couponModel.CouponCode.Trim();
                coupon.Description = couponModel.Description;
                coupon.ValidFrom = couponModel.ValidFrom;
                coupon.ValidUpto = couponModel.ValidUpto;
                coupon.ProductId = couponModel.ProductId;
                coupon.ContextId = couponModel.ContextId;
                coupon.MonthlyCost = couponModel.MonthlyCost;
                coupon.AppFeeDiscount = couponModel.AppFeeDiscount;
                coupon.ProductDiscount = couponModel.ProductDiscount;
                coupon.RateStructure = couponModel.RateStructure.Trim();
                coupon.GroupName = couponModel.GroupName.Trim();
                coupon.RateCode = couponModel.RateCode.Trim();
                coupon.UpdateDate = DateTime.UtcNow;
                coupon.UpdateSource = "CouponController - SaveCouponDetails";
                StoreService.SaveChanges();
                UpdatePersonifyMappingTbl(coupon, product);
                var couponList = StoreService.GetAll<Coupon>(true).OrderByDescending(x => x.UpdateDate).ToList();
                return View("../Store/Coupon/CouponList", couponList);
            }

            return View("../Store/Coupon/CouponDetails", couponModel);
        }

        public ActionResult Delete(int id)
        {
            Coupon coupon = StoreService.GetAll<Coupon>().FirstOrDefault(item => item.Id == id);
            if (coupon != null)
            {
                StoreService.Delete<Coupon>(coupon);
                StoreService.SaveChanges();
            }

            IList<Coupon> couponList = StoreService.GetAll<Coupon>(true).OrderByDescending(x => x.CreateDate).ToList();
            return View("../Store/Coupon/CouponList", couponList);
        }

        private bool ValidatePersonifyRateCode(string rateStructure, string groupName, string rateCode)
        {
            var isValid = string.IsNullOrEmpty(rateStructure) || string.IsNullOrEmpty(groupName) ||
                          string.IsNullOrEmpty(rateCode);
            if (isValid)
            {
                // validate against Personify WS
            }

            return isValid;
        }

        private void UpdatePersonifyMappingTbl(Coupon coupon, ContextProduct product)
        {
            if (string.IsNullOrEmpty(coupon.RateStructure))
                return;

            var mappings = StoreService.GetAll<PersonifyMapping>()
                .Where( map => map.StoreContext == null && map.StoreProduct == coupon.ProductId && map.StoreOption == null).ToList();

            if (mappings.Any())
            {
                var existMappings = StoreService.GetAll<PersonifyMapping>()
                    .Where(map => map.StoreOption == coupon.CouponCode).ToList();

                // delete existing rows
                for (int i = 0; i < existMappings.Count; i++)
                {
                    StoreService.Delete(existMappings[i]);
                }

                // need to find out all rows needed for the product
                foreach (var m in mappings)
                {
                    var productMapping = new PersonifyMapping()
                    {
                        StoreContext = coupon.ContextId,
                        StoreProduct = coupon.ProductId,
                        StoreOption = coupon.CouponCode,
                        PersonifyProduct = m.PersonifyProduct,
                        CreateDateUTC = DateTime.UtcNow,
                        UpdateDateUTC = DateTime.UtcNow,
                        UpdateSource = "Asicentral.store"
                    };

                    if (m.PersonifyRateStructure == "Bundle" || !string.IsNullOrEmpty(m.PersonifyBundle))
                    {
                        productMapping.PersonifyBundle = coupon.GroupName;
                        productMapping.PersonifyRateStructure = coupon.RateStructure;
                        productMapping.PersonifyRateCode = coupon.RateCode;
                    }
                    else if(product.IsMembership() )
                    {
                        
                    }
                }
            }
        }
    }
}
