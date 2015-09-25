using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace asi.asicentral.services.PersonifyProxy
{
    [Serializable]
    [XmlRoot("NewDataSet")]
    public class PersonifyCustomerInfos
    {
        [XmlElement("Table")]
        public List<PersonifyCustomerInfo> CustometInfoList { get; set; }
    }


    public class PersonifyCustomerInfo
    {
        [XmlElement("USR_ASI_NUMBER")]
        public string AsiNumber { get; set; }
        [XmlElement("USR_CUSTOMER_NUMBER")]
        public int CustomerNumber { get; set; }
        [XmlElement("MASTER_CUSTOMER_ID")] 
        public string MasterCustomerId { get; set; }
        [XmlElement("SUB_CUSTOMER_ID")]
        public int SubCustomerId { get; set; }
        [XmlElement("RECORD_TYPE")]
        public string RecordType { get; set; }
        [XmlElement("LAST_NAME")]
        public string LastName { get; set; }
        [XmlElement("FIRST_NAME")]
        public string FirstName { get; set; }
        [XmlElement("LABEL_NAME")]
        public string LabelName { get; set; }
        [XmlElement("CUSTOMER_STATUS_CODE")]
        public string CustomerStatusCode { get; set; }
        [XmlElement("USR_MEMBER_STATUS")]
        public string MemberStatus { get; set; }
        [XmlElement("PRIMARY_EMAIL_ADDRESS")]
        public string PrimaryEmail { get; set; }
        [XmlElement("CUSTOMER_CLASS_CODE")]
        public string CustomerClassCode { get; set; }
        [XmlElement("USR_SUB_CLASS")]
        public string SubClassCode { get; set; }
        [XmlElement("USR_DNS_FLAG")]
        public bool DNSFlag { get; set; }
    }
}
