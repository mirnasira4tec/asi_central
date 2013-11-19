using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreDetailCatalog
    {
        public int OrderDetailId { get; set; }
        public int CoverId { get; set; }
        public int AreaId { get; set; }
        public int ColorId { get; set; }
        public int ImprintId { get; set; }
        public int SupplementId { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Line5 { get; set; }
        public string Line6 { get; set; }
        public string BackLine1 { get; set; }
        public string BackLine2 { get; set; }
        public string BackLine3 { get; set; }
        public string BackLine4 { get; set; }
        public string ArtworkOption { get; set; }
        public string LogoPath { get; set; }
        public bool IsArtworkToProof { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
