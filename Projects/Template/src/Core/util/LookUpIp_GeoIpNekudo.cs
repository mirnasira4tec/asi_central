using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using asi.asicentral.interfaces;
using asi.asicentral.services;

namespace asi.asicentral.util
{
    /// <summary>
    /// IP look up service from https://geoip.nekudo.com
    /// </summary>
    public class LookUpIp_GeoIpNekudo: ILookupIp
    {
        private static readonly ILogService Log = LogService.GetLog(typeof(LookUpIp_GeoIpNekudo));
        private static readonly string _lookupServiceUrl = "https://geoip.nekudo.com/api/{0}/en";

        public string LookupVendor
        {
            get { return "geoip.nekudo.com"; }
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
                    var pattern = @"""country"":{""name"":""(.*?)"",\s*""code"":""(.*?)""},\s*""location""";
                    var match = Regex.Match(result, pattern);
                    if( match.Success)
                    {
                        country = match.Groups[1].Value;
                    }
                }
                
                if( string.IsNullOrEmpty(country))
                {
                    Log.Error(string.Format("LookUpIp_GeoIpNekudo returns no country for IP: {0}", ipAddress));
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("LookUpIp_GeoIpNekudo.GetCountry Exception: {0}", ex.Message));
            }

            return country;
        }
    }
}
