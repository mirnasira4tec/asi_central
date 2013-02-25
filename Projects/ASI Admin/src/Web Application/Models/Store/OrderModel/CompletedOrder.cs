using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using asi.asicentral.model.store;
using asi.asicentral.interfaces;
using System.Text;

namespace asi.asicentral.web.Models.Store
{
    public class CompletedOrder
    {
        private OrderDetail orderDetail;
        
        public OrderDetailApplication Application { set; get; }
        
        public String Email 
        {
            get 
            {
                if (this.orderDetail.Order.Membership == null) return "";
                else return (this.orderDetail.Order.Membership.Email);
            } 
        }

        public String Name
        {
            get 
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (!String.IsNullOrEmpty(this.orderDetail.Order.BillFirstName)) stringBuilder.Append(this.orderDetail.Order.BillFirstName + ", ");
                if (!String.IsNullOrEmpty(this.orderDetail.Order.BillLastName)) stringBuilder.Append(this.orderDetail.Order.BillLastName);
                return stringBuilder.ToString();
            }
        }

        public int Quantity
        {
            get { return orderDetail.Quantity.HasValue ? orderDetail.Quantity.Value : 0; }
        }

        public int OrderId
        {
            get { return orderDetail.OrderId; }
        }
        public String Item
        {
            get { return orderDetail.Product.Description; }
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
                StringBuilder stringBuilder = new StringBuilder();
                if (!String.IsNullOrEmpty(orderDetail.Order.BillFirstName)) stringBuilder.Append(orderDetail.Order.BillFirstName + ", ");
                if (!String.IsNullOrEmpty(orderDetail.Order.BillLastName)) stringBuilder.Append(orderDetail.Order.BillLastName + ", ");
                if (!String.IsNullOrEmpty(orderDetail.Order.BillCity)) stringBuilder.Append(orderDetail.Order.BillCity + ", ");
                if (!String.IsNullOrEmpty(orderDetail.Order.BillState)) stringBuilder.Append(orderDetail.Order.BillState + ", ");
                if (!String.IsNullOrEmpty(orderDetail.Order.BillZip)) stringBuilder.Append(orderDetail.Order.BillZip + ", ");
                if (!String.IsNullOrEmpty(orderDetail.Order.BillCountry)) stringBuilder.Append(orderDetail.Order.BillCountry + "");
                return stringBuilder.ToString();
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