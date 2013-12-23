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
using asi.asicentral.Resources;

namespace asi.asicentral.web.Controllers.Store
{
    [Authorize]
    public class OrdersController : Controller
    {
        public IStoreService StoreService { get; set; }

        public IEncryptionService EncryptionService { get; set; }

        [HttpGet]
        public virtual ActionResult List(Nullable<DateTime> dateStart, Nullable<DateTime> dateEnd, string product, Nullable<int> id, string name, String formTab, String orderTab, string CompanyName,Nullable<Boolean> HasAddress )
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
                if (HasAddress == null) HasAddress = true;
                else dateEnd = dateEnd.Value.Date + new TimeSpan(23, 59, 59);
                //create new value converted to UTC time to make sure getting the right database records
                DateTime dateStartParam = dateStart.Value.ToUniversalTime();
                DateTime dateEndParam = dateEnd.Value.ToUniversalTime();
                orderDetailQuery = orderDetailQuery.Where(detail => detail.CreateDate >= dateStartParam && detail.CreateDate <= dateEndParam);
            }
            if (formTab == OrderPageModel.TAB_PRODUCT && !string.IsNullOrEmpty(product))
            {
                product = Server.HtmlDecode(product);
                orderDetailQuery = orderDetailQuery.Where(detail =>
                    (detail.Product != null && detail.Product.Name != null && detail.Product.Name == product)
                    || (detail.Order.Context != null && detail.Order.Context.Name == product));
            }
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
            if (formTab == OrderPageModel.COMPANY_NAME && !string.IsNullOrEmpty(CompanyName))
            {
                orderDetailQuery = orderDetailQuery
                    .Where(detail => detail.Order.Company.Name != null && (detail.Order.Company.Name.Contains(CompanyName)));
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
            {
                if (HasAddress != null)
                {
                    if (HasAddress.Value)
                        orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.IsCompleted == false && detail.Order.CompletedStep > 1);
                    else
                        orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.IsCompleted == false);
                }
                else
                    orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.IsCompleted == false && detail.Order.CompletedStep > 1);
                
            }
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
            viewModel.CompanyName = CompanyName;
            if (HasAddress.HasValue)
            {
                viewModel.HasAddress = HasAddress.Value.ToString();
                viewModel.chkHasAddress = HasAddress.Value;
            }
            else
            {
                viewModel.chkHasAddress = true;
            }
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
                return View("../Store/Admin/ProductStatistics", orderStatisticsData);
            }
            if (orderStatisticsData.Campaign == null && (orderStatisticsData.StartDate == null || orderStatisticsData.EndDate == null)) 
            {
                DateTime now = DateTime.Now;
                //make sure we do not retrieve everything
                if (orderStatisticsData.StartDate == null) orderStatisticsData.StartDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(-7);
                else orderStatisticsData.StartDate = new DateTime(orderStatisticsData.StartDate.Value.Year, orderStatisticsData.StartDate.Value.Month, orderStatisticsData.StartDate.Value.Day, 0, 0, 0);
                if (orderStatisticsData.EndDate == null) orderStatisticsData.EndDate = now;
                else orderStatisticsData.EndDate = new DateTime(orderStatisticsData.EndDate.Value.Year, orderStatisticsData.EndDate.Value.Month, orderStatisticsData.EndDate.Value.Day, 23, 59, 59);
            }
            IQueryable<StoreOrder> ordersQuery = GetCampainQuery(orderStatisticsData);
            IList<StoreOrder> orders = ordersQuery.ToList();
            IList<Group> groups = new List<Group>();
            //get list of products
            foreach (string campaign in orders.Select(order => order.Campaign).Distinct().OrderBy(name => name))
            {
                //for each product create section and populate with data already there
                var productdata = orders.Where(order => order.Campaign == campaign)
                    .GroupBy(order => new { order.CompletedStep })
                    .Select(grouped => new
                    {
                        CompletedStep = grouped.Key.CompletedStep,
                        Count = grouped.Count(),
                        Amount = grouped.Sum(order => order.Total),
                        AnnualizedAmout = grouped.Sum(order => order.AnnualizedTotal),
                    })
                    .OrderBy(data => data.CompletedStep)
                    .ToList();
                Group group = new Group() { Name = string.IsNullOrEmpty(campaign) ? "(Unknown)" : campaign };
                foreach (var item in productdata)
                {
                    int index = item.CompletedStep >= 4 ? 4 : item.CompletedStep;
                    //anything after Place order counts as place order
                    group.Data[index].Count += item.Count;
                    group.Data[index].Amount += item.Amount;
                }
                //combine the total
                for (int i = 3; i >= 0; i--) group.Data[i].Count += group.Data[i + 1].Count;
                //check rejected
                group.Data[5].Count = orders.Where(order => order.Campaign == campaign && order.ProcessStatus == OrderStatus.Rejected).Count();
                group.Data[5].Amount = orders.Where(order => order.Campaign == campaign && order.ProcessStatus == OrderStatus.Rejected).Sum(order => order.Total);
                //check pending approval
                group.Data[6].Count = orders.Where(order => order.Campaign == campaign && order.ProcessStatus == OrderStatus.Pending && order.IsCompleted).Count();
                group.Data[6].Amount = orders.Where(order => order.Campaign == campaign && order.ProcessStatus == OrderStatus.Pending && order.IsCompleted).Sum(order => order.Total);
                //check approved
                group.Data[7].Count = orders.Where(order => order.Campaign == campaign && order.IsCompleted && order.ProcessStatus == OrderStatus.Approved).Count();
                group.Data[7].Amount = orders.Where(order => order.Campaign == campaign && order.IsCompleted && order.ProcessStatus == OrderStatus.Approved).Sum(order => order.Total);
                group.Data[7].AnnualizedAmount = orders.Where(order => order.Campaign == campaign && order.IsCompleted && order.ProcessStatus == OrderStatus.Approved).Sum(order => order.AnnualizedTotal);
                groups.Add(group);
            }
            orderStatisticsData.Data = groups;
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
                return View("../Store/Admin/Statistics", orderStatisticsData);
            }
            if (orderStatisticsData.StartDate.HasValue) orderStatisticsData.StartDate = new DateTime(orderStatisticsData.StartDate.Value.Year, orderStatisticsData.StartDate.Value.Month, orderStatisticsData.StartDate.Value.Day, 0, 0, 0);
            if (orderStatisticsData.EndDate.HasValue) orderStatisticsData.EndDate = new DateTime(orderStatisticsData.EndDate.Value.Year, orderStatisticsData.EndDate.Value.Month, orderStatisticsData.EndDate.Value.Day, 23, 59, 59);

            IQueryable<StoreOrder> ordersQuery = GetProductQuery(orderStatisticsData);
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
                        AnnualizedAmout = grouped.Sum(order => order.AnnualizedTotal),
                    })
                    .OrderBy(data => data.CompletedStep)
                    .ToList();
                Group group = new Group() { Name = product };
                foreach (var item in productdata)
                {
                    int index = item.CompletedStep >= 4 ? 4 : item.CompletedStep;
                    //anything after Place order counts as place order
                    group.Data[index].Count += item.Count;
                    group.Data[index].Amount += item.Amount;
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
                group.Data[7].Count = orders.Where(order => order.ProductName == product && order.IsCompleted && order.ProcessStatus == OrderStatus.Approved).Count();
                group.Data[7].Amount = orders.Where(order => order.ProductName == product && order.IsCompleted && order.ProcessStatus == OrderStatus.Approved).Sum(order => order.Total);
                group.Data[7].AnnualizedAmount = orders.Where(order => order.ProductName == product && order.IsCompleted && order.ProcessStatus == OrderStatus.Approved).Sum(order => order.AnnualizedTotal);
                groups.Add(group);
            }
            orderStatisticsData.Data = groups;
            return View("../Store/Admin/ProductStatistics", orderStatisticsData);
        }

        /// <summary>
        /// Download the product data
        /// </summary>
        /// <param name="orderStatisticsData"></param>
        /// <returns></returns>
        public ActionResult DownloadCampaignCSV(OrderStatisticData orderStatisticsData)
        {
            IQueryable<StoreOrder> ordersQuery = GetCampainQuery(orderStatisticsData);
            if (string.IsNullOrEmpty(orderStatisticsData.Campaign))
                ordersQuery = ordersQuery.Where(order => order.Campaign == null || order.Campaign == string.Empty);
            return Download(ordersQuery);
        }

        /// <summary>
        /// Download the product data
        /// </summary>
        /// <param name="orderStatisticsData"></param>
        /// <returns></returns>
        public ActionResult DownloadProductCSV(ProductStatisticData orderStatisticsData)
        {
            IQueryable<StoreOrder> ordersQuery = GetProductQuery(orderStatisticsData);
            if (orderStatisticsData.Product != null) {
                //product name not available as query
                ordersQuery = ordersQuery.ToList().Where(order => order.ProductName == orderStatisticsData.Product).AsQueryable();
            }
            return Download(ordersQuery);
        }

        /// <summary>
        /// Creates csv for the order query
        /// </summary>
        /// <param name="ordersQuery"></param>
        /// <returns></returns>
        private ActionResult Download(IQueryable<StoreOrder> ordersQuery)
        {
            StringBuilder csv = new StringBuilder();
            string separator = ",";
            csv.Append("Order ID" + separator + "Timss ID" + separator + "Company Name" + separator + "Contact Name" + separator + "Contact Phone" + separator + "Contact Email" + separator + "Orderstatus" + separator + "Amount" + separator + "Created Date" + separator + "Product Name"+ separator + "Approved Date" + separator + "Annualized Amount");
            csv.Append(System.Environment.NewLine);

            foreach (StoreOrder order in ordersQuery)
            {
                string orderid = string.Empty, timss = string.Empty, companyname = string.Empty, contactname = string.Empty, contactphone = string.Empty, contactemail = string.Empty, orderstatus = string.Empty, amount = string.Empty, annualizedamount = string.Empty, productname = string.Empty, approveddate = string.Empty;
                DateTime date = new DateTime();
                
                orderid = order.Id.ToString();
                timss = order.ExternalReference;
                orderstatus = order.ProcessStatus == OrderStatus.Approved ? "True" : "False";
                amount = order.Total.ToString("C");
                date = order.CreateDate;
                annualizedamount = order.AnnualizedTotal.ToString("C");
                approveddate = (order.ApprovedDate==DateTime.MinValue)?string.Empty:order.ApprovedDate.ToString();
                if (order.OrderDetails != null && order.OrderDetails.Count > 0 && order.OrderDetails.ElementAt(0) != null && order.OrderDetails.ElementAt(0).Product!=null)
                {
                    productname = order.OrderDetails.ElementAt(0).Product.Name.Replace(",","");
                }
                if (order.Company != null)
                {
                    companyname = order.Company.Name.Replace(",", "");
                    StoreIndividual primaryContact = order.GetContact();
                    if (primaryContact != null)
                    {
                        contactname = (primaryContact.FirstName + " " + primaryContact.LastName).Replace(",", "");
                        contactphone = primaryContact.Phone;
                        contactemail = primaryContact.Email;
                    }
                }
                csv.Append(orderid + separator + timss + separator + companyname + separator + contactname + separator + contactphone + separator + contactemail + separator + orderstatus + separator + amount + separator + date.ToString() + separator + productname + separator + approveddate + separator + annualizedamount);
                csv.Append(System.Environment.NewLine);
            }
            return File(new System.Text.UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "report.csv");
        }

        private IQueryable<StoreOrder> GetCampainQuery(OrderStatisticData orderStatisticsData)
        {
            IQueryable<StoreOrder> ordersQuery = StoreService.GetAll<StoreOrder>("Company;Company.Individuals;BillingIndividual", true);
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
            if (orderStatisticsData.Campaign != null)
            {
                ordersQuery = ordersQuery.Where(order => order.Campaign == orderStatisticsData.Campaign);
            }

            return ordersQuery;
        }

        private IQueryable<StoreOrder> GetProductQuery(ProductStatisticData orderStatisticsData)
        {
            IQueryable<StoreOrder> ordersQuery = StoreService.GetAll<StoreOrder>("Company;Company.Individuals;BillingIndividual", true);
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
