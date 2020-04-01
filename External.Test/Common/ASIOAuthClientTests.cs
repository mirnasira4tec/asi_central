using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.oauth;
using NUnit.Framework;

namespace External.Test.Common
{
    [TestFixture]
    class ASIOAuthClientTests
    {
        public string email = "wesptest@mail.com";
        public string username = "wesptest123";
        public string password = "password2";

        [Test]
        public void GetUserTest()
        {
            int sso = 242511;
            var user = ASIOAuthClient.GetUser(sso);
        }
    }
}
