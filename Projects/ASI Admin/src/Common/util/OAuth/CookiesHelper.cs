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
using ASI.Jade.UserManagement.DataObjects;
using Newtonsoft.Json.Linq;
using asi.asicentral.services;
using asi.asicentral.interfaces;

namespace asi.asicentral.oauth
{
    public class CookiesHelper
    {
        public static void SetFormsAuthenticationCookie(HttpRequestBase request, HttpResponseBase response, asi.asicentral.model.User user, bool isCreate, string userCookieName = "Name", string domainName = null)
        {
            var redirectParams = new CrossApplication.RedirectParams();
            redirectParams.AccessToken = user.AccessToken;
            redirectParams.RefreshToken = user.RefreshToken;
            redirectParams.TokenExpirationTime = DateTime.Now.AddHours(2); //defaulting token to expire in 2 hours
            string userName = ASIOAuthClient.GetUserName(user.FirstName, user.LastName);
            var extraData = JsonConvert.SerializeObject(redirectParams, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddYears(1), true, extraData, FormsAuthentication.FormsCookiePath);
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
            else if (addCookie && response != null)
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
            if (request == null && response == null) return cookieValue;
            HttpCookie cookie = null;
            //we look in the request
            if (request != null && request.Cookies != null && request.Cookies.AllKeys.Contains(key))
                cookie = request.Cookies.Get(key);
            //response takes precedence
            if (response != null && response.Cookies != null && response.Cookies.AllKeys.Contains(key))
                cookie = response.Cookies.Get(key);
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                cookieValue = cookie.Value;
            return cookieValue;
        }

