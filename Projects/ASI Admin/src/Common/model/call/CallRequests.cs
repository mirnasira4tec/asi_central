using System;
using System.Collections.Generic;

namespace asi.asicentral.model.call
{
    public partial class CallRequest
    {
        public int Requests_ID { get; set; }
        public Nullable<long> Req_IP { get; set; }
        public long Req_CallbackNumber { get; set; }
        public System.DateTime Req_CallbackTime { get; set; }
        public int Req_Status { get; set; }
        public string Req_Sec_Honeypot { get; set; }
        public byte Req_Sec_Cookie { get; set; }
        public byte Req_Sec_Queue { get; set; }
        public byte Req_Sec_JavaScript { get; set; }
        public string Req_Ent_ASINum { get; set; }
        public string Req_Ent_Name { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateSource { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public string AuditStatus_CD { get; set; }
        public string Audit_XML { get; set; }
        public int ROW_ID { get; set; }
        public Nullable<System.Guid> MOD_ID { get; set; }
        public byte Req_Queue { get; set; }
    }
}
