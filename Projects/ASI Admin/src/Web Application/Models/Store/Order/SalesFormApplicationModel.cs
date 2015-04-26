using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.Resources;
using asi.asicentral.util.store;
using asi.asicentral.util.store.catalogadvertising;
using asi.asicentral.util.store.magazinesadvertising;
using asi.asicentral.web.Controllers.Store;
using asi.asicentral.web.model.store;
using Castle.DynamicProxy.Contributors;

namespace asi.asicentral.web.model.store
{
    public class SalesFormApplicationModel : MembershipModel
    {
        #region Sales Form information

        public IList<StoreDetailSpecialProductItem> SpecialProductItems { get; set; }

        #endregion Sales Form information

        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public SalesFormApplicationModel()
            : base()
        {
        }

        public SalesFormApplicationModel(StoreOrderDetail orderdetail, IList<StoreDetailSpecialProductItem> specialProducItems, IStoreService storeService)
            : base()
        {
            StoreOrder order = orderdetail.Order;
            BillingIndividual = order.BillingIndividual;
            OrderDetailId = orderdetail.Id;

            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            if (orderdetail.Product != null)
            {
                ProductName = HttpUtility.HtmlDecode(orderdetail.Product.Name);
                ProductId = orderdetail.Product.Id;
            }
            else
            {
                throw new Exception("Order detail doesn't exist.");
            }

            OrderId = order.Id;
            Price = order.Total;
            OrderStatus = order.ProcessStatus;
            IsCompleted = order.IsCompleted;
            MembershipModelHelper.PopulateModel(this, orderdetail);
            this.SpecialProductItems = specialProducItems;
        }
    }
}