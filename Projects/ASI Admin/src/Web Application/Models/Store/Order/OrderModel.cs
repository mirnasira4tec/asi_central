using asi.asicentral.model.store;
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
        private StoreOrderDetail orderDetail;

        private OrderModel()
        {
        }

        public int OrderId
        {
            get { return orderDetail.Order.Id; }
        }

        public int? ContextId
        {
            get { return this.orderDetail.Order.Context != null ? this.orderDetail.Order.Context.Id : (int?)null; }
        }

        public String Email
        {
            get
            {
                if (this.orderDetail.Order.GetContact() == null)
                {
                    return string.IsNullOrEmpty(this.orderDetail.Order.LoggedUserEmail) ? "" : this.orderDetail.Order.LoggedUserEmail;
                }
                else
                {
                    return (this.orderDetail.Order.GetContact().Email);
                }
            }
        }

        public String Name
        {
            get
            {
                string name = string.Empty;
                StoreIndividual contact = this.orderDetail.Order.GetContact();
                if ( contact != null)
                {
                    if (!String.IsNullOrEmpty(contact.LastName))
                        name = contact.LastName;
                    if (!String.IsNullOrEmpty(contact.FirstName))
                        name = contact.FirstName + (name.Length > 0 ? ", " + name : string.Empty);
                }
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
            get { return (orderDetail.Product != null ? orderDetail.Product.Name : String.Empty); }
        }

        public String ProductType
        {
            get { return (orderDetail.Product != null ? orderDetail.Product.Type : String.Empty); }
        }

        public int OrderDetailId
        {
            get { return orderDetail.Id; }
        }

        public int Quantity
        {
            get { return orderDetail.Quantity; }
        }

        public Decimal Price
        {
            get
            {
                return Math.Round(orderDetail.Cost, 2);
            }
        }

        public String Billing
        {
            get
            {
                StringBuilder billing = new StringBuilder();
                if (orderDetail.Order != null)
                {
                    if (orderDetail.Order.CreditCard != null && !String.IsNullOrEmpty(orderDetail.Order.CreditCard.CardHolderName)) billing.Append(orderDetail.Order.CreditCard.CardHolderName + ", ");
                    if (orderDetail.Order.BillingIndividual != null && orderDetail.Order.BillingIndividual.Address != null)
                    {
                        StoreAddress address = orderDetail.Order.BillingIndividual.Address;
                        if (!String.IsNullOrEmpty(address.City)) billing.Append(address.City + ", ");
                        if (!String.IsNullOrEmpty(address.State)) billing.Append(address.State + ", ");
                        if (!String.IsNullOrEmpty(address.Zip)) billing.Append(address.Zip + ", ");
                        if (!String.IsNullOrEmpty(address.Country)) billing.Append(address.Country);
                    }
                }
                return billing.ToString();
            }
        }

        public DateTime DateOrderCreated
        {
            get {
                DateTime date = DateTime.MinValue;
                if (this.orderDetail != null && this.orderDetail.Order != null )
                    date = this.orderDetail.Order.CreateDate;
                return date;
            }
        }

        public string ContextType { get; set; }
        public bool IsProduct { get; set; }
    

        public bool ShowIcons { get; set; }
        public string CompletedStep { get; set; }
        public String Company { get; private set; }

        public StoreDetailApplication Application { private set; get; }

        public static OrderModel CreateOrder(IStoreService storeService, IEncryptionService encryptionService, StoreOrderDetail orderDetail)
        {
            OrderModel order = new OrderModel();
            order.orderDetail = orderDetail;
            order.CompletedStep = orderDetail.Order.CompletedStep.ToString();
            order.Application = storeService.GetApplication(orderDetail);
            bool isShowIcons = orderDetail.Order != null && orderDetail.Order.ProcessStatus == OrderStatus.Pending && orderDetail.Order.IsCompleted;
            order.ShowIcons =  isShowIcons && order.Application != null;
            if (!order.ShowIcons) order.ShowIcons = isShowIcons && order.ProductType == "Product";
            if (!order.ShowIcons) order.ShowIcons = isShowIcons && orderDetail.Order.ContextId != null;
            if (orderDetail.Order.Company != null)
                order.Company = orderDetail.Order.Company.Name;
            if (orderDetail.Order.CreditCard != null && !string.IsNullOrEmpty(orderDetail.Order.CreditCard.CardNumber))
            {
                order.CreditCard = orderDetail.Order.CreditCard.CardType;
                if (orderDetail.Order.CreditCard.CardNumber.Length > 8)
                    order.CreditCard += "-" + encryptionService.LegacyDecrypt(DECRYPT_KEY, orderDetail.Order.CreditCard.CardNumber);
                else
                    order.CreditCard += "-" + orderDetail.Order.CreditCard.CardNumber;
                if (orderDetail.Order.CreditCard.ExpMonth != null && orderDetail.Order.CreditCard.ExpMonth.Length > 1 &&
                    orderDetail.Order.CreditCard.ExpYear != null && orderDetail.Order.CreditCard.ExpYear.Length > 1 &&
                    !string.IsNullOrEmpty(orderDetail.Order.CreditCard.CardHolderName) &&
                    orderDetail.Order.CreditCard.CardHolderName != asi.asicentral.util.store.Helper.CARD_ON_FILE)

                    order.CreditCard += " (" + orderDetail.Order.CreditCard.ExpMonth + "/" + orderDetail.Order.CreditCard.ExpYear.Substring(orderDetail.Order.CreditCard.ExpYear.Length - 2) + ")";
            }

            Context context = null;
            if (orderDetail != null && orderDetail.Order != null && orderDetail.Order.ContextId != null)
            {
                context = storeService.GetAll<Context>().Where(ctx => ctx.Id == orderDetail.Order.ContextId).SingleOrDefault();
                if (context != null && !string.IsNullOrEmpty(context.Type))
                {
                    order.ContextType = context.Name;
                    if (context.Type == "Product") order.IsProduct = true; else order.IsProduct = false;
                }
            }

            return order;
        }
    }
}