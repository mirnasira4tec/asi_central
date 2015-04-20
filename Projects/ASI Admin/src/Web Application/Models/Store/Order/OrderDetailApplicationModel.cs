using asi.asicentral.model.store;
using asi.asicentral.Resources;
using asi.asicentral.util.store;
using asi.asicentral.web.Models.Store.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.model.store
{
    public class OrderDetailApplicationModel : MembershipModel
    { 
        public string StartDate { get; set; }
        public decimal Cost { get; set; }
        public bool IsStoreRequest { get; set; }
        public IList<SelectListItem> Options
        {
            get { return SupplierSpecialsHelper.GetPackagesOptions(); }
        }
        public string AcceptedByName { get; set; }
       
        public int? OptionId { get; set; }
       
        
        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public OrderDetailApplicationModel()
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
        }

        public OrderDetailApplicationModel(StoreOrderDetail orderdetail)
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
            StoreOrder order = orderdetail.Order;
            BillingIndividual = order.BillingIndividual;
            OrderDetailId = orderdetail.Id;
            this.AcceptedByName = orderdetail.AcceptedByName;
            if (orderdetail.OptionId.HasValue) this.OptionId = orderdetail.OptionId;
            if (orderdetail.DateOption.HasValue) this.StartDate = orderdetail.DateOption.Value.ToString("MM/dd/yyyy");
            this.Quantity = orderdetail.Quantity;
            this.AcceptedByName = orderdetail.AcceptedByName;
           
            if (orderdetail.Product != null)
            {
                ProductName = orderdetail.Product.Name;
                ProductId = orderdetail.Product.Id;
                Cost = orderdetail.Cost;
            }

            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            OrderId = order.Id;
            OrderStatus = order.ProcessStatus;
            Price = order.Total;
            IsCompleted = order.IsCompleted;
            IsStoreRequest = order.IsStoreRequest;
            MembershipModelHelper.PopulateModel(this, orderdetail);
        }
    }
}