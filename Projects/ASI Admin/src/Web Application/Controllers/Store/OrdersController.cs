﻿using System;
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
    [Authorize]
    public class OrdersController : Controller
    {
        public IStoreService StoreService { get; set; }

        public IEncryptionService EncryptionService { get; set; }

        [HttpGet]
        public virtual ActionResult List(Nullable<DateTime> dateStart, Nullable<DateTime> dateEnd, string product, Nullable<int> id, string name, String formTab, String orderTab)
        {
            if (dateStart > dateEnd) ViewBag.Message = Resource.StoreDateErrorMessage;
            IQueryable<StoreOrderDetail> orderDetailQuery = StoreService.GetAll<StoreOrderDetail>(true);
            if (string.IsNullOrEmpty(formTab)) formTab = OrderPageModel.TAB_DATE; //setting the default tab
            if (string.IsNullOrEmpty(orderTab)) orderTab = OrderPageModel.ORDER_COMPLETED; //setting the default tab
            //
            // Filter the data based on the filter tab selected
            //
            if (formTab == OrderPageModel.TAB_DATE || formTab == OrderPageModel.TAB_PRODUCT || formTab == OrderPageModel.TAB_NAME)
            {
                //form uses date filter
                if (dateStart == null) dateStart = DateTime.Now.AddDays(-7);
                if (dateEnd == null) dateEnd = DateTime.Now;
                else dateEnd = dateEnd.Value.Date + new TimeSpan(23, 59, 59);
                //create new value converted to UTC time to make sure getting the right database records
                DateTime dateStartParam = dateStart.Value.ToUniversalTime();
                DateTime dateEndParam = dateEnd.Value.ToUniversalTime();
                orderDetailQuery = orderDetailQuery.Where(detail => detail.CreateDate >= dateStartParam && detail.CreateDate <= dateEndParam);
            }
            if (formTab == OrderPageModel.TAB_PRODUCT && !string.IsNullOrEmpty(product))
                orderDetailQuery = orderDetailQuery.Where(detail => 
                    (detail.Product != null && detail.Product.Name != null && detail.Product.Name == product)
                    || (detail.Order.Context != null && detail.Order.Context.Name == product) );

            if (formTab == OrderPageModel.TAB_ORDER && id.HasValue)
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.Id == id.Value);

            if (formTab == OrderPageModel.TAB_NAME && !string.IsNullOrEmpty(name))
            {
                string nameCondition = name.ToLower();
                orderDetailQuery = orderDetailQuery
                    .Where(detail => detail.Order.BillingIndividual != null && (detail.Order.BillingIndividual.FirstName.Contains(nameCondition) ||
                    detail.Order.BillingIndividual.LastName.Contains(nameCondition) ||
                    (detail.Order.CreditCard != null && detail.Order.CreditCard.CardHolderName.Contains(nameCondition)) ||
                    detail.Order.BillingIndividual.Email.Contains(nameCondition)));
            }
            if (formTab == OrderPageModel.TAB_TIMMS && id.HasValue)
            {
                string timssIdentifier = id.Value.ToString().Trim();
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.ExternalReference.Contains(timssIdentifier));
            }
            //
            // Filter the data based on the order tab selected
            //
            if (orderTab == OrderPageModel.ORDER_COMPLETED)
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.IsCompleted == true);
            else if (orderTab == OrderPageModel.ORDER_INCOMPLETE)
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.IsCompleted == false);
            else if (orderTab == OrderPageModel.ORDER_PENDING)
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.IsCompleted == true && detail.Order.ProcessStatus == OrderStatus.Pending);

            //query has been constructed - get the data
            //IList<LegacyOrderDetail> orderDetails = orderDetailQuery.OrderByDescending(detail => detail.OrderId).ToList();
            IList<StoreOrderDetail> orderDetails = orderDetailQuery.OrderByDescending(detail => detail.Order.Id).ToList();

            OrderPageModel viewModel = new OrderPageModel(StoreService, EncryptionService, orderDetails);

            //pass the search values back into the page model so they can be displayed again
            if (dateStart.HasValue) viewModel.StartDate = dateStart.Value.ToString("MM/dd/yyyy");
            if (dateEnd.HasValue) viewModel.EndDate = dateEnd.Value.ToString("MM/dd/yyyy");
            if (id.HasValue && id > 0) viewModel.Identifier = id.Value;
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
            asi.asicentral.model.store.StoreOrder order = StoreService.GetAll<asi.asicentral.model.store.StoreOrder>().Where(ordr => ordr.Id == id).SingleOrDefault();
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

        /// <summary>
        /// Statistical data for the campaigns
        /// </summary>
        /// <param name="orderStatisticsData"></param>
        /// <returns></returns>
        public virtual ActionResult Statistics(OrderStatisticData orderStatisticsData)
        {
            if (orderStatisticsData.StartDate > orderStatisticsData.EndDate)
            {
                orderStatisticsData.Message = Resource.StoreDateErrorMessage;
                orderStatisticsData.StartDate = DateTime.Now.AddDays(-7).Date;
                orderStatisticsData.EndDate = DateTime.Now;
                return View("../Store/Admin/Statistics", orderStatisticsData);
            }
            IQueryable<StoreOrder> ordersQuery = GetOrderBy(orderStatisticsData);
            orderStatisticsData.Data = ordersQuery
                .GroupBy(order => new { order.Campaign, order.CompletedStep })
                .Select(grouped => new GroupedData() {
                    GroupName = grouped.Key.Campaign, 
                    CompletedStep = grouped.Key.CompletedStep, 
                    Count = grouped.Count(), 
                    Amount = grouped.Sum(order => order.ContextId),  //need overall order amount
                    CountRejected = grouped.Count(order => order.ProcessStatus == OrderStatus.Rejected),
                    CountApproved = grouped.Count(order => order.ProcessStatus == OrderStatus.Approved),
                    AmountRejected = grouped.Where(order => order.ProcessStatus == OrderStatus.Rejected).Sum(order => order.ContextId), //need overall order amount 
                    AmountApproved = grouped.Where(order => order.ProcessStatus == OrderStatus.Approved).Sum(order => order.ContextId),  //need overall order amount
                })
                .OrderBy(data => new { Campaign = data.GroupName, data.CompletedStep })
                .ToList();
            PopulateStepData(orderStatisticsData.Data);
            return View("../Store/Admin/Statistics", orderStatisticsData);
        }

        /// <summary>
        /// Statistical data for the campaigns
        /// </summary>
        /// <param name="orderStatisticsData"></param>
        /// <returns></returns>
        public virtual ActionResult Products(ProductStatisticData orderStatisticsData)
        {
            if (orderStatisticsData.StartDate > orderStatisticsData.EndDate)
            {
                orderStatisticsData.Message = Resource.StoreDateErrorMessage;
                orderStatisticsData.StartDate = DateTime.Now.AddDays(-7).Date;
                orderStatisticsData.EndDate = DateTime.Now;
                return View("../Store/Admin/ProductStatistics", orderStatisticsData);
            }
            IQueryable<StoreOrder> ordersQuery = StoreService.GetAll<StoreOrder>();
            if (!orderStatisticsData.StartDate.HasValue) orderStatisticsData.StartDate = DateTime.Now.AddDays(-7).Date;
            if (!orderStatisticsData.EndDate.HasValue) orderStatisticsData.EndDate = DateTime.Now.Date;
            if (orderStatisticsData.EndDate.HasValue) orderStatisticsData.EndDate = orderStatisticsData.EndDate.Value.Date + new TimeSpan(23, 59, 59);
            if (orderStatisticsData.StartDate.HasValue)
            {
                DateTime dateParam = orderStatisticsData.StartDate.Value.ToUniversalTime();
                ordersQuery = ordersQuery.Where(order => order.CreateDate >= dateParam);
            }
            if (orderStatisticsData.EndDate.HasValue)
            {
                DateTime dateParam = orderStatisticsData.EndDate.Value.ToUniversalTime();
                ordersQuery = ordersQuery.Where(order => order.CreateDate <= dateParam);
            }
            IList<StoreOrder> orders = ordersQuery.ToList();
            IList<Group> groups = new List<Group>();
            //get list of products
            foreach (string product in orders.Select(order => order.ProductName).Distinct().OrderBy(name => name))
            {
                //for each product create section and populate with data already there
                var productdata = orders.Where(order => order.ProductName == product)
                    .GroupBy(order => new { order.CompletedStep })
                    .Select(grouped => new
                    {
                        CompletedStep = grouped.Key.CompletedStep,
                        Count = grouped.Count(),
                        Amount = grouped.Sum(order => order.Total),
                    })
                    .OrderBy(data => data.CompletedStep)
                    .ToList();
                Group group = new Group() { Name = product };
                foreach (var item in productdata)
                {
                    group.Data[item.CompletedStep].Count = item.Count;
                    group.Data[item.CompletedStep].Amount = item.Amount;
                }
                //combine the total
                for (int i = 3; i >= 0; i--) group.Data[i].Count += group.Data[i + 1].Count;
                //check rejected
                group.Data[5].Count = orders.Where(order => order.ProductName == product && order.ProcessStatus == OrderStatus.Rejected).Count();
                group.Data[5].Amount = orders.Where(order => order.ProductName == product && order.ProcessStatus == OrderStatus.Rejected).Sum(order => order.Total);
                //check pending approval
                group.Data[6].Count = orders.Where(order => order.ProductName == product && order.ProcessStatus == OrderStatus.Pending && order.IsCompleted).Count();
                group.Data[6].Amount = orders.Where(order => order.ProductName == product && order.ProcessStatus == OrderStatus.Pending && order.IsCompleted).Sum(order => order.Total);
                //check approved
                group.Data[7].Count = orders.Where(order => order.ProductName == product && order.ProcessStatus == OrderStatus.Approved).Count();
                group.Data[7].Amount = orders.Where(order => order.ProductName == product && order.ProcessStatus == OrderStatus.Approved).Sum(order => order.Total);
                group.Data[7].AnnualizedAmount = group.Data[7].Amount;
                if (group.Data[7].Amount > 0)
                {
                    StoreOrder ordr = orders.Where(order => order.ProductName == product && order.ProcessStatus == OrderStatus.Approved).FirstOrDefault();
                    if (ordr != null && ordr.OrderDetails.Count > 0) {
                        ContextProduct contextProduct = ordr.OrderDetails[0].Product;
                        if (contextProduct.IsSubscription && contextProduct.SubscriptionFrequency == "M") 
                            group.Data[7].AnnualizedAmount = group.Data[7].Amount * 12;
                    }
                }
                groups.Add(group);
            }
            orderStatisticsData.Data = groups;
            return View("../Store/Admin/ProductStatistics", orderStatisticsData);
        }

        /// <summary>
        /// Changes the step from position to name of the steps
        /// </summary>
        /// <param name="dataList"></param>
        private void PopulateStepData(IList<GroupedData> dataList)
        {
            //change completed steps with more meaningful title
            foreach (GroupedData data in dataList)
            {
                switch (data.CompletedStep)
                {
                    case 0:
                        data.StepLabel = "Clicked on the link Only";
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
                    case 6:
                        data.StepLabel = "Supplier provided a product List";
                        break;
                    default:
                        data.StepLabel = data.CompletedStep.ToString();
                        break;
                }
            }
        }

        /// <summary>
        /// Download the product data
        /// </summary>
        /// <param name="orderStatisticsData"></param>
        /// <returns></returns>
        public ActionResult DownloadCSV(OrderStatisticData orderStatisticsData)
        {
            IQueryable<StoreOrder> ordersQuery = GetOrderBy(orderStatisticsData);
            if (string.IsNullOrEmpty(orderStatisticsData.Campaign))
                ordersQuery = ordersQuery.Where(order => order.Campaign == null || order.Campaign == string.Empty);

            StringBuilder csv = new StringBuilder();
            string separator = ",";
            csv.Append("Order ID" + separator + "Timss ID" + separator + "Company Name" + separator + "Contact Name" + separator + "Contact Phone" + separator + "Contact Email" + separator + "Orderstatus" + separator + "Amount" + separator + "Date");
            csv.Append(System.Environment.NewLine);

            foreach (StoreOrder order in ordersQuery)
            {
                string orderid = string.Empty, timss = string.Empty, companyname = string.Empty, contactname = string.Empty, contactphone = string.Empty, contactemail = string.Empty, orderstatus = string.Empty, amount = string.Empty;
                DateTime date = new DateTime();

                orderid = order.Id.ToString();
                timss = order.ExternalReference;
                orderstatus = order.ProcessStatus == OrderStatus.Approved ? "True" : "False";
                amount = order.Total.ToString("C");
                date = order.CreateDate;
                if (order.Company != null)
                {
                    companyname = order.Company.Name;
                    StoreIndividual primaryContact = order.GetContact();
                    if (primaryContact != null)
                    {
                        contactname = primaryContact.FirstName + " " + primaryContact.LastName;
                        contactphone = primaryContact.Phone;
                        contactemail = primaryContact.Email;
                    }
                }
                csv.Append(orderid + separator + timss + separator + companyname + separator + contactname + separator + contactphone + separator + contactemail + separator + orderstatus + separator + amount + separator + date.ToString());
                csv.Append(System.Environment.NewLine);
            }
            return File(new System.Text.UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "report.csv");
        }

        private IQueryable<StoreOrder> GetOrderBy(OrderStatisticData orderStatisticsData)
        {
            if (string.IsNullOrEmpty(orderStatisticsData.Campaign) && !orderStatisticsData.StartDate.HasValue)
                orderStatisticsData.StartDate = DateTime.Now.AddDays(-7).Date;
            if (string.IsNullOrEmpty(orderStatisticsData.Campaign) && !orderStatisticsData.EndDate.HasValue)
                orderStatisticsData.EndDate = DateTime.Now.Date;
            if (orderStatisticsData.EndDate.HasValue) orderStatisticsData.EndDate = orderStatisticsData.EndDate.Value.Date + new TimeSpan(23, 59, 59);
            IQueryable<StoreOrder> ordersQuery = StoreService.GetAll<StoreOrder>();
            if (!string.IsNullOrEmpty(orderStatisticsData.Campaign)) ordersQuery = ordersQuery.Where(order => order.Campaign == orderStatisticsData.Campaign);
            if (orderStatisticsData.StartDate.HasValue)
            {
                DateTime dateParam = orderStatisticsData.StartDate.Value.ToUniversalTime();
                ordersQuery = ordersQuery.Where(order => order.CreateDate >= dateParam);
            }
            if (orderStatisticsData.EndDate.HasValue)
            {
                DateTime dateParam = orderStatisticsData.EndDate.Value.ToUniversalTime();
                ordersQuery = ordersQuery.Where(order => order.CreateDate <= dateParam);
            }

            return ordersQuery;
        }

        private StoreDetailApplication GetOrderDetailApplication(StoreOrder order)
        {
            if (order != null && order.OrderDetails != null && order.OrderDetails.Count > 0)
            {
                foreach (StoreOrderDetail orderDetail in order.OrderDetails)
                {
                    StoreDetailApplication application = StoreService.GetApplication(orderDetail);
                    if (application != null) return application;
                }
                return null;
            }
            else
                return null;
        }
    }
}
