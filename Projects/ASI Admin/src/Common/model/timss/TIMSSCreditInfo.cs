using System;
using System.Collections.Generic;

namespace asi.asicentral.model.timss
{
    public class TIMSSCreditInfo
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<System.Guid> UserId { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public Nullable<System.Guid> TransactionId { get; set; }
        public System.Guid DAPP_UserId { get; set; }
        public string SecurityCode { get; set; }
    }
}
