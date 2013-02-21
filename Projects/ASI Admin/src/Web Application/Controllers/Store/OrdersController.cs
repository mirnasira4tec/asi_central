using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.Models.Store;
using System.Text;

namespace asi.asicentral.web.Controllers.Store
{
    public class OrdersController : Controller
    {
        public IStoreService StoreObjectService { get; set; }

        // TODO find a better way to get list of orders by order id
        //[HttpGet]
        //public virtual ActionResult List(int orderid)
        //{
        //    List<OrderDetail> orderDetails = StoreObjectService.GetAll<OrderDetail>().Where
        //        (detail => detail.OrderId == orderid).ToList();

        //    ViewOrders viewOrders = new ViewOrders();
        //    foreach (OrderDetail orderdetail in orderDetails)
        //    {
        //        ClosedOrder order = new ClosedOrder();
        //        order.SetOrderDetail(orderdetail);
        //        order.SetApplicationFromService(this.StoreObjectService);
        //        // TODO find a better way to get application
        //        // TODO find a better way to get email from aspnetmembership table
        //        viewOrders.closedOrders.Add(order);
        //    }

        //    return View("../Store/Admin/Orders", viewOrders);
        //}

        [HttpGet]
        public virtual ActionResult List(Nullable<DateTime> startDate, Nullable<DateTime> endDate, String testing)
        {

            if (startDate == null || endDate == null)
            {
                startDate = DateTime.Now;
                endDate = DateTime.Now;
            }

            if (startDate > endDate) ViewBag.Message = Resource.StoreDateErrorMessage;

            ViewBag.StartDate = startDate.Value.ToString("MM/dd/yyyy");
            ViewBag.EndDate = endDate.Value.ToString("MM/dd/yyyy");

            // get closed orders: status = true means closed 
            IList<OrderDetail> orderDetails =
                StoreObjectService.GetAll<OrderDetail>(true).Where
                (detail => detail.Order.DateCreated >= startDate && detail.Order.DateCreated <= endDate
                && detail.Order.Status == true).OrderByDescending(detail => detail.OrderId).ToList();

            ViewOrders viewOrders = new ViewOrders();
            foreach (OrderDetail order in orderDetails)
            {
                ClosedOrder closedOrder = new ClosedOrder();
                closedOrder.SetOrderDetail(order);
                closedOrder.SetApplicationFromService(this.StoreObjectService);
                // TODO find a better way to get email from aspnetmembership table
                ASPNetMembership member = StoreObjectService.GetAll<ASPNetMembership>().Where(m => m.UserId == order.Order.UserId).SingleOrDefault();
                if (member != null) closedOrder.Email = member.Email;

                viewOrders.closedOrders.Add(closedOrder);
            }

            return View("../Store/Admin/Orders", viewOrders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Reject(int orderid, string startDate, string endDate)
        {
            // TODO
            // if valid order id
            // reject order, redirect to "../Store/Admin/Orders"
            Order order = StoreObjectService.GetAll<Order>().Where(theOrder => theOrder.Id == orderid).SingleOrDefault();
            ViewOrders viewOrders = new ViewOrders();

            foreach (OrderDetail item in order.OrderDetails)
            {
                ClosedOrder closedOrder = new ClosedOrder();
                closedOrder.SetOrderDetail(item);
                viewOrders.closedOrders.Add(closedOrder);
            }
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Accept(int orderid, string startDate, string endDate)
        {
            // TODO
            // if valid order id
            // accept order, redirect to "../Store/Admin/Orders"
            Order order = StoreObjectService.GetAll<Order>().Where(theOrder => theOrder.Id == orderid).SingleOrDefault();
            ViewOrders viewOrders = new ViewOrders();

            foreach (OrderDetail item in order.OrderDetails)
            {
                ClosedOrder closedOrder = new ClosedOrder();
                closedOrder.SetOrderDetail(item);
                viewOrders.closedOrders.Add(closedOrder);
            }
            return null;
        }
    }
}
