using DotNetOpenAuth.AspNet;
using Jade.OAuth2.Clients;
using Jade.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.oauth
{
    public class ASIOAuthClient : AspNetClient
    {
        public static User _user { get; set; }
        public static bool IsAuthenticatedUser { get; set; }
        public ASIOAuthClient(string clientIdentifier, string clientSecret, System.Uri authorizationEndpoint, System.Uri tokenEndpoint, System.Uri apiEndpoint)
            : base(clientIdentifier, clientSecret, authorizationEndpoint, tokenEndpoint, apiEndpoint)
        {
            
        }

        public Jade.UserManagement.DataObjects.User GetUser(int sso)
        {
            if (_user == null) _user = new User();
            Jade.UserManagement.DataObjects.User user = _user.Get(sso);
            return user;
        }
    }
}
