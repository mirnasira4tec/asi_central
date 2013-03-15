using System;
using System.Collections.Generic;

namespace asi.asicentral.model.timss
{
    public class TIMSSCompany
    {
        public System.Guid DAPP_UserId { get; set; }
        public decimal SequenceNumber { get; set; }
        public Nullable<decimal> CompanyRecordId { get; set; }
        public string MasterCustomerId { get; set; }
        public Nullable<decimal> SUB_CUSTOMER_ID { get; set; }
        public Nullable<decimal> ASI_Central_ID { get; set; }
        public string Name { get; set; }
        public string CustomerClass { get; set; }
        public string BillAddress1 { get; set; }
        public string BillAddress2 { get; set; }
        public string BillAddress3 { get; set; }
        public string BillAddress4 { get; set; }
        public string BillCity { get; set; }
        public string BillState { get; set; }
        public string BillPostalCode { get; set; }
        public string BillCountryCode { get; set; }
        public string ShipAddress1 { get; set; }
        public string ShipAddress2 { get; set; }
        public string ShipAddress3 { get; set; }
        public string ShipAddress4 { get; set; }
        public string ShipCity { get; set; }
        public string ShipState { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountryCode { get; set; }
        public string PhoneCountryCode { get; set; }
        public string PhoneAreaCode { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneExtension { get; set; }
        public string FaxCountryCode { get; set; }
        public string FaxAreaCode { get; set; }
        public string FaxNumber { get; set; }
        public string FaxExtension { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string PROCESSED_FLAG { get; set; }
        public string ERROR_FLAG { get; set; }
        public string REJECT_REASON { get; set; }
        public string CONCURRENCY_ID { get; set; }
        public string Load_Status { get; set; }
        public Nullable<System.DateTime> Load_date { get; set; }
    }
}
