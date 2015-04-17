using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asi.asicentral.interfaces;
using System.Web.Mvc;
using asi.asicentral.Resources;

namespace asi.asicentral.util.store
{
    public class ASIBrandBuilderHelper
    {
        //Supplement options
        public static readonly int AD_WORDS_INCREMENT = 50;
        public static readonly int[] MAIN_FEATURES = { 227, 234 };
        public static readonly int[] ASI_BRAND_BUILDER_PRODUCTIDS = { 96, 97, 98 };

        public static IList<SelectListItem> GetGoogleAdWordOptions()
        {
            IList<SelectListItem> selectedItems = new List<SelectListItem>();
            for (int option = 2; option <= 15; option++)
            {
                string text = string.Format("${0}", AD_WORDS_INCREMENT * option);
                selectedItems.Add(new SelectListItem() { Text = text, Value = option.ToString(), Selected = false });
            }
            return selectedItems;
        }

        public static IList<string> GetSummaryDetails(IStoreService storeService, StoreOrderDetail orderDetail)
        {
            IList<string> summaryItems = null;
            IList<ContextFeature> features = storeService.GetAll<ContextFeature>(true).Where(feature => MAIN_FEATURES.Contains(feature.Id)).ToList();
            if (features != null && features.Count > 0)
            {
                summaryItems = new List<string>();
                foreach (ContextFeature feature in features)
                {
                    foreach (ContextFeature featureProduct in feature.ChildFeatures)
                    {
                        summaryItems.Add(featureProduct.Name);
                    }
                }
                if (orderDetail != null && orderDetail.OptionId.HasValue && orderDetail.OptionId.Value != 0)
                    summaryItems.Add(string.Format("Extra Google AdWords Cost is ${0}", (orderDetail.OptionId.Value * AD_WORDS_INCREMENT).ToString()));
            }
            
            return summaryItems;
        }
    }
}
