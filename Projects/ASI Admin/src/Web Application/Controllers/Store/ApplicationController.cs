using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.model.store;
using asi.asicentral.services;
using asi.asicentral.util.store;

namespace asi.asicentral.web.Controllers.Store
{
    [Authorize]
    public class ApplicationController : Controller
    {
        public const string COMMAND_SAVE = "Save";
        public const string COMMAND_REJECT = "Reject";
        public const string COMMAND_ACCEPT = "Accept";
        public static readonly int[] ORDERDETAIL_PRODUCT_IDS = { 45, 46, 55, 62 };
        public static readonly int[] DISTRIBUTOR_CATALOG_PRODUCT_IDS = { 35, 36, 37, 38, 39, 40, 41 };
        public static readonly int[] SUPPLIER_ESP_ADVERTISING_PRODUCT_IDS = { 48, 49, 50, 51, 52, 53, 54 };
        public static readonly int[] SUPPLIER_ESP_PAYFORPLACEMENT_PRODUCT_IDS = { 47, 63 };
        public static readonly int SUPPLIER_Email_Express_PRODUCT_ID = 61;
        public static readonly int SUPPLIER_ESP_WEBSITES_PRODUCT_COLLECTIONS_ID = 64;
       
        
        public IStoreService StoreService { get; set; }
        public IFulfilmentService FulfilmentService { get; set; }
        public ICreditCardService CreditCardService { get; set; }

