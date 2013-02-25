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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Reject(int orderid, string startDate, string endDate)
        {
            // TODO
            // if valid order id
            // reject order, redirect to "../Store/Admin/Orders"
            //Order order = StoreObjectService.GetAll<Order>().Where(theOrder => theOrder.Id == orderid).SingleOrDefault();
            //PageViewModel viewOrders = new PageViewModel(storeo);
            
            //foreach (OrderDetail item in order.OrderDetails)
            //{
            //    CompletedOrder closedOrder = new CompletedOrder();
            //    closedOrder.SetOrderDetail(item);
            //    viewOrders.CompletedOrders.Add(closedOrder);
            //}
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Accept(int orderid, string startDate, string endDate)
       { 
            // TODO
            // if valid order id
            // accept order, redirect to "../Store/Admin/Orders"
            //Order order = StoreObjectService.GetAll<Order>().Where(theOrder => theOrder.Id == orderid).SingleOrDefault();
            //PageViewModel viewOrders = new PageViewModel();

            //foreach (OrderDetail item in order.OrderDetails)
            //{
            //    CompletedOrder closedOrder = new CompletedOrder();
            //    closedOrder.SetOrderDetail(item);
            //    viewOrders.CompletedOrders.Add(closedOrder);
            //}
            return null;
        }
    }
}
