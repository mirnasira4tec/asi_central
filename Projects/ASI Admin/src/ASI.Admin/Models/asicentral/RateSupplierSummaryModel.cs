namespace asi.asicentral.web.Models.asicentral
{
    public class RateSupplierSummaryModel
    {
        public string DistASINum { get; set; }
        public string DistName { get; set; }
        public string SupASINum { get; set; }
        public string SupCompanyName { get; set; }
        public int NumOfTransImport { get; set; }
        public int NumOfTransSubmit { get; set; }
        public int? TransDifference { get; set; }
        public int? OverallRating { get; set; }
        public int? ProdQualityRating { get; set; }
        public int? CommunicationRating { get; set; }
        public int? DeliveryRating { get; set; }
        public int? ProbResolutionRating { get; set; }
        public int? ImprintingRating { get; set; }
    }
}