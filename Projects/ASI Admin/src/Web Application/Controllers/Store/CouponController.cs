using asi.asicentral.interfaces;
using asi.asicentral.model.personify;
using asi.asicentral.model.store;
using asi.asicentral.services;
using asi.asicentral.util.store.coupon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers.Store
{
    public class CouponController : Controller
    {
        public IStoreService StoreService { get; set; }
        public IBackendService BackendService { get; set; }

        public ActionResult Index()
        {
            return List();
        }

        public ActionResult List(string couponCode = null, string memberType = null, bool showValidOnly = false)
        {
            var coupons = new CouponListModel();
            var couponList = StoreService.GetAll<Coupon>(true);
            if (!string.IsNullOrEmpty(couponCode))
            {
                couponList = couponList.Where(item => item.CouponCode != null && item.CouponCode.Contains(couponCode));
            }
            if (!string.IsNullOrEmpty(memberType))
            {
                if (memberType == "Others")
                    couponList = couponList.Where(item => item.Product != null && item.Product.Type != "Distributor Membership" && item.Product.Type != "Supplier Membership" && item.Product.Type != "Decorator Membership");
                else
                    couponList = couponList.Where(item => item.Product != null && item.Product.Type == memberType);
            }

            coupons.Coupons = couponList.OrderByDescending(c => c.UpdateDate).ToList();

            if (showValidOnly)
            {
                coupons.Coupons = coupons.Coupons.Where(item => item.ValidUpto.Date >= DateTime.Now.Date).ToList();
            }

            coupons.CouponCode = couponCode;
            coupons.MemberType = memberType;
            coupons.ShowValidOnly = showValidOnly;

            return View("../Store/Coupon/CouponList", coupons);
        }
        public ActionResult Add()
        {
            CouponModel productToUpdate = new CouponModel();
            productToUpdate.RateStructure = "BUNDLE";
            productToUpdate.Products = GetSelectedProductList();
            productToUpdate.Contexts = GetSelectedContextList();
            productToUpdate.ValidFrom = DateTime.UtcNow;
            productToUpdate.ValidUpto = DateTime.UtcNow.AddYears(5);
            return View("../Store/Coupon/CouponDetails", productToUpdate);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var productToUpdate = new CouponModel();
            productToUpdate.Products = GetSelectedProductList();
            productToUpdate.Contexts = GetSelectedContextList();
            if (id != 0)
            {
                Coupon couponModel = StoreService.GetAll<Coupon>().FirstOrDefault(item => item.Id == id);
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
                    productToUpdate.RateStructure = !string.IsNullOrEmpty(couponModel.RateStructure) ? couponModel.RateStructure : "BUNDLE";
                    productToUpdate.GroupName = couponModel.GroupName;
                    productToUpdate.RateCode = couponModel.RateCode;
                }
            }
            else
            {
                productToUpdate.ValidFrom = DateTime.UtcNow;
                productToUpdate.ValidUpto = DateTime.UtcNow.AddYears(5);
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
                    text = product.Id + "-" + product.Name;
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
        public JsonResult GetProductInfo(int productId, int contextId)
        {
            var isSubscription = false;
            var hasBackEndIntegration = false;
            var applicationCost = 0M;
            var cost = 0M;

            if (productId != 0)
            {
                var product = StoreService.GetAll<ContextProduct>(true).FirstOrDefault(p => p.Id == productId );
                if (product != null)
                {
                    isSubscription = product.IsSubscription;
                    hasBackEndIntegration = product.HasBackEndIntegration;
                    applicationCost = product.ApplicationCost;
                    cost = product.Cost;
                }
            }

            if (contextId != 0)
            {
                var context = StoreService.GetAll<Context>(true)
                                                 .FirstOrDefault(c => c.Id == contextId );
                if (context != null)
                {
                    var contextProduct = context.Products.FirstOrDefault(p => p.Product.Id == productId);
                    if (contextProduct != null)
                    {
                        applicationCost = contextProduct.ApplicationCost;
                        cost = contextProduct.Cost;
                    }
                }
            }

           return new JsonResult{   
                                   Data = new {  IsSubscription = isSubscription, HasBackEndIntegration = hasBackEndIntegration,
                                                 ApplicationCost = applicationCost, Cost = cost } 
                                };
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
                
                if (couponModel.ContextId == null && couponModel.ProductId == null)
                {
                    ModelState.AddModelError("Error", "Please select at least one out of Product and Context");
                }
                else if (couponModel.ProductId.HasValue && couponModel.ProductId.Value != 0)
                {
                    product = StoreService.GetAll<ContextProduct>(true)
                                          .FirstOrDefault(detail => detail.Id == couponModel.ProductId.Value);
                    if (product != null && product.HasBackEndIntegration && 
                        ( couponModel.MonthlyCost.HasValue || couponModel.ProductDiscount == product.Cost) )
                    {
                        if (!ValidatePersonifyRateCode(couponModel.RateStructure, couponModel.GroupName, couponModel.RateCode))
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
                if (!string.IsNullOrEmpty(coupon.RateStructure) && !string.IsNullOrEmpty(coupon.GroupName) &&
                    !string.IsNullOrEmpty(coupon.RateCode))
                {
                    coupon.RateStructure = couponModel.RateStructure.Trim();
                    coupon.GroupName = couponModel.GroupName.Trim();
                    coupon.RateCode = couponModel.RateCode.Trim();
                }
                coupon.UpdateDate = DateTime.UtcNow;
                coupon.UpdateSource = "CouponController - SaveCouponDetails";
                StoreService.SaveChanges();
                UpdatePersonifyMappingTbl(coupon, product);

                return List();
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

            return List();
        }

        private bool ValidatePersonifyRateCode(string rateStructure, string groupName, string rateCode)
        {
            var isValid = !string.IsNullOrEmpty(rateStructure) && !string.IsNullOrEmpty(groupName) &&
                          !string.IsNullOrEmpty(rateCode);
            if (isValid)
            {
                isValid = BackendService.ValidateRateCode(groupName, rateStructure, rateCode);
            }

            return isValid;
        }

        private void UpdatePersonifyMappingTbl(Coupon coupon, ContextProduct product)
        {
            if (string.IsNullOrEmpty(coupon.RateStructure) || string.IsNullOrEmpty(coupon.GroupName) || string.IsNullOrEmpty(coupon.RateCode))
                return;

            try
            {
                // get baseline personify bundle/products for the product
                var mappings = StoreService.GetAll<PersonifyMapping>()
                    .Where(map => map.StoreContext == null && map.StoreProduct == coupon.ProductId).ToList();

                mappings = mappings.FindAll(m => string.IsNullOrEmpty(m.StoreOption));

                if (mappings.Any())
                {
                    // delete existing rows for the coupon
                    var existMappings = StoreService.GetAll<PersonifyMapping>()
                        .Where(map => map.StoreOption == coupon.CouponCode).ToList();

                    for (int i = 0; i < existMappings.Count; i++)
                    {
                        StoreService.Delete(existMappings[i]);
                    }

                    // use product baseline to add rows for coupon
                    foreach (var m in mappings)
                    {
                        var productMapping = new PersonifyMapping()
                        {
                            Identifier = Guid.NewGuid(),
                            StoreContext = coupon.ContextId,
                            StoreOption = coupon.CouponCode,
                            StoreProduct = m.StoreProduct,
                            PaySchedule = m.PaySchedule,
                            ClassCode = m.ClassCode,
                            PersonifyProduct = m.PersonifyProduct,
                            PersonifyRateStructure = m.PersonifyRateStructure,
                            PersonifyRateCode = m.PersonifyRateCode,
                            PersonifyBundle = m.PersonifyBundle,
                            CreateUserUTC = "Store",
                            ESBSendGlag = m.ESBSendGlag,
                            NewAsiNumFlag = m.NewAsiNumFlag,
                            NotifyByEmailFlag = m.NotifyByEmailFlag,
                            CreateDateUTC = DateTime.UtcNow,
                            UpdateDateUTC = DateTime.UtcNow,
                            UpdateSource = "Store"
                        };

                        if (m.PersonifyRateStructure == "Bundle" || !string.IsNullOrEmpty(m.PersonifyBundle))
                        {   // membership bundle or package
                            productMapping.PersonifyBundle = coupon.GroupName;
                            productMapping.PersonifyRateCode = coupon.RateCode;
                        }
                        else if (m.PersonifyProduct.HasValue)
                        {
                            if (CouponHelper.FREE_MONTH_RATECODES.Keys.Contains(m.PersonifyProduct.Value) &&
                                (coupon.MonthlyCost.HasValue && coupon.ProductDiscount == coupon.MonthlyCost.Value) ||
                                (!coupon.MonthlyCost.HasValue && coupon.ProductDiscount == product.Cost))
                            { // bolt on products
                                productMapping.PersonifyRateCode = CouponHelper.FREE_MONTH_RATECODES[m.PersonifyProduct.Value][1];
                            }
                            else if (CouponHelper.WAIVE_APP_FEE_RATECODES.Keys.Contains(m.PersonifyProduct.Value) &&
                                     coupon.AppFeeDiscount == product.ApplicationCost)
                            {
                                productMapping.PersonifyRateCode = CouponHelper.WAIVE_APP_FEE_RATECODES[m.PersonifyProduct.Value][1];
                            }
                        }

                        StoreService.Add(productMapping);
                    }

                    StoreService.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var log = LogService.GetLog(this.GetType());
                log.Debug(string.Format("Exception occurred when updating Personify mapping table for coupon '{0}', message : {1}", coupon.CouponCode, ex.Message));
            }
        }
    }
}
