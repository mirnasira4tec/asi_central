using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.findsupplier
{
    public class SupplierRating
    {
        public int SupplierId { get; set; }
        public string ASINumber { get; set; }
        public int Overall { get; set; }
        public int OverallDist { get; set; }
        public int OverallTran { get; set; }
        public int Quality { get; set; }
        public int QualityDist { get; set; }
        public int QualityTran { get; set; }
        public int Communication { get; set; }
        public int CommunicationDist { get; set; }
        public int CommunicationTran { get; set; }
        public int Delivery { get; set; }
        public int DeliveryDist { get; set; }
        public int DeliveryTran { get; set; }
        public int ProblemResolution { get; set; }
        public int ProblemResolutionDist { get; set; }
        public int ProblemResolutionTran { get; set; }
        public int Imprint { get; set; }
        public int ImprintDist { get; set; }
        public int ImprintTran { get; set; }
        public string RateAvgCount { get; set; }
        public string RateCount { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateSource { get; set; }
        
    }
}
