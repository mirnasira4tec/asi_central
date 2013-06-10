using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace asi.asicentral.util
{
    public class HtmlHelper
    {
        /// <summary>
        /// Get a list of attributes for the EditorTemplates
        /// </summary>
        /// <param name="ViewData"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static IDictionary<string, object> GetAttributes(ViewDataDictionary<dynamic> ViewData, string className)
        {
            IDictionary<string, object> attributes = new Dictionary<string, object>();
            string classValue = (string.IsNullOrWhiteSpace(className) ? string.Empty : className);
            if (!string.IsNullOrWhiteSpace(ViewData["class"] as string)) classValue += " " + ViewData["class"];
            attributes.Add("class", classValue);
            if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Watermark) && !attributes.ContainsKey("placeholder"))
            {
                attributes.Add("placeholder", ViewData.ModelMetadata.Watermark);
            }
            //TODO iterate through html- attributes to add new ones
            //Add html5 attributes
            if (ViewData.ModelMetadata.ModelType == typeof(int) || ViewData.ModelMetadata.ModelType == typeof(Nullable<int>))
                attributes.Add("type", "number");
            return attributes;
        }

        /// <summary>
        /// Create a list of countries and their ISO country code for displaying in the UI
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> GetCountries()
        {
            IList<SelectListItem> countries = new List<SelectListItem>();

            Dictionary<string, string> countriesDic = new Dictionary<string, string>();

            foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures & ~CultureTypes.NeutralCultures))
            {
                RegionInfo objRegionInfo = new RegionInfo(cultureInfo.Name);
                if (!countriesDic.ContainsKey(objRegionInfo.EnglishName))
                {
                    countriesDic.Add(objRegionInfo.EnglishName, objRegionInfo.ThreeLetterISORegionName);
                }
            }
            List<string> countryNames = countriesDic.Keys.ToList();
            countries.Add(new SelectListItem { Selected = true, Value = "", Text = "-- Select a Country --" });
            countryNames.Sort();
            foreach (string countryName in countryNames)
            {
                countries.Add(new SelectListItem { Text = countryName, Value = countriesDic[countryName] });
            }
            return countries;
        }

        /// <summary>
        /// Get a list of US states and their codes for displaying in the UI
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> GetStates()
        {
            IList<SelectListItem> states = new List<SelectListItem>();
            states.Add(new SelectListItem() { Selected = false, Text = "- Select a State -", Value = "" });
            states.Add(new SelectListItem() { Text = "Alabama", Value = "AL" });
            states.Add(new SelectListItem() { Text = "Alaska", Value = "AK" });
            states.Add(new SelectListItem() { Text = "Arizona", Value = "AZ" });
            states.Add(new SelectListItem() { Text = "Arkansas", Value = "AK" });
            states.Add(new SelectListItem() { Text = "California", Value = "CA" });
            states.Add(new SelectListItem() { Text = "Colorado", Value = "CO" });
            states.Add(new SelectListItem() { Text = "Connecticut", Value = "CT" });
            states.Add(new SelectListItem() { Text = "Delaware", Value = "DE" });
            states.Add(new SelectListItem() { Text = "District of Columbia", Value = "DC" });
            states.Add(new SelectListItem() { Text = "Florida", Value = "FL" });
            states.Add(new SelectListItem() { Text = "Georgia", Value = "GA" });
            states.Add(new SelectListItem() { Text = "Hawaii", Value = "HI" });
            states.Add(new SelectListItem() { Text = "Idaho", Value = "ID" });
            states.Add(new SelectListItem() { Text = "Illinois", Value = "IL" });
            states.Add(new SelectListItem() { Text = "Indiana", Value = "IN" });
            states.Add(new SelectListItem() { Text = "Iowa", Value = "IA" });
            states.Add(new SelectListItem() { Text = "Kansas", Value = "KS" });
            states.Add(new SelectListItem() { Text = "Kentucky", Value = "KY" });
            states.Add(new SelectListItem() { Text = "Louisiana", Value = "LA" });
            states.Add(new SelectListItem() { Text = "Maine", Value = "ME" });
            states.Add(new SelectListItem() { Text = "Maryland", Value = "MD" });
            states.Add(new SelectListItem() { Text = "Massachusetts", Value = "MA" });
            states.Add(new SelectListItem() { Text = "Michigan", Value = "MI" });
            states.Add(new SelectListItem() { Text = "Minnesota", Value = "MN" });
            states.Add(new SelectListItem() { Text = "Mississippi", Value = "MS" });
            states.Add(new SelectListItem() { Text = "Missouri", Value = "MO" });
            states.Add(new SelectListItem() { Text = "Montana", Value = "MT" });
            states.Add(new SelectListItem() { Text = "Nebraska", Value = "NE" });
            states.Add(new SelectListItem() { Text = "Nevada", Value = "NV" });
            states.Add(new SelectListItem() { Text = "New Hampshire", Value = "NH" });
            states.Add(new SelectListItem() { Text = "New Jersey", Value = "NJ" });
            states.Add(new SelectListItem() { Text = "New Mexico", Value = "NM" });
            states.Add(new SelectListItem() { Text = "New York", Value = "NY" });
            states.Add(new SelectListItem() { Text = "North Carolina", Value = "NC" });
            states.Add(new SelectListItem() { Text = "North Dakota", Value = "ND" });
            states.Add(new SelectListItem() { Text = "Ohio", Value = "OH" });
            states.Add(new SelectListItem() { Text = "Oklahoma", Value = "OK" });
            states.Add(new SelectListItem() { Text = "Oregon", Value = "OR" });
            states.Add(new SelectListItem() { Text = "Pennsylvania", Value = "PA" });
            states.Add(new SelectListItem() { Text = "Rhode Island", Value = "RI" });
            states.Add(new SelectListItem() { Text = "South Carolina", Value = "SC" });
            states.Add(new SelectListItem() { Text = "South Dakota", Value = "SD" });
            states.Add(new SelectListItem() { Text = "Tennessee", Value = "TN" });
            states.Add(new SelectListItem() { Text = "Texas", Value = "TX" });
            states.Add(new SelectListItem() { Text = "Utah", Value = "UT" });
            states.Add(new SelectListItem() { Text = "Vermont", Value = "VT" });
            states.Add(new SelectListItem() { Text = "Virginia", Value = "VA" });
            states.Add(new SelectListItem() { Text = "Washington", Value = "WA" });
            states.Add(new SelectListItem() { Text = "West Virginia", Value = "WV" });
            states.Add(new SelectListItem() { Text = "Wisconsin", Value = "WI" });
            states.Add(new SelectListItem() { Text = "Wyoming", Value = "WY" });
            return states;
        }

        /// <summary>
        /// Used to allow for multiple displays based on the device.
        /// This method needs to be called in Application_Start method
        /// </summary>
        public static void EvaluateDisplayMode()
        {
            //set up condition for mobile devices
            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("Mobile")
            {
                //look at user agent to figure out what the client is
                ContextCondition = (ctx => IsMobileDevice(ctx)),
            });

            //set up condition for tablet devices
            DisplayModeProvider.Instance.Modes.Insert(1, new DefaultDisplayMode("Tablet")
            {
                //look at user agent to figure out what the client is
                ContextCondition = (ctx => IsTabletDevice(ctx)),
            });
        }

        /// <summary>
        /// Checks if the agent is a mobile one
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsMobileDevice(HttpContextBase context)
        {
            bool isMobile = context.GetOverriddenUserAgent() != null &&
            (
                context.GetOverriddenUserAgent().IndexOf("iPhone", StringComparison.OrdinalIgnoreCase) >= 0 ||
                context.GetOverriddenUserAgent().IndexOf("iPod", StringComparison.OrdinalIgnoreCase) >= 0 ||
                (context.GetOverriddenUserAgent().IndexOf("android", StringComparison.OrdinalIgnoreCase) >= 0 && context.GetOverriddenUserAgent().IndexOf("mobile", StringComparison.OrdinalIgnoreCase) >= 0) ||
                context.GetOverriddenBrowser().IsMobileDevice
            );
            return isMobile && !IsTabletDevice(context);
        }

        /// <summary>
        /// Checks if the agent is a tablet one
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsTabletDevice(HttpContextBase context)
        {
            bool isTablet = context.GetOverriddenUserAgent() != null &&
            (
                context.GetOverriddenUserAgent().IndexOf("iPad", StringComparison.OrdinalIgnoreCase) >= 0 ||
                context.GetOverriddenUserAgent().IndexOf("Playbook", StringComparison.OrdinalIgnoreCase) >= 0 ||
                (context.GetOverriddenUserAgent().IndexOf("android", StringComparison.OrdinalIgnoreCase) >= 0 && context.GetOverriddenUserAgent().IndexOf("mobile", StringComparison.OrdinalIgnoreCase) == -1)
            );
            return isTablet;
        }
    }
}
