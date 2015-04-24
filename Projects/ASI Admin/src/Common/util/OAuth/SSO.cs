using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Web.Security;
using System.Web;
using Umbraco.Core.Services;
using Umbraco.Core.Models;
using Umbraco.Core;
using asi.asicentral.interfaces;
using asi.asicentral.services;

namespace asi.asicentral.oauth
{
    public class SSO
    {
        public const string COOKIES_CMPSSO = "CMPSSO";
        public const string COOKIES_USERNAME = "Name";
        public const string COOKIES_MEMBERTYPE_CODE = "MemberType";
        public const string COOKIES_ASP_NET_SESSION_ID = "ASP.NET_SessionId";

        public enum MemberType
        {
            [Description("Distributor")]
            DIST,
            [Description("Distributor")]
            DISTRIBUTOR,
            [Description("Prospective Distributor")]
            PROSDISTRIBUTOR,
            [Description("Prospective Distributor")]
            NM_DISTRIBUTOR,

            [Description("MultiLineRep")]
            MULTILINE_REP,
            [Description("MultiLineRep")]
            MULTI_LINE_REP,

            [Description("Supplier")]
            SPLR,
            [Description("Supplier")]
            SUPPLIER,
            [Description("Prospective Supplier")]
            PROSSUPPLIER,
            [Description("Prospective Supplier")]
            NM_SUPPLIER,

            [Description("Decorator")]
            DECR,
            [Description("Decorator")]
            DECORATOR,
            [Description("Prospective Decorator")]
            PROSDECORATOR,
            [Description("Prospective Decorator")]
            NM_DECORATOR,

            [Description("End Buyer")]
            EDBY,
            [Description("End Buyer")]
            END_BUYER,

            [Description("Affiliate")]
            AFFL,
            [Description("Affiliate")]
            AFFILIATE,
            [Description("Prospective Affiliate")]
            PROSAFFILIATE,

            [Description("Guest")]
            UNKN,
            [Description("Guest")]
            UNKNOWN,
            [Description("Guest")]
            NM_UNKNOWN,
            [Description("Guest")]
            ADV_AGENCY,
            [Description("Guest")]
            ANZ_DISTRIBUTOR,
            [Description("Guest")]
            ANZ_SUPPLIER,
            [Description("Guest")]
            BILLING,
            [Description("Guest")]
            FACILITY,
            [Description("Guest")]
            NA_BANK,
            [Description("Guest")]
            OUT_INDUSTRY,
            [Description("Guest")]
            PROVIDER,
            [Description("Guest")]
            REGION,
            [Description("Guest")]
            SHOWCONTRACT,
            [Description("Guest")]
            SGR_SUPPLIER,
            [Description("Guest")]
            VIP
        }

        public static string GetRoleName(string memberType, string memberStatus)
        {
            string rolename = GetMemberTypeDesciptionForRole<MemberType>(MemberType.UNKN.ToString());
            if (!string.IsNullOrEmpty(memberType) && !string.IsNullOrEmpty(memberStatus))
            {
                if (ASIOAuthClient.IsActiveUser(memberStatus)) rolename = GetMemberTypeDesciptionForRole<MemberType>(memberType);
                else
                {
                    MemberType code = (MemberType)Enum.Parse(typeof(MemberType), memberType);
                    switch (code)
                    {
                        case MemberType.SUPPLIER:
                        case MemberType.SPLR:
                            rolename = GetMemberTypeDesciptionForRole<MemberType>(MemberType.PROSSUPPLIER.ToString());
                            break;
                        case MemberType.DISTRIBUTOR:
                        case MemberType.DIST:
                            rolename = GetMemberTypeDesciptionForRole<MemberType>(MemberType.PROSDISTRIBUTOR.ToString());
                            break;
                        case MemberType.DECORATOR:
                        case MemberType.DECR:
                            rolename = GetMemberTypeDesciptionForRole<MemberType>(MemberType.PROSDECORATOR.ToString());
                            break;
                        case MemberType.AFFILIATE:
                        case MemberType.AFFL:
                            rolename = GetMemberTypeDesciptionForRole<MemberType>(MemberType.PROSAFFILIATE.ToString());
                            break;
                        default:
                            rolename = GetMemberTypeDesciptionForRole<MemberType>(MemberType.UNKN.ToString());
                            break;
                    }
                }
            }
            return rolename;
        }

