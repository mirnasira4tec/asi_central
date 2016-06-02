using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace asi.asicentral.web.Helpers
{
    public class Authorized
    {
        public static bool IsAuthorizedUser()
        {
            string[] allowedusers = ConfigurationManager.AppSettings["AuthorizedUsers"].Split(';');
            WindowsIdentity identity = HttpContext.Current.Request.LogonUserIdentity;
            var Name = identity.Name;
            var AuthorizedUser = false;
            if (allowedusers.Any())
            {
                for (int i = 0; i < allowedusers.Count(); i++)
                {
                    if (Name.ToLower().Equals(allowedusers[i].ToLower()))
                    {
                        AuthorizedUser = true;
                        break;
                    }
                    else
                        AuthorizedUser = false;
                }
            }
            return AuthorizedUser;
        }
    }
}