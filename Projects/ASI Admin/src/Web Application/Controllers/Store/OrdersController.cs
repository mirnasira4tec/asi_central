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
            IList<Order> orders =
                StoreObjectService.GetAll<Order>(true).Where
                (theOrder => theOrder.DateCreated >= dateTimeStart && theOrder.DateCreated <= dateTimeEnd
                && theOrder.Status == true).OrderBy(theOrder => theOrder.Id).ToList();

            ViewOrders viewOrders = new ViewOrders();
            foreach (Order orderItem in orders)
            {
                ClosedOrder closedOrder = new ClosedOrder();
                closedOrder.GetDataFrom(orderItem);
                viewOrders.closedOrders.Add(closedOrder);

                foreach (OrderDetail orderDetailItem in orderItem.OrderDetails)
                {
                    Detail detail = new Detail();

                    OrderDetailApplication application = StoreObjectService.GetApplication(orderDetailItem);
                    if (application != null)
                    {
                        detail.Application = application;
                        detail.HasApplication = true;
                    }

                    detail.GetDataFrom(orderDetailItem);
                    closedOrder.Details.Add(detail);
                }
            }
            return View("../Store/Admin/Orders", viewOrders);
        }
    }
}
