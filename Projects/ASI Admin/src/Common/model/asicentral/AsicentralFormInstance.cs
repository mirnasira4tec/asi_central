using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace asi.asicentral.model.asicentral
{
    public class AsicentralFormInstance
    {
        public AsicentralFormInstance()
        {
            if (this.GetType() == typeof(AsicentralFormInstance))
            {
                Values = new List<AsicentralFormValue>();
            }
        }

        public int Id { get; set; }
        public string Reference { get; set; }
        public int TypeId { get; set; }
        public int? CCProfileId { get; set; }
        [RegularExpression("[0-9]{2,13}", ErrorMessage = "Please enter valid constituent Id")]
        public string CompanyConstituentId { get; set; }
        public string Email { get; set; }
        public string IPAddress { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public string ApprovedBy { get; set; }
        public string Status { get; set; }
        public bool IsCCRequestSent { get; set; }

        public virtual AsicentralFormType FormType { get; set; }
        public virtual IList<AsicentralFormValue> Values { get; set; }
    }
}
