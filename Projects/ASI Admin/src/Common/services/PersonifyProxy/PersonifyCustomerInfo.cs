
namespace asi.asicentral.services.PersonifyProxy
{
    public class PersonifyCustomerInfo
    {
        public string AsiNumber { get; set; }
        public int CustomerNumber { get; set; }
        public string MasterCustomerId { get; set; }
        public int SubCustomerId { get; set; }
        public string RecordType { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string LabelName { get; set; }
        public string CustomerStatusCode { get; set; }
        public string MemberStatus { get; set; }
        public string PrimaryEmail { get; set; }
        public string CustomerClassCode { get; set; }
        public string SubClassCode { get; set; }
        public bool DNSFlag { get; set; }

        public override bool Equals(object obj)
        {
            var customerInfo = obj as PersonifyCustomerInfo;
            return customerInfo != null && 
                   MasterCustomerId == customerInfo.MasterCustomerId && SubCustomerId == customerInfo.SubCustomerId;
        }

        public override int GetHashCode()
        {
           return MasterCustomerId.GetHashCode();
        }
    }
}
