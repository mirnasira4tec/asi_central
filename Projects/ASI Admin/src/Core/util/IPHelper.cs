using asi.asicentral.interfaces;
using asi.asicentral.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace asi.asicentral.util
{
    public class IPHelper
    {
        private static readonly ILogService Log = LogService.GetLog(MethodBase.GetCurrentMethod().DeclaringType);
        private static string[] ASIAN_COUNTRIES = new string[] { "CHINA", "HONG KONG","INDIA","KOREA, DEMOCRATIC PEOPLE'S REPUBLIC OF","KOREA, REPUBLIC OF","TAIWAN, PROVINCE OF CHINA" };
        private static readonly string DEFAULTCOUNTRY = "UNITED STATES";
        /// <summary>
        /// Gives country based on give IP addres, caches the value in session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static string GetCountry(HttpSessionStateBase session, string ipAddress)
        {
            var vendor = string.Empty;
            return GetCountryAndVendor(session, ipAddress, ref vendor);
        }

        public static string GetCountryAndVendor(HttpSessionStateBase session, string ipAddress, ref string vendor)
        {
            if (!string.IsNullOrEmpty(session["IpCountry"] as string))
                return session["IpCountry"] as string;

            ILookupIp ipLookup = new LookUpIp_GeoIpNekudo();
            var country = ipLookup.GetCountry(ipAddress);
            // try ipstack.com for the same ip
            if (string.IsNullOrEmpty(country))
            {
                ipLookup = new LookupIp_ipstack();
                country = ipLookup.GetCountry(ipAddress);
            }

            vendor = ipLookup.LookupVendor;

            session["IpCountry"] = !string.IsNullOrEmpty(country) ? country : DEFAULTCOUNTRY;
            Log.Debug(string.Format("Country basesd on GetCountry: {0}", country));
            return country;
        }

        /// <summary>
        /// Checks whether the IP comes from an asian country
        /// </summary>
        /// <param name="session"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsFromAsia(HttpSessionStateBase session, string ipAddress)
        {
            string result = GetCountry(session, ipAddress);
            return ASIAN_COUNTRIES.Contains(result, StringComparer.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// Class to lookup countru from ip address based on ipinfodb.com database
    /// </summary>
    public class IpLookup
    {
        private static readonly ILogService Log = LogService.GetLog(MethodBase.GetCurrentMethod().DeclaringType);
        /*API key and service obtained from api.ipinfodb.com*/
        public const string ApiKey = "abee718c443d7ec82acf30d0996ecea2507deacab2f646516e7bed165070059e";
        public const string DEFAULTCOUNTRY = "UNITED STATES";
        public const string CANADA = "CANADA";
        public const string CHINA = "CHINA";
        public const string HONGKONG = "HONG KONG";
        public const string LookupServiceUrl = "http://api.ipinfodb.com/v3/ip-country/?key=" + ApiKey + "&ip=";

        private IpLookup() { }

        public static string LookupByIp(string ipAddress)
        {
            try
            {
                string result = string.Empty;
                var client = new WebClient();
                byte[] data = client.DownloadData(IpLookup.LookupServiceUrl + ipAddress);
                var responseData = Encoding.Default.GetString(data);
                string[] dataResults = responseData.Split(";".ToArray());

                if (dataResults.Length == 0)
                {
                    return result;
                }

                Log.Debug(string.Format("Response data for IP {0}: {1}", ipAddress, responseData));
                result = dataResults[4] == "-" ? DEFAULTCOUNTRY.ToLower() : dataResults[4];
                Log.Debug(string.Format("Country based on LookupByIp: {0}", result.ToLower()));
                return result.ToLower();
            }
            catch (Exception ex)
            {
                Log.Debug(string.Format("Exception on LookupByIp: {0}", ex.Message));
                return DEFAULTCOUNTRY.ToLower();
            }
        }
    }
}
