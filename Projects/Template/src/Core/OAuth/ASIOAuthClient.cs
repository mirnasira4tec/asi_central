using DotNetOpenAuth.AspNet;
using ASI.Jade.OAuth2;
using ASI.Jade.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using ASI.Jade.Utilities;
using ASI.Jade.UserManagement.DataObjects;

namespace asi.asicentral.oauth
{
    public class ASIOAuthClient : AspNetClient
    {
        private ASIOAuthClient _asiOAuthClient;
        
        private static ASI.Jade.UserManagement.User _juser { get; set; }
        private static ASI.Jade.UserManagement.User JUser
        {
            get
            {
                if (_juser == null)
                {
                    _juser = new ASI.Jade.UserManagement.User();
                    return _juser;
                }
                else { return _juser; }
            }
        }

        public ASIOAuthClient(string clientIdentifier, string clientSecret, System.Uri authorizationEndpoint, System.Uri tokenEndpoint, System.Uri apiEndpoint)
            : base(clientIdentifier, clientSecret, authorizationEndpoint, tokenEndpoint, apiEndpoint)
        {
            _asiOAuthClient = this;
        }

        public delegate ASIOAuthClient GetProviderDelegate();

        public ASIOAuthClient GetProvider()
        {
            if (_asiOAuthClient != null)
                return _asiOAuthClient;
            return null;
        }

        public static asi.asicentral.model.User GetUser(string token)
        {
            asi.asicentral.model.User user = null;
            try
            {
                ASI.Jade.Utilities.CrossApplication.RedirectParams redirectParams = CrossApplication.ParseTokenUrl(token);
                var client = new ASI.Jade.OAuth2.WebServerClient();
                IDictionary<string, string> userDetails = client.GetUserDetails(redirectParams.AccessToken);
                if (userDetails != null)
                {
                    var loggedInUser = new TokenizedUser();
                    loggedInUser = new TokenizedUser()
                    {
                        SSOId = Convert.ToInt32(userDetails["sign_in_id"]),
                        FirstName = userDetails["first_name"],
                        LastName = userDetails["last_name"],
                        Email = userDetails["email"],
                        CompanyId = Convert.ToInt32(userDetails["company_id"]),
                        Token = new UserToken() { AccessToken = redirectParams.AccessToken, RefreshToken = redirectParams.RefreshToken, TokenExpirationTime = redirectParams.TokenExpirationTime },
                        AsiNumber = userDetails["asi_number"],
                        CompanyName = userDetails["company_name"],
                        MemberTypeCode = userDetails["MemberType_CD"],
                    };
                    GetUser(loggedInUser.SSOId);
                }
            }
            catch 
            {
                return null;
            }
            return user;
        }

