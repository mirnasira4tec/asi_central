using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.model.store
{
    public class OrderPageModel
    {
        public const String TAB_DATE = "date";
        public const String TAB_ORDER = "order";
        public const String TAB_PRODUCT = "product";
        public const String TAB_NAME = "name";
        public const String TAB_TIMMS = "timms";
        public const String ORDER_COMPLETED = "completedorders";
        public const String ORDER_INCOMPLETE = "incompleteorders";
        public const String ORDER_PENDING = "pendingorders";
        public const String COMPANY_NAME = "companyname";

        public IList<OrderModel> Orders { set; get; }
        public String FormTab { get; set; }
        public String OrderTab { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ASIContactEmail { get; set; }
        public bool chkHasAddress { get; set; }
        public string HasAddress { get; set; }
        public string Product { get; set; }
        public int? Identifier { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public Decimal Total { get; set; }
        public IList<SelectListItem> campaign { get; set; }

        public OrderPageModel(IStoreService storeService, IEncryptionService encryptionService, IList<StoreOrderDetail> orderDetails, bool isShowForm = false) 
        {
            Orders = new List<OrderModel>();
            if (orderDetails != null && storeService != null)
            {
                string contactEmail = string.Empty;
                foreach (StoreOrderDetail orderDetail in orderDetails)
                {
                    if (isShowForm)
                    {
                        StoreDetailSpecialProductItem specialProductItem = storeService.GetAll<StoreDetailSpecialProductItem>(true).FirstOrDefault(detail => detail.OrderDetailId == orderDetail.Id);
                        if (specialProductItem != null)
                        {
                            contactEmail = specialProductItem.ASIContactEmail;
                        }
                    }
                    Orders.Add(OrderModel.CreateOrder(storeService, encryptionService, orderDetail, contactEmail));
                }
            }
        }
    }
}