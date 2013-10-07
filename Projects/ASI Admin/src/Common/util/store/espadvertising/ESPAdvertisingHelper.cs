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
        public static readonly decimal[] ESPAdvertising_CLEARANCE_COST = { 0.00M, 300.00M, 166.25M };
        public static readonly decimal[] ESPAdvertising_NEW_COST = { 0.00M, 300.00M, 166.25M };
        public static readonly decimal[] ESPAdvertising_RUSH_COST = { 0.00M, 400.00M, 166.25M };
        public static readonly decimal[] ESPAdvertising_Video_COST = { 41.58M, 6.58M };
        public static readonly string[] ESPAdvertising_PROMO_CAFE_COST = { "750", "199", "450" };
        public static readonly decimal[] ESPAdvertising_PFP_COST = { 295.00M, 275.00M, 260.00M, 240.00M, 230.00M, 220.00M, 210.00M };

        public static IList<SelectListItem> GetAdTypeOptions(string value = null)
        {
            IList<SelectListItem> adTypeOptions = new List<SelectListItem>();
            adTypeOptions.Add(new SelectListItem() { Text = Resource.BannerAd, Value = "1", Selected = ("1" == value) });
            adTypeOptions.Add(new SelectListItem() { Text = Resource.TitleAd, Value = "2", Selected = ("2" == value) });
            adTypeOptions.Add(new SelectListItem() { Text = Resource.TowerTuesdayAd, Value = "3", Selected = ("3" == value) });
            return adTypeOptions;
        }

        public static IList<SelectListItem> GetNumberOfProducts_Clearance(string value = null)
        {
            IList<SelectListItem> clearanceOptions = new List<SelectListItem>();
            clearanceOptions.Add(new SelectListItem() { Text = Resource.SelectNumberOfProducts, Value = "0", Selected = ("0" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_300_5_Products_PerMonth, Value = "1", Selected = ("1" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_1995_50_Products_PerYear, Value = "2", Selected = ("2" == value) });
            return clearanceOptions;
        }

        public static IList<SelectListItem> GetNumberOfProducts_New(string value = null)
        {
            IList<SelectListItem> clearanceOptions = new List<SelectListItem>();
            clearanceOptions.Add(new SelectListItem() { Text = Resource.SelectNumberOfProducts, Value = "0", Selected = ("0" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_300_5_Products_PerMonth, Value = "1", Selected = ("1" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_1995_50_Products_PerYear, Value = "2", Selected = ("2" == value) });
            return clearanceOptions;
        }

        public static IList<SelectListItem> GetNumberOfProducts_Rush(string value = null)
        {
            IList<SelectListItem> clearanceOptions = new List<SelectListItem>();
            clearanceOptions.Add(new SelectListItem() { Text = Resource.SelectNumberOfProducts, Value = "0", Selected = ("0" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_400_5_Products_PerMonth, Value = "1", Selected = ("1" == value) });
            clearanceOptions.Add(new SelectListItem() { Text = Resource.Cost_1995_50_Products_PerYear, Value = "2", Selected = ("2" == value) });
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
    

        public static IList<SelectListItem> GetCPMOptions(string value = null)
        {
            IList<SelectListItem> cpmOptions = new List<SelectListItem>();
            cpmOptions.Add(new SelectListItem() { Text = "1st - $295", Value = "0", Selected = ("0" == value) });
            cpmOptions.Add(new SelectListItem() { Text = "2nd - $275", Value = "1", Selected = ("1" == value) });
            cpmOptions.Add(new SelectListItem() { Text = "3rd - $260", Value = "2", Selected = ("2" == value) });
            cpmOptions.Add(new SelectListItem() { Text = "4th - $240", Value = "3", Selected = ("3" == value) });
            cpmOptions.Add(new SelectListItem() { Text = "5th - $230", Value = "4", Selected = ("4" == value) });
            cpmOptions.Add(new SelectListItem() { Text = "6th - $220", Value = "5", Selected = ("5" == value) });
            cpmOptions.Add(new SelectListItem() { Text = "7th - $210", Value = "6", Selected = ("6" == value) });
            return cpmOptions;
        }

        public static IList<SelectListItem> GetPaymentOptions(string value = null)
        {
            IList<SelectListItem> paymentOptions = new List<SelectListItem>();
            paymentOptions.Add(new SelectListItem() { Text = "Fixed Cost", Value = "FB", Selected = ("FB" == value) });
            paymentOptions.Add(new SelectListItem() { Text = "Impressions", Value = "IPM", Selected = ("IPM" == value) });
            return paymentOptions;
        }
    }
}
