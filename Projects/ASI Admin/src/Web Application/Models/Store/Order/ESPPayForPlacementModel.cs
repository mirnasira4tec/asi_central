using asi.asicentral.interfaces;
using asi.asicentral.model.ROI;
using asi.asicentral.model.store;
using asi.asicentral.oauth;
using asi.asicentral.Resources;
using asi.asicentral.services;
using asi.asicentral.util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public class ESPPayForPlacementModel : MembershipModel
    {
        #region ESP Advertising information
        public IList<PFPCategory> Categries { get; set; }
        #endregion ESP Advertising information

        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public ESPPayForPlacementModel()
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
        }

        public ESPPayForPlacementModel(StoreOrderDetail orderdetail, IStoreService storeService)
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

            #region Fill ESP Advertising details based on product
            string userEmail = order.LoggedUserEmail.ToLower();
            if (userEmail != null)
            {
	            string asiNumber = string.Empty;
				var user = ASIOAuthClient.GetUserByEmail(userEmail);
	            if (user != null) asiNumber = user.AsiNumber;
				if (!string.IsNullOrEmpty(asiNumber))
                {
                    IList<StoreDetailPayForPlacement> selectedCategories = storeService.GetAll<StoreDetailPayForPlacement>().Where(pfp => pfp.OrderDetailId == orderdetail.Id).ToList();
                    IROIService ROIService = new ROIService();
                    IEnumerable<Category> allCategories = ROIService.GetImpressionsPerCategory(Convert.ToInt32(asiNumber));
                    if (allCategories != null)
                    {
                        Categries = new List<PFPCategory>();
                        foreach (asi.asicentral.model.ROI.Category category in allCategories)
                        {
                            PFPCategory pfpc = new PFPCategory();
                            StoreDetailPayForPlacement dbPlacement = selectedCategories.Where(item => item.CategoryName == category.Name).SingleOrDefault();
                            if (dbPlacement != null)
                            {
                                pfpc.CategoryName = dbPlacement.CategoryName;
                                pfpc.IsSelected = true;
                                pfpc.CPMOption = dbPlacement.CPMOption;
                                pfpc.PaymentOption = dbPlacement.PaymentType;
                                if(dbPlacement.ImpressionsRequested != 0) pfpc.Impressions = dbPlacement.ImpressionsRequested.ToString();
                                if (dbPlacement.Cost != 0.0M)  pfpc.PaymentAmount = dbPlacement.Cost.ToString();
                            }
                            else
                            {
                                pfpc.CategoryName = category.Name;
                                pfpc.IsSelected = false;
                                pfpc.CPMOption = 0;
                                pfpc.PaymentOption = "FB";
                                pfpc.PaymentAmount = string.Empty;
                            }
                            Categries.Add(pfpc);
                        }
                    }
                }
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