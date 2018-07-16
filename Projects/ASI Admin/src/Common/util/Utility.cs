using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace asi.asicentral.util
{
  public static  class Utility
    {
        public static bool IsServiceAvailable(string url)
        {
            bool isServiceRunning = false;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    isServiceRunning = true;
                }
            }
            catch (Exception ex)
            {

            }
            return isServiceRunning;
        }

        public static bool IsPersonifyAvailable()
        {
            var usePersonify = Convert.ToBoolean(ConfigurationManager.AppSettings["UsePersonifyServices"]);
            if (usePersonify)
            {
                var svcUrl = ConfigurationManager.AppSettings["svcUri"];
                usePersonify = IsServiceAvailable(svcUrl);
            }
            return usePersonify;
        }

        public static IList<SelectListItem> GetStates(string countrycode)
        {
            if (countrycode == "USA")
                return asi.asicentral.util.HtmlHelper.GetStates();
            else
                return GetStateValues(countrycode);
        }

        public static IList<SelectListItem> GetCountriesList()
        {
            var countryArr = GetCountries();
            IList<SelectListItem> countryList = new List<SelectListItem>();
            if (countryArr != null && countryArr.Count > 0)
            {
                for (int i = 0; i < countryArr.Count; i++)
                {
                    countryList.Add(new SelectListItem { Selected = false, Text = countryArr[i]["Descr"].ToString(), Value = countryArr[i]["Code"].ToString() });
                }
            }
            return countryList;
        }

        public static JArray GetCountries()
        {
            // Create web client.
            JArray jsResult = null;
            try
            {
                using (var client = new WebClient())
                {
                    var url = ConfigurationManager.AppSettings["CountryApiUrl"];
                    var result = client.DownloadData(url);
                    var responseString = Encoding.Default.GetString(result);
                    var a = responseString.Length;
                    // deserializing nested JSON string to object
                    jsResult = JArray.Parse(responseString);
                }
            }
            catch
            {

            }
            return jsResult;
        }
        public static IList<SelectListItem> GetStateValues(string countrycode)
        {
            // Create web client.
            JArray jsResult = null;
            IList<SelectListItem> statesList = new List<SelectListItem>();
            try
            {
                using (var client = new WebClient())
                {
                    var url = ConfigurationManager.AppSettings["StateApiUrl"] + $"?countrycode={countrycode}";
                    var result = client.DownloadData(url);
                    var responseString = Encoding.Default.GetString(result);
                    var a = responseString.Length;
                    // deserializing nested JSON string to object
                    jsResult = JArray.Parse(responseString);
                    if (jsResult != null && jsResult.Count > 0)
                    {
                        for (int i = 0; i < jsResult.Count; i++)
                        {
                            statesList.Add(new SelectListItem { Selected = false, Text = jsResult[i]["Descr"].ToString(), Value = jsResult[i]["Code"].ToString() });
                        }
                    }
                }
            }
            catch
            {

            }
            return statesList;
        }
    }
}
