using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using asi.asicentral.interfaces;
using asi.asicentral.services;

namespace asi.asicentral.util
{
    /// <summary>
    /// IP look up service from https://ipstack.com
    /// </summary>
    public class LookupIp_ipstack : ILookupIp
    {
        private static readonly ILogService Log = LogService.GetLog(typeof(LookupIp_ipstack));
        private static readonly string _lookupServiceUrl = "http://api.ipstack.com/{0}?access_key=4b917cafd695fbda4c614f6105d9a89e";

        public string LookupVendor
        {
            get { return "api.ipstack.com"; }
        }
        public string GetCountry(string ipAddress)
        {
            var country = string.Empty;
            try
            {
                var client = new WebClient();
                byte[] data = client.DownloadData(string.Format(_lookupServiceUrl, ipAddress));
                var result = Encoding.Default.GetString(data);

                if (!string.IsNullOrEmpty(result))
                {
                    var pattern = @"""country_name"":""(.*?)"",\s*""region_code";
                    var match = Regex.Match(result, pattern);
                    if (match.Success)
                    {
                        country = match.Groups[1].Value;
                    }
                }

                if (string.IsNullOrEmpty(country))
                {
                    Log.Error(string.Format("LookupIp_ipstack returns no country for IP: {0}", ipAddress));
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("LookupIp_ipstack.GetCountry Exception: {0}", ex.Message));
            }

            return country;
        }
    }
}
