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
        public virtual ActionResult List(Nullable<DateTime> dateStart, Nullable<DateTime> dateEnd, string product, Nullable<int> id, string name, String formTab, String orderTab, string CompanyName, Nullable<Boolean> HasAddress)
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
                if (HasAddress == null) HasAddress = true;
               
                //create new value converted to UTC time to make sure getting the right database records
                DateTime dateStartParam = dateStart.Value.ToUniversalTime();
                DateTime dateEndParam = dateEnd.Value.ToUniversalTime();
              orderDetailQuery = orderDetailQuery.Where(detail => detail.CreateDate >= dateStartParam && detail.CreateDate <= dateEndParam);
            }
            if (formTab == OrderPageModel.TAB_PRODUCT && !string.IsNullOrEmpty(product))
            {
                product = Server.UrlDecode(product);
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
                if (order.ProcessStatus == OrderStatus.Pending || order.ProcessStatus == OrderStatus.Approved )
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
        public static readonly string[] IGNORED_ASI_NUMBERS = { "30232", "30235", "125724" };
        public virtual ActionResult Statistics(OrderStatisticData orderStatisticsData, string statistics)
        {
            orderStatisticsData.FormTab = statistics + "Tab";
            orderStatisticsData.Name = statistics;
            if (orderStatisticsData.StartDate > orderStatisticsData.EndDate)
            {
                orderStatisticsData.Message = Resource.StoreDateErrorMessage;
                orderStatisticsData.StartDate = DateTime.Now.AddDays(-7).Date;
                orderStatisticsData.EndDate = DateTime.Now;
                return View("../Store/Admin/Statistics", orderStatisticsData);
            }
            if (orderStatisticsData.StatisticsValue == null && (orderStatisticsData.StartDate == null || orderStatisticsData.EndDate == null))
            {
                DateTime now = DateTime.Now;
                //make sure we do not retrieve everything
                if (orderStatisticsData.StartDate == null) orderStatisticsData.StartDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(-7);
                else orderStatisticsData.StartDate = new DateTime(orderStatisticsData.StartDate.Value.Year, orderStatisticsData.StartDate.Value.Month, orderStatisticsData.StartDate.Value.Day, 0, 0, 0);
                if (orderStatisticsData.EndDate == null) orderStatisticsData.EndDate = now;
                else orderStatisticsData.EndDate = new DateTime(orderStatisticsData.EndDate.Value.Year, orderStatisticsData.EndDate.Value.Month, orderStatisticsData.EndDate.Value.Day, 23, 59, 59);
            }
            IQueryable<StoreOrder> ordersQuery = GetQuery(orderStatisticsData);
            IList<StoreOrder> orders = ExcludeInternalOrders(ordersQuery);

            IList<Group> groups = new List<Group>();

            switch (orderStatisticsData.Name)
            {
                case "Campaign":
                    //get list of products
                    foreach (string campaign in orders.Select(order => order.Campaign).Distinct().OrderBy(name => name))
                    {
                        //for each campaign create section and populate with data already there
                        var filteredOrders = orders.Where(order => order.Campaign == campaign);
                        var name = string.IsNullOrEmpty(campaign) ? "(Unknown)" : campaign;
                        SetStatisticsCounts(filteredOrders, groups, name);
                    }
                    break;
                case "Product":
                    //get list of products
                    foreach (string product in orders.Select(order => order.ProductName).Distinct().OrderBy(name => name))
                    {
                        //for each product create section and populate with data already there
                        var filteredOrders = orders.Where(order => order.ProductName == product);
                        SetStatisticsCounts(filteredOrders, groups, product);
                    }
                    break;
                case "Coupon":
                    //get list of products
                    foreach (string coupon in orders.Select(order => order.CouponCode).Distinct().OrderBy(name => name))
                    {
                        //for each coupon create section and populate with data already there
                        var filteredOrders = orders.Where(order => order.CouponCode == coupon);
                        var name = string.IsNullOrEmpty(coupon.ToString()) ? "(Unknown)" : coupon;
                        SetStatisticsCounts(filteredOrders, groups, name);
                    }

                    break;
                case "Type":
                    var filter = orderStatisticsData.StatisticsValue;
                    if (string.IsNullOrEmpty(filter) || !OrderStatisticData.Statistics_Special_Types.Values.Contains(filter))
                    { 
                        // get all order types but "Sales" and "Show"
                        foreach (var type in orders.Select(order => order.OrderRequestType).Distinct().OrderBy(type => type))
                        {
                            //for each type create section and populate with data already there
                            var typeOrders = orders.Where(order => order.OrderRequestType == type && !OrderStatisticData.Statistics_Special_Types.Keys.Contains(order.OrderTypeId));

                            var typeName = string.IsNullOrEmpty(type) ? "(Unknown)" : type;
                            SetStatisticsCounts(typeOrders, groups, typeName); 
                        }
                    }

                    // get statistics for Show - ProductId 17; Sales - ProductId 99
                    foreach (var typeId in OrderStatisticData.Statistics_Special_Types.Keys)
                    {
                        var type = OrderStatisticData.Statistics_Special_Types[typeId];
                        if (string.IsNullOrEmpty(filter) || type == filter)
                        {
                            var typeOrders = orders.Where(order => order.OrderTypeId == typeId);
                            SetStatisticsCounts(typeOrders, groups, type);
                        }
                    }

                    break;
                default:
                    break;
            }
            orderStatisticsData.Data = groups;
            return View("../Store/Admin/Statistics", orderStatisticsData);
        }      

        /// <summary>
        /// Download the product data
        /// </summary>
        /// <param name="orderStatisticsData"></param>
        /// <returns></returns>
        public ActionResult DownloadCSV(OrderStatisticData orderStatisticsData)
        {
            if (orderStatisticsData.StatisticsValue == "(Unknown)") orderStatisticsData.StatisticsValue = null;
            IQueryable<StoreOrder> ordersQuery = GetQuery(orderStatisticsData);
            return Download(ordersQuery);
        }

        private IList<StoreOrder> ExcludeInternalOrders(IQueryable<StoreOrder> ordersQuery)
        {
            //exclude the orders made using @asicentral.com, for orders made after login 
            ordersQuery = ordersQuery.Where(o => (!string.IsNullOrEmpty(o.UserId) && !o.LoggedUserEmail.Contains("@asicentral.com"))
                || (string.IsNullOrEmpty(o.UserId)));

            //exclude the orders made with asinumbers 30232, 30235, 125724
	        //ordersQuery = ordersQuery.Where(o => o.Company == null || !IGNORED_ASI_NUMBERS.Contains(o.Company.ASINumber));
            IList<StoreOrder> orders = ordersQuery.ToList();

            //exclude the orders made using @asicentral.com, for orders made without login  
	        orders =
		        orders.Where(
			        order =>
				        (order.Company == null || order.Company.Individuals == null || order.Company.Individuals.Count == 0
				            || string.IsNullOrEmpty(order.Company.Individuals.ElementAt(0).Email)
				            || !order.Company.Individuals.ElementAt(0).Email.Contains("@asicentral.com")) &&
				        (order.Company == null || !IGNORED_ASI_NUMBERS.Contains(order.Company.ASINumber))).ToList();
            return orders;
        }

        private void SetStatisticsCounts(IEnumerable<StoreOrder> orders, IList<Group> groups, string type)
        {
            var typeData = orders.GroupBy(order => new { order.CompletedStep })
                                 .Select(grouped => new
                                 {
                                     CompletedStep = grouped.Key.CompletedStep,
                                     Count = grouped.Count(),
                                     Amount = grouped.Sum(order => order.Total),
                                     AnnualizedAmout = grouped.Sum(order => order.AnnualizedTotal),
                                 })
                                 .OrderBy(data => data.CompletedStep)
                                 .ToList();            

            if (typeData.Count > 0)
            {
                Group group = new Group() { Name = type };
                foreach (var item in typeData)
                {
                    int index = item.CompletedStep >= 4 ? 4 : item.CompletedStep;
                    //anything after Place order counts as place order
                    group.Data[index].Count += item.Count;
                    group.Data[index].Amount += item.Amount;
                }
                //combine the total
                for (int i = 3; i >= 0; i--) group.Data[i].Count += group.Data[i + 1].Count;
                //check rejected
                group.Data[5].Count = orders.Where(order => order.ProcessStatus == OrderStatus.Rejected).Count();
                group.Data[5].Amount = orders.Where(order => order.ProcessStatus == OrderStatus.Rejected).Sum(order => order.Total);
                //check pending approval
                group.Data[6].Count = orders.Where(order => order.ProcessStatus == OrderStatus.Pending && order.IsCompleted).Count();
                group.Data[6].Amount = orders.Where(order => order.ProcessStatus == OrderStatus.Pending && order.IsCompleted).Sum(order => order.Total);
                //check approved
                group.Data[7].Count = orders.Where(order => order.IsCompleted && order.ProcessStatus == OrderStatus.Approved).Count();
                group.Data[7].Amount = orders.Where(order => order.IsCompleted && order.ProcessStatus == OrderStatus.Approved).Sum(order => order.Total);
                group.Data[7].AnnualizedAmount = orders.Where(order => order.IsCompleted && order.ProcessStatus == OrderStatus.Approved).Sum(order => order.AnnualizedTotal);

                groups.Add(group);
            }
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
            csv.Append("Order ID" + separator + "Timss ID" + separator + "Company Name" + separator + "Contact Name" + separator + "Contact Phone" + separator + "Contact Email" + separator + "Orderstatus" + separator + "Amount" + separator + "Created Date" + separator + "Product Name" + separator + "Approved Date" + separator + "Annualized Amount");
            csv.Append(System.Environment.NewLine);

            IList<StoreOrder> orders = ExcludeInternalOrders(ordersQuery);
            foreach (StoreOrder order in orders)
            {
                string orderid = string.Empty, timss = string.Empty, companyname = string.Empty, contactname = string.Empty, contactphone = string.Empty, contactemail = string.Empty, orderstatus = string.Empty, amount = string.Empty, annualizedamount = string.Empty, productname = string.Empty, approveddate = string.Empty, date = string.Empty;
                orderid = order.Id.ToString();
                timss = order.ExternalReference;
                orderstatus = order.ProcessStatus == OrderStatus.Approved ? "Approved" : order.ProcessStatus == OrderStatus.Rejected ? "Rejected" : "";
                amount = order.Total.ToString("C").Replace(",", "");
                date = order.CreateDate.ToString().Replace(",", "");
                annualizedamount = order.AnnualizedTotal.ToString("C").Replace(",", "");
                approveddate = (order.ApprovedDate == null) ? string.Empty : order.ApprovedDate.ToString().Replace(",", "");
                if (order.OrderDetails != null && order.OrderDetails.Count > 0 && order.OrderDetails.ElementAt(0) != null && order.OrderDetails.ElementAt(0).Product != null)
                {
                    productname = order.OrderDetails.ElementAt(0).Product.Name.Replace(",", "");
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
                if (string.IsNullOrEmpty(contactemail) && !string.IsNullOrEmpty(order.LoggedUserEmail))
                    contactemail = order.LoggedUserEmail;
                csv.Append(orderid + separator + timss + separator + companyname + separator + contactname + separator + contactphone + separator + contactemail + separator + orderstatus + separator + amount + separator + date.ToString() + separator + productname + separator + approveddate + separator + annualizedamount);
                csv.Append(System.Environment.NewLine);

            }
            byte[] data = Encoding.UTF8.GetBytes(csv.ToString());
            byte[] result = Encoding.UTF8.GetPreamble().Concat(data).ToArray();
            return File(result, "text/csv", "report.csv");
        }

        private IQueryable<StoreOrder> GetQuery(OrderStatisticData orderStatisticsData)
        {
            string query = "Company;Company.Individuals;BillingIndividual";

            if (orderStatisticsData.Name == "Coupon") 
                query = query + ";OrderDetails";
            else if (orderStatisticsData.Name == "Product" || orderStatisticsData.Name == "Type")
                query = query + ";OrderDetails.Product";

            IQueryable<StoreOrder> ordersQuery = StoreService.GetAll<StoreOrder>(query, true);
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
            if (orderStatisticsData.StatisticsValue != null)
            {
                switch (orderStatisticsData.Name)
                {
                    case "Campaign":
                        ordersQuery = ordersQuery.Where(order => order.Campaign == orderStatisticsData.StatisticsValue);
                        break;
                    case "Product":
                        ordersQuery = ordersQuery.Where(order => order.OrderDetails.Count(det => det.Product.Name == orderStatisticsData.StatisticsValue) > 0 || order.Context.Name == orderStatisticsData.StatisticsValue);
                        break;
                    case "Coupon":
                        ordersQuery = ordersQuery.Where(order => order.OrderDetails.Count(det => det.Coupon != null && det.Coupon.CouponCode == orderStatisticsData.StatisticsValue) > 0);
                        break;
                    case "Type":
                        ordersQuery = ordersQuery.Where(order => order.OrderDetails.Count(det => //(OrderStatisticData.Statistics_Special_Types.Keys.Contains(det.Product.Id) &&
                                                                                                 // OrderStatisticData.Statistics_Special_Types[det.Product.Id] == orderStatisticsData.StatisticsValue)
                                                                                                 (det.Product.Id == 17 && orderStatisticsData.StatisticsValue == "Show")
                                                                                                 || (det.Product.Id == 99 && orderStatisticsData.StatisticsValue == "Sales")
                                                                                                 || (!OrderStatisticData.Statistics_Special_Types.Keys.Contains(det.Product.Id) &&
                                                                                                      order.OrderRequestType == orderStatisticsData.StatisticsValue)) > 0);
                        break;
                    default:
                        break;
                }
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