        public static string GetApplicationUrl(HttpRequestBase request, HttpResponseBase response, ApplicationCodes appCode, string userCookieName = "Name")
        {
            string redirectUrl = string.Empty;
            if (SSO.IsLoggedIn())
            {
                string cookie = GetCookieValue(request, response, FormsAuthentication.FormsCookieName);
                if (!string.IsNullOrEmpty(cookie))
                {
                    var extraData = GetLatestTokens(request, response, cookie, userCookieName);
                    if (extraData != null)
                    {
                        var redirectParams = new CrossApplication.RedirectParams();
                        redirectParams.AccessToken = extraData.AccessToken;
                        redirectParams.RefreshToken = extraData.RefreshToken;
                        redirectParams.TokenExpirationTime = (extraData.TokenExpirationTime.HasValue &&
                                                              extraData.TokenExpirationTime.Value > DateTime.Now)
                            ? extraData.TokenExpirationTime.Value
                            : DateTime.Now.Add(new TimeSpan(2, 0, 0));
                        if (ApplicationCodes.WESP == appCode)
                        {
                            var session = new ASI.Jade.UserManagement.Session();
                            var sessionData = new Session(GetId(false, request, response, "CMPSSO"), ApplicationCodes.ASIC.ToString(),
                                "1.0.0", HttpContext.Current.Request.UserHostAddress);
                            string sessionId = session.Create(sessionData);
                            if (!string.IsNullOrEmpty(sessionId)) redirectParams.ExtGuid = sessionId;
                            redirectParams.FromApplicationVer = "1.0.0";
                        }
                        else if (ApplicationCodes.UPSIDE == appCode)
                        {
                            string encryptedToken = EncriptToken(extraData.AccessToken);
                            var Lmsurl = ConfigurationManager.AppSettings["LMSRedirectUrl"];
                            redirectUrl = string.Format("{0}learnerssologin.jsp?tokenid={1}", Lmsurl, HttpUtility.UrlEncode(encryptedToken));
                        }
                        else
                        {
                            redirectParams.ExtGuid = string.Empty;
                            redirectParams.FromApplicationVer = "1";
                        }
                        if (ApplicationCodes.UPSIDE != appCode)
                        {
                            redirectParams.ToApplicationCode = appCode.ToString();
                            redirectParams.FromApplicationCode = ApplicationCodes.ASIC.ToString();
                            var url = ConfigurationManager.AppSettings["RedirectUrl"];
                            redirectUrl = CrossApplication.GetDashboardRedirectorUrl(url, redirectParams);
                        }
                    }
                }
            }
            else
            {
                //user is not logged in
                switch (appCode)
                {
                    case ApplicationCodes.WESP:
                        redirectUrl = "http://espweb.asicentral.com/";
                        break;
                    case ApplicationCodes.UPSIDE:
                        redirectUrl = string.Format("{0}lr_login.jsp", ConfigurationManager.AppSettings["LMSRedirectUrl"]);
                        break;
                }
            }

            if (string.IsNullOrEmpty(redirectUrl))
            {
                if (ApplicationCodes.WESP == appCode && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["RedirectUrl"]))
                    redirectUrl = ConfigurationManager.AppSettings["RedirectUrl"];
                else if (ApplicationCodes.UPSIDE == appCode && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["LMSRedirectUrl"]))
                    redirectUrl = string.Format("{0}lr_login.jsp", ConfigurationManager.AppSettings["LMSRedirectUrl"]);
            }
            return redirectUrl;
        }

        private static CrossApplication.RedirectParams GetLatestTokens(HttpRequestBase request, HttpResponseBase response, string cookie, string userCookieName = "Name")
        {
            ILogService log = LogService.GetLog(typeof(CookiesHelper));
            log.Debug("GetLatestTokens - Start");
            var hashedTicket = FormsAuthentication.Decrypt(cookie);
            var extraData = JsonConvert.DeserializeObject<CrossApplication.RedirectParams>(hashedTicket.UserData,
                new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            log.Debug("GetLatestTokens - Refresh token - " + (!string.IsNullOrEmpty(extraData.RefreshToken) ? extraData.RefreshToken : "No Refresh token"));
            log.Debug("GetLatestTokens - TokenExpirationTime - " + extraData.TokenExpirationTime);
            if (extraData != null && !string.IsNullOrEmpty(extraData.RefreshToken) &&
                (extraData.TokenExpirationTime == null || extraData.TokenExpirationTime < DateTime.Now))
            {
                log.Debug("GetLatestTokens - Requesting a new token");
                var tokens = ASIOAuthClient.RefreshToken(extraData.RefreshToken);
                if (tokens != null && tokens.Count > 0)
                {
                    foreach (var key in tokens.Keys)
                    {
                        log.Debug("GetLatestTokens - RefreshToken - " + key + " " + tokens[key]);
                    }
                    var user = new model.User();
                    if (tokens.ContainsKey("AuthToken")) user.AccessToken = tokens["AuthToken"];
                    if (tokens.ContainsKey("RefreshToken")) user.RefreshToken = tokens["RefreshToken"];
                    SetFormsAuthenticationCookie(request, response, user, false, userCookieName);
                    extraData.AccessToken = user.AccessToken;
                    extraData.RefreshToken = user.RefreshToken;
                }
                else
                {
                    log.Error("GetLatestTokens - RefreshToken - did not get a new token");
                }
            }
            if (extraData != null) log.Debug("GetLatestTokens - End: " + extraData.AccessToken);
            return extraData;
        }

        private static string EncriptToken(string accessToken)
        {
            ILogService log = LogService.GetLog(typeof(CookiesHelper));
            log.Debug("EncriptToken - Start");
            string encryptedToken = string.Empty;
            var encryptionService = new EncryptionService();
            encryptedToken = encryptionService.ECBEncrypt("ASIP@ssWord34567", accessToken);
            log.Debug("EncriptToken - End: " + encryptedToken);
            return encryptedToken;
        }

        public static string GetLMSToken(HttpRequestBase request, HttpResponseBase response, ApplicationCodes appCode, string userCookieName = "Name")
        {
            string lmsToken = string.Empty;
            string cookie = GetCookieValue(request, response, FormsAuthentication.FormsCookieName);
            if (!string.IsNullOrEmpty(cookie))
            {
                var extraData = GetLatestTokens(request, response, cookie, userCookieName);
                if (extraData != null)
                    lmsToken = EncriptToken(extraData.AccessToken);
            }
            return lmsToken;
        }

        public static int GetId(bool isCompanyId, HttpRequestBase request, HttpResponseBase response, string cookieName)
        {
            int id = 0;
            string cmpsso = GetCookieValue(request, response, cookieName);
            if (!string.IsNullOrEmpty(cmpsso))
            {
                if (isCompanyId) id = Convert.ToInt32(cmpsso.Substring(0, cmpsso.IndexOf('-')));
                else id = Convert.ToInt32(cmpsso.Substring(cmpsso.IndexOf('-') + 1));
            }
            return id;
        }
    }
}
