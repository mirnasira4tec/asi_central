using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.oauth;
using System.Collections.Generic;
using ASI.Jade.v2;
using System.Threading.Tasks;
using asi.asicentral.model;
using unirest_net.http;
using System.Net.Http;

namespace Core.Tests.OAuth
{
    [TestClass]
    public class ASIOAuthClientTests
    {
        //[TestMethod]
        public void UserTestScenarios()
        {
            asi.asicentral.model.User user = new asi.asicentral.model.User();
            user.Email = "TestCentralUser1114@abc.com";
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

            //var res =  Task.Factory.StartNew(() => UMS.UserDelete(user.SSOId).Result, TaskCreationOptions.LongRunning).Result;
            //Assert.AreEqual(res, user.SSOId.ToString());
        }

        [TestMethod]
        public void IsValidUserByFalseCredentials()
        {
            IDictionary<string, string> result = ASIOAuthClient.IsValidUser("125724pk1", "password1");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void VerifyUser()
        {
            IDictionary<string, string> result = ASIOAuthClient.IsValidUser("125724pk", "password1");
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