        [HttpGet]
        public virtual ActionResult Edit(int id)
        {
            StoreOrderDetail orderDetail = StoreService.GetAll<StoreOrderDetail>(true).Where(detail => detail.Id == id).FirstOrDefault();
            if (orderDetail == null) throw new Exception("Invalid Order Detail Id");
            StoreDetailApplication application = null;
            if (orderDetail != null) application = StoreService.GetApplication(orderDetail);
            if (application != null)
            {
                if (application is StoreDetailSupplierMembership) return View("../Store/Application/Supplier", new SupplierApplicationModel((StoreDetailSupplierMembership)application, orderDetail));
                else if (application is StoreDetailDistributorMembership) return View("../Store/Application/Distributor", new DistributorApplicationModel((StoreDetailDistributorMembership)application, orderDetail));
                else if (application is StoreDetailDecoratorMembership) return View("../Store/Application/Decorator", new DecoratorApplicationModel((StoreDetailDecoratorMembership)application, orderDetail));
                else throw new Exception("Retieved an unknown type of application");
            }
            else if(orderDetail.Product != null && orderDetail.Product.Type == "Product")
            {
                if (orderDetail.MagazineSubscriptions != null && orderDetail.MagazineSubscriptions.Count > 0) return View("../Store/Application/Magazines", new MagazinesApplicationModel(orderDetail, StoreService));
                else if (DISTRIBUTOR_CATALOG_PRODUCT_IDS.Contains(orderDetail.Product.Id))
                {
                    StoreDetailCatalog storeDetailCatalog = StoreService.GetAll<StoreDetailCatalog>().Where(catalog => catalog.OrderDetailId == orderDetail.Id).SingleOrDefault();
                    if (storeDetailCatalog != null) return View("../Store/Application/Catalogs", new CatalogsApplicationModel(orderDetail, storeDetailCatalog, StoreService));
                }
                else if (SUPPLIER_ESP_ADVERTISING_PRODUCT_IDS.Contains(orderDetail.Product.Id))
                {
                     StoreDetailESPAdvertising detailESPAdvertising = StoreService.GetAll<StoreDetailESPAdvertising>().Where(espadvertising => espadvertising.OrderDetailId == orderDetail.Id).SingleOrDefault();
                     if (detailESPAdvertising != null) return View("../Store/Application/ESPAdvertising", new ESPAdvertisingModel(orderDetail, detailESPAdvertising, StoreService));
                }
                else if (SUPPLIER_ESP_PAYFORPLACEMENT_PRODUCT_IDS.Contains(orderDetail.Product.Id)) return View("../Store/Application/PayForPlacement", new ESPPayForPlacementModel(orderDetail, StoreService));
                else if (SUPPLIER_Email_Express_PRODUCT_ID == orderDetail.Product.Id) 
                {
                    StoreDetailEmailExpress detailEmailExpress = StoreService.GetAll<StoreDetailEmailExpress>().Where(emailexpress => emailexpress.OrderDetailId == orderDetail.Id).SingleOrDefault();
                    return View("../Store/Application/EmailExpress", new EmailExpressModel(orderDetail, detailEmailExpress,StoreService));
                }
                else if(ORDERDETAIL_PRODUCT_IDS.Contains(orderDetail.Product.Id)) return View("../Store/Application/OrderDetailProduct", new OrderDetailApplicationModel(orderDetail));
                else if (SUPPLIER_ESP_WEBSITES_PRODUCT_COLLECTIONS_ID == orderDetail.Product.Id)  return View("../Store/Application/ProductCollections", new ProductCollectionsModel(orderDetail,  StoreService));
            }
            throw new Exception("Retieved an unknown type of application");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult EditDistributor(DistributorApplicationModel application)
        {
            if (ModelState.IsValid)
            {
                StoreOrderDetail orderDetail = StoreService.GetAll<StoreOrderDetail>().Where(detail => detail.Id == application.OrderDetailId).FirstOrDefault();
                if (orderDetail == null) throw new Exception("Invalid id, could not find the OrderDetail record");
                StoreOrder order = orderDetail.Order;
                StoreDetailDistributorMembership distributorApplication = StoreService.GetAll<StoreDetailDistributorMembership>().Where(app => app.OrderDetailId == application.OrderDetailId).SingleOrDefault();
                if (order == null) throw new Exception("Invalid reference to an order");
                if (distributorApplication == null) throw new Exception("Invalid reference to an application");
                order.ExternalReference = application.ExternalReference;

                //view does not contain some of the collections, copy from the ones in the database
                application.SyncAccountTypesToApplication(StoreService.GetAll<LookDistributorAccountType>().ToList(), distributorApplication);
                application.SyncProductLinesToApplication(StoreService.GetAll<LookProductLine>().ToList(), distributorApplication);
                application.ProductLines = distributorApplication.ProductLines;
                application.AccountTypes = distributorApplication.AccountTypes;

                LookDistributorRevenueType PrimaryBusinessRevenue = StoreService.GetAll<LookDistributorRevenueType>(false).Where(revenue => revenue.Name == application.BuisnessRevenue).SingleOrDefault();
                if (PrimaryBusinessRevenue != null)
                {
                    application.PrimaryBusinessRevenue = PrimaryBusinessRevenue;
                }
                else
                {
                    application.PrimaryBusinessRevenue = null;
                }
                order = UpdateCompanyInformation(application, order);
                application.CopyTo(distributorApplication);

                ProcessCommand(StoreService, FulfilmentService, order, distributorApplication, application.ActionName);
                distributorApplication.UpdateDate = DateTime.UtcNow;
                distributorApplication.UpdateSource = "ASI Admin Application - EditDistributor";
                StoreService.SaveChanges();
                if (application.ActionName == ApplicationController.COMMAND_REJECT)
                    return RedirectToAction("List", "Orders");
                else
                    return RedirectToAction("Edit", "Application", new { id = application.OrderDetailId });
            }
            else
            {
                return View("../Store/Application/Distributor", application);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult EditProductCollections(ProductCollectionsModel application)
        {
            if (ModelState.IsValid)
            {
                StoreOrderDetail orderDetail = StoreService.GetAll<StoreOrderDetail>().Where(detail => detail.Id == application.OrderDetailId).FirstOrDefault();
                if (orderDetail == null) throw new Exception("Invalid id, could not find the OrderDetail record");
                StoreOrder order = orderDetail.Order;
                if (order == null) throw new Exception("Invalid reference to an order");
                order.ExternalReference = application.ExternalReference;
                order = UpdateCompanyInformation(application, order);
                    
                //Update Product Collections Information
                if(orderDetail.Product != null)
                {
                    switch(orderDetail.Product.Id)
                    {
                        case 64:
                            if (application.productCollections != null && application.productCollections.Count > 0)
                            {
                                foreach (StoreDetailProductCollection collection in application.productCollections)
                                {
                                    if(collection != null && collection.ProductCollectionItems != null && collection.ProductCollectionItems.Count > 0)
                                    {
                                        foreach(StoreDetailProductCollectionItem newCollectionItem in collection.ProductCollectionItems)
                                        {
                                            StoreDetailProductCollectionItem oldCollectionItem = StoreService.GetAll<StoreDetailProductCollectionItem>().Where(details => details.ItemId == newCollectionItem.ItemId).SingleOrDefault();
                                            if (oldCollectionItem != null)
                                            {
                                                oldCollectionItem.ItemNumbers = newCollectionItem.ItemNumbers;
                                                oldCollectionItem.Collection = newCollectionItem.Collection;
                                                StoreService.Update<StoreDetailProductCollectionItem>(oldCollectionItem);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
                StoreAddress address = order.Company.GetCompanyShippingAddress();
                StoreService.UpdateTaxAndShipping(order);
                orderDetail.UpdateDate = DateTime.UtcNow;
                orderDetail.UpdateSource = "ApplicationController - EditProductCollections";

                ProcessCommand(StoreService, FulfilmentService, order, null, application.ActionName);
                StoreService.SaveChanges();
                if (application.ActionName == ApplicationController.COMMAND_REJECT)
                    return RedirectToAction("List", "Orders");
                else
                    return RedirectToAction("Edit", "Application", new { id = application.OrderDetailId });
            }
            else
            {
                return View("../Store/Application/ProductCollections", application);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult EditSupplier(SupplierApplicationModel application)
        {
            if (ModelState.IsValid)
            {
                StoreOrderDetail orderDetail = StoreService.GetAll<StoreOrderDetail>().Where(detail => detail.Id == application.OrderDetailId).FirstOrDefault();
                if (orderDetail == null) throw new Exception("Invalid id, could not find the OrderDetail record");
                StoreOrder order = orderDetail.Order;
                StoreDetailSupplierMembership supplierApplication = StoreService.GetAll<StoreDetailSupplierMembership>(false).Where(app => app.OrderDetailId == application.OrderDetailId).SingleOrDefault();
                if (order == null) throw new Exception("Invalid reference to an order");
                if (supplierApplication == null) throw new Exception("Invalid reference to an application");
                order.ExternalReference = application.ExternalReference;
                //copy decorating types bool to the collections
                application.SyncDecoratingTypes(StoreService.GetAll<LookSupplierDecoratingType>().ToList(), supplierApplication);
                application.DecoratingTypes = supplierApplication.DecoratingTypes;
                order = UpdateCompanyInformation(application, order);
                application.CopyTo(supplierApplication);
                supplierApplication.UpdateDate = DateTime.UtcNow;
                supplierApplication.UpdateSource = "ASI Admin Application - EditSupplier";
                ProcessCommand(StoreService, FulfilmentService, order, supplierApplication, application.ActionName);
                StoreService.SaveChanges();
                if (application.ActionName == ApplicationController.COMMAND_REJECT)
                    return RedirectToAction("List", "Orders");
                else
                    return RedirectToAction("Edit", "Application", new { id = application.OrderDetailId });
            }
            else
            {
                return View("../Store/Application/Supplier", application);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult EditDecorator(DecoratorApplicationModel application)
        {
            if (ModelState.IsValid)
            {
                StoreOrderDetail orderDetail = StoreService.GetAll<StoreOrderDetail>().Where(detail => detail.Id == application.OrderDetailId).FirstOrDefault();
                if (orderDetail == null) throw new Exception("Invalid id, could not find the OrderDetail record");
                StoreOrder order = orderDetail.Order;
                StoreDetailDecoratorMembership decoratorApplication = StoreService.GetAll<StoreDetailDecoratorMembership>().Where(app => app.OrderDetailId == application.OrderDetailId).SingleOrDefault();
                if (order == null) throw new Exception("Invalid reference to an order");
                if (decoratorApplication == null) throw new Exception("Invalid reference to an application");
                order.ExternalReference = application.ExternalReference;
                //copy imprinting types bool to the collections
                application.SyncImprintingTypesToApplication(StoreService.GetAll<LookDecoratorImprintingType>().ToList(), decoratorApplication);
                application.ImprintTypes = decoratorApplication.ImprintTypes;
                order = UpdateCompanyInformation(application, order);
                application.CopyTo(decoratorApplication);
                decoratorApplication.UpdateDate = DateTime.UtcNow;
                decoratorApplication.UpdateSource = "ASI Admin Application - EditDecorator";
                ProcessCommand(StoreService, FulfilmentService, order, decoratorApplication, application.ActionName);
                StoreService.SaveChanges();
                if (application.ActionName == ApplicationController.COMMAND_REJECT)
                    return RedirectToAction("List", "Orders");
                else
                    return RedirectToAction("Edit", "Application", new { id = application.OrderDetailId });
            }
            else
            {
                return View("../Store/Application/Decorator", application);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult EditMagazines(MagazinesApplicationModel application)
        {
            if (ModelState.IsValid)
            {
                StoreOrderDetail orderDetail = StoreService.GetAll<StoreOrderDetail>().Where(detail => detail.Id == application.OrderDetailId).FirstOrDefault();
                if (orderDetail == null) throw new Exception("Invalid id, could not find the OrderDetail record");
                StoreOrder order = orderDetail.Order;
                if (order == null) throw new Exception("Invalid reference to an order");
                order.ExternalReference = application.ExternalReference;
                if (application.ProductName != "Stitches" && application.ProductName != "Wearables")
                {
                    order = UpdateCompanyInformation(application, order);
                }
                orderDetail = UpdateMagazineSubscriptionInformation(application, orderDetail);
                ProcessCommand(StoreService, FulfilmentService, order, null, application.ActionName);
                StoreService.SaveChanges();
                if (application.ActionName == ApplicationController.COMMAND_REJECT)
                    return RedirectToAction("List", "Orders");
                else
                    return RedirectToAction("Edit", "Application", new { id = application.OrderDetailId });
            }
            else
            {
                return View("../Store/Application/Magazines", application);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult EditCatalogs(CatalogsApplicationModel application)
        {
            if (ModelState.IsValid)
            {
                StoreOrderDetail orderDetail = StoreService.GetAll<StoreOrderDetail>().Where(detail => detail.Id == application.OrderDetailId).FirstOrDefault();
                if (orderDetail == null) throw new Exception("Invalid id, could not find the OrderDetail record");
                StoreDetailCatalog storeDetailCatalog = StoreService.GetAll<StoreDetailCatalog>().Where(catalog => catalog.OrderDetailId == orderDetail.Id).SingleOrDefault();
                if (storeDetailCatalog == null) throw new Exception("Invalid id, could not find the Catalog information record");
                StoreOrder order = orderDetail.Order;
                if (order == null) throw new Exception("Invalid reference to an order");
                order.ExternalReference = application.ExternalReference;
                order = UpdateCompanyInformation(application, order);

                //Update Catalog Information
                orderDetail.Quantity = Convert.ToInt32(application.Quantity);
                orderDetail.ShippingMethod = application.ShippingMethod;
                StoreAddress address = order.Company.GetCompanyShippingAddress();
                StoreService.UpdateTaxAndShipping(order);
                orderDetail.UpdateDate = DateTime.UtcNow;
                orderDetail.UpdateSource = "ApplicationController - EditCatalogs";

                storeDetailCatalog.AreaId = Convert.ToInt32(application.Area);
                storeDetailCatalog.CoverId = Convert.ToInt32(application.Cover);
                storeDetailCatalog.ColorId = Convert.ToInt32(application.Color);
                storeDetailCatalog.ImprintId = Convert.ToInt32(application.Imprint);
                if (application.ProductId == 39) storeDetailCatalog.SupplementId = Convert.ToInt32(application.Supplement);
                if (storeDetailCatalog.ImprintId != 18) storeDetailCatalog.IsArtworkToProof = application.IsArtworkToProof;
                
                if ((storeDetailCatalog.AreaId == 8 || storeDetailCatalog.AreaId == 25) && (storeDetailCatalog.ImprintId == 20 || (storeDetailCatalog.ImprintId == 21 && storeDetailCatalog.ArtworkOption == "PRINT")))
                {
                    storeDetailCatalog.Line1 = application.Line1;
                    storeDetailCatalog.Line2 = application.Line2;
                    storeDetailCatalog.Line3 = application.Line3;
                    storeDetailCatalog.Line4 = application.Line4;
                    storeDetailCatalog.Line5 = application.Line5;
                    storeDetailCatalog.Line6 = application.Line6;
                }
                else
                {
                    storeDetailCatalog.Line1 = null;
                    storeDetailCatalog.Line2 = null;
                    storeDetailCatalog.Line3 = null;
                    storeDetailCatalog.Line4 = null;
                    storeDetailCatalog.Line5 = null;
                    storeDetailCatalog.Line6 = null;
                }

                if ((storeDetailCatalog.AreaId == 9 || storeDetailCatalog.AreaId == 25) && (storeDetailCatalog.ImprintId == 20 || (storeDetailCatalog.ImprintId == 21 && storeDetailCatalog.ArtworkOption == "PRINT")))
                {
                    storeDetailCatalog.BackLine1 = application.BackLine1;
                    storeDetailCatalog.BackLine2 = application.BackLine2;
                    storeDetailCatalog.BackLine3 = application.BackLine3;
                    storeDetailCatalog.BackLine4 = application.BackLine4;
                }
                else
                {
                    storeDetailCatalog.BackLine1 = null;
                    storeDetailCatalog.BackLine2 = null;
                    storeDetailCatalog.BackLine3 = null;
                    storeDetailCatalog.BackLine4 = null;
                }
                storeDetailCatalog.UpdateDate = DateTime.UtcNow;
                storeDetailCatalog.UpdateSource = "ApplicationController - EditCatalogs";

                ProcessCommand(StoreService, FulfilmentService, order, null, application.ActionName);
                StoreService.SaveChanges();
                if (application.ActionName == ApplicationController.COMMAND_REJECT)
                    return RedirectToAction("List", "Orders");
                else
                    return RedirectToAction("Edit", "Application", new { id = application.OrderDetailId });
            }
            else
            {
                return View("../Store/Application/Catalogs", application);
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult EditOrderDetailProduct(OrderDetailApplicationModel application)
        {
            if (ModelState.IsValid)
            {
                StoreOrderDetail orderDetail = StoreService.GetAll<StoreOrderDetail>().Where(detail => detail.Id == application.OrderDetailId).FirstOrDefault();
                if (orderDetail == null) throw new Exception("Invalid id, could not find the OrderDetail record");
                StoreOrder order = orderDetail.Order;
                if (order == null) throw new Exception("Invalid reference to an order");
                order.ExternalReference = application.ExternalReference;
                order = UpdateCompanyInformation(application, order);
                    
                //Update Catalog Information
                if(orderDetail.Product != null)
                {
                    switch(orderDetail.Product.Id)
                    {
                        case 46:
                            if (application.OptionId.HasValue)
                            {
                                orderDetail.OptionId = application.OptionId;
                                orderDetail.Cost = ASISmartSalesHelper.GetCost(application.OptionId.Value);
                            }
                            orderDetail.Quantity = Convert.ToInt32(application.Quantity);
                            break;
                        case 62:
                            orderDetail.AcceptedByName = application.AcceptedByName;
                            break;
                        default:
                            orderDetail.Quantity = Convert.ToInt32(application.Quantity);
                            break;
                    }
                }
                StoreAddress address = order.Company.GetCompanyShippingAddress();
                StoreService.UpdateTaxAndShipping(order);
                orderDetail.UpdateDate = DateTime.UtcNow;
                orderDetail.UpdateSource = "ApplicationController - EditCatalogs";

                ProcessCommand(StoreService, FulfilmentService, order, null, application.ActionName);
                StoreService.SaveChanges();
                if (application.ActionName == ApplicationController.COMMAND_REJECT)
                    return RedirectToAction("List", "Orders");
                else
                    return RedirectToAction("Edit", "Application", new { id = application.OrderDetailId });
            }
            else
            {
                return View("../Store/Application/OrderDetailProduct", application);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult EditESPAdvertising(ESPAdvertisingModel application)
        {
            if (ModelState.IsValid)
            {
                StoreOrderDetail orderDetail = StoreService.GetAll<StoreOrderDetail>().Where(detail => detail.Id == application.OrderDetailId).FirstOrDefault();
                if (orderDetail == null) throw new Exception("Invalid id, could not find the OrderDetail record");
                StoreOrder order = orderDetail.Order;
                if (order == null) throw new Exception("Invalid reference to an order");
                order.ExternalReference = application.ExternalReference;
                order = UpdateCompanyInformation(application, order);

                #region Update ESP Advertising information
                StoreDetailESPAdvertising espAdvertising = StoreService.GetAll<StoreDetailESPAdvertising>().Where(product => product.OrderDetailId == orderDetail.Id).SingleOrDefault();
                if (espAdvertising == null) throw new Exception("Invalid id, could not find the Catalog information record");

                if (orderDetail.Product != null)
                {
                    switch (orderDetail.Product.Id)
                    {
                        case 49:
                            espAdvertising.FirstItemList = application.NumberOfItems_First;
                            break;
                        case 50:
                            espAdvertising.FirstItemList = application.NumberOfItems_First;
                            espAdvertising.FirstOptionId = application.Products_OptionId_First;
                            if (!string.IsNullOrEmpty(espAdvertising.FirstItemList)) orderDetail.Cost = ESPAdvertisingHelper.ESPAdvertising_CLEARANCE_COST[1];

                            espAdvertising.SecondItemList = application.NumberOfItems_Second;
                            espAdvertising.SecondOptionId = application.Products_OptionId_Second;
                            if (!string.IsNullOrEmpty(espAdvertising.SecondItemList)) orderDetail.Cost += ESPAdvertisingHelper.ESPAdvertising_NEW_COST[1];

                            espAdvertising.ThirdItemList = application.NumberOfItems_Third;
                            espAdvertising.ThirdOptionId = application.Products_OptionId_Third;
                            if(!string.IsNullOrEmpty(espAdvertising.ThirdItemList)) orderDetail.Cost += ESPAdvertisingHelper.ESPAdvertising_RUSH_COST[1];
                            break;
                        case 52:
                            List<string> LoginScreen_Dates = new List<string>();
                            List<DateTime> updatedDateList = new List<DateTime>();
                            LoginScreen_Dates = System.Text.RegularExpressions.Regex.Split(string.IsNullOrEmpty(application.LoginScreen_Dates) ? string.Empty : application.LoginScreen_Dates, "\r\n").ToList();
                            LoginScreen_Dates = LoginScreen_Dates.Where(u => u.ToString() != string.Empty).ToList();
                            List<StoreDetailESPAdvertisingItem> loginScreen_previousItems = StoreService.GetAll<StoreDetailESPAdvertisingItem>().Where(details => details.OrderDetailId == application.OrderDetailId).ToList();
                            //Adding or updating exisitng records
                            if (LoginScreen_Dates != null && LoginScreen_Dates.Count > 0)
                            {
                                int count = 1;
                                orderDetail.Cost = 0;
                                foreach (string slecteddate in LoginScreen_Dates)
                                {
                                    string[] dateString = slecteddate.Split('-');
                                    DateTime date = new DateTime(int.Parse(dateString[2]), int.Parse(dateString[1]), int.Parse(dateString[0]));
                                    StoreDetailESPAdvertisingItem existingItem = loginScreen_previousItems.Where(item => item.AdSelectedDate == date).SingleOrDefault();
                                    if (existingItem != null)
                                    {
                                        existingItem.Sequence = count++;
                                        existingItem.UpdateDate = DateTime.UtcNow;
                                        existingItem.UpdateSource = "ApplicationController - EditESPAdvertising";
                                        orderDetail.Cost += GetCostBasedOnday(existingItem.AdSelectedDate);
                                        StoreService.Update<StoreDetailESPAdvertisingItem>(existingItem);
                                    }
                                    else
                                    {
                                        StoreDetailESPAdvertisingItem newitem = new StoreDetailESPAdvertisingItem();
                                        newitem.AdSelectedDate = date;
                                        newitem.Sequence = count++;
                                        newitem.OrderDetailId = application.OrderDetailId;
                                        newitem.CreateDate = DateTime.UtcNow;
                                        newitem.UpdateDate = DateTime.UtcNow;
                                        orderDetail.Cost += GetCostBasedOnday(newitem.AdSelectedDate);
                                        newitem.UpdateSource = "ApplicationController - EditESPAdvertising";
                                        StoreService.Add<StoreDetailESPAdvertisingItem>(newitem);
                                    }
                                    updatedDateList.Add(date);
                                }
                            }
                            //Deleting extra if any extra dates added in earlier submit.
                            if (updatedDateList != null && updatedDateList.Count > 0 && loginScreen_previousItems != null && loginScreen_previousItems.Count > 0 && loginScreen_previousItems.Count != updatedDateList.Count)
                            {
                                foreach (StoreDetailESPAdvertisingItem item in loginScreen_previousItems)
                                {
                                    if(updatedDateList.Where(date => date == item.AdSelectedDate).SingleOrDefault() == DateTime.MinValue)
                                        StoreService.Delete<StoreDetailESPAdvertisingItem>(item);
                                }
                            }
                            break;
                        case 54:
                            espAdvertising.FirstOptionId = application.Products_OptionId_First + 1;
                            orderDetail.Cost = Convert.ToDecimal(ESPAdvertisingHelper.ESPAdvertising_PROMO_CAFE_COST[application.Products_OptionId_First]);
                            break;
                    }
                    espAdvertising.UpdateDate = DateTime.UtcNow;
                    espAdvertising.UpdateSource = "ApplicationController - EditESPAdvertising";
                }
                #endregion

                //Update ESP Advertising Information
                StoreAddress address = order.Company.GetCompanyShippingAddress();
                StoreService.UpdateTaxAndShipping(order);
                orderDetail.UpdateDate = DateTime.UtcNow;
                orderDetail.UpdateSource = "ApplicationController - EditESPAdvertising";

                ProcessCommand(StoreService, FulfilmentService, order, null, application.ActionName);
                StoreService.SaveChanges();
                if (application.ActionName == ApplicationController.COMMAND_REJECT)
                    return RedirectToAction("List", "Orders");
                else
                    return RedirectToAction("Edit", "Application", new { id = application.OrderDetailId });
            }
            else
            {
                return View("../Store/Application/ESPAdvertising", application);
            }
        }

        private int GetCostBasedOnday(DateTime dt)
        {
            int cost = 0;
            if (dt != null)
            {
                int day = Convert.ToInt32(dt.DayOfWeek);
                if (day == 0 || day == 6) return @ESPAdvertisingHelper.LoginScreenWeekendPrice;
                else return @ESPAdvertisingHelper.LoginScreenWeekDayPrice;
            }
            return cost;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult EditPayForPlacement(ESPPayForPlacementModel application)
        {
            if (ModelState.IsValid)
            {
                StoreOrderDetail orderDetail = StoreService.GetAll<StoreOrderDetail>().Where(detail => detail.Id == application.OrderDetailId).FirstOrDefault();
                if (orderDetail == null) throw new Exception("Invalid id, could not find the OrderDetail record");
                StoreOrder order = orderDetail.Order;
                if (order == null) throw new Exception("Invalid reference to an order");
                order.ExternalReference = application.ExternalReference;
                order = UpdateCompanyInformation(application, order);

                #region Update ESP Advertising information
                IList<StoreDetailPayForPlacement> espPayForPlacements = StoreService.GetAll<StoreDetailPayForPlacement>().Where(product => product.OrderDetailId == orderDetail.Id).ToList();
                if (espPayForPlacements == null) throw new Exception("Invalid id, could not find the Catalog information record");

                if (orderDetail.Product != null && application.Categries != null && application.Categries.Count > 0)
                {
                    orderDetail.Cost = 0.0m;
                    foreach (PFPCategory detail in application.Categries)
                    {
                        StoreDetailPayForPlacement detailPayForPlacement = espPayForPlacements.Where(placement => placement.CategoryName == detail.CategoryName).SingleOrDefault();
                        if (detail.IsSelected)
                        {
                            if (detailPayForPlacement == null)
                            {
                                detailPayForPlacement = new StoreDetailPayForPlacement();
                                detailPayForPlacement.CreateDate = DateTime.UtcNow;
                                StoreService.Add<StoreDetailPayForPlacement>(detailPayForPlacement);
                            }
                            detailPayForPlacement.CategoryName = detail.CategoryName;
                            if(application.ProductId == 47) detailPayForPlacement.CPMOption = detail.CPMOption;
                            detailPayForPlacement.PaymentType = detail.PaymentOption;
                            decimal totalCost = 0.0m;
                            if (detail.PaymentOption == "FB" && !string.IsNullOrEmpty(detail.PaymentAmount))
                            {
                                detailPayForPlacement.Cost = Convert.ToDecimal(detail.PaymentAmount);
                                totalCost = Convert.ToDecimal(detail.PaymentAmount);
                            }
                            else if (detail.PaymentOption == "IPM" && !string.IsNullOrEmpty(detail.Impressions))
                            {
                                int impressionsCount = Convert.ToInt32(detail.Impressions);
                                detailPayForPlacement.ImpressionsRequested = impressionsCount;
                                if (application.ProductId == 47) totalCost = (Convert.ToDecimal(impressionsCount) / 1000) * ESPAdvertisingHelper.ESPAdvertising_PFP_COST[detail.CPMOption];
                                else if (application.ProductId == 63 && order.IsStoreRequest && orderDetail.Product != null) totalCost = (Convert.ToDecimal(impressionsCount) / 1000) * orderDetail.Product.Cost;
                                else if(application.ProductId == 63 && !order.IsStoreRequest && orderDetail.Product != null)
                                {
                                    decimal specialCost = StoreService.GetAll<ContextProductSequence>(true).Where(prod => prod.Product.Id == application.ProductId).Select(item => item.Cost).SingleOrDefault();
                                    totalCost = (Convert.ToDecimal(impressionsCount) / 1000) * specialCost;
                                }
                            }
                            orderDetail.Cost += totalCost;
                            detailPayForPlacement.OrderDetailId = orderDetail.Id;
                            detailPayForPlacement.UpdateDate = DateTime.UtcNow;
                            detailPayForPlacement.UpdateSource = "ApplicationController - EditPayForPlacement";
                        }
                        else if(detailPayForPlacement != null) StoreService.Delete<StoreDetailPayForPlacement>(detailPayForPlacement);
                    }
                }
                #endregion

                //Update ESP Advertising Information
                StoreAddress address = order.Company.GetCompanyShippingAddress();
                StoreService.UpdateTaxAndShipping(order);
                orderDetail.UpdateDate = DateTime.UtcNow;
                orderDetail.UpdateSource = "ApplicationController - EditESPAdvertising";

                ProcessCommand(StoreService, FulfilmentService, order, null, application.ActionName);
                StoreService.SaveChanges();
                if (application.ActionName == ApplicationController.COMMAND_REJECT)
                    return RedirectToAction("List", "Orders");
                else
                    return RedirectToAction("Edit", "Application", new { id = application.OrderDetailId });
            }
            else
            {
                return View("../Store/Application/PayForPlacement", application);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public virtual ActionResult EditEmailExpress(EmailExpressModel application)
        {
            if (ModelState.IsValid)
            {
                StoreOrderDetail orderDetail = StoreService.GetAll<StoreOrderDetail>().Where(detail => detail.Id == application.OrderDetailId).FirstOrDefault();
                if (orderDetail == null) throw new Exception("Invalid id, could not find the OrderDetail record");
                StoreOrder order = orderDetail.Order;
                if (order == null) throw new Exception("Invalid reference to an order");
                order.ExternalReference = application.ExternalReference;
                order = UpdateCompanyInformation(application, order);

                #region Update Email Express information
                StoreDetailEmailExpress emailexpress = StoreService.GetAll<StoreDetailEmailExpress>().Where(product => product.OrderDetailId == orderDetail.Id).SingleOrDefault();
                if (emailexpress == null) throw new Exception("Invalid id, could not find the Catalog information record");

                if (orderDetail.Product != null)
                {
                    List<string> Dates = new List<string>();
                    List<DateTime> updatedDateList = new List<DateTime>();
                    Dates = System.Text.RegularExpressions.Regex.Split(string.IsNullOrEmpty(application.Dates) ? string.Empty : application.Dates, "\r\n").ToList();
                    Dates = Dates.Where(u => u.ToString() != string.Empty).ToList();
                    List<StoreDetailEmailExpressItem> loginScreen_previousItems = StoreService.GetAll<StoreDetailEmailExpressItem>().Where(details => details.OrderDetailId == application.OrderDetailId).ToList();
                    if (application.ItemTypeId == 3 || application.ItemTypeId == 4)
                    {
                        foreach (StoreDetailEmailExpressItem item in loginScreen_previousItems)
                        {
                            StoreService.Delete<StoreDetailEmailExpressItem>(item);
                        }
                    }
                    else
                    {
                        //Adding or updating exisitng records
                        if (Dates != null && Dates.Count > 0)
                        {
                            int count = 1;
                            foreach (string slecteddate in Dates)
                            {
                                DateTime date = DateTime.Parse(slecteddate);
                                StoreDetailEmailExpressItem existingItem = loginScreen_previousItems.Where(item => item.AdSelectedDate == date).SingleOrDefault();
                                if (existingItem != null)
                                {
                                    existingItem.Sequence = count++;
                                    existingItem.UpdateDate = DateTime.UtcNow;
                                    existingItem.UpdateSource = "ApplicationController - EditEmailExpress";
                                    StoreService.Update<StoreDetailEmailExpressItem>(existingItem);
                                }
                                else
                                {
                                    StoreDetailEmailExpressItem newitem = new StoreDetailEmailExpressItem();
                                    newitem.AdSelectedDate = date;
                                    newitem.Sequence = count++;
                                    newitem.OrderDetailId = application.OrderDetailId;
                                    newitem.CreateDate = DateTime.UtcNow;
                                    newitem.UpdateDate = DateTime.UtcNow;
                                    newitem.UpdateSource = "ApplicationController - EditEmailExpress";
                                    StoreService.Add<StoreDetailEmailExpressItem>(newitem);
                                }
                                updatedDateList.Add(date);
                            }
                        }
                        //Deleting extra if any extra dates added in earlier submit.
                        if (updatedDateList != null && updatedDateList.Count > 0 && loginScreen_previousItems != null && loginScreen_previousItems.Count > 0 && loginScreen_previousItems.Count != updatedDateList.Count)
                        {
                            foreach (StoreDetailEmailExpressItem item in loginScreen_previousItems)
                            {
                                if (updatedDateList.Where(date => date == item.AdSelectedDate).SingleOrDefault() == DateTime.MinValue)
                                    StoreService.Delete<StoreDetailEmailExpressItem>(item);
                            }
                        }
                    }
                    emailexpress.ItemTypeId = application.ItemTypeId;
                    if (application.ItemTypeId == 3 || application.ItemTypeId == 4)
                    {
                        orderDetail.Quantity = 1;
                        emailexpress.NumberOfDates = 0;
                    }
                    else
                    {
                        if (updatedDateList != null) orderDetail.Quantity = updatedDateList.Count;
                        emailexpress.NumberOfDates = updatedDateList.Count;
                    }
                    orderDetail.Cost = EmailExpressHelper.GetCost(application.ItemTypeId, updatedDateList.Count);
                    emailexpress.UpdateDate = DateTime.UtcNow;
                    emailexpress.UpdateSource = "ApplicationController - EditEmailExpress";
                }
                #endregion

                //Update Email Express Information
                if (order.Company != null)
                {
                    StoreAddress address = order.Company.GetCompanyShippingAddress();
                    StoreService.UpdateTaxAndShipping(order);
                    orderDetail.UpdateDate = DateTime.UtcNow;
                    orderDetail.UpdateSource = "ApplicationController - EditEmailExpress";
                }

                ProcessCommand(StoreService, FulfilmentService, order, null, application.ActionName);
                StoreService.SaveChanges();
                if (application.ActionName == ApplicationController.COMMAND_REJECT)
                    return RedirectToAction("List", "Orders");
                else
                    return RedirectToAction("Edit", "Application", new { id = application.OrderDetailId });
            }
            else
            {
                return View("../Store/Application/EmailExpress", application);
            }
        }

        /// <summary>
        /// Common code between Edit supplier and distributor
        /// </summary>
        /// <param name="storeService"></param>
        /// <param name="order"></param>
        /// <param name="applicationId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private void ProcessCommand(IStoreService storeService, IFulfilmentService fulfilmentService, StoreOrder order, StoreDetailApplication application, string command)
        {
            if (command == ApplicationController.COMMAND_ACCEPT)
            {
                //make sure we have external reference
                if (string.IsNullOrEmpty(order.ExternalReference)) throw new Exception("You need to specify a Timms id to approve an order");

                //make sure timms id contains numbers only
                int num;
                bool success = int.TryParse(order.ExternalReference, out num);
                if (!success) throw new Exception("Timms id must be numbers only.");
                
                fulfilmentService.Process(order, application);
                order.ProcessStatus = OrderStatus.Approved;
            }
            else if (command == ApplicationController.COMMAND_REJECT)
            {
                order.ProcessStatus = OrderStatus.Rejected;
                try
                {
                    if (CreditCardService != null && order.CreditCard != null && !string.IsNullOrEmpty(order.CreditCard.ExternalReference))
                        CreditCardService.Delete(order.CreditCard.ExternalReference);
                }
                catch (Exception exception)
                {
                    ILogService log = LogService.GetLog(this.GetType());
                    log.Error("Could not remove a credit card record: " + exception.Message);
                }
                order.CreditCard.ExternalReference = null;
            }
        }

        /// <summary>
        /// UpdateCompanyInformation
        /// </summary>
        /// <param name="model"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private StoreOrder UpdateCompanyInformation(IMembershipModel model, StoreOrder order)
        {
            if (order == null || model == null) return null;
            //Update in company fields
            if (model.Company != null && order.Company != null)
            {
                order.Company.Name = model.Company;
                order.Company.Phone =model.Phone;
                order.Company.WebURL = model.BillingWebUrl;
                order.Company.ASINumber = model.ASINumber;
                order.UpdateDate = DateTime.UtcNow;
                order.UpdateSource = "ASI Admin Application - UpdateCompanyInformation";

                StoreCompanyAddress companyAddress = order.Company.Addresses.Where(add => !add.IsShipping && !add.IsBilling).FirstOrDefault();
                if (companyAddress != null && CompareAddresses(companyAddress.Address, model.Address1, model.Address2, model.State, model.City,model.Country, model.Zip))
                {
                    companyAddress.Address.Street1 = model.Address1;
                    companyAddress.Address.Street2 = model.Address2;
                    companyAddress.Address.City = model.City;
                    companyAddress.Address.Zip = model.Zip;
                    companyAddress.Address.State = model.State;
                    companyAddress.Address.Country = model.Country;
                    companyAddress.Address.UpdateDate = DateTime.UtcNow;
                    companyAddress.Address.UpdateSource = "ASI Admin Application - UpdateCompanyInformation";
                }
                //Set contact information
                if (model.Contacts != null)
                {
                    int i = 0;
                    foreach(StoreIndividual individual in model.Contacts)
                    {
                        order.Company.Individuals.ElementAt(i).IsPrimary = individual.IsPrimary;
                        order.Company.Individuals.ElementAt(i).FirstName = individual.FirstName;
                        order.Company.Individuals.ElementAt(i).LastName = individual.LastName;
                        order.Company.Individuals.ElementAt(i).Email = individual.Email;
                        order.Company.Individuals.ElementAt(i).Title = individual.Title;
                        order.Company.Individuals.ElementAt(i).Phone = individual.Phone;
                        order.Company.Individuals.ElementAt(i).Fax = individual.Fax;
                        order.Company.Individuals.ElementAt(i).UpdateDate = DateTime.UtcNow;
                        order.Company.Individuals.ElementAt(i).UpdateSource = "ASI Admin Application - UpdateCompanyInformation";
                        i++;
                    }
                }
            
                //Set billing information
                if (order.BillingIndividual != null)
                {
                    if(CompareIndividuals(order.BillingIndividual, model.BillingPhone, model.BillingFax, model.BillingEmail))
                    {
                        order.BillingIndividual.Email = model.BillingEmail;
                        order.BillingIndividual.Fax = model.BillingFax;
                        order.BillingIndividual.Phone = model.BillingPhone;
                        order.BillingIndividual.UpdateDate = DateTime.UtcNow;
                        order.BillingIndividual.UpdateSource = "ASI Admin Application - UpdateCompanyInformation";
                    }
                    if (order.BillingIndividual.Address != null)
                    {
                        if (CompareAddresses(order.BillingIndividual.Address, model.BillingAddress1, model.BillingAddress2, model.BillingState, model.BillingCity, model.BillingCountry, model.BillingZip))
                        {
                            if (companyAddress.Address.Id == order.BillingIndividual.Address.Id)
                            {
                                //address was not yet in use, create a new one for both individual and company
                                order.BillingIndividual.Address = new StoreAddress()
                                {
                                    CreateDate = DateTime.UtcNow,
                                };
                                order.Company.Addresses.Add(new StoreCompanyAddress()
                                {
                                    Address = order.BillingIndividual.Address,
                                    IsBilling = true,
                                    CreateDate = DateTime.UtcNow,
                                    UpdateDate = DateTime.UtcNow,
                                    UpdateSource = "ASI Admin Application - UpdateCompanyInformation",
                                });
                                StoreService.Add<StoreAddress>(order.BillingIndividual.Address);
                            }
                            order.BillingIndividual.Address.Street1 = model.BillingAddress1;
                            order.BillingIndividual.Address.Street2 = model.BillingAddress1;
                            order.BillingIndividual.Address.City = model.BillingCity;
                            order.BillingIndividual.Address.State = model.BillingState;
                            order.BillingIndividual.Address.Zip = model.BillingZip;
                            order.BillingIndividual.Address.Country = model.BillingCountry;
                            order.BillingIndividual.Address.UpdateDate = DateTime.UtcNow;
                            order.BillingIndividual.Address.UpdateSource = "ASI Admin Application - UpdateCompanyInformation";
                        }
                    }
                }
                //Set shipping information
                if (model.HasShipAddress)
                {
                    StoreCompanyAddress address = order.Company.Addresses.Where(add => add.IsShipping).FirstOrDefault();
                    if (address != null && address.Address != null && CompareAddresses(address.Address, model.ShippingStreet1, model.ShippingStreet2, model.ShippingState, model.ShippingCity, model.ShippingCountry, model.ShippingZip))
                    {
                        address.Address.Street1 = model.ShippingStreet1;
                        address.Address.Street2 = model.ShippingStreet2;
                        address.Address.City = model.ShippingCity;
                        address.Address.Zip = model.ShippingZip;
                        address.Address.State = model.ShippingState;
                        address.Address.Country = model.ShippingCountry;
                        address.Address.UpdateDate = DateTime.UtcNow;
                        address.Address.UpdateSource = "ASI Admin Application - UpdateCompanyInformation";
                    }
                }
            }
            return order;
        }

        private bool CompareIndividuals(StoreIndividual individual1, string phone, string fax, string email)
        {
            bool isChangesInInidividuals = false;

            if (individual1 == null || individual1.Phone != phone || individual1.Fax != fax || individual1.Email != email)
                isChangesInInidividuals = true;
            else
                isChangesInInidividuals = false;

            return isChangesInInidividuals;
        }

        private bool CompareAddresses(StoreAddress address1, string street1, string street2, string state, string city, string country, string zip)
        {
            bool isChangesInAddress = false;

            if (address1 == null || address1.Street1 != street1 || address1.Street2 != street2 || address1.State != state || address1.City != city || address1.Country != country || address1.Zip != zip)
                isChangesInAddress = true;
            else
                isChangesInAddress = false;

            return isChangesInAddress;
        }

        private StoreOrderDetail UpdateMagazineSubscriptionInformation(MagazinesApplicationModel application, StoreOrderDetail orderDetail)
        {
            //copy decorating types bool to the collections
            foreach (StoreMagazineSubscription subscription in application.Subscriptions)
            {
                StoreMagazineSubscription existing = orderDetail.MagazineSubscriptions.Where(item => item.Id == subscription.Id).SingleOrDefault();
                if (subscription.Contact != null && existing != null)
                {
                    existing.CompanyName = subscription.CompanyName;
                    existing.ASINumber = subscription.ASINumber;
                    existing.IsDigitalVersion = subscription.IsDigitalVersion;
                    existing.PrimaryBusiness = subscription.PrimaryBusiness;
                    existing.PrimaryBusinessOtherDesc = subscription.PrimaryBusinessOtherDesc;
                    existing.UpdateDate = DateTime.UtcNow;
                    existing.UpdateSource = "ASI Admin Application - EditMagazines";
                    StoreService.Update<StoreMagazineSubscription>(existing);

                    if (subscription.Contact != null && existing.Contact != null)
                    {
                        existing.Contact.FirstName = subscription.Contact.FirstName;
                        existing.Contact.LastName = subscription.Contact.LastName;
                        existing.Contact.Email = subscription.Contact.Email;
                        existing.Contact.Title = subscription.Contact.Title;
                        existing.Contact.Phone = subscription.Contact.Phone;
                        existing.Contact.Fax = subscription.Contact.Fax;
                        existing.Contact.Department = subscription.Contact.Department;
                        existing.Contact.UpdateDate = DateTime.UtcNow;
                        existing.Contact.UpdateSource = "ASI Admin Application - EditMagazines";
                        StoreService.Update<StoreIndividual>(existing.Contact);
                    }

                    if (subscription.Contact.Address != null && existing.Contact.Address != null)
                    {
                        existing.Contact.Address.Street1 = subscription.Contact.Address.Street1;
                        existing.Contact.Address.Street2 = subscription.Contact.Address.Street2;
                        existing.Contact.Address.City = subscription.Contact.Address.City;
                        existing.Contact.Address.State = subscription.Contact.Address.State;
                        existing.Contact.Address.Zip = subscription.Contact.Address.Zip;
                        existing.Contact.Address.Country = subscription.Contact.Address.Country;
                        existing.Contact.Address.UpdateDate = DateTime.UtcNow;
                        existing.Contact.Address.UpdateSource = "ASI Admin Application - EditMagazines";
                        StoreService.Update<StoreAddress>(existing.Contact.Address);
                    }
                }
            }
            orderDetail.UpdateDate = DateTime.UtcNow;
            orderDetail.UpdateSource = "ASI Admin Application - EditMagazines";
            StoreService.Update<StoreOrderDetail>(orderDetail);
            return orderDetail;
        }
    }
}