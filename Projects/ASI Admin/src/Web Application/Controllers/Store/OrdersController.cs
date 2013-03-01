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

        public IEncryptionService encryptionService { get; set; }

        [HttpGet]
        public virtual ActionResult List(Nullable<DateTime> dateStart, Nullable<DateTime> dateEnd, string product, Nullable<int> orderId, string name, String formTab, String orderTab)
        {
            if (dateStart > dateEnd) ViewBag.Message = Resource.StoreDateErrorMessage;

            IQueryable<OrderDetail> orderDetailQuery = StoreService.GetAll<OrderDetail>("Order;Order.Membership;Order.CreditCard", true);
            if (string.IsNullOrEmpty(formTab)) formTab = OrderPageModel.TAB_DATE; //setting the default tab
            if (string.IsNullOrEmpty(orderTab)) orderTab = OrderPageModel.ORDER_COMPLETED; //setting the default tab
            if (formTab == OrderPageModel.TAB_DATE || formTab == OrderPageModel.TAB_PRODUCT || formTab == OrderPageModel.TAB_NAME)
            {
                //form uses date filter
                if (dateStart == null) dateStart = DateTime.Now.AddDays(-7);
                if (dateEnd == null) dateEnd = DateTime.Now;
                else dateEnd = dateEnd.Value.Date + new TimeSpan(23, 59, 50);
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.DateCreated >= dateStart && detail.Order.DateCreated <= dateEnd);
            }
            if (formTab == OrderPageModel.TAB_PRODUCT && !string.IsNullOrEmpty(product))
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Product != null && detail.Product.Description != null && detail.Product.Description.Contains(product.ToLower()));

            if (formTab == OrderPageModel.TAB_ORDER && orderId.HasValue)
                orderDetailQuery = orderDetailQuery.Where(detail => detail.OrderId == orderId.Value);

            if (formTab == OrderPageModel.TAB_NAME && !string.IsNullOrEmpty(name))
            {
                string nameCondition = name.ToLower();
                orderDetailQuery = orderDetailQuery
                    .Where(detail => detail.Order.BillFirstName.Contains(nameCondition) ||
                    detail.Order.BillLastName.Contains(nameCondition) ||
                    (detail.Order.CreditCard != null && detail.Order.CreditCard.Name.Contains(nameCondition)) ||
                    (detail.Order.Membership != null && detail.Order.Membership.Email.Contains(nameCondition)));
            }
            if (orderTab == OrderPageModel.ORDER_COMPLETED)
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.Status == true);
            else if (orderTab == OrderPageModel.ORDER_INCOMPLETE)
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.Status == false);
            else if (orderTab == OrderPageModel.ORDER_PENDING)
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.Status == true && detail.Order.ProcessStatus == OrderStatus.Pending);

            //query has been constructed - get the data
            IList<OrderDetail> orderDetails = orderDetailQuery.OrderByDescending(detail => detail.OrderId).ToList();

            OrderPageModel viewModel = new OrderPageModel(StoreService, encryptionService, orderDetails);

            //pass the search values back into the page model so they can be displayed again
            if (dateStart.HasValue) viewModel.StartDate = dateStart.Value.ToString("MM/dd/yyyy");
            if (dateEnd.HasValue) viewModel.EndDate = dateEnd.Value.ToString("MM/dd/yyyy");
            if (orderId.HasValue && orderId > 0) viewModel.OrderIdentifier = orderId.Value;
            if (name != null) viewModel.Name = name;
            viewModel.Product = product;
            viewModel.FormTab = formTab;
            viewModel.OrderTab = orderTab;

            return View("../Store/Admin/Orders", viewModel);
        }
    }
}
