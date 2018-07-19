using asi.asicentral.services;
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

        public static IList<SelectListItem> GetCountriesList()
        {
            var jsResult = GetValueFromUrl(ConfigurationManager.AppSettings["CountryApiUrl"]);
            var countryList = new List<SelectListItem>();
            var jArr = JArray.Parse(jsResult);
            if (jArr != null && jArr.Count > 0)
            {
                countryList.Add(new SelectListItem { Text = "--Select a Country--", Value = "" });
                for (int i = 0; i < jArr.Count; i++)
                {
                    countryList.Add(new SelectListItem { Selected = false, Text = jArr[i]["Descr"].ToString(), Value = jArr[i]["Code"].ToString() });
                }
            }
            return countryList;
        }

        public static IList<SelectListItem> GetStateValues(string countrycode)
        {
            // Create web client.
            var statesList = new List<SelectListItem>();
            var url = ConfigurationManager.AppSettings["StateApiUrl"];
            var parmeters = new List<KeyValuePair<string, string>>();
            parmeters.Add(new KeyValuePair<string, string>("countrycode", countrycode));
            var jsResult = GetValueFromUrl(url, parmeters);
            var jArr = JArray.Parse(jsResult);
            if (jArr != null && jArr.Count > 0)
            {
                statesList.Add(new SelectListItem { Text = "--Select a State--", Value = "" });
                for (int i = 0; i < jArr.Count; i++)
                {
                    statesList.Add(new SelectListItem { Selected = false, Text = jArr[i]["Descr"].ToString(), Value = jArr[i]["Code"].ToString() });
                }
            }
            return statesList;
        }
        public static string GetValueFromUrl(string url, List<KeyValuePair<string, string>> parameters = null)
        {
            var queryString = string.Empty;
            var responseString = string.Empty;
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var parameter in parameters)
                {
                    queryString += $"?{parameter.Key}={parameter.Value}";
                }
            }
            url += queryString;
            try
            {
                using (var client = new WebClient())
                {
                    var result = client.DownloadData(url);
                    responseString = Encoding.Default.GetString(result);
                }
            }
            catch (Exception ex)
            {
                LogService log = LogService.GetLog("Api Call");
                log.Error(ex.Message);
            }
            return responseString;
        }
    }
}
