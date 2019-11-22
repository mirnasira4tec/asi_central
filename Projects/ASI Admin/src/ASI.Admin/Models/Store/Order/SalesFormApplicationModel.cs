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

        // distributor questions
        public bool? IsAuthorizedToBindCompany { get; set; }
        public bool? IsForResale { get; set; }
        public bool? IsOnlyProfitReseller { get; set; }
        public bool? IsDetailsProvider { get; set; }
        public bool? IsApplyingForMembership { get; set; }
        public bool? IsChangesInformed { get; set; }
        public bool? IsDataCertified { get; set; }
        public string OtherCompanyName { get; set; }
        public string ApprovedSignature { get; set; }

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
	        OrderDetailComments = orderdetail.Comments;

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

            var distQuestions = storeService.GetAll<StoreDetailDistributorMembership>(false).FirstOrDefault(detail => detail.OrderDetailId == OrderDetailId);
            if( distQuestions != null)
            {
                IsAuthorizedToBindCompany = distQuestions.IsAuthorizedToBindCompany;
                IsForResale = distQuestions.IsForResale;
                IsOnlyProfitReseller = distQuestions.IsOnlyProfitReseller;
                IsDetailsProvider = distQuestions.IsDetailsProvider;
                IsApplyingForMembership = distQuestions.IsApplyingForMembership;
                IsChangesInformed = distQuestions.IsChangesInformed;
                IsDataCertified = distQuestions.IsDataCertified;
                OtherCompanyName = distQuestions.OtherCompanyName;
                ApprovedSignature = distQuestions.ApprovedSignature;
            }
        }
    }
}