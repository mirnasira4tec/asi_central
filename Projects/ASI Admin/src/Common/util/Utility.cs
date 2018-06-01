using System;
using System.Configuration;
using System.Net;

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
    }
}
