using asi.asicentral.interfaces;
using asi.asicentral.services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
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
            //@todo iterate through html- attributes to add new ones, has not yet been required to add new ones, expected.
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
            DisplayModeProvider.Instance.Modes.Clear();
            //set up condition for mobile devices
            DisplayModeProvider.Instance.Modes.Add(new DefaultDisplayMode("Mobile")
            {
                //look at user agent to figure out what the client is
                ContextCondition = (ctx => IsMobileDevice(ctx)),
            });

            //set up condition for tablet devices
            DisplayModeProvider.Instance.Modes.Add(new DefaultDisplayMode("Tablet")
            {
                //look at user agent to figure out what the client is
                ContextCondition = (ctx => IsTabletDevice(ctx)),
            });
            //default condition
            DisplayModeProvider.Instance.Modes.Add(new DefaultDisplayMode("")
            {
                //default, always true
                ContextCondition = (ctx => true),
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
            isMobile = isMobile && !IsTabletDevice(context);
            return isMobile;
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

        /// <summary>
        /// Submits a web request and reads the result into a string
        /// </summary>
        /// <param name="url">where to send the request</param>
        /// <param name="headerParam">way to overide some of the request headers</param>
        /// <param name="content">The content to post for the web request</param>
        /// <param name="post"></param>
        /// <param name="returnContent">wether to process the result</param>
        /// <returns></returns>
        public static string SubmitWebRequest(string url, IDictionary<string, string> headerParam, string content, bool post = true, bool returnContent = true)
        {
            StringBuilder resultContent = new StringBuilder();
            ILogService logService = LogService.GetLog(typeof(HtmlHelper));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = (post ? "POST" : "GET");
            request.Timeout = 120000; //2 minutes timeout
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.168 Safari/535.19";
            request.Accept = "text/html,text/xml,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //process the header values passed in
            if (headerParam != null)
            {
                foreach (string key in headerParam.Keys)
                {
                    switch (key.ToLower())
                    {
                        case "useragent":
                            request.UserAgent = headerParam[key];
                            break;
                        case "accept":
                            request.Accept = headerParam[key];
                            break;
                        case "contenttype":
                            request.ContentType = headerParam[key];
                            break;
                        case "expect":
                            request.Expect = headerParam[key];
                            break;
                    }
                }
            }
            //set the content into the request if available
            if (!string.IsNullOrEmpty(content))
            {
                //writting the content into the request stream
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] postBytes = encoding.GetBytes(content);
                request.ContentLength = postBytes.Length;

                //set query into the request
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(postBytes, 0, postBytes.Length);
                }
            }
            // Execute the request
            if (System.Net.ServicePointManager.Expect100Continue) System.Net.ServicePointManager.Expect100Continue = false;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                logService.Debug("Submit Form - Checking return: " + response.StatusCode);
                if (response.StatusCode != HttpStatusCode.OK) throw new Exception("The web request was not successfully completed: (code " + response.StatusCode + ")");
                if (returnContent)
                {
                    //read the return value
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        // used on each read operation
                        char[] buf = new char[256];
                        int count = 0;
                        do
                        {
                            // fill the buffer with data
                            count = sr.Read(buf, 0, buf.Length);
                            // make sure we read some data
                            if (count != 0)
                            {
                                // translate from bytes to ASCII text
                                string tempString = new String(buf, 0, count);
                                // continue building the string
                                resultContent.Append(tempString);
                            }
                        }
                        while (count > 0); // any more data to read?
                        logService.Debug("Submit Form - Read return content: " + resultContent);
                    }
                }
                return resultContent.ToString();
            }
        }

        /// <summary>
        /// Submit a form request to a web page
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string SubmitForm(string url, IDictionary<string, string> parameters, bool post = false, bool returnContent = false)
        {
            ILogService logService = LogService.GetLog(typeof(HtmlHelper));
            //construct the url
            StringBuilder webParams = new StringBuilder();
            if (!post && parameters.Count > 0)
                webParams.Append("?");
            //build query string
            foreach (string key in parameters.Keys)
            {
                if (webParams.Length > 1) webParams.Append("&");
                webParams.Append(key)
                    .Append("=")
                    .Append(HttpUtility.UrlEncode(parameters[key]));
                logService.Debug("\t" + key + "=" + parameters[key]);
            }
            //create the web request
            string finalUrl = (post ? url : url + webParams.ToString());
            string content = (post ? webParams.ToString() : null);
            logService.Debug("Submit Form - Calling: " + finalUrl);
            return SubmitWebRequest(finalUrl, null, content, post, returnContent);
        }
    }
}
