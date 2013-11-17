using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public class ASPNetUser
    {
        public ASPNetUser()
        {
        }

        public System.Guid ApplicationId { get; set; }
        public System.Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LoweredUserName { get; set; }
        public string MobileAlias { get; set; }
        public bool IsAnonymous { get; set; }
        public System.DateTime LastActivityDate { get; set; }
        public virtual CENTUserProfilesPROF CENT_UserProfiles_PROF { get; set; }
        public virtual ASPNetMembership Membership { get; set; }
    }
}
