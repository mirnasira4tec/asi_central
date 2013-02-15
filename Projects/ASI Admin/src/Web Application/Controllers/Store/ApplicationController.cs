using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.Models.Store;

namespace asi.asicentral.web.Controllers.Store
{
    public class ApplicationController : Controller
    {
        public IStoreService StoreObjectService { get; set; }

        [HttpGet]
        public virtual ActionResult Edit(OrderDetailApplication orderDetailApplication)
        {
            OrderDetailApplication application;

            application = StoreObjectService.GetAll<SupplierMembershipApplication>(true).Where(theApp => theApp.Id == orderDetailApplication.Id).SingleOrDefault() as SupplierMembershipApplication;
            if (application != null) return View("../Store/Application/Supplier", application);

            application = StoreObjectService.GetAll<DistributorMembershipApplication>(true).Where(theApp => theApp.Id == orderDetailApplication.Id).SingleOrDefault() as DistributorMembershipApplication;
            if (application != null) return View("../Store/Application/Distributor", application);

            // can't find any application - return nothing
            return new EmptyResult();
        }

        //[HttpGet]
        //public virtual ActionResult Edit(OrderDetail orderDetail)
        //{
        //    if (orderDetail == null)
        //        throw new Exception("Parameter orderDetail cannot be null");

        //    orderDetail = StoreObjectService.GetAll<OrderDetail>(true).Where(orderDtail => orderDtail.OrderId == orderDetail.OrderId && orderDtail.ProductId == orderDetail.ProductId).SingleOrDefault();
        //    if (orderDetail == null)
        //        throw new Exception("Invalid identifier for an order detail with id " + orderDetail.OrderId);

        //    OrderDetailApplication application = StoreObjectService.GetApplication(orderDetail);
        //    if (application != null)
        //    {
        //        if (application is DistributorMembershipApplication) return View("../Store/Application/Distributor", application);
        //        else if (application is SupplierMembershipApplication) return View("../Store/Application/Supplier", application);
        //    }

        //    // can't find any application - return nothing
        //    return new EmptyResult();
        //}
    }
}
