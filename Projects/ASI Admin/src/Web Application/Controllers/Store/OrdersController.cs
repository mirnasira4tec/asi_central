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
        public IStoreService StoreService { get; set; }
       
        [HttpGet]
        public virtual ActionResult List(Nullable<DateTime> dateStart, Nullable<DateTime> dateEnd, string product, String formtab, String ordertab)
        {
            if (dateStart > dateEnd) ViewBag.Message = Resource.StoreDateErrorMessage;

            IQueryable<OrderDetail> orderDetailQuery = StoreService.GetAll<OrderDetail>(true);
            if (string.IsNullOrEmpty(formtab)) formtab = OrderPageModel.TAB_DATE; //setting the default tab
            if (formtab == OrderPageModel.TAB_DATE || formtab == OrderPageModel.TAB_PRODUCT || formtab == OrderPageModel.TAB_NAME) 
            {
                //form uses date filter
                if (dateStart == null) dateStart = DateTime.Now.AddDays(-7);
                if (dateEnd == null) dateEnd = DateTime.Now;
                else dateEnd = dateEnd.Value.Date + new TimeSpan(23, 59, 50);
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.DateCreated >= dateStart && detail.Order.DateCreated <= dateEnd);
            }
            if (formtab == OrderPageModel.TAB_PRODUCT && product != null) 
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Product != null && detail.Product.Description.Contains(product));

            IList<OrderDetail> orderDetails = orderDetailQuery.OrderByDescending(detail => detail.OrderId).ToList();
            
            OrderPageModel viewModel = new OrderPageModel(StoreService, orderDetails);

            viewModel.FormTab = String.IsNullOrEmpty(formtab) ? OrderPageModel.TAB_DATE : formtab;
            viewModel.OrderTab = String.IsNullOrEmpty(ordertab) ? OrderPageModel.COMPLETED : ordertab;

            viewModel.StartDate = dateStart.Value.ToString("MM/dd/yyyy");
            viewModel.EndDate = dateEnd.Value.ToString("MM/dd/yyyy");
            viewModel.Product = product;

            return View("../Store/Admin/Orders", viewModel);
        }
    }
}
