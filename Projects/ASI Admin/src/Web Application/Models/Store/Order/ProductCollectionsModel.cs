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
    public class ProductCollectionsModel : MembershipModel
    { 
        public decimal Cost { get; set; }
        public bool IsStoreRequest { get; set; }
        public IList<StoreDetailProductCollection> productCollections { get; set; }
        
        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public ProductCollectionsModel()
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
        }

        public ProductCollectionsModel(StoreOrderDetail orderdetail, IStoreService storeService)
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
            StoreOrder order = orderdetail.Order;
            BillingIndividual = order.BillingIndividual;
            OrderDetailId = orderdetail.Id;
            if (orderdetail.OptionId.HasValue) this.OptionId = orderdetail.OptionId;
            this.Quantity = orderdetail.Quantity;
           
            if (orderdetail.Product != null)
            {
                ProductName = orderdetail.Product.Name;
                ProductId = orderdetail.Product.Id;
                Cost = orderdetail.Cost;
            }

            productCollections = storeService.GetAll<StoreDetailProductCollection>().Where(collection => collection.OrderDetailId == OrderDetailId).ToList();

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