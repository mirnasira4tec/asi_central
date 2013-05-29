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

        public IEncryptionService encryptionService { get; set; }

        [HttpGet]
        public virtual ActionResult List(Nullable<DateTime> dateStart, Nullable<DateTime> dateEnd, string product, Nullable<int> id, string name, String formTab, String orderTab)
        {
            if (dateStart > dateEnd) ViewBag.Message = Resource.StoreDateErrorMessage;

            IQueryable<LegacyOrderDetail> orderDetailQuery = StoreService.GetAll<LegacyOrderDetail>("Order;Order.Membership;Order.CreditCard", true);
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
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.DateCreated >= dateStart && detail.Order.DateCreated <= dateEnd);
            }
            if (formTab == OrderPageModel.TAB_PRODUCT && !string.IsNullOrEmpty(product))
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Product != null && detail.Product.Description != null && detail.Product.Description.Contains(product.ToLower()));

            if (formTab == OrderPageModel.TAB_ORDER && id.HasValue)
                orderDetailQuery = orderDetailQuery.Where(detail => detail.OrderId == id.Value);

            if (formTab == OrderPageModel.TAB_NAME && !string.IsNullOrEmpty(name))
            {
                string nameCondition = name.ToLower();
                orderDetailQuery = orderDetailQuery
                    .Where(detail => detail.Order.BillFirstName.Contains(nameCondition) ||
                    detail.Order.BillLastName.Contains(nameCondition) ||
                    (detail.Order.CreditCard != null && detail.Order.CreditCard.Name.Contains(nameCondition)) ||
                    (detail.Order.Membership != null && detail.Order.Membership.Email.Contains(nameCondition)));
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
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.Status == true);
            else if (orderTab == OrderPageModel.ORDER_INCOMPLETE)
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.Status == false);
            else if (orderTab == OrderPageModel.ORDER_PENDING)
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.Status == true && detail.Order.ProcessStatus == OrderStatus.Pending);

            //query has been constructed - get the data
            IList<LegacyOrderDetail> orderDetails = orderDetailQuery.OrderByDescending(detail => detail.OrderId).ToList();

            OrderPageModel viewModel = new OrderPageModel(StoreService, encryptionService, orderDetails);

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
            asi.asicentral.model.store.LegacyOrder order = StoreService.GetAll<asi.asicentral.model.store.LegacyOrder>().Where(ordr => ordr.Id == id).SingleOrDefault();
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
            if (orderStatisticsData.StartDate > orderStatisticsData.EndDate)
            {
                orderStatisticsData.Message = Resource.StoreDateErrorMessage;
                orderStatisticsData.StartDate = DateTime.Now.AddDays(-7).Date;
                orderStatisticsData.EndDate = DateTime.Now;
                return View("../Store/Admin/Statistics", orderStatisticsData);
            }
            IQueryable<LegacyOrder> ordersQuery = GetOrderBy(orderStatisticsData);
            orderStatisticsData.Data = ordersQuery
                .GroupBy(order => new { order.Campaign, order.CompletedStep })
                .Select(grouped => new GroupedData() {
                    Campaign = grouped.Key.Campaign, 
                    CompletedStep = grouped.Key.CompletedStep, 
                    StepLabel = SqlFunctions.StringConvert((double)grouped.Key.CompletedStep), 
                    Count = grouped.Count(), 
                    Amount = grouped.Sum(order => order.CreditCard.TotalAmount),
                    CountRejected = grouped.Count(order => order.ProcessStatus == OrderStatus.Rejected),
                    CountApproved = grouped.Count(order => order.ProcessStatus == OrderStatus.Approved),
                    AmountRejected = grouped.Where(order => order.ProcessStatus == OrderStatus.Rejected).Sum(order => order.CreditCard.TotalAmount),
                    AmountApproved = grouped.Where(order => order.ProcessStatus == OrderStatus.Approved).Sum(order => order.CreditCard.TotalAmount),
                })
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
                    case 6:
                        data.StepLabel = "Supplier provided a product List";
                        break;
                }
            }
            return View("../Store/Admin/Statistics", orderStatisticsData);
        }

        public ActionResult DownloadCSV(OrderStatisticData orderStatisticsData)
        {
            IQueryable<LegacyOrder> ordersQuery = GetOrderBy(orderStatisticsData);
            if (string.IsNullOrEmpty(orderStatisticsData.Campaign))
                ordersQuery = ordersQuery.Where(order => order.Campaign == null || order.Campaign == string.Empty);

            StringBuilder csv = new StringBuilder();
            string separator = ",";
            csv.Append("Order ID" + separator + "Timss ID" + separator + "Company Name" + separator + "Contact Name" + separator + "Contact Phone" + separator + "Contact Email" + separator + "Orderstatus" + separator + "Amount" + separator + "Date");
            csv.Append(System.Environment.NewLine);

            foreach (LegacyOrder order in ordersQuery)
            {
                string orderid = string.Empty, timss = string.Empty, companyname = string.Empty, contactname = string.Empty, contactphone = string.Empty, contactemail = string.Empty, orderstatus = string.Empty, amount = string.Empty;
                DateTime date = new DateTime();

                orderid = order.Id.ToString();
                timss = order.ExternalReference;
                orderstatus = order.ProcessStatus == OrderStatus.Approved ? "True" : "False";
                if (order.CreditCard != null && order.CreditCard.TotalAmount.HasValue) amount = order.CreditCard.TotalAmount.Value.ToString("C");
                else amount = String.Format("{0:C}", 0m);
                if (order.DateCreated.HasValue) date = order.DateCreated.Value;

                LegacyOrderDetailApplication application = GetOrderDetailApplication(order);
                if (application != null)
                {
                    companyname = application.Company;
                    if (application is LegacySupplierMembershipApplication)
                    {
                        LegacySupplierMembershipApplicationContact contacts = ((LegacySupplierMembershipApplication)application).Contacts.Where(c => c.IsPrimary == true).SingleOrDefault();
                        if (contacts != null)
                        {
                            contactname = contacts.Name;
                            contactphone = contacts.Phone;
                            contactemail = contacts.Email;
                        }
                    }
                    if (application is LegacyDistributorMembershipApplication)
                    {
                        LegacyDistributorMembershipApplicationContact contacts = ((LegacyDistributorMembershipApplication)application).Contacts.Where(c => c.IsPrimary == true).SingleOrDefault();
                        if (contacts != null)
                        {
                            contactname = contacts.Name;
                            contactphone = contacts.Phone;
                            contactemail = contacts.Email;
                        }
                    }
                }
                csv.Append(orderid + separator + timss + separator + companyname + separator + contactname + separator + contactphone + separator + contactemail + separator + orderstatus + separator + amount + separator + date.ToString());
                csv.Append(System.Environment.NewLine);
            }
            return File(new System.Text.UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "report.csv");
        }

        private IQueryable<LegacyOrder> GetOrderBy(OrderStatisticData orderStatisticsData)
        {
            if (string.IsNullOrEmpty(orderStatisticsData.Campaign) && !orderStatisticsData.StartDate.HasValue)
                orderStatisticsData.StartDate = DateTime.Now.AddDays(-7).Date;
            if (string.IsNullOrEmpty(orderStatisticsData.Campaign) && !orderStatisticsData.EndDate.HasValue)
                orderStatisticsData.EndDate = DateTime.Now.Date;
            if (orderStatisticsData.EndDate.HasValue) orderStatisticsData.EndDate = orderStatisticsData.EndDate.Value.Date + new TimeSpan(23, 59, 59);
            IQueryable<LegacyOrder> ordersQuery = StoreService.GetAll<LegacyOrder>();
            if (!string.IsNullOrEmpty(orderStatisticsData.Campaign)) ordersQuery = ordersQuery.Where(order => order.Campaign == orderStatisticsData.Campaign);
            if (orderStatisticsData.StartDate.HasValue) ordersQuery = ordersQuery.Where(order => order.DateCreated >= orderStatisticsData.StartDate);
            if (orderStatisticsData.EndDate.HasValue) ordersQuery = ordersQuery.Where(order => order.DateCreated <= orderStatisticsData.EndDate);

            return ordersQuery;
        }

        private LegacyOrderDetailApplication GetOrderDetailApplication(LegacyOrder order)
        {
            if (order != null && order.OrderDetails != null && order.OrderDetails.Count > 0)
            {
                foreach (LegacyOrderDetail orderDetail in order.OrderDetails)
                {
                    LegacyOrderDetailApplication application = StoreService.GetApplication(orderDetail);
                    if (application != null) return application;
                }
                return null;
            }
            else
                return null;
        }
    }
}
