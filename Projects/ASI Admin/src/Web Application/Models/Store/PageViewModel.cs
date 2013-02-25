using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.Models.Store
{
    public class PageViewModel
    {
        public const String DATE = "date";
        public const String ORDER = "order";
        public const String PRODUCT = "product";
        public const String NAME = "name";
        public const String TIMMS = "timms";
        public const String COMPLETED = "completedorders";
        public const String PENDING = "pendingorders";

        private IStoreService StoreObjectService { get; set; }
        public IList<CompletedOrder> CompletedOrders { set; get; }
        public IList<PendingOrder> PendingOrders { set; get; }
        public String FormTab { get; set; }
        public String OrderTab { get; set; }

        public PageViewModel(IStoreService StoreObjectService) 
        {
            this.StoreObjectService = StoreObjectService;
            CompletedOrders = new List<CompletedOrder>();
            PendingOrders = new List<PendingOrder>();
        }

        public void ConstructPendingOrders(IList<OrderDetail> orderDetails)
        {
            foreach (OrderDetail order in orderDetails)
            {
                PendingOrder pendingOrder = new PendingOrder();
                pendingOrder.SetOrderDetail(order);
                pendingOrder.SetApplicationFromService(this.StoreObjectService);
                this.PendingOrders.Add(pendingOrder);
            }
        }

        public void ConstructCompletedOrders(IList<OrderDetail> orderDetails)
        {
            foreach (OrderDetail order in orderDetails)
            {
                CompletedOrder completedOrder = new CompletedOrder();
                completedOrder.SetOrderDetail(order);
                completedOrder.SetApplicationFromService(this.StoreObjectService);
                this.CompletedOrders.Add(completedOrder);
            }
        }
    }
}