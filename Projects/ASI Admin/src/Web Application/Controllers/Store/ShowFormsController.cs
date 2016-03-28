using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.Resources;
using asi.asicentral.web.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers.Store
{

    [Authorize]
    public class ShowFormsController : Controller
    {
        public IStoreService StoreService { get; set; }
        public IEncryptionService EncryptionService { get; set; }
        public const int ProductId = 99;
        [HttpGet]
        public virtual ActionResult List(Nullable<DateTime> dateStart, Nullable<DateTime> dateEnd, string name, string email, String formTab, String orderTab, Nullable<Boolean> HasAddress)
        {
            if (dateStart > dateEnd) ViewBag.Message = Resource.StoreDateErrorMessage;
            IQueryable<StoreOrderDetail> orderDetailQuery = StoreService.GetAll<StoreOrderDetail>(true).Where(detail => detail.Product.Id == ProductId);
            if (string.IsNullOrEmpty(formTab)) formTab = OrderPageModel.TAB_NAME; //setting the default tab
            if (string.IsNullOrEmpty(orderTab)) orderTab = OrderPageModel.ORDER_COMPLETED; //setting the default tab
            if (dateStart == null) dateStart = DateTime.Now.AddDays(-7);
            if (dateEnd == null) dateEnd = DateTime.Now;
            else dateEnd = dateEnd.Value.Date + new TimeSpan(23, 59, 59);
            if (HasAddress == null) HasAddress = true;

            DateTime dateStartParam = dateStart.Value.ToUniversalTime();
            DateTime dateEndParam = dateEnd.Value.ToUniversalTime();
            orderDetailQuery = orderDetailQuery.Where(detail => detail.CreateDate >= dateStartParam && detail.CreateDate <= dateEndParam);
            if (formTab == OrderPageModel.TAB_NAME && (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(email)))
            {
                orderDetailQuery = orderDetailQuery.Where(detail => detail.Order.Campaign != null && (detail.Order.Campaign.Contains(name)));
                if (email != string.Empty)
                {
                    IQueryable<StoreOrderDetail> tempQuery = orderDetailQuery;
                    orderDetailQuery = Enumerable.Empty<StoreOrderDetail>().AsQueryable();
                    var order = new List<StoreOrderDetail>();
                    IQueryable<StoreDetailSpecialProductItem> specialProductItems = StoreService.GetAll<StoreDetailSpecialProductItem>(true).Where(detail => detail.ASIContactEmail != null && detail.ASIContactEmail.Contains(email));
                    if (specialProductItems != null)
                    {
                        foreach (var item in specialProductItems)
                        {
                            IQueryable<StoreOrderDetail> orderDetail = tempQuery.Where(detail => detail.Id == item.OrderDetailId);
                            if (orderDetail != null && orderDetail.Any())
                            {
                                order.AddRange(orderDetail.ToList());
                            }
                        }
                        orderDetailQuery = order.Distinct().AsQueryable();
                    }
                }
            }
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
            IList<StoreOrderDetail> orderDetails = orderDetailQuery.OrderByDescending(detail => detail.Order.Id).ToList();

            OrderPageModel viewModel = new OrderPageModel(StoreService, EncryptionService, orderDetails, true);
            //pass the search values back into the page model so they can be displayed again
            viewModel.Total = orderDetails.Sum(item => item.Order.Total);
            viewModel.campaign = GetCampaign();
            if (dateStart.HasValue) viewModel.StartDate = dateStart.Value.ToString("MM/dd/yyyy");
            if (dateEnd.HasValue) viewModel.EndDate = dateEnd.Value.ToString("MM/dd/yyyy");
            if (name != null) viewModel.Name = name;
            viewModel.ASIContactEmail = email;
            viewModel.FormTab = formTab;
            viewModel.OrderTab = orderTab;
            if (HasAddress.HasValue)
            {
                viewModel.HasAddress = HasAddress.Value.ToString();
                viewModel.chkHasAddress = HasAddress.Value;
            }
            else
            {
                viewModel.chkHasAddress = true;
            }
            return View("../Store/Admin/ShowForms", viewModel);
        }

        private IList<SelectListItem> GetCampaign()
        {
            IList<SelectListItem> campaignList = null;
            IQueryable<StoreOrderDetail> orderDetailQuery = StoreService.GetAll<StoreOrderDetail>(true).Where(detail => detail.Product.Id == ProductId);
            if (orderDetailQuery != null)
            {
                campaignList = new List<SelectListItem>();
                foreach (var campaign in orderDetailQuery.Select(detail => detail.Order.Campaign).Distinct().OrderBy(name => name))
                {
                    if (!string.IsNullOrEmpty(campaign))
                    {
                        campaignList.Add(new SelectListItem() { Text = campaign, Value = campaign, Selected = false });
                    }
                }
            }
            return campaignList;
        }
    }
}
