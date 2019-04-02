using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.model.asicentral
{
   public class RateSupplierFormDetail
    {
        public int RateSupplierFormDetailId { get; set; }
        public int RateSupplierFormId { get; set; }
        public string SupASINum { get; set; }
        public string SupCompanyName { get; set; }
        public int NumOfTransImport { get; set; }
        public int NumOfTransSubmit { get; set; }
        public int? OverallRating { get; set; }
        public int? ProdQualityRating { get; set; }
        public int? CommunicationRating { get; set; }
        public int? DeliveryRating { get; set; }
        public int? ImprintingRating { get; set; }
        public int? ProbResolutionRating { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
        public bool SubmitSuccessful { get; set; }
        public virtual RateSupplierForm RateSupplierForm { get; set; }
    }
}
