using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model
{
    public class User
    {
        public int SSOId { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }
        public Nullable<int> InternalUserId { get; set; }
        public Nullable<int> IndividualId { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public int CompanyId { get; set; }
        public string PasswordHint { get; set; }
        public string PasswordAnswer { get; set; }
        public string PasswordQuestionCode { get; set; }
        public string PasswordQuestion { get; set; }
        public int IsTelephoneUpdatesAllowed { get; set; }
        public string TelephonePassword { get; set; }
        public int PasswordResetRequired { get; set; }
        public string PasswordResetKey { get; set; }
        public Nullable<System.DateTime> PasswordResetExpireDate { get; set; }
        public Nullable<System.DateTime> TerminatedDate { get; set; }
        public int IsSalesRep { get; set; }
        public int IsPVAdmin { get; set; }
        public int IsConnectPrimary { get; set; }
        public string StatusCode { get; set; }
        public string SignonTypeCode { get; set; }

        public string MmsLink { get; set; }

        public string AsiNumber { get; set; }
        public string CompanyName { get; set; }
        public string MemberStatus_CD { get; set; }
        public string MemberType_CD { get; set; }

        public string Phone { get; set; }
        public string PhoneAreaCode { get; set; }
        public string Cell { get; set; }
        public string FaxAreaCode { get; set; }
        public string Fax { get; set; }

        //Added more for address
        public string Title { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}
