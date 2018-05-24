using System;
using System.Configuration;
using asi.asicentral.services;
using asi.asicentral.oauth;
using System.Collections.Generic;
using System.Threading.Tasks;
using asi.asicentral.model;
using System.Net.Http;
using System.Net;
using ASI.Services.Http.SmartLink;
using ASI.Services.Http.Security;
using System.Security.Claims;
using NUnit.Framework;
using asi.asicentral.util;

namespace Core.Tests.OAuth
{
    [TestFixture]
    public class ASIOAuthClientTests
    {
        public string email = "wesptest@mail.com";
        public string username = "wesptest123";
        public string password = "password2";

        [Test]
        [Ignore("UserTestScenarios")]
        public void UserTestScenarios()
        {
            var tag = DateTime.Now.Ticks;
            asi.asicentral.model.User user = new asi.asicentral.model.User();
            user.Email = string.Format("TestCentralUser{0}@abc.com", tag.ToString());
            user.FirstName = "First1";
            user.LastName = "Last1";
            //Title
            user.Title = "TL";
            //Company
            user.CompanyName = "MacroSoft";
            user.CompanyId = 115143;
            user.UserName = user.Email;
            //ASI Number
            user.StatusCode = StatusCode.ACTV.ToString(); ;
            user.AsiNumber = "634567";
            user.MemberType_CD = "DISTRIBUTOR";
            user.PhoneAreaCode = "315";
            user.Phone = "5533255";
            user.FaxAreaCode = "315";
            user.Fax = "5533255";
            user.Street1 = "Street1";
            user.Street1 = "Street2";
            user.City = "TVM";
            user.CountryCode = "USA";
            user.Country = "India";
            user.State = "NY";
            user.Zip = "6995581";

            user.Password = "password1";
            user.PasswordHint = "password1";

            string result = ASIOAuthClient.CreateUser(user);
            Assert.AreNotEqual(Convert.ToInt32(result), 0);

            asi.asicentral.model.User createdUser = ASIOAuthClient.GetUser(Convert.ToInt32(result));
            Assert.AreEqual(user.Email, createdUser.Email);
            Assert.AreEqual(user.Street1, createdUser.Street1);
            Assert.AreEqual(user.FirstName, createdUser.FirstName);
            Assert.AreEqual(user.MemberType_CD, createdUser.MemberType_CD); // 4440
        }

        [Test]

        public void UserTestScenarios1()
        {
            var tag = DateTime.Now.Ticks;
            var email = string.Format("TestCentralUser{0}@abc.com", tag.ToString());
            asi.asicentral.model.User user = new asi.asicentral.model.User();
            user.Email = string.Format(email);
            user.FirstName = "Phani";
            user.LastName = "Kumar";
            //Title
            user.Title = "TL";
            //Company
            user.CompanyName = "A4Technologies";
            user.CompanyId = 491276;
            user.UserName = user.Email;
            //ASI Number
            user.StatusCode = StatusCode.ACTV.ToString(); ;
            user.AsiNumber = "68507";
            user.MemberType_CD = "DISTRIBUTOR";
            user.PhoneAreaCode = "315";
            user.Phone = "5533255";
            user.FaxAreaCode = "315";
            user.Fax = "5533255";
            user.Street1 = "19th Main";
            user.Street1 = "HSR Layout";
            user.City = "Banglore";
            user.CountryCode = "IND";
            user.Country = "India";
            user.State = "KA";
            user.Zip = "560068";

            user.Password = "password1";
            user.PasswordHint = "password1";

            string result = ASIOAuthClient.CreateUser(user);
            Assert.AreNotEqual(Convert.ToInt32(result), 0);

            var tokens = ASIOAuthClient.Login_FetchUserDetails(user.UserName, user.Password);
            Assert.IsNotNull(tokens);
            Assert.IsNotNull(tokens["AuthToken"]);
            Assert.IsNotNull(tokens["RefreshToken"]);
        }

        [Test]
        public void IsValidEmailFailureTest()
        {
            bool result1 = ASIOAuthClient.IsValidEmail("failtest@asicentral.com");
            Assert.IsFalse(result1);
        }

        [Test]
        public void IsValidEmailTest()
        {
            bool result1 = ASIOAuthClient.IsValidEmail(email);
            Assert.IsTrue(result1);
        }

        [Test]
        public void GetUserBySSOFailureTest()
        {
            var result1 = ASIOAuthClient.GetUser(123);
            Assert.IsNull(result1);
        }

