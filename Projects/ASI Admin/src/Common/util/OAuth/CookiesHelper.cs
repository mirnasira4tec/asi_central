using ASI.Jade.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Security;

namespace asi.asicentral.oauth
{
    public class CookiesHelper 
    {
        public static void SetFormsAuthenticationCookie(HttpRequestBase request, HttpResponseBase response, asi.asicentral.model.User user, bool isCreate, string userCookieName = "Name", string domainName = null)
        {
            CrossApplication.RedirectParams redirectParams = new CrossApplication.RedirectParams();
            redirectParams.AccessToken = user.AccessToken;
            redirectParams.RefreshToken = user.RefreshToken;
            string userName = GetCookieValue(request, response, userCookieName);
            var extraData = JsonConvert.SerializeObject(redirectParams, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddHours(1), true, extraData, FormsAuthentication.FormsCookiePath);
            string hashedTicket = FormsAuthentication.Encrypt(ticket);
            SetCookieValue(request, response, FormsAuthentication.FormsCookieName, hashedTicket, isCreate, domainName);
        }

        /// <summary>
        /// SetCookieValue - Set value of the cookie based on the key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetCookieValue(HttpRequestBase request, HttpResponseBase response, string key, string value, bool addCookie = false, string domain = null, bool persist = true, int year = 1)
        {
            HttpCookie cookie = request.Cookies.Get(key);
            if (cookie != null)
            {
                if (!string.IsNullOrEmpty(domain)) cookie.Domain = domain;
                cookie.Value = value;
                if (persist) cookie.Expires = DateTime.Now.AddYears(year);
                response.Cookies.Set(cookie);
            }
            else if (addCookie)
            {
                cookie = !string.IsNullOrEmpty(domain)
                    ? new HttpCookie(key, value) { Domain = domain }
                    : new HttpCookie(key, value);
                if (persist) cookie.Expires = DateTime.Now.AddYears(year);
                response.Cookies.Add(cookie);
            }
        }

        public static string GetCookieValue(HttpRequestBase request, HttpResponseBase response, string key)
        {
            string cookieValue = string.Empty;
            HttpCookie cookie = request.Cookies.Get(key);
            if (cookie == null) cookie = response.Cookies.Get(key);
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                cookieValue = cookie.Value;
            return cookieValue;
        }

        public static string GetApplicationUrl(HttpRequestBase request, HttpResponseBase response, ApplicationCodes appCode, string userCookieName = "Name")
        {
            string redirectUrl = string.Empty;
            string cookie = GetCookieValue(request, response, FormsAuthentication.FormsCookieName);
            if (!string.IsNullOrEmpty(cookie))
            {
                var hashedTicket = FormsAuthentication.Decrypt(cookie);
                var extraData = JsonConvert.DeserializeObject<CrossApplication.RedirectParams>(hashedTicket.UserData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                if (extraData != null && !string.IsNullOrEmpty(extraData.RefreshToken) && extraData.TokenExpirationTime < DateTime.Now)
                {
                    var tokens = ASIOAuthClient.RefreshToken(extraData.RefreshToken);
                    if (tokens != null && tokens.Count > 0)
                    {
                        asi.asicentral.model.User user = new asicentral.model.User();
                        if (tokens.ContainsKey("AccessToken")) user.AccessToken = tokens["AccessToken"];
                        if (tokens.ContainsKey("RefreshToken")) user.RefreshToken = tokens["RefreshToken"];
                        SetFormsAuthenticationCookie(request, response, user, false, userCookieName);
                        hashedTicket = FormsAuthentication.Decrypt(cookie);
                        extraData = JsonConvert.DeserializeObject<CrossApplication.RedirectParams>(hashedTicket.UserData);
                    }
                }
                if (extraData != null)
                {
                    var redirectParams = new ASI.Jade.Utilities.CrossApplication.RedirectParams();
                    redirectParams.AccessToken = extraData.AccessToken;
                    redirectParams.RefreshToken = extraData.RefreshToken;
                    redirectParams.TokenExpirationTime = (extraData.TokenExpirationTime.HasValue && extraData.TokenExpirationTime.Value > DateTime.Now) ? 
                        extraData.TokenExpirationTime.Value : DateTime.Now.Add(new TimeSpan(2, 0, 0));
                    redirectParams.ExtGuid = string.Empty;
                    redirectParams.ToApplicationCode = appCode.ToString();
                    redirectParams.FromApplicationCode = asi.asicentral.oauth.ApplicationCodes.ASIC.ToString();
                    redirectParams.FromApplicationVer = "1";
                    var url = ConfigurationManager.AppSettings["RedirectUrl"];
                    redirectUrl = CrossApplication.GetDashboardRedirectorUrl(url, redirectParams);
                }
            }

            if (string.IsNullOrEmpty(redirectUrl) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["RedirectUrl"]))
                redirectUrl = ConfigurationManager.AppSettings["RedirectUrl"];

            return redirectUrl;
        }

        public static int GetId(bool isCompanyId, HttpRequestBase request, HttpResponseBase response, string cookieName)
        {
            int id = 0;
            HttpCookie cookie = null;
            if (request != null) cookie = request.Cookies.Get(cookieName);
            if (cookie == null && response != null) cookie = response.Cookies.Get(cookieName);
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                if (isCompanyId) id = Convert.ToInt32(cookie.Value.Substring(0, cookie.Value.IndexOf('-')));
                else id = Convert.ToInt32(cookie.Value.Substring(cookie.Value.IndexOf('-') + 1));
            }
            return id;
        }

    }
}
