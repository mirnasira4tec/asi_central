using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public partial class LegacyOrderCatalog
    {
        public int OrderID { get; set; }
        public int ProdID { get; set; }
        public string NewLine1 { get; set; }
        public string NewLine2 { get; set; }
        public string NewLine3 { get; set; }
        public string NewLine4 { get; set; }
        public string NewLine5 { get; set; }
        public string NewLine6 { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
        public string BackLine1 { get; set; }
        public string BackLine2 { get; set; }
        public string BackLine3 { get; set; }
        public string BackLine4 { get; set; }
        public Nullable<bool> ArtworkProof { get; set; }
        public string Artwork { get; set; }
        public string Logo { get; set; }
        public Nullable<int> ShipType { get; set; }
    }
}