        public static int AddOrRemoveUserFromRole(string username, string email, string memberType, string memberStatus, bool isAddRole)
        {
            ILogService logService = LogService.GetLog(typeof(SSO));
            logService.Debug("AddOrRemoveUserFromRole - Start: U-" + username + " Type-" + memberType + " S-" + memberStatus);
            int status = 0;
            if (!string.IsNullOrEmpty(username))
            {
                string rolename = GetRoleName(memberType, memberStatus);
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(rolename))
                {
                    IMemberService memberService = ApplicationContext.Current.Services.MemberService;
                    if (Roles.IsUserInRole(username, rolename))
                    {
                        status = 1; // When user already added in role
                        if (Roles.IsUserInRole(username, "Guest") && string.Compare("Guest", rolename) != 0)
                            Roles.RemoveUserFromRole(username, "Guest");

                        IMember member = memberService.GetByUsername(username);
                        if (!isAddRole)
                        {
                            memberService.Delete(member);
                            status = 2; // when user is successfully removed from role
                        }
                    }
                    else if (isAddRole)
                    {
                        IMember member = memberService.GetByUsername(username);
                        if (member == null)
                        {
                            member = memberService.CreateMember(username, email, username, "Member");
                            memberService.Save(member);
                        }
                        IEnumerable<string> roles = memberService.GetAllRoles();
                        if (member != null)
                        {
                            if (roles == null || (roles != null && roles.Count() > 0 && !roles.Contains(rolename)))
                                memberService.AddRole(rolename);
                            memberService.AssignRole(member.Id, rolename);
                        }
                        if (Roles.IsUserInRole(username, "Guest") && string.Compare("Guest", rolename) != 0)
                            Roles.RemoveUserFromRole(username, "Guest");
                        status = 3; // when user newly added to the role
                    }
                }
                else status = 4; // Username or rolename are empty

            }
            logService.Debug("AddOrRemoveUserFromRole - End:" + status);
            return status;
        }

        public static bool VerifyPassword(string password, System.Security.Principal.IPrincipal securityUser, asi.asicentral.model.User user = null, int sso = 0)
        {
            bool isValidPassword = false;
            if (!string.IsNullOrEmpty(password) && securityUser != null)
            {
                if (sso == 0) sso = Convert.ToInt32(securityUser.Identity.Name);
                if (user == null) user = ASIOAuthClient.GetUser(sso);
                IDictionary<string, string> userDetails = ASIOAuthClient.IsValidUser(user.UserName, password);
                if (userDetails != null && userDetails.Count > 0) isValidPassword = true;
            }
            return isValidPassword;
        }
        public static bool ResetPassword(string newPassword, int ssoid)
        {
            bool isPasswordchanged = false;
            if (!string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(ssoid.ToString()))
            {
                asi.asicentral.model.Security security = new asi.asicentral.model.Security();
                security.Password = newPassword;
                isPasswordchanged = ASIOAuthClient.ChangePassword(ssoid, security, false);
            }
            return isPasswordchanged;
        }
        public static bool ChangePassword(string currentPassword, string newPassword, System.Security.Principal.IPrincipal securityUser, HttpRequestBase request, HttpResponseBase response, bool isUserVerified = false, asi.asicentral.model.User user = null, bool passwordResetRequired = false)
        {
            bool isPasswordchanged = false;

            if ((!string.IsNullOrEmpty(currentPassword) || isUserVerified)
                && !string.IsNullOrEmpty(newPassword)
                && securityUser != null)
            {
                int sso = (user == null) ? CookiesHelper.GetId(false, request, response, COOKIES_CMPSSO) : user.SSOId;
                if (!isUserVerified)
                {
                    user = ASIOAuthClient.GetUser(sso);
                    isUserVerified = VerifyPassword(currentPassword, securityUser, user, sso);
                }

                if (isUserVerified)
                {
                    asi.asicentral.model.Security security = new asi.asicentral.model.Security();
                    security.Password = newPassword;
                    isPasswordchanged = ASIOAuthClient.ChangePassword(sso, security, passwordResetRequired);
                }
            }
            return isPasswordchanged;
        }