        public static asi.asicentral.model.User GetUser(int sso)
        {
            model.User user = null;
            try
            {
                ASI.Jade.UserManagement.DataObjects.User jadeuser = JUser.Get(sso);
                if (jadeuser != null)
                {
                    user = new model.User();
                    user.SSOId = jadeuser.SSOId;
                    user.Email = jadeuser.Email;
                    user.UserName = jadeuser.UserName;
                    user.InternalUserId = jadeuser.InternalUserId;
                    user.IndividualId = jadeuser.IndividualId;
                    user.Password = jadeuser.Password;
                    user.FirstName = jadeuser.FirstName;
                    user.MiddleName = jadeuser.MiddleName;
                    user.LastName = jadeuser.LastName;
                    user.Prefix = jadeuser.Prefix;
                    user.Suffix = jadeuser.Suffix;
                    user.CreateDate = jadeuser.CreateDate;
                    user.UpdateDate = jadeuser.UpdateDate;
                    user.UpdateSource = jadeuser.UpdateSource;
                    user.CompanyId = jadeuser.CompanyId;
                    user.PasswordHint = jadeuser.PasswordHint;
                    user.PasswordAnswer = jadeuser.PasswordAnswer;
                    user.PasswordQuestionCode = jadeuser.PasswordQuestionCode;
                    user.PasswordQuestion = jadeuser.PasswordQuestion;
                    user.IsTelephoneUpdatesAllowed = jadeuser.IsTelephoneUpdatesAllowed;
                    user.TelephonePassword = jadeuser.TelephonePassword;
                    user.PasswordResetRequired = jadeuser.PasswordResetRequired;
                    user.PasswordResetKey = jadeuser.PasswordResetKey;
                    user.PasswordResetExpireDate = jadeuser.PasswordResetExpireDate;
                    user.TerminatedDate = jadeuser.TerminatedDate;
                    user.IsSalesRep = jadeuser.IsSalesRep;
                    user.IsPVAdmin = jadeuser.IsPVAdmin;
                    user.IsConnectPrimary = jadeuser.IsConnectPrimary;
                    user.StatusCode = jadeuser.StatusCode;
                    user.SignonTypeCode = jadeuser.SignonTypeCode;
                    user.MmsLink = jadeuser.MmsLink;
                    user.AsiNumber = jadeuser.AsiNumber;
                    user.CompanyName = jadeuser.CompanyName;
                    user.MemberStatus_CD = jadeuser.MemberStatus_CD;
                    user.MemberType_CD = jadeuser.MemberType_CD;
                    user.Phone = jadeuser.Phone;
                    user.Cell = jadeuser.Cell;
                    user.Fax = jadeuser.Fax;

                    //if (jadeuser.CompanyId != 0)
                    //{
                    //    ASI.EntityModel.Company jadeCompany = ASI.Jade.Company.Retriever.Get(jadeuser.CompanyId);
                    //    if(jadeCompany != null)
                    //    {
                    //        if(jadeCompany.Contacts != null && jadeCompany.Contacts.Count > 0)
                    //        {
                    //            ASI.EntityModel.Contact contact = jadeCompany.Contacts.ElementAt(0);
                    //            user.Title = contact.Title;
                    //            user.Suffix = contact.Suffix;
                    //            if (jadeCompany.Contacts.ElementAt(0).Addresses != null &&
                    //               jadeCompany.Contacts.ElementAt(0).Addresses.Count > 0)
                    //            {
                    //                ASI.EntityModel.Address address = jadeCompany.Contacts.ElementAt(0).Addresses.ElementAt(0);
                    //                user.Street1 = address.AddressLine1 + " " + address.AddressLine2;
                    //                user.Street2 = address.AddressLine3 + " " + address.AddressLine4;
                    //                user.State = address.State;
                    //                user.CountryCode = address.CountryCode;
                    //                user.Country = address.County;
                    //                user.Zip = address.ZipCode;
                    //                user.City = address.City;
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            catch { }
            return user;
        }

        public static bool VerifyUserCredentials(string asiNumber, string userName, string password)
        {
            IDictionary<string, string> tokens = null;
            bool isValidUser = false;
            var oAuthEndpoint = ConfigurationManager.AppSettings["AuthorizationEndPoint"];
            if (!string.IsNullOrEmpty(oAuthEndpoint))
            {
                string _clientIdentifier = ConfigurationManager.AppSettings["AsiOAuthClientId"];
                string _clientSecret = ConfigurationManager.AppSettings["AsiOAuthClientSecret"];
                ASI.Jade.OAuth2.WebServerClient webServerClient = new WebServerClient(_clientIdentifier, _clientSecret, 
                        new Uri(oAuthEndpoint + "/oauth/authorize"),
                        new Uri(oAuthEndpoint + "/oauth/token"),
                        new Uri(oAuthEndpoint + "/api/users"));
                try
                {
                    tokens = webServerClient.Login(asiNumber, userName, password);
                    isValidUser = true;
                }
                catch { isValidUser = false; }
            }
            return isValidUser;
        }
    }
}
