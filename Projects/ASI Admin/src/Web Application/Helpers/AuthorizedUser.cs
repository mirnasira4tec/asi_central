using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace asi.asicentral.web
{
    public class Authorized
    {
        private readonly string[] allowedusers = ConfigurationManager.AppSettings["AuthorizedUsers"].Split(';');

        public bool IsAuthorizedUser()
        {
            WindowsIdentity identity = HttpContext.Current.Request.LogonUserIdentity;
            var Name = identity.Name;
            var AuthorizedUser = false;
            if (allowedusers.Any())
            {
                for (int i = 0; i < allowedusers.Count(); i++)
                {
                    if (Name.Equals(allowedusers[i]))
                        AuthorizedUser = true; 
                    else
                        AuthorizedUser = false;
                }
            }
            return AuthorizedUser;
        }
    }
}