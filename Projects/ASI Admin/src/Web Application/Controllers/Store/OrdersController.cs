using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.model.store;
using System.Text;
using asi.asicentral.web.model.store.order;
using System.Data.Objects.SqlClient;

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
                else dateEnd = dateEnd.Value.Date + new TimeSpan(23, 59, 59);
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

        [HttpPost]
        public JsonResult Reject(int id)
        {
            string error = string.Empty;
            asi.asicentral.model.store.Order order = StoreService.GetAll<asi.asicentral.model.store.Order>().Where(ordr => ordr.Id == id).SingleOrDefault();
            if (order != null)
            {
                if (order.ProcessStatus == OrderStatus.Pending)
                {
                    order.ProcessStatus = OrderStatus.Rejected;
                    StoreService.SaveChanges();
                }
            }
            else
            {
                error = "Could not find the order";
            }
            return new JsonResult
            {
                Data = new { Success = (error.Length == 0), Error = error }
            };
        }

        public virtual ActionResult Statistics(OrderStatisticData orderStatisticsData)
        {
            if (string.IsNullOrEmpty(orderStatisticsData.Campaign) && !orderStatisticsData.StartDate.HasValue) 
                orderStatisticsData.StartDate = DateTime.Now.AddDays(-7);
            if (string.IsNullOrEmpty(orderStatisticsData.Campaign) && !orderStatisticsData.EndDate.HasValue)
                orderStatisticsData.EndDate = DateTime.Now;
            if (orderStatisticsData.EndDate.HasValue) orderStatisticsData.EndDate = orderStatisticsData.EndDate.Value.Date + new TimeSpan(23, 59, 59);
            IQueryable<Order> ordersQuery = StoreService.GetAll<Order>();
            if (!string.IsNullOrEmpty(orderStatisticsData.Campaign)) ordersQuery = ordersQuery.Where(order => order.Campaign == orderStatisticsData.Campaign);
            if (orderStatisticsData.StartDate.HasValue) ordersQuery = ordersQuery.Where(order => order.DateCreated >= orderStatisticsData.StartDate);
            if (orderStatisticsData.EndDate.HasValue) ordersQuery = ordersQuery.Where(order => order.DateCreated <= orderStatisticsData.EndDate);

            orderStatisticsData.Data = ordersQuery
                .GroupBy(order => new { order.Campaign, order.CompletedStep })
                .Select(grouped => new GroupedData() { 
                    Campaign = grouped.Key.Campaign, 
                    CompletedStep = grouped.Key.CompletedStep, 
                    StepLabel = SqlFunctions.StringConvert((double)grouped.Key.CompletedStep), 
                    Count = grouped.Count(), 
                    Amount = grouped.Sum(order => order.CreditCard.TotalAmount)})
                .OrderBy(data => new { data.Campaign, data.CompletedStep })
                .ToList();
            //change completed steps with more meaningful title
            foreach (GroupedData data in orderStatisticsData.Data)
            {
                switch (data.CompletedStep)
                {
                    case 0:
                        data.StepLabel = "Clicked on the link";
                        break;
                    case 1:
                        data.StepLabel = "Selected a product";
                        break;
                    case 2:
                        data.StepLabel = "Entered Company information";
                        break;
                    case 3:
                        data.StepLabel = "Entered billing/shipping information";
                        break;
                    case 4:
                        data.StepLabel = "Confirmed the order";
                        break;
                    case 5:
                        data.StepLabel = "Entered optional information";
                        break;
                }
            }
            return View("../Store/Admin/Statistics", orderStatisticsData);
        }
    }
}
