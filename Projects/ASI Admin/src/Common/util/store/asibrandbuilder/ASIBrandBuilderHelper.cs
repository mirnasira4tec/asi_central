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
        public static readonly int[] ASI_BRAND_BUILDER_CONTEXTIDS = { 24, 11012 };
        public static readonly int[] MAIN_FEATURES = { 227, 234 };

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

        public static IList<ContextFeature> GetFeatureDetails(IStoreService storeService, StoreOrderDetail orderDetail)
        {
            IList<ContextFeature>  featureDetails= new List<ContextFeature>();
            IList<ContextFeature> features = storeService.GetAll<ContextFeature>(true).Where(feature => MAIN_FEATURES.Contains(feature.Id)).ToList();
            foreach (ContextFeature feature in features)
            {
                foreach (ContextFeature featureProduct in feature.ChildFeatures)
                {
                    featureDetails.Add(featureProduct);
                }
            }
            return featureDetails;
        }
    }
}
