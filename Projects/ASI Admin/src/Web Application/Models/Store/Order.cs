﻿using asi.asicentral.model.store;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.interfaces;

namespace asi.asicentral.web.Models.Store
{
    public class Order
    {
        private OrderDetail orderDetail;

        private Order()
        {
        }

        public int OrderId
        {
            get { return orderDetail.OrderId; }
        }

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
                string name = string.Empty;
                if (!String.IsNullOrEmpty(this.orderDetail.Order.BillLastName)) 
                    name = this.orderDetail.Order.BillLastName;
                if (!String.IsNullOrEmpty(this.orderDetail.Order.BillFirstName)) 
                    name = this.orderDetail.Order.BillFirstName + (name.Length > 0 ? ", " + name : string.Empty);
                return name;
            }
        }
        public String Item
        {
            get { return (orderDetail.Product != null ? orderDetail.Product.Description : String.Empty); }
        }

        public int Quantity
        {
            get { return orderDetail.Quantity.HasValue ? orderDetail.Quantity.Value : 0; }
        }

        public Decimal Price
        {
            get
            {
                if (orderDetail.Subtotal.HasValue) return Math.Round(orderDetail.Subtotal.Value, 2);
                else return 0.00M;
            }
        }
        
        public String Billing
        {
            get
            {
                StringBuilder billing = new StringBuilder();
                if (!String.IsNullOrEmpty(orderDetail.Order.BillFirstName)) billing.Append(orderDetail.Order.BillFirstName + ", ");
                if (!String.IsNullOrEmpty(orderDetail.Order.BillLastName)) billing.Append(orderDetail.Order.BillLastName + ", ");
                if (!String.IsNullOrEmpty(orderDetail.Order.BillCity)) billing.Append(orderDetail.Order.BillCity + ", ");
                if (!String.IsNullOrEmpty(orderDetail.Order.BillState)) billing.Append(orderDetail.Order.BillState + ", ");
                if (!String.IsNullOrEmpty(orderDetail.Order.BillZip)) billing.Append(orderDetail.Order.BillZip + ", ");
                if (!String.IsNullOrEmpty(orderDetail.Order.BillCountry)) billing.Append(orderDetail.Order.BillCountry);
                return billing.ToString();
            }
        }

        public DateTime DateOrderCreated
        {
            get { return this.orderDetail.Order.DateCreated.Value; }
        }

        public String Company { get; private set; }

        public OrderDetailApplication Application { private set; get; }

        public static Order CreateOrder(IStoreService storeService, OrderDetail orderDetail) 
        {
            Order order = new Order();
            order.orderDetail = orderDetail;
            order.Application = storeService.GetApplication(orderDetail);
            if (order.Application != null) 
                order.Company = order.Application.Company;
            return order;
        }
    }
}