using asi.asicentral.oauth;
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace asi.asicentral.util.OAuth
{
    public class TokenMonitorModule : IHttpModule
    {
        public void Init(HttpApplication httpApp)
        {
            httpApp.BeginRequest += OnBeginRequest;
        }

        public void OnBeginRequest(Object sender, EventArgs e)
        {
            var httpApp = (HttpApplication)sender;
            var request = httpApp.Request;
            var requestPath = request.AppRelativeCurrentExecutionFilePath;
            if (!Regex.IsMatch(requestPath, @"/.*?\..*?") || Regex.IsMatch(requestPath, @"\.aspx?"))
            {
                var cookieValue = CookiesHelper.GetCookieValue(new HttpRequestWrapper(request), new HttpResponseWrapper(httpApp.Response), FormsAuthentication.FormsCookieName);
                if (!string.IsNullOrEmpty(cookieValue))
                {
                    var hashedTicket = FormsAuthentication.Decrypt(cookieValue);
                    if (hashedTicket != null && !string.IsNullOrEmpty(hashedTicket.UserData))
                    {
                        var match = Regex.Match(hashedTicket.UserData, @"""AccessToken"":""(.*?)"",""RefreshToken");
                        if (match.Success)
                        {
                            var accessToken = match.Groups[1].Value;
                            if (!string.IsNullOrEmpty(accessToken) && !accessToken.Contains("."))
                            {
                                FormsAuthentication.SignOut(); 
                                CookiesHelper.SetCookieValue(new HttpRequestWrapper(request), new HttpResponseWrapper(httpApp.Response), SSO.COOKIES_CMPSSO, "");
                            }
                        }
                    }
                }
            }
        }

        public void Dispose() { /* Not needed */ }
    }
}
