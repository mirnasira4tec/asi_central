using System;
using System.Collections.Generic;

namespace asi.asicentral.model.timss
{
    public partial class TIMSSAdditionalInfo
    {
        public System.Guid DAPP_AppId { get; set; }
        public System.Guid DAPP_UserId { get; set; }
        public string NumberOfEmployees { get; set; }
        public string NumberOfSalesPeople { get; set; }
        public string AnnualSalesVol { get; set; }
        public string ASIContact { get; set; }
        public string AnnualSalesVolumeASP { get; set; }
        public string BusinessRevenue { get; set; }
        public string BusinessRevenueOther { get; set; }
        public string IPAddress { get; set; }
        public string LoadStatus { get; set; }
        public Nullable<System.DateTime> LoadDate { get; set; }
        public Nullable<decimal> YearEstablished { get; set; }
        public string Imprinter { get; set; }
        public string Importer { get; set; }
        public string Manufacturer { get; set; }
        public string Retailer { get; set; }
        public string Wholesaler { get; set; }
        public string Etching { get; set; }
        public string HotStamping { get; set; }
        public string SilkScreen { get; set; }
        public string PadPrinting { get; set; }
        public string DirectEmbroidery { get; set; }
        public string FoilStamping { get; set; }
        public string Lithography { get; set; }
        public string Sublimation { get; set; }
        public string FourColorProcess { get; set; }
        public string Engraving { get; set; }
        public string Laser { get; set; }
        public string Offset { get; set; }
        public string Transfer { get; set; }
        public string FullColorProcess { get; set; }
        public string DieStamp { get; set; }
        public string ImprintOther { get; set; }
        public Nullable<decimal> YearEstablishedAsAdSpecialist { get; set; }
        public string WomanOwned { get; set; }
        public string MinorityOwned { get; set; }
        public string AMER_MADE_AVAIL { get; set; }
        public string BusinessHours { get; set; }
        public Nullable<decimal> ProductionTime { get; set; }
        public string RushService { get; set; }
    }
}
