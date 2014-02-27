using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using asi.asicentral.Resources;

namespace asi.asicentral.util.store
{
    public class EmailExpressHelper
    {
        public static readonly int[] EMAILEXPRESS_NUMBER_OF_ITEMS = { 1, 3, 6, 12, 26, 52, 120 };
        public static readonly decimal[] EMAILEXPRESS_PLATINUM = { 899M, 849M, 825M, 799M, 775M, 749M, 699M };
        public static readonly decimal[] EMAILEXPRESS_REGULAR = { 799M, 749M, 725M, 699M, 675M, 649M, 599M };
        public static readonly decimal EMAILEXPRESS_TARGETED = 349M;
        public static readonly int[] EMAILEXPRESS_ITEM_TYPE_IDS = { 1, 2, 3, 4 };
        public static readonly string[] EMAILEXPRESS_ITEM_TYPE_NAMES = { Resource.EmailExpress_Platinum, Resource.EmailExpress_Regular, Resource.EmailExpress_TargetedList, Resource.EmailExpress_Special };

        public static decimal GetCost(int ItemTypeId,int NumberOfItems)
        {
            decimal Cost = 0.0M;
            if ((ItemTypeId == 1 || ItemTypeId == 2) && NumberOfItems == 0) return Cost;
            switch(ItemTypeId)
            {
                case 1:
                    if (NumberOfItems < EMAILEXPRESS_NUMBER_OF_ITEMS[1])
                        Cost = EMAILEXPRESS_PLATINUM[0];
                    else if (NumberOfItems >= EMAILEXPRESS_NUMBER_OF_ITEMS[1] && NumberOfItems < EMAILEXPRESS_NUMBER_OF_ITEMS[2])
                        Cost = EMAILEXPRESS_PLATINUM[1];
                    else if (NumberOfItems >= EMAILEXPRESS_NUMBER_OF_ITEMS[2] && NumberOfItems < EMAILEXPRESS_NUMBER_OF_ITEMS[3])
                        Cost = EMAILEXPRESS_PLATINUM[2];
                    else if (NumberOfItems >= EMAILEXPRESS_NUMBER_OF_ITEMS[3] && NumberOfItems < EMAILEXPRESS_NUMBER_OF_ITEMS[4])
                        Cost = EMAILEXPRESS_PLATINUM[3];
                    else if (NumberOfItems >= EMAILEXPRESS_NUMBER_OF_ITEMS[4] && NumberOfItems < EMAILEXPRESS_NUMBER_OF_ITEMS[5])
                        Cost = EMAILEXPRESS_PLATINUM[4];
                    else if (NumberOfItems >= EMAILEXPRESS_NUMBER_OF_ITEMS[5] && NumberOfItems < EMAILEXPRESS_NUMBER_OF_ITEMS[6])
                        Cost = EMAILEXPRESS_PLATINUM[5];
                    else 
                        Cost = EMAILEXPRESS_PLATINUM[6];
                    break;
                case 2:
                    if (NumberOfItems < EMAILEXPRESS_NUMBER_OF_ITEMS[1])
                        Cost = EMAILEXPRESS_REGULAR[0];
                    else if (NumberOfItems >= EMAILEXPRESS_NUMBER_OF_ITEMS[1] && NumberOfItems < EMAILEXPRESS_NUMBER_OF_ITEMS[2])
                        Cost = EMAILEXPRESS_REGULAR[1];
                    else if (NumberOfItems >= EMAILEXPRESS_NUMBER_OF_ITEMS[2] && NumberOfItems < EMAILEXPRESS_NUMBER_OF_ITEMS[3])
                        Cost = EMAILEXPRESS_REGULAR[2];
                    else if (NumberOfItems >= EMAILEXPRESS_NUMBER_OF_ITEMS[3] && NumberOfItems < EMAILEXPRESS_NUMBER_OF_ITEMS[4])
                        Cost = EMAILEXPRESS_REGULAR[3];
                    else if (NumberOfItems >= EMAILEXPRESS_NUMBER_OF_ITEMS[4] && NumberOfItems < EMAILEXPRESS_NUMBER_OF_ITEMS[5])
                        Cost = EMAILEXPRESS_REGULAR[4];
                    else if (NumberOfItems >= EMAILEXPRESS_NUMBER_OF_ITEMS[5] && NumberOfItems < EMAILEXPRESS_NUMBER_OF_ITEMS[6])
                        Cost = EMAILEXPRESS_REGULAR[5];
                    else
                        Cost = EMAILEXPRESS_REGULAR[6];
                    break;
                case 3:
                    Cost = EMAILEXPRESS_TARGETED;
                    break;
            }
            return Cost;
        }

        public static List<SelectListItem> GetItemTypeOptions()
        {
            List<SelectListItem> dropdownOptions = new List<SelectListItem>();
            if (EMAILEXPRESS_ITEM_TYPE_IDS.Length > 0 && EMAILEXPRESS_ITEM_TYPE_NAMES.Length > 0)
            {
                for (int i = 0;i<EMAILEXPRESS_ITEM_TYPE_IDS.Length;i++)
                    dropdownOptions.Add(new SelectListItem() { Text = EMAILEXPRESS_ITEM_TYPE_NAMES[i], Value = EMAILEXPRESS_ITEM_TYPE_IDS[i].ToString(), Selected = false });
            }
            return dropdownOptions;
        }
    }
}



