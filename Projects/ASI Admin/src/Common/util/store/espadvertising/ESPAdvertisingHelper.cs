using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace asi.asicentral.util.store
{
    public class ESPAdvertisingHelper
    {
        public static IList<SelectListItem> GetAdTypeOptions(string value = null)
        {
            IList<SelectListItem> adTypeOptions = new List<SelectListItem>();
            adTypeOptions.Add(new SelectListItem() { Text = Resource.BannerAd, Value = "750", Selected = ("750" == value) });
            adTypeOptions.Add(new SelectListItem() { Text = Resource.TitleAd, Value = "450", Selected = ("450" == value) });
            adTypeOptions.Add(new SelectListItem() { Text = Resource.TowerTuesdayAd, Value = "1500", Selected = ("1500" == value) });
            return adTypeOptions;
        }

        public static IList<SelectListItem> GetNumberOfProducts_Clearance(string value = null)
        {
            IList<SelectListItem> clearanceOptions = new List<SelectListItem>();
            clearanceOptions.Add(new SelectListItem() { Text = Resource.SelectNumberOfProducts, Value = "0", Selected = ("0" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_300_5_Products_PerMonth, Value = "300", Selected = ("300" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_1995_50_Products_PerYear, Value = "166.25", Selected = ("166.25" == value) });
            return clearanceOptions;
        }

        public static IList<SelectListItem> GetNumberOfProducts_New(string value = null)
        {
            IList<SelectListItem> clearanceOptions = new List<SelectListItem>();
            clearanceOptions.Add(new SelectListItem() { Text = Resource.SelectNumberOfProducts, Value = "0", Selected = ("0" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_300_5_Products_PerMonth, Value = "300", Selected = ("300" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_1995_50_Products_PerYear, Value = "166.25", Selected = ("166.25" == value) });
            return clearanceOptions;
        }

        public static IList<SelectListItem> GetNumberOfProducts_Rush(string value = null)
        {
            IList<SelectListItem> clearanceOptions = new List<SelectListItem>();
            clearanceOptions.Add(new SelectListItem() { Text = Resource.SelectNumberOfProducts, Value = "0", Selected = ("0" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_400_5_Products_PerMonth, Value = "400", Selected = ("400" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_1995_50_Products_PerYear, Value = "166.25", Selected = ("166.25" == value) });
            return clearanceOptions;
        }

        public static IList<SelectListItem> GetESPOnlineOptions(string value = null)
        {
            IList<SelectListItem> espOnlineOptions = new List<SelectListItem>();
            espOnlineOptions.Add(new SelectListItem() { Text = Resource.MainChatRoom_750, Value = "750", Selected = ("750" == value) });
            espOnlineOptions.Add(new SelectListItem() { Text = Resource.BuddyList_199, Value = "199", Selected = ("199" == value) });
            espOnlineOptions.Add(new SelectListItem() { Text = Resource.InstantMessenger_450, Value = "450", Selected = ("450" == value) });
            return espOnlineOptions;
        }
    }
}
