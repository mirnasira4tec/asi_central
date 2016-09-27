﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Security;
using System.Web.SessionState;
using Newtonsoft.Json.Linq;
using asi.asicentral.services;
using asi.asicentral.interfaces;
using ASI.Services.Http.Security;
using ASI.Services.Http.SmartLink;

namespace asi.asicentral.oauth
{
    public class CookiesHelper
    {
        public static void SetFormsAuthenticationCookie(HttpRequestBase request, HttpResponseBase response, asi.asicentral.model.User user, bool isCreate, string userCookieName = "Name", string domainName = null)
        {
            var redirectParams = new CrossApplication.RedirectParams();
            redirectParams.AccessToken = user.AccessToken;
            redirectParams.RefreshToken = user.RefreshToken;
            redirectParams.TokenExpirationTime = DateTime.Now.AddHours(1).AddMinutes(55); //defaulting token to expire in 1 .55 hours
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
        public static void SetCookieValue(HttpRequestBase request, HttpResponseBase response, string key, string value, bool addCookie = false, string domainName = null, bool persist = true, int year = 1)
        {
            HttpCookie cookie = request.Cookies.Get(key);
            if (request.Url.Authority.Contains("localhost")) domainName = null;
            if (cookie != null)
            {
                if (!string.IsNullOrEmpty(domainName)) cookie.Domain = domainName;
                cookie.Value = value;
                if (persist) cookie.Expires = DateTime.Now.AddYears(year);
                response.Cookies.Set(cookie);
            }
            else if (addCookie && response != null)
            {
                cookie = !string.IsNullOrEmpty(domainName)
                    ? new HttpCookie(key, value) { Domain = domainName }
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

        public static string GetApplicationUrl(HttpRequestBase request, HttpResponseBase response, ApplicationCodes appCode, string domainName, string userCookieName = "Name")
        {
            var redirectUrl = string.Empty;
            if (SSO.IsLoggedIn())
            {
                try
                {
                    var cookie = GetCookieValue(request, response, FormsAuthentication.FormsCookieName);
                    if (!string.IsNullOrEmpty(cookie))
                    {
                        var redirectParams = GetLatestTokens(request, response, cookie, domainName, userCookieName, toAppCode: appCode);
                        if (redirectParams != null)
                        {
                            if (ApplicationCodes.WESP == appCode)
                            {
                                redirectParams.FromApplicationVer = "1.0.0";
                            }
                            else if (ApplicationCodes.ASED == appCode)
                            {
                                string encryptedToken = EncriptToken(redirectParams.AccessToken);
                                var Lmsurl = ConfigurationManager.AppSettings["LMSRedirectUrl"];
                                redirectUrl = string.Format("{0}learnerssologin.jsp?tokenid={1}", Lmsurl, HttpUtility.UrlEncode(encryptedToken));
                            }
                            else
                            {
                                redirectParams.FromApplicationVer = "1";
                            }
                            if (ApplicationCodes.ASED != appCode)
                            {
                                redirectParams.ToApplicationCode = appCode.ToString();
                                redirectParams.FromApplicationCode = ApplicationCodes.ASCT.ToString();
                                var url = ConfigurationManager.AppSettings["RedirectUrl"];
                                redirectUrl = CrossApplication.GetDashboardRedirectorUrl(url, redirectParams);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var log = LogService.GetLog(typeof(CookiesHelper));
                    log.Debug(string.Format("GetApplicationUrl - exception: {0}; StackTrace : {1}", ex.Message, ex.StackTrace));
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
                    case ApplicationCodes.ASED:
                        redirectUrl = string.Format("{0}lr_login.jsp", ConfigurationManager.AppSettings["LMSRedirectUrl"]);
                        break;
                }
            }

            if (string.IsNullOrEmpty(redirectUrl))
            {
                if (ApplicationCodes.WESP == appCode && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["RedirectUrl"]))
                    redirectUrl = ConfigurationManager.AppSettings["RedirectUrl"];
                else if (ApplicationCodes.ASED == appCode && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["LMSRedirectUrl"]))
                    redirectUrl = string.Format("{0}lr_login.jsp", ConfigurationManager.AppSettings["LMSRedirectUrl"]);
            }
            return redirectUrl;
        }

        private static CrossApplication.RedirectParams GetCrossAppTokens(CrossApplication.RedirectParams redirectParams, ApplicationCodes toAppCode)
        {
            if (redirectParams != null && !string.IsNullOrEmpty(redirectParams.AccessToken))
            {
                var accessToken = redirectParams.AccessToken;
                var host = ConfigurationManager.AppSettings["SecurityHost"];
                var relativePath = ConfigurationManager.AppSettings["RelativePath"];
                if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(relativePath))
                {
                    try
                    {
                        var oAuth2Client = new OAuth2Client(host, relativePath: relativePath);
                        var authenticatedUser = ASIOAuthClient.GetAuthenticatedUser(accessToken);
                        if(authenticatedUser != null && authenticatedUser.Token != null)
                        {
                            var sessionId = authenticatedUser.Token.Value;
                            if (toAppCode == ApplicationCodes.ASED)
                            {
                                var asiOAuthClientId = ConfigurationManager.AppSettings["AsiOAuthClientId"];
                                var asiOAuthClientSecret = ConfigurationManager.AppSettings["AsiOAuthClientSecret"];
                                if (!string.IsNullOrEmpty(asiOAuthClientId) && !string.IsNullOrEmpty(asiOAuthClientSecret))
                                {
                                    var responseMessage = oAuth2Client.CrossApplication(asiOAuthClientId, asiOAuthClientSecret, sessionId, toAppCode.ToString()).Result;
                                    if (responseMessage != null)
                                    {
                                        redirectParams.AccessToken = responseMessage.AccessToken;
                                        redirectParams.RefreshToken = responseMessage.RefreshToken;
                                    }
                                }
                            }
                            else
                            {
                                redirectParams.ExtGuid = sessionId;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var log = LogService.GetLog(typeof(CookiesHelper));
                        log.Debug(string.Format("GetLatestTokens - exception: {0}", ex.Message));
                    }
                }                
            }

            return redirectParams;
        }

        private static CrossApplication.RedirectParams GetLatestTokens(HttpRequestBase request, HttpResponseBase response, string cookie, string domainName, string userCookieName = "Name", ApplicationCodes toAppCode = ApplicationCodes.ASCT)
        {
            ILogService log = LogService.GetLog(typeof(CookiesHelper));
            log.Debug("GetLatestTokens - Start");
            var hashedTicket = FormsAuthentication.Decrypt(cookie);
            var extraData = JsonConvert.DeserializeObject<CrossApplication.RedirectParams>(hashedTicket.UserData,
                new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            log.Debug("GetLatestTokens - Refresh token - " + (!string.IsNullOrEmpty(extraData.RefreshToken) ? extraData.RefreshToken : "No Refresh token"));
            log.Debug("GetLatestTokens - TokenExpirationTime - " + extraData.TokenExpirationTime);
            try
            {
                if (extraData != null && !string.IsNullOrEmpty(extraData.RefreshToken)
                    && (extraData.TokenExpirationTime != null && extraData.TokenExpirationTime.HasValue && extraData.TokenExpirationTime < DateTime.Now)
                    && !string.IsNullOrEmpty(extraData.AccessToken)
                    && !ASIOAuthClient.IsValidAccessToken(extraData.AccessToken))
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
                            user.FirstName = HttpContext.Current.User.Identity.Name;
                            SetFormsAuthenticationCookie(request, response, user, false, userCookieName, domainName);

                            extraData.AccessToken = user.AccessToken;
                            extraData.RefreshToken = user.RefreshToken;
                        }
                        else
                        {
                            log.Error("GetLatestTokens - RefreshToken - did not get a new token");
                        }
                }

                extraData = GetCrossAppTokens(extraData, toAppCode);
            }
            catch (Exception ex)
            {
                log.Debug(string.Format("GetLatestTokens - exception: {0}", ex.Message));
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

        public static string GetLMSToken(HttpRequestBase request, HttpResponseBase response, ApplicationCodes appCode, string domainName, string userCookieName = "Name")
        {
            var lmsToken = string.Empty;
            try
            {
                var cookie = GetCookieValue(request, response, FormsAuthentication.FormsCookieName);
                if (!string.IsNullOrEmpty(cookie))
                {
                    var redirectParms = GetLatestTokens(request, response, cookie, domainName, userCookieName: userCookieName, toAppCode: appCode);
                    if (redirectParms != null && !string.IsNullOrEmpty(redirectParms.AccessToken))
                        lmsToken = EncriptToken(redirectParms.AccessToken);
                }
            }
            catch (Exception ex)
            {
                var log = LogService.GetLog(typeof(CookiesHelper));
                log.Debug(string.Format("GetLMSToken - exception: {0}; StackTrace : {1}", ex.Message, ex.StackTrace));
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
