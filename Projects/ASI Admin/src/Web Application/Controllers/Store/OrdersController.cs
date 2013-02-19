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

        [HttpGet]
        public virtual ActionResult List(string startDate, string endDate)
        {
            DateTime dateTimeStart;
            DateTime dateTimeEnd;

            try
            {
                dateTimeStart = DateTime.Parse(startDate);
                dateTimeEnd = DateTime.Parse(endDate);
            }
            catch (Exception)
            {
                dateTimeStart = DateTime.Now;
                dateTimeEnd = DateTime.Now;
            }

            if (dateTimeStart > dateTimeEnd) ViewBag.Message = Resource.StoreDateErrorMessage;

            ViewBag.StartDate = dateTimeStart.ToString("MM/dd/yyyy");
            ViewBag.EndDate = dateTimeEnd.ToString("MM/dd/yyyy");

            // get closed orders: status = true means closed 
            IList<OrderDetail> orderDetails =
                StoreObjectService.GetAll<OrderDetail>(true).Where
                (detail => detail.Order.DateCreated >= dateTimeStart && detail.Order.DateCreated <= dateTimeEnd
                && detail.Order.Status == true).OrderByDescending(detail => detail.OrderId).ToList();

            ViewOrders viewOrders = new ViewOrders();
            foreach (OrderDetail order in orderDetails)
            {
                ClosedOrder closedOrder = new ClosedOrder();
                closedOrder.orderDetail = order;
                closedOrder.SetApplicationFromService(this.StoreObjectService);

                viewOrders.closedOrders.Add(closedOrder);
            }

            return View("../Store/Admin/Orders", viewOrders);
        }
    }
}
