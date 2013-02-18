using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class ASPNetMembership
    {
        public Guid UserId { get; set; }
        public Guid ApplicationId { get; set; }
        public string Password { get; set; }
        public int PasswordFormat { get; set; }
        public string PasswordSalt { get; set; }
        public string MobilePIN { get; set; }
        public string Email { get; set; }
        public string LoweredEmail { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<bool> IsLockedOut { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<DateTime> LastLoginDate { get; set; }
        public Nullable<DateTime> LastPasswordChangedDate { get; set; }
        public Nullable<DateTime> LastLockoutDate { get; set; }
        public int FailedPasswordAttemptCount { get; set; }
        public Nullable<DateTime> FailedPasswordAttemptWindowStart { get; set; }
        public int FailedPasswordAnswerAttemptCount { get; set; }
        public Nullable<DateTime> FailedPasswordAnswerAttemptWindowStart { get; set; }
        public string Comment { get; set; }
    }
}
