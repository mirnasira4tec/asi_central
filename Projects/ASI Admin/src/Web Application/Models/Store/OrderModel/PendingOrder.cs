using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace asi.asicentral.web.Models.Store
{
    public class PendingOrder
    {
        private OrderDetail orderDetail;

        public int OrderId
        {
            get { return orderDetail.OrderId; }
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
        
        public Nullable<int> Phone { get; set; }

        public String Item
        {
            get { return orderDetail.Product.Description; }
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
        
        public DateTime DateOrerCreated 
        { 
            get { return this.orderDetail.Order.DateCreated.Value; }
        }

        public String Company { get; set; }

        public void SetOrderDetail(OrderDetail orderDetail)
        {
            this.orderDetail = orderDetail;
        }

        public void SetApplicationFromService(IStoreService StoreService)
        {
            OrderDetailApplication application = StoreService.GetApplication(this.orderDetail);
            
            // just for name of company
            if (application != null)
            {
                SupplierMembershipApplication supplierApplication = StoreService.GetSupplierApplication(this.orderDetail);
                if (supplierApplication != null) this.Company = supplierApplication.Company;

                DistributorMembershipApplication membershipApplication = StoreService.GetDistributorApplication(this.orderDetail);
                if (membershipApplication != null) this.Company = membershipApplication.Company;
            }
        }
    }
}