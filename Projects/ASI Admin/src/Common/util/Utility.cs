using asi.asicentral.services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace asi.asicentral.util
{
    public static class Utility
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
            var countryList = new List<SelectListItem>();
            countryList.Add(new SelectListItem { Text = "--Select a Country--", Value = "" });
            var jsResult = GetValueFromUrl(ConfigurationManager.AppSettings["CountryApiUrl"]);
            if (!string.IsNullOrWhiteSpace(jsResult))
            {
                var jArr = JArray.Parse(jsResult);
                if (jArr != null && jArr.Count > 0)
                {
                    for (int i = 0; i < jArr.Count; i++)
                    {
                        countryList.Add(new SelectListItem { Selected = false, Text = jArr[i]["Descr"].ToString(), Value = jArr[i]["Code"].ToString() });
                    }
                }
            }
            return countryList;
        }

        public static IList<SelectListItem> GetStateValues(string countrycode)
        {
            // Create web client.
            var statesList = new List<SelectListItem>();
            statesList.Add(new SelectListItem { Text = "--Select a State--", Value = "" });
            var url = ConfigurationManager.AppSettings["StateApiUrl"];
            var parmeters = new List<KeyValuePair<string, string>>();
            parmeters.Add(new KeyValuePair<string, string>("countrycode", countrycode));
            var jsResult = GetValueFromUrl(url, parmeters);
            if (!string.IsNullOrWhiteSpace(jsResult))
            {
                var jArr = JArray.Parse(jsResult);
                if (jArr != null && jArr.Count > 0)
                {
                    for (int i = 0; i < jArr.Count; i++)
                    {
                        statesList.Add(new SelectListItem { Selected = false, Text = jArr[i]["Descr"].ToString(), Value = jArr[i]["Code"].ToString() });
                    }
                }
            }
            return statesList;
        }
        public static string GetValueFromUrl(string url, List<KeyValuePair<string, string>> parameters = null)
        {
            var queryString = "?";
            var responseString = string.Empty;
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var parameter in parameters)
                {
                    queryString += $"{parameter.Key}={parameter.Value}&";
                }
            }
            queryString = queryString.Remove(queryString.Length - 1, 1);
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

        public static string ObjectToXML<T>(T source)
        {
            var xml = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();           
            XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, source);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                xml = xmlDoc.InnerXml;
            }
            return xml;
        }
    }
}
