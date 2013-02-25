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
        public virtual ActionResult List(Nullable<DateTime> dateStart, Nullable<DateTime> dateEnd, Nullable<int> productid, String formtab, String ordertab)
        {
            if (dateStart == null || dateEnd == null)
            {
                dateStart = DateTime.Now;
                dateEnd = DateTime.Now;
            }

            if (dateStart > dateEnd) ViewBag.Message = Resource.StoreDateErrorMessage;

            IList<OrderDetail> orderDetails =            
                StoreObjectService.GetAll<OrderDetail>(true).Where
                (detail => detail.Order.DateCreated >= dateStart && detail.Order.DateCreated <= dateEnd
                && detail.Order.Status == true).OrderByDescending(detail => detail.OrderId).ToList();

            if (productid != null) orderDetails = orderDetails.Where(detail => detail.Product.Id == productid).ToList();
            
            PageViewModel viewModel = new PageViewModel(StoreObjectService);
            if (ordertab == PageViewModel.PENDING) viewModel.ConstructPendingOrders(orderDetails);
            if (ordertab == PageViewModel.COMPLETED) viewModel.ConstructCompletedOrders(orderDetails);

            viewModel.FormTab = String.IsNullOrEmpty(formtab) ? PageViewModel.DATE : formtab;
            viewModel.OrderTab = String.IsNullOrEmpty(ordertab) ? PageViewModel.COMPLETED : ordertab;

            ViewBag.StartDate = dateStart.Value.ToString("MM/dd/yyyy");
            ViewBag.EndDate = dateEnd.Value.ToString("MM/dd/yyyy");

            return View("../Store/Admin/Orders", viewModel);
        }
    }
}
