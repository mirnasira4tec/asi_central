using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.Resources;
using asi.asicentral.util.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.model.store
{
    public class EmailExpressModel : MembershipModel
    {

        #region Email Express information

        public int ItemTypeId { get; set; }
        public string Dates { get; set; }
        public int Sends { get; set; }
        
        public List<System.Web.Mvc.SelectListItem> ItemTypes { get { return asi.asicentral.util.store.EmailExpressHelper.GetItemTypeOptions(); } }
        
        #endregion Email Express information

        public IList<StoreDetailEmailExpressItem> Videos { get; set; }
        
        

        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public EmailExpressModel()
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
        }

        public EmailExpressModel(StoreOrderDetail orderdetail, StoreDetailEmailExpress emailepxress, IStoreService storeService)
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
            StoreOrder order = orderdetail.Order;
            BillingIndividual = order.BillingIndividual;
            OrderDetailId = orderdetail.Id;

            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            if (orderdetail.Product != null)
            {
                ProductName = orderdetail.Product.Name;
                ProductId = orderdetail.Product.Id;
            }

            #region Fill Email Express data

            StoreDetailEmailExpress item = storeService.GetAll<StoreDetailEmailExpress>().Where(model => model.OrderDetailId == OrderDetailId).SingleOrDefault();
            if (item != null)
            {
                this.ItemTypeId = item.ItemTypeId;
                this.TotalCost = orderdetail.Quantity * orderdetail.Cost;
                string Dates = string.Empty;
                this.Sends = orderdetail.Quantity;
                this.Dates = Dates;
                
            }
            #endregion
            OrderId = order.Id;
            Price = order.Total;
            OrderStatus = order.ProcessStatus;
            IsCompleted = order.IsCompleted;
            MembershipModelHelper.PopulateModel(this, orderdetail);
        }
    }
}