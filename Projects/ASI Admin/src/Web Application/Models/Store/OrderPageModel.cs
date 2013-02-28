using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.Models.Store
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

        public IList<Order> Orders { set; get; }
        public String FormTab { get; set; }
        public String OrderTab { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Product { get; set; }
        public int OrderIdentifier { get; set; }
        public string Name { get; set; }

        public OrderPageModel(IStoreService storeService, IList<OrderDetail> orderDetails) 
        {
            Orders = new List<Order>();
            if (orderDetails != null && storeService != null)
            {
                foreach (OrderDetail orderDetail in orderDetails)
                {
                    Orders.Add(Order.CreateOrder(storeService, orderDetail));
                }
            }
        }
    }
}