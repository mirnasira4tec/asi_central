using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.oauth;
using System.Collections.Generic;

namespace Core.Tests.OAuth
{
    [TestClass]
    public class ASIOAuthClientTests
    {
        [TestMethod]
        public void UserTestScenarios()
        {
            asi.asicentral.model.User user = new asi.asicentral.model.User();
            user.Email = "Test11@abc.com";
            user.FirstName = "First1";
            user.LastName = "Last1";
            //Title
            user.Title = "TL";
            //Company
            user.CompanyName = "MacroSoft";
            user.CompanyId = 115143;
            user.UserName = "unique1234";
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

            var result = ASIOAuthClient.CreateUser(user);
        }

        [TestMethod]
        public void IsValidUserByEmail()
        {
            //bool result = ASIOAuthClient.IsValidEmail("kphani@macrosoftindia.com");
            bool result = ASIOAuthClient.IsValidEmail("kphani@macrosoftindia.com");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidEmail()
        {
            bool result = ASIOAuthClient.IsValidEmail("test123@hotmail.com");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidUserByCredentials()
        {
            IDictionary<string, string> result = ASIOAuthClient.IsValidUser("125724pk", "password1");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IsValidUserByFalseCredentials()
        {
            IDictionary<string, string> result = ASIOAuthClient.IsValidUser("125724pk1", "password1");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetUserBySSO()
        {
            asi.asicentral.model.User result = ASIOAuthClient.GetUser(168793);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUserByToken()
        {
            asi.asicentral.model.User result = ASIOAuthClient.GetUser("endfOr-JCWaCHazhs25cMHS1N4ddMqjV7jqjgMi62_m4ifiU19TLfnOOUfzOXIvQUli25TFs3xAF8AVXp6sxSTikZaM1");
            Assert.IsNull(result);
        }
    }
}
