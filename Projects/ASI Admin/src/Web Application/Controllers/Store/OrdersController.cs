using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.Models.Store;
using asi.asicentral.web.Models.Store.PageModel;
using System.Text;

namespace asi.asicentral.web.Controllers.Store
{
    public class OrdersController : Controller
    {
        public IStoreService StoreObjectService { get; set; }

        [HttpGet]
        public virtual ActionResult ListByName(String firstname, String lastname)
        {
            if (String.IsNullOrEmpty(firstname) && String.IsNullOrEmpty(lastname)) ViewBag.Message = "Please enter a first or last name.";

            // TODO get order by first and last name

            ViewBag.FormTab = "name";
            return View("../Store/Admin/Orders", new PageViewModel());
        }

        [HttpGet]
        public virtual ActionResult ListByTimms(String id)
        {
            if (String.IsNullOrEmpty(id)) ViewBag.Message = "Please enter a Timms id.";

            // TODO get order by external reference

            ViewBag.FormTab = "timms";
            return View("../Store/Admin/Orders", new PageViewModel());
        }
        
        // TODO maybe there's a better way to do this
        [HttpGet]
        public virtual ActionResult ListByOrderId(Nullable<int> orderid)
        {
            if (orderid == null) ViewBag.Message = "Please enter an order id.";

            ViewBag.FormTab = "order";

            List<OrderDetail> orderDetails = StoreObjectService.GetAll<OrderDetail>().Where
                (detail => detail.OrderId == orderid).ToList();

            PageViewModel viewOrders = new PageViewModel();
            // TODO put this in the ViewOrders class
            foreach (OrderDetail orderdetail in orderDetails)
            {
                CompletedOrders order = new CompletedOrders();
                order.SetOrderDetail(orderdetail);
                order.SetApplicationFromService(this.StoreObjectService);
                // TODO find a better way to get application
                // TODO find a better way to get email from aspnetmembership table
                viewOrders.completedOrders.Add(order);
            }
            return View("../Store/Admin/Orders", viewOrders);
        }

        [HttpGet]
        [ActionName("ListByName")]
        public virtual ActionResult List(DateRange dateRange, String firstName)
        {

            return View("../Store/Admin/Orders", new PageViewModel());
        }

        [HttpGet]
        [ActionName("ListByDateRange")]
        public virtual ActionResult List(DateRange dateRange)
        {
            ViewBag.FormTab = "date";

            return View("../Store/Admin/Orders", new PageViewModel());
        }

        [HttpGet]
        public virtual ActionResult List(Nullable<DateTime> dateStart, Nullable<DateTime> dateEnd, Nullable<int> productid, String formtab)
        {
            if (dateStart == null || dateEnd == null)
            {
                dateStart = DateTime.Now;
                dateEnd = DateTime.Now;
            }

            if (dateStart > dateEnd) ViewBag.Message = Resource.StoreDateErrorMessage;

            // get closed orders: status = true means user has completed their order 
            IList<OrderDetail> orderDetails =            
                StoreObjectService.GetAll<OrderDetail>(true).Where
                (detail => detail.Order.DateCreated >= dateStart && detail.Order.DateCreated <= dateEnd
                && detail.Order.Status == true).OrderByDescending(detail => detail.OrderId).ToList();

            if (productid != null) orderDetails = orderDetails.Where(detail => detail.Product.Id == productid).ToList();
            
            PageViewModel viewOrders = new PageViewModel();

            // TODO put this in the ViewOrders class
            foreach (OrderDetail order in orderDetails)
            {
                CompletedOrders closedOrder = new CompletedOrders();
                closedOrder.SetOrderDetail(order);
                closedOrder.SetApplicationFromService(this.StoreObjectService);
                // TODO find a better way to get email from aspnetmembership table
                ASPNetMembership member = StoreObjectService.GetAll<ASPNetMembership>().Where(m => m.UserId == order.Order.UserId).SingleOrDefault();
                if (member != null) closedOrder.Email = member.Email;

                viewOrders.completedOrders.Add(closedOrder);
            }

            ViewBag.FormTab = productid == null ? "date" : "product";
            ViewBag.StartDate = dateStart.Value.ToString("MM/dd/yyyy");
            ViewBag.EndDate = dateEnd.Value.ToString("MM/dd/yyyy");
            
            return View("../Store/Admin/Orders", viewOrders);
        }
    }
}
