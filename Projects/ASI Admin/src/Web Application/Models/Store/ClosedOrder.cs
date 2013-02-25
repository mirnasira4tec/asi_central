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
        private OrderDetail orderDetail;
        
        public OrderDetailApplication Application { set; get; }
        public String Email { set; get; }
        public int Quantity
        {
            get
            {
                return orderDetail.Quantity.HasValue ? orderDetail.Quantity.Value : 0;
            }
        }

        public int OrderId
        {
            get
            {
                return orderDetail.OrderId;
            }
        }
        public String Item
        {
            get
            {
                return orderDetail.Product.Description;
            }
        }
        public Decimal Price
        {
            get
            {
                if (this.orderDetail.Subtotal.HasValue) return Math.Round(this.orderDetail.Subtotal.Value, 2);
                else return 0.00M;
            }
        }
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

        public void SetOrderDetail(OrderDetail orderDetail)
        {
            this.orderDetail = orderDetail;
        }

        public void SetApplicationFromService(IStoreService StoreService)
        {
            this.Application = StoreService.GetApplication(this.orderDetail);
        }
    }
}