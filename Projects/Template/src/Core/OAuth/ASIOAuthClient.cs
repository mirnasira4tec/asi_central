﻿using DotNetOpenAuth.AspNet;
using ASI.Jade.OAuth2;
using ASI.Jade.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace asi.asicentral.oauth
{
    public class ASIOAuthClient : AspNetClient
    {
        private static string _clientIdentifier;
        private static string _clientSecret;

        private static Uri _authorizationEndpoint;
        private static Uri _tokenEndpoint;
        private static Uri _apiEndpoint;
        private static ASIOAuthClient _asiOAuthClient;
        
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
            _clientIdentifier = clientIdentifier;
            _clientSecret = clientSecret;
            _authorizationEndpoint = authorizationEndpoint;
            _tokenEndpoint = tokenEndpoint;
            _apiEndpoint = apiEndpoint;
            _asiOAuthClient = this;
        }

        public delegate ASIOAuthClient GetProviderDelegate();

        public static ASIOAuthClient GetProvider()
        {
            if (_asiOAuthClient != null)
                return _asiOAuthClient;
            return null;
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
                }
            }
            catch { }
            return user;
        }

        public static bool VerifyUserCredentials(string asiNumber, string userName, string password)
        {
            IDictionary<string, string> tokens = null;
            bool isValidUser = false;
            ASI.Jade.OAuth2.WebServerClient webServerClient = new WebServerClient(_clientIdentifier, _clientSecret, _authorizationEndpoint, _tokenEndpoint, _apiEndpoint);
            try
            {
                tokens = webServerClient.Login(asiNumber, userName, password);
                isValidUser = true;
            }
            catch { isValidUser = false; }
            return isValidUser;
        }
    }
}
