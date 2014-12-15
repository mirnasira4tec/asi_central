using System;
using System.Configuration;
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
    }
}