        [Test]
        public void GetUserBySSOTest()
        {
            var result1 = ASIOAuthClient.GetUser(4419);
            Assert.IsNotNull(result1);
            Assert.AreEqual(result1.SSOId, 4419);
        }

        [Test]
        public void UpdateUserTest()
        {
            var result = ASIOAuthClient.GetUser(4419);
            Assert.IsNotNull(result);

            string street1 = result.Street1;
            result.Street1 = "Updated";
            bool isUpdated = ASIOAuthClient.UpdateUser(result);
            Assert.IsTrue(isUpdated);

            result = ASIOAuthClient.GetUser(4419);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Street1, "Updated");

            result.Street1 = street1;
            isUpdated = ASIOAuthClient.UpdateUser(result);
            Assert.IsTrue(isUpdated);

            result = ASIOAuthClient.GetUser(4419);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Street1, street1);
        }

        [Test]
        public void ChangePasswordTest()
        {
            asi.asicentral.model.Security security = new asi.asicentral.model.Security();
            security.Password = "password2";

            bool isPasswordChanged = ASIOAuthClient.ChangePassword(4419, security);
            Assert.IsTrue(isPasswordChanged);

            security.Password = "password1";
            isPasswordChanged = ASIOAuthClient.ChangePassword(4419, security);
            Assert.IsTrue(isPasswordChanged);
        }

        [Test]
        public void GetCopmanyByASITest()
        {
            asi.asicentral.model.User user = ASIOAuthClient.GetCompanyByASINumber("342495");
            Assert.IsNotNull(user);
            Assert.AreEqual(user.AsiNumber, "342495");
        }

        [Test]
        public void GetCopmanyByASIFailedTest()
        {
            asi.asicentral.model.User user = ASIOAuthClient.GetCompanyByASINumber("12345");
            Assert.IsNull(user);
        }

        [Test]
        public void IsValidUserByTrueCredentials()
        {
            IDictionary<string, string> result = ASIOAuthClient.IsValidUser(this.username, this.password);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result["AuthToken"]);
            Assert.IsNotNull(result["RefreshToken"]);
        }

        [Test]
        public void IsValidUserByFalseCredentials()
        {
            IDictionary<string, string> result = ASIOAuthClient.IsValidUser("125724pk1", "password1");
            Assert.IsNull(result);
        }

        [Test]
        public void LoginTest()
        {
            var tokens = ASIOAuthClient.Login_FetchUserDetails(this.username, this.password);
            Assert.IsNotNull(tokens);
            Assert.IsNotNull(tokens["AuthToken"]);
            Assert.IsNotNull(tokens["RefreshToken"]);
        }

        [Test]
        public void RefreshTokenTest()
        {
            var tokens = ASIOAuthClient.Login_FetchUserDetails(this.username, this.password);
            Assert.IsNotNull(tokens);
            Assert.IsNotNull(tokens["AuthToken"]);
            Assert.IsNotNull(tokens["RefreshToken"]);

            tokens = ASIOAuthClient.RefreshToken(tokens["RefreshToken"]);
            Assert.IsNotNull(tokens);
            Assert.IsNotNull(tokens["AuthToken"]);
            Assert.IsNotNull(tokens["RefreshToken"]);
        }

        [Test]
        public void GenarateTokens()
        {
            var userName = this.username;
            var password = this.password;

            var host = ConfigurationManager.AppSettings["SecurityHost"]; //"local-authentication.asicentral.com"; //READ FROM CONFIGURATION
            var relativePath = ConfigurationManager.AppSettings["RelativePath"];// "OAuth2"; //READ FROM CONFIGURATION
            var clientId = ConfigurationManager.AppSettings["AsiOAuthClientId"]; //"3734"; //READ FROM CONFIGURATION
            var clientSecret = ConfigurationManager.AppSettings["AsiOAuthClientSecret"];// "68ab10fb8ad45dce030b7f6d3eb129e5"; //READ FROM CONFIGURATION

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true; //TO IGNORE CERTIFICATE ERRORS

            OAuth2Client oauth2Client = new OAuth2Client(host, relativePath: relativePath);
            var oauth2Response = oauth2Client.Login(clientId, clientSecret, userName, password, scope: "AsiNumberOptional").Result;
            Assert.IsNotNull(oauth2Response.AccessToken);
            Assert.IsNotNull(oauth2Response.RefreshToken);
            var authenticatedUser = GetAuthenticatedUser(oauth2Response.AccessToken);
            Assert.IsNotNull(authenticatedUser != null);
        }

        private AuthenticatedUser GetAuthenticatedUser(string accessToken)
        {
            IList<Claim> claims = new List<Claim>();
            var claimsPrincipal = Token.Validate(accessToken, out claims);
            return claimsPrincipal.Identity as AuthenticatedUser;
        }

        [Test]
        public void TestRefreshTokenIfAccessTokenIsNotValid()
        {
            string accessToken = string.Empty;
            string refreshToken = string.Empty;
            string accessTokenNew = string.Empty;
            string refreshTokenNew = string.Empty;
            //string accessToken = "b3ff0a9e522801bdbae57323b396dea0b9002a8cecb539c1f8c562c7193648a9";
            //string refreshToken = "2db23f88dd8b837700df59c3b3a3183e86adbbdddf52210b9f818386468b9774";

            var encryptionService = new EncryptionService();
            var userName = this.username;
            var password = this.password;

            var host = ConfigurationManager.AppSettings["SecurityHost"]; //"local-authentic
            var relativePath = ConfigurationManager.AppSettings["RelativePath"];// "OAuth2"
            var clientId = ConfigurationManager.AppSettings["AsiOAuthClientId"]; //"3734"; ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true; //TO IGNORE CERTIFICATE ERRORS
            var clientSecret = ConfigurationManager.AppSettings["AsiOAuthClientSecret"];// 

            OAuth2Client oauth2Client = new OAuth2Client(host, relativePath: relativePath);
            var oauth2Response = oauth2Client.Login(clientId, clientSecret, userName, password, scope: "AsiNumberOptional").Result;

            if (oauth2Response != null)
            {
                accessToken = oauth2Response.AccessToken;
                refreshToken = oauth2Response.RefreshToken;
            }

            bool isValidToken = ASIOAuthClient.IsValidAccessToken(accessToken);
            if (!isValidToken)
            {
                //call lms with valid token
                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("tokenid", encryptionService.ECBEncrypt("ASIP@ssWord34567", accessToken));
                string data = asi.asicentral.util.HtmlHelper.SubmitForm("http://asi.upsidelms.com/asi/rest/curriculumtranscriptdetail", parameters, true, true);
                Assert.IsNotNull(data);

                IDictionary<string, string> tokens = ASIOAuthClient.RefreshToken(refreshToken);
                if (tokens.ContainsKey("AuthToken")) accessTokenNew = tokens["AuthToken"];
                if (tokens.ContainsKey("RefreshToken")) refreshTokenNew = tokens["RefreshToken"];
                Assert.IsNotNull(accessTokenNew);
                Assert.IsNotNull(refreshTokenNew);
                Assert.AreNotEqual(accessToken, accessTokenNew);
                Assert.AreNotEqual(refreshToken, refreshTokenNew);
                //test new access token
                isValidToken = ASIOAuthClient.IsValidAccessToken(accessTokenNew);
                Assert.IsTrue(isValidToken, "The new token was not valid from: " + accessToken + " to " + accessTokenNew);
                //call lms
                parameters = new Dictionary<string, string>();
                parameters.Add("tokenid", encryptionService.ECBEncrypt("ASIP@ssWord34567", accessTokenNew));
                data = asi.asicentral.util.HtmlHelper.SubmitForm("http://asi.upsidelms.com/asi/rest/curriculumtranscriptdetail", parameters, true, true);
                Assert.IsNotNull(data);
                //test again
                isValidToken = ASIOAuthClient.IsValidAccessToken(accessTokenNew);
                Assert.IsTrue(isValidToken, "The new token was not valid after lms from: " + accessToken + " to " + accessTokenNew);
            }
        }
        [Test]
        public void TestIsServiceAvailable()
        {
            var sUrl = ConfigurationManager.AppSettings["svcUri"];
            var isServiceRunning = Utility.IsServiceAvailable(sUrl);
            Assert.IsTrue(isServiceRunning);

            sUrl = "ftp" + ConfigurationManager.AppSettings["svcUri"];//making url invalid
            isServiceRunning = Utility.IsServiceAvailable(sUrl);
            Assert.IsFalse(isServiceRunning);
        }
        [Test]
        public void TestIsPersonifyAvailable()
        {
            var sUrl = ConfigurationManager.AppSettings["svcUri"];
            var isPersonifyRunning = Utility.IsPersonifyAvailable(true, sUrl);
            Assert.IsTrue(isPersonifyRunning);

            isPersonifyRunning = Utility.IsPersonifyAvailable(false, sUrl);
            Assert.False(isPersonifyRunning);
        }
    }
}
