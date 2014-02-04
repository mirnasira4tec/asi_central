using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.model.store
{

    public class StoreDetailMagazineAdvertisingItem
    {
        public int Id { get; set; }
        public int OrderDetailId { get; set; }
        public virtual LookMagazineIssue Issue { get; set; }
        public virtual LookAdSize Size { get; set; }
        public virtual LookAdPosition Position { get; set; }
        public bool ArtWork { get; set; }
        public int Sequence { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }

    public enum MagazineType: short
    {
        Counselor = 72,
        Advantages = 73,
        Stitches = 74,
        Wearables = 75,
        SGR = 76
    }
}