        /// <summary>
        /// Generate random password
        /// Code - http://forums.asp.net/p/1493859/3520369.aspx
        /// </summary>
        /// <param name="passwordLength"></param>
        /// <returns></returns>
        public static string CreateRandomPassword(int passwordLength)
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            const string allowedNumbers = "0123456789";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = (i > 0 && (i == passwordLength - 2)) ? allowedNumbers[rd.Next(0, allowedNumbers.Length)] : allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }

        public static string GetMemberTypeDesciptionForRole<T>(string memberType)
        {
            var code = (Enum)Enum.Parse(typeof(T), memberType);
            var attribute = asi.asicentral.util.EnumHelper.GetAttributeOfType<System.Attribute>(code);
            if (attribute != null)
                return ((DescriptionAttribute)(attribute)).Description;
            else return null;
        }

        public static bool IsLoggedIn()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                HttpRequestBase request = GetHttpRequestBase(HttpContext.Current.Request);
                bool loggedIn = request != null && request.Cookies != null &&
                                HttpContext.Current.User != null &&
                                HttpContext.Current.User.Identity != null &&
                                HttpContext.Current.User.Identity.IsAuthenticated &&
                                request.Cookies[COOKIES_USERNAME] != null &&
                                !string.IsNullOrEmpty(request.Cookies[COOKIES_USERNAME].Value);
                return loggedIn;
            }
            else return false;
        }

        public static bool HasMemberType(HttpRequestBase request, HttpResponseBase response)
        {
            string value = CookiesHelper.GetCookieValue(request, response, COOKIES_MEMBERTYPE_CODE);
            return !string.IsNullOrEmpty(value);
        }

        public static string GetMemberType(HttpRequestBase request, HttpResponseBase response)
        {
            string value = CookiesHelper.GetCookieValue(request, response, COOKIES_MEMBERTYPE_CODE);
            if (string.IsNullOrEmpty(value)) value = "Unknown";
            return value;
        }

        public static bool IsUserInRole(string rolename)
        {
            return (!string.IsNullOrEmpty(rolename) && IsLoggedIn() &&
                Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, rolename));
        }

        public static HttpRequestBase GetHttpRequestBase(HttpRequest request)
        {
            return new HttpRequestWrapper(request);
        }

        public static HttpResponseBase GetHttpResponseBase(HttpResponse response)
        {
            return new HttpResponseWrapper(response);
        }

        public static void ProcessUserInfo(asi.asicentral.model.User user, HttpRequestBase request, HttpResponseBase response, string domain, bool isAddRoles = false)
        {
            if (user != null)
            {
                // we then log the user into our application we could have done a database lookup for a more user-friendly username for our app
                //Define variable for cookie values

                string companyId = string.Empty;
                string sso = string.Empty;
                string membertypeCode = string.Empty;
                string username = string.Empty;

                if (!string.IsNullOrEmpty(user.FirstName)) username = ASIOAuthClient.GetUserName(user.FirstName, user.LastName);
                if (user.CompanyId != 0) companyId = user.CompanyId.ToString();
                if (user.SSOId != 0) sso = user.SSOId.ToString();
                if (!string.IsNullOrEmpty(user.MemberType_CD)) membertypeCode = user.MemberType_CD;

                //Store values in cookies for valid user
                ClearUserCookies(request, response, domain);
                string domainName = null;
                if (!request.Url.Authority.Contains("localhost")) domainName = domain;
                if (!String.IsNullOrEmpty(username)) CookiesHelper.SetCookieValue(request, response, COOKIES_USERNAME, username, true, domainName);
                if (!String.IsNullOrEmpty(membertypeCode)) CookiesHelper.SetCookieValue(request, response, COOKIES_MEMBERTYPE_CODE, membertypeCode, true, domainName);
                CookiesHelper.SetFormsAuthenticationCookie(request, response, user, true, domainName: domainName);
                CookiesHelper.SetCookieValue(request, response, COOKIES_CMPSSO, companyId + "-" + sso, true, domainName);

                //Code to add userrole
                if (isAddRoles) AddOrRemoveUserFromRole(username, user.Email, user.MemberType_CD, user.MemberStatus_CD, true);
            }
        }

        public static void ClearUserCookies(HttpRequestBase request, HttpResponseBase response, string domain)
        {
            string domainName = null;
            if (!request.Url.Authority.Contains("localhost")) domainName = domain;
            CookiesHelper.SetCookieValue(request, response, COOKIES_CMPSSO, string.Empty, domainName: domainName, year: -1);
            CookiesHelper.SetCookieValue(request, response, FormsAuthentication.FormsCookieName, string.Empty, domainName: domainName, persist: true, year: -1);
            CookiesHelper.SetCookieValue(request, response, COOKIES_ASP_NET_SESSION_ID, string.Empty, persist: true, year: -1);
        }

        public static string GetHttpUrl(string url)
        {
            if (!string.IsNullOrEmpty(url) && url.Contains("https:") && !url.Contains("localhost"))
                url = url.Replace("https:", "http:");
            return url;
        }
    }
}
