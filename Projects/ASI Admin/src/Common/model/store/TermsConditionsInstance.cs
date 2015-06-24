using asi.asicentral.model.store;
using System;

namespace asi.asicentral.model.store
{
    public class TermsConditionsInstance
    {
        public int Id { get; set; }
        public string GUID { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
        public int TypeId { get; set; }
        public string IPAddress { get; set; }
        public int? OrderId { get; set; }
        public DateTime? DateAgreedOn { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public string NotificationEmail { get; set; }
        public string Messages { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public virtual TermsConditionsType TermsAndConditions { get; set; }
        public virtual StoreOrder StoreOrder { get; set; }
    }
}
