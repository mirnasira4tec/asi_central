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
        public static readonly string[] ESPAdvertising_BANNER_TILE_TOWER_COST = { "750","450","1500"};
        public static readonly string[] ESPAdvertising_PROMO_CAFE_COST = { "750", "199", "450" };
        public static readonly string[] ESPAdvertising_CLEARANCE_COST = { "300", "1995" };
        public static readonly string[] ESPAdvertising_NEW_COST = { "300", "1995" };
        public static readonly string[] ESPAdvertising_RUSH_COST = { "400", "1995" };
        public static readonly string[] ESPAdvertising_Video_COST = { "499", "79" };

        public static IList<SelectListItem> GetAdTypeOptions(string value = null)
        {
            IList<SelectListItem> adTypeOptions = new List<SelectListItem>();
            adTypeOptions.Add(new SelectListItem() { Text = Resource.BannerAd, Value = "0", Selected = ("0" == value) });
            adTypeOptions.Add(new SelectListItem() { Text = Resource.TitleAd, Value = "1", Selected = ("1" == value) });
            adTypeOptions.Add(new SelectListItem() { Text = Resource.TowerTuesdayAd, Value = "2", Selected = ("2" == value) });
            return adTypeOptions;
        }

        public static IList<SelectListItem> GetNumberOfProducts_Clearance(string value = null)
        {
            IList<SelectListItem> clearanceOptions = new List<SelectListItem>();
            clearanceOptions.Add(new SelectListItem() { Text = Resource.SelectNumberOfProducts, Value = "1", Selected = ("1" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_300_5_Products_PerMonth, Value = "2", Selected = ("2" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_1995_50_Products_PerYear, Value = "3", Selected = ("3" == value) });
            return clearanceOptions;
        }

        public static IList<SelectListItem> GetNumberOfProducts_New(string value = null)
        {
            IList<SelectListItem> clearanceOptions = new List<SelectListItem>();
            clearanceOptions.Add(new SelectListItem() { Text = Resource.SelectNumberOfProducts, Value = "1", Selected = ("1" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_300_5_Products_PerMonth, Value = "2", Selected = ("2" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_1995_50_Products_PerYear, Value = "3", Selected = ("3" == value) });
            return clearanceOptions;
        }

        public static IList<SelectListItem> GetNumberOfProducts_Rush(string value = null)
        {
            IList<SelectListItem> clearanceOptions = new List<SelectListItem>();
            clearanceOptions.Add(new SelectListItem() { Text = Resource.SelectNumberOfProducts, Value = "1", Selected = ("1" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_400_5_Products_PerMonth, Value = "2", Selected = ("2" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_1995_50_Products_PerYear, Value = "3", Selected = ("3" == value) });
            return clearanceOptions;
        }

        public static IList<SelectListItem> GetESPOnlineOptions(string value = null)
        {
            IList<SelectListItem> espOnlineOptions = new List<SelectListItem>();
            espOnlineOptions.Add(new SelectListItem() { Text = Resource.MainChatRoom_750, Value = "0", Selected = ("0" == value) });
            espOnlineOptions.Add(new SelectListItem() { Text = Resource.BuddyList_199, Value = "1", Selected = ("1" == value) });
            espOnlineOptions.Add(new SelectListItem() { Text = Resource.InstantMessenger_450, Value = "2", Selected = ("2" == value) });
            return espOnlineOptions;
        }
    }
}
