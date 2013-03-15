using System;
using System.Collections.Generic;

namespace asi.asicentral.model.timss
{
    public class TIMSSContact
    {
        public System.Guid DAPP_UserId { get; set; }
        public int RecordId { get; set; }
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Credentials { get; set; }
        public string Title { get; set; }
        public string CustomerClass { get; set; }
        public string PhoneCountryCode { get; set; }
        public string PhoneAreaCode { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneExtension { get; set; }
        public string FaxCountryCode { get; set; }
        public string FaxAreaCode { get; set; }
        public string FaxNumber { get; set; }
        public string FaxExtension { get; set; }
        public string Email { get; set; }
        public string PrimaryFlag { get; set; }
        public string ProcessedFlag { get; set; }
        public string ErrorFlag { get; set; }
        public string RejectReason { get; set; }
        public string ConcurrencyId { get; set; }
        public string LoadStatus { get; set; }
        public Nullable<System.DateTime> LoadDate { get; set; }
    }
}
