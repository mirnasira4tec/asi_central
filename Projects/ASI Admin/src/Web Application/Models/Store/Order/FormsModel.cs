using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.model.store
{
    public class FormsModel : MembershipModel
    {
        public string StartDate { get; set; }
        public decimal Cost { get; set; }
        public bool IsStoreRequest { get; set; }
        public string AcceptedByName { get; set; }
        public int? OptionId { get; set; }
        public FormInstance FormInstanceObject { get; set; }
        public int FormInstanceId { get; set; }
        
        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public FormsModel()
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
        }

        public FormsModel(StoreOrderDetail orderdetail, IStoreService storeService)
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
            FormInstanceObject = storeService.GetAll<FormInstance>(false).SingleOrDefault(f => f.OrderDetailId == OrderDetailId);
            if(FormInstanceObject != null) FormInstanceId = FormInstanceObject.Id;

            MembershipModelHelper.PopulateModel(this, orderdetail);
        }
    }
}