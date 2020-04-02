using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.model.asicentral
{
  public  class RateSupplierForm
    {
        public int RateSupplierFormId { get; set; }
        public int RateSupplierImportId { get; set; }
        public string DistASINum { get; set; }
        public string DistCompanyName { get; set; }
        public string DistFax { get; set; }
        public string DistPhone { get; set; }
        public string SubmitBy { get; set; }
        public DateTime? SubmitDateUTC { get; set; }
        public bool SubmitSuccessful { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
        public string SubmitName { get; set; }
        public string SubmitEmail { get; set; }
        public string IPAddress { get; set; }
        public bool IsDirty { get; set; }
        public virtual RateSupplierImport RateSupplierImports { get; set; }
        public virtual List<RateSupplierFormDetail> RateSupplierFormDetails { get; set; }
    }
}
