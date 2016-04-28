using System;
using System.Configuration;
using asi.asicentral.services;
using ASI.Jade.OAuth2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.oauth;
using System.Collections.Generic;
using ASI.Jade.v2;
using System.Threading.Tasks;
using asi.asicentral.model;
using System.Net.Http;

namespace Core.Tests.OAuth
{
    [TestClass]
    public class ASIOAuthClientTests
    {
        [Ignore]
        [TestMethod]
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
            user.StatusCode = StatusCode.ACTV.ToString();;
            user.AsiNumber = "634567";
            user.MemberType_CD = "DIST";
            user.PhoneAreaCode = "315";
            user.Phone = "5533255";
            user.FaxAreaCode = "315";
            user.Fax = "5533255";
            user.Street1 = "Street1";
            user.Street1 = "Street2";
            user.City= "TVM";
            user.CountryCode = "USA";
            user.Country = "India";
            user.State = "NY";
            user.Zip = "6995581";

            user.Password = "password1";
            user.PasswordHint = "password1";

            string result = ASIOAuthClient.CreateUser(user);
            Assert.AreNotEqual(Convert.ToInt32(result), 0);

            asi.asicentral.model.User getUser = ASIOAuthClient.GetUser(Convert.ToInt32(result));
                       
            Assert.AreEqual(getUser.FirstName, user.FirstName);
            Assert.AreEqual(getUser.UserName, user.UserName);
            Assert.AreEqual(getUser.UserName, user.Email);

            bool result1 = ASIOAuthClient.IsValidEmail(user.Email);
            Assert.IsTrue(result1);

            user.SSOId = getUser.SSOId;
            user.FirstName = "FirstEdit";
            user.LastName = "LastEdit";
            result1 = ASIOAuthClient.UpdateUser(user);
            Assert.IsTrue(result1);

            IDictionary<string, string> result2 = ASIOAuthClient.IsValidUser(user.UserName, user.Password);
            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void IsValidEmailFailureTest()
        {
            bool result1 = ASIOAuthClient.IsValidEmail("anyone@macrosoftindia.com");
            Assert.IsFalse(result1);
        }

        [TestMethod]
        public void GetCopmanyByASITest()
        {
            asi.asicentral.model.User user = ASIOAuthClient.GetCompanyByASINumber("342495");
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void GetCopmanyByASIFailedTest()
        {
            asi.asicentral.model.User user = ASIOAuthClient.GetCompanyByASINumber("12345");
            Assert.IsNull(user);
        }

        [TestMethod]
        public void IsValidUserByTrueCredentials()
        {
            IDictionary<string, string> result = ASIOAuthClient.IsValidUser("yperrin", "asiCentral5");
            Assert.IsNotNull(result);
		}
                
        [TestMethod]
        public void IsValidUserByFalseCredentials()
        {
            IDictionary<string, string> result = ASIOAuthClient.IsValidUser("125724pk1", "password1");
            Assert.IsNull(result);
        }

	    [TestMethod]
	    public void Login()
	    {
			string accessToken = string.Empty;
			string refreshToken = string.Empty;
			string accessTokenNew = string.Empty;
			string refreshTokenNew = string.Empty;

			var asiOAuthClientId = ConfigurationManager.AppSettings["AsiOAuthClientId"];
			var asiOAuthClientSecret = ConfigurationManager.AppSettings["AsiOAuthClientSecret"];
			if (!string.IsNullOrEmpty(asiOAuthClientId) && !string.IsNullOrEmpty(asiOAuthClientSecret))
			{
				WebServerClient webServerClient = new WebServerClient(asiOAuthClientId, asiOAuthClientSecret);
				IDictionary<string, string> tokens = webServerClient.Login("yperrin", "asiCentral5");
				if (tokens.ContainsKey("AuthToken")) accessToken = tokens["AuthToken"];
				if (tokens.ContainsKey("RefreshToken")) refreshToken = tokens["RefreshToken"];
				Assert.IsNotNull(accessToken);
				Assert.IsNotNull(refreshToken);

				tokens = webServerClient.RefreshToken(refreshToken);
				if (tokens.ContainsKey("AuthToken")) accessTokenNew = tokens["AuthToken"];
				if (tokens.ContainsKey("RefreshToken")) refreshTokenNew = tokens["RefreshToken"];
				Assert.IsNotNull(accessTokenNew);
				Assert.IsNotNull(refreshTokenNew);
				Assert.AreNotEqual(accessToken, accessTokenNew);
				Assert.AreNotEqual(refreshToken, refreshTokenNew);
			}
		}

        [TestMethod]
        public void GenarateTokens()
        {
            string accessToken = string.Empty;
			string refreshToken = string.Empty;
            
            var asiOAuthClientId = ConfigurationManager.AppSettings["AsiOAuthClientId"];
			var asiOAuthClientSecret = ConfigurationManager.AppSettings["AsiOAuthClientSecret"];
            if (!string.IsNullOrEmpty(asiOAuthClientId) && !string.IsNullOrEmpty(asiOAuthClientSecret))
            {
                var webServerClient = new WebServerClient(asiOAuthClientId, asiOAuthClientSecret);
                var tokens = webServerClient.Login("yperrin", "asiCentral5");
                if (tokens.ContainsKey("AuthToken")) accessToken = tokens["AuthToken"];
                if (tokens.ContainsKey("RefreshToken")) refreshToken = tokens["RefreshToken"];
                Assert.IsNotNull(accessToken);
                Assert.IsNotNull(refreshToken);
				Console.WriteLine("Auth:" + accessToken);
				Console.WriteLine("Refresh:" + refreshToken);
			}
        }

        [TestMethod]
        public void TestRefreshTokenIfAccessTokenIsNotValid()
        {
			string accessToken = string.Empty;
			string refreshToken = string.Empty;
			string accessTokenNew = string.Empty;
			string refreshTokenNew = string.Empty;
			//string accessToken = "b3ff0a9e522801bdbae57323b396dea0b9002a8cecb539c1f8c562c7193648a9";
			//string refreshToken = "2db23f88dd8b837700df59c3b3a3183e86adbbdddf52210b9f818386468b9774";

			var encryptionService = new EncryptionService();
            var asiOAuthClientId = ConfigurationManager.AppSettings["AsiOAuthClientId"];
            var asiOAuthClientSecret = ConfigurationManager.AppSettings["AsiOAuthClientSecret"];
            if (!string.IsNullOrEmpty(asiOAuthClientId) && !string.IsNullOrEmpty(asiOAuthClientSecret))
            {
                var webServerClient = new WebServerClient(asiOAuthClientId, asiOAuthClientSecret);
	            if (accessToken == string.Empty)
	            {
					var tokens = webServerClient.Login("yperrin", "asiCentral5");
					if (tokens.ContainsKey("AuthToken")) accessToken = tokens["AuthToken"];
					if (tokens.ContainsKey("RefreshToken")) refreshToken = tokens["RefreshToken"];		            
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
        }
    }
}
