﻿using asi.asicentral.model.store;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.interfaces;

namespace asi.asicentral.web.model.store
{
    public class OrderModel
    {
        private const string DECRYPT_KEY = "mk8$3njkl";
        private LegacyOrderDetail orderDetail;

        private OrderModel()
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

        public OrderStatus ApprovalStatus
        {
            get { return orderDetail.Order.ProcessStatus; }
        }

        public string CreditCard { private set; get; }

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
                if (orderDetail.Order.CreditCard != null && !String.IsNullOrEmpty(orderDetail.Order.CreditCard.Name)) billing.Append(orderDetail.Order.CreditCard.Name + ", ");
                if (!String.IsNullOrEmpty(orderDetail.Order.BillCity)) billing.Append(orderDetail.Order.BillCity + ", ");
                if (!String.IsNullOrEmpty(orderDetail.Order.BillState)) billing.Append(orderDetail.Order.BillState + ", ");
                if (!String.IsNullOrEmpty(orderDetail.Order.BillZip)) billing.Append(orderDetail.Order.BillZip + ", ");
                if (!String.IsNullOrEmpty(orderDetail.Order.BillCountry)) billing.Append(orderDetail.Order.BillCountry);
                return billing.ToString();
            }
        }

        public DateTime DateOrderCreated
        {
            get {
                DateTime date = DateTime.MinValue;
                if (this.orderDetail != null && this.orderDetail.Order != null && this.orderDetail.Order.DateCreated.HasValue)
                    date = this.orderDetail.Order.DateCreated.Value;
                return date;
            }
        }

        public string ContextType { get; set; }
    

        public bool ShowIcons { get; set; }
        public string CompletedStep { get; set; }
        public String Company { get; private set; }

        public LegacyOrderDetailApplication Application { private set; get; }

        public static OrderModel CreateOrder(IStoreService storeService, IEncryptionService encryptionService, LegacyOrderDetail orderDetail)
        {
            OrderModel order = new OrderModel();
            order.orderDetail = orderDetail;
            order.CompletedStep = orderDetail.Order.CompletedStep.ToString();
            order.Application = storeService.GetApplication(orderDetail);
            order.ShowIcons = orderDetail.Order.ProcessStatus == OrderStatus.Pending && order.Application != null && orderDetail.Order.Status != null && orderDetail.Order.Status == true;
            if (order.Application != null)
                order.Company = order.Application.Company;
            if (orderDetail.Order.CreditCard != null && !string.IsNullOrEmpty(orderDetail.Order.CreditCard.Number))
            {
                order.CreditCard = orderDetail.Order.CreditCard.Type;
                if (orderDetail.Order.CreditCard.Number.Length > 8)
                    order.CreditCard += "-" + encryptionService.LegacyDecrypt(DECRYPT_KEY, orderDetail.Order.CreditCard.Number);
                else
                    order.CreditCard += "-" + orderDetail.Order.CreditCard.Number;
                if (orderDetail.Order.CreditCard.ExpMonth != null && orderDetail.Order.CreditCard.ExpMonth.Length > 1 &&
                    orderDetail.Order.CreditCard.ExpYear != null && orderDetail.Order.CreditCard.ExpYear.Length > 1)

                    order.CreditCard += " (" + orderDetail.Order.CreditCard.ExpMonth + "/" + orderDetail.Order.CreditCard.ExpYear.Substring(orderDetail.Order.CreditCard.ExpYear.Length - 2) + ")";
            }

            Context context = null;
            if (orderDetail != null && orderDetail.Order != null && orderDetail.Order.ContextId != null)
            {
                context = storeService.GetAll<Context>().Where(ctx => ctx.Id == orderDetail.Order.ContextId).SingleOrDefault();
                if (context != null && !string.IsNullOrEmpty(context.Type))
                    order.ContextType = context.Type;
            }

            return order;
        }
    }
}