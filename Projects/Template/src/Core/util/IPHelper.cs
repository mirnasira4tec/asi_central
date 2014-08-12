﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace asi.asicentral.util
{
    public class IPHelper
    {
        private static string[] ASIAN_COUNTRIES = new string[] { "CHINA", "HONG KONG","INDIA","KOREA, DEMOCRATIC PEOPLE'S REPUBLIC OF","KOREA, REPUBLIC OF","TAIWAN, PROVINCE OF CHINA" };
        /// <summary>
        /// Gives country based on give IP addres, caches the value in session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static string GetCountry(HttpSessionStateBase session, string ipAddress)
        {
            if (!string.IsNullOrEmpty(session["IpCountry"] as string))
                return session["IpCountry"] as string;

            string result = IpLookup.LookupByIp(ipAddress);
            session["IpCountry"] = result;
            return result;
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
            return ASIAN_COUNTRIES.Contains(result);
        }
    }

    /// <summary>
    /// Class to lookup countru from ip address based on ipinfodb.com database
    /// </summary>
    public class IpLookup
    {
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
                WebClient client = new WebClient();
                byte[] data = client.DownloadData(IpLookup.LookupServiceUrl + ipAddress);
                string[] dataResults = Encoding.Default.GetString(data).Split(";".ToArray());

                if (dataResults.Length == 0)
                {
                    return result;
                }

                result = dataResults[4].ToString() == "-" ? DEFAULTCOUNTRY : dataResults[4].ToString();
                return result;
            }
            catch (Exception)
            {
                return DEFAULTCOUNTRY;
            }
        }
    }
}
