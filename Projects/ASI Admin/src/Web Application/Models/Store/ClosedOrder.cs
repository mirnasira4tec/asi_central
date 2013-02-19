using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using asi.asicentral.model.store;
using asi.asicentral.interfaces;

namespace asi.asicentral.web.Models.Store
{
    public class ClosedOrder
    {
        public OrderDetail orderDetail { set; get; }
        public OrderDetailApplication Application { set; get; }
        public String Billing
        {
            get
            {
                return orderDetail.Order.BillFirstName + " " +
                    orderDetail.Order.BillLastName + ", " +
                    orderDetail.Order.BillCity + ", " +
                    orderDetail.Order.BillState + ", " +
                    orderDetail.Order.BillZip + ", " +
                    orderDetail.Order.BillCountry + ". Phone: " + orderDetail.Order.BillPhone;
            }
        }

        public void SetApplicationFromService(IStoreService StoreService)
        {
            this.Application = StoreService.GetApplication(this.orderDetail);
        }
    }
}