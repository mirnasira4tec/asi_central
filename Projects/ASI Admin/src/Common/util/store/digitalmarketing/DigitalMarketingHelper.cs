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
    public class DigitalMarketingHelper
    {
        //Supplement options
        public static readonly int AD_WORDS_INCREMENT = 50;
        public static readonly int DIGITAL_MARKETING_CONTEXTID = 24;

        public static IList<SelectListItem> GetGoogleAdWordOptions()
        {
            IList<SelectListItem> selectedItems = new List<SelectListItem>();
            for (int option = 2; option <= 15; option++)
            {
                selectedItems.Add(new SelectListItem() { Text = option.ToString(), Value = option.ToString(), Selected = false });
            }
            return selectedItems;
        }
    }
}